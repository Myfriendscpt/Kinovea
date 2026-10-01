using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Kinovea.Services;

namespace Kinovea.ScreenManager
{
    public enum GaitEventType
    {
        InitialContact,
        Midstance,
        ToeOff
    }

    public class GaitEvent
    {
        public GaitEventType Type { get; set; }
        public long Timestamp { get; set; }
        public int FrameIndex { get; set; }
        public PointF Position { get; set; }
        public double VerticalPosition { get; set; }
        public double HorizontalVelocity { get; set; }

        public string DisplayName
        {
            get
            {
                switch (Type)
                {
                    case GaitEventType.InitialContact: return "Initial Contact (Heel Strike)";
                    case GaitEventType.Midstance: return "Midstance";
                    case GaitEventType.ToeOff: return "Toe-Off";
                    default: return Type.ToString();
                }
            }
        }
    }

    public class GaitStep
    {
        public int StepNumber { get; set; }
        public GaitEvent InitialContact { get; set; }
        public GaitEvent ToeOff { get; set; }
        public GaitEvent NextInitialContact { get; set; }

        public double StepDurationMs { get; set; }
        public double StanceDurationMs { get; set; }
        public double SwingDurationMs { get; set; }
        public double StancePercentage { get; set; }
        public double SwingPercentage { get; set; }
        public double StepLength { get; set; }
        public double DutyFactor => StepDurationMs > 0 ? StanceDurationMs / StepDurationMs : 0;
    }

    public class GaitAnalysisResult
    {
        public string TrackName { get; set; } = "Track";
        public List<GaitEvent> Events { get; set; } = new List<GaitEvent>();
        public List<GaitStep> Steps { get; set; } = new List<GaitStep>();

        public int TotalSteps => Steps.Count;
        public double CadenceStepsPerMinute { get; set; }
        public double AverageStepTimeMs { get; set; }
        public double AverageStanceTimeMs { get; set; }
        public double AverageSwingTimeMs { get; set; }
        public double AverageStancePercentage { get; set; }
        public double AverageSwingPercentage { get; set; }
        public double AverageStepLength { get; set; }
        public string LengthUnit { get; set; } = "px";
    }

    public static class GaitDetector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static GaitAnalysisResult Analyze(DrawingTrack track, CalibrationHelper calibration)
        {
            if (track == null || track.FilteredTrajectory == null)
                return new GaitAnalysisResult { TrackName = track?.Name ?? "Empty" };

            return Analyze(track.FilteredTrajectory, track.Name, calibration);
        }

        public static GaitAnalysisResult Analyze(FilteredTrajectory traj, string trackName, CalibrationHelper calibration)
        {
            if (traj == null || traj.Length < 5)
                return new GaitAnalysisResult { TrackName = trackName ?? "Track" };

            int count = traj.Length;
            long[] timestamps = traj.Times;
            long startTimestamp = timestamps[0];

            string unit = calibration != null ? calibration.GetLengthAbbreviation() : "px";

            double[] times = new double[count];
            double[] rawYs = new double[count];
            double[] rawXs = new double[count];
            PointF[] positions = new PointF[count];

            for (int i = 0; i < count; i++)
            {
                PointF pt = traj.CanFilter ? traj.Coordinates(i) : traj.RawCoordinates(i);
                positions[i] = pt;

                if (calibration != null)
                {
                    PointF cal = calibration.GetPoint(pt);
                    rawXs[i] = cal.X;
                    rawYs[i] = cal.Y;
                }
                else
                {
                    rawXs[i] = pt.X;
                    rawYs[i] = pt.Y;
                }
                times[i] = (timestamps[i] - startTimestamp) / 10000.0; // ticks to milliseconds
            }

            // Smooth vertical and horizontal displacement to remove sensor noise
            double[] smoothYs = Smooth(rawYs, 3);
            double[] vYs = ComputeFirstDerivative(smoothYs, times);
            double[] vXs = ComputeFirstDerivative(rawXs, times);

            // Ground contact in image space occurs at maximum Y (lowest point on screen).
            // In physical calibration, Y may increase upwards (minimum Y).
            bool groundIsMaxY = calibration == null || !calibration.IsCalibrated;

            List<int> heelStrikeIndices = new List<int>();
            List<int> toeOffIndices = new List<int>();

            double minStepTimeMs = 200.0;
            int lastHeelStrike = -1;

            // 1. Detect Initial Contact (Heel Strikes)
            for (int i = 2; i < count - 2; i++)
            {
                bool isGroundPeak;
                if (groundIsMaxY)
                {
                    isGroundPeak = smoothYs[i] > smoothYs[i - 1] && smoothYs[i] >= smoothYs[i - 2] &&
                                   smoothYs[i] > smoothYs[i + 1] && smoothYs[i] >= smoothYs[i + 2];
                }
                else
                {
                    isGroundPeak = smoothYs[i] < smoothYs[i - 1] && smoothYs[i] <= smoothYs[i - 2] &&
                                   smoothYs[i] < smoothYs[i + 1] && smoothYs[i] <= smoothYs[i + 2];
                }

                if (isGroundPeak)
                {
                    if (lastHeelStrike < 0 || (times[i] - times[lastHeelStrike]) >= minStepTimeMs)
                    {
                        heelStrikeIndices.Add(i);
                        lastHeelStrike = i;
                    }
                }
            }

            // 2. Detect Toe-Off between consecutive heel strikes
            for (int h = 0; h < heelStrikeIndices.Count - 1; h++)
            {
                int hsCurrent = heelStrikeIndices[h];
                int hsNext = heelStrikeIndices[h + 1];

                int searchStart = hsCurrent + (int)((hsNext - hsCurrent) * 0.35);
                int searchEnd = hsCurrent + (int)((hsNext - hsCurrent) * 0.85);

                int bestToeOff = -1;
                double maxVerticalLiftSpeed = double.MinValue;

                for (int j = searchStart; j <= searchEnd && j < count; j++)
                {
                    double liftSpeed = groundIsMaxY ? -vYs[j] : vYs[j];
                    if (liftSpeed > maxVerticalLiftSpeed)
                    {
                        maxVerticalLiftSpeed = liftSpeed;
                        bestToeOff = j;
                    }
                }

                if (bestToeOff > hsCurrent && bestToeOff < hsNext)
                {
                    toeOffIndices.Add(bestToeOff);
                }
            }

            GaitAnalysisResult result = new GaitAnalysisResult
            {
                TrackName = trackName,
                LengthUnit = unit
            };

            foreach (int idx in heelStrikeIndices)
            {
                result.Events.Add(new GaitEvent
                {
                    Type = GaitEventType.InitialContact,
                    Timestamp = timestamps[idx],
                    FrameIndex = idx,
                    Position = positions[idx],
                    VerticalPosition = smoothYs[idx],
                    HorizontalVelocity = vXs[idx]
                });
            }

            foreach (int idx in toeOffIndices)
            {
                result.Events.Add(new GaitEvent
                {
                    Type = GaitEventType.ToeOff,
                    Timestamp = timestamps[idx],
                    FrameIndex = idx,
                    Position = positions[idx],
                    VerticalPosition = smoothYs[idx],
                    HorizontalVelocity = vXs[idx]
                });
            }

            result.Events = result.Events.OrderBy(e => e.Timestamp).ToList();

            // Construct Step metrics
            for (int h = 0; h < heelStrikeIndices.Count - 1; h++)
            {
                int hs1Idx = heelStrikeIndices[h];
                int hs2Idx = heelStrikeIndices[h + 1];

                int toIdx = toeOffIndices.FirstOrDefault(t => t > hs1Idx && t < hs2Idx);

                GaitEvent hs1Event = result.Events.FirstOrDefault(e => e.FrameIndex == hs1Idx && e.Type == GaitEventType.InitialContact);
                GaitEvent hs2Event = result.Events.FirstOrDefault(e => e.FrameIndex == hs2Idx && e.Type == GaitEventType.InitialContact);
                GaitEvent toEvent = toIdx > 0 ? result.Events.FirstOrDefault(e => e.FrameIndex == toIdx && e.Type == GaitEventType.ToeOff) : null;

                double stepDuration = times[hs2Idx] - times[hs1Idx];
                double stanceDuration = toIdx > 0 ? (times[toIdx] - times[hs1Idx]) : stepDuration * 0.60;
                double swingDuration = Math.Max(0, stepDuration - stanceDuration);
                double stepLength = Math.Abs(rawXs[hs2Idx] - rawXs[hs1Idx]);

                GaitStep step = new GaitStep
                {
                    StepNumber = h + 1,
                    InitialContact = hs1Event,
                    ToeOff = toEvent,
                    NextInitialContact = hs2Event,
                    StepDurationMs = stepDuration,
                    StanceDurationMs = stanceDuration,
                    SwingDurationMs = swingDuration,
                    StancePercentage = stepDuration > 0 ? (stanceDuration / stepDuration) * 100.0 : 0,
                    SwingPercentage = stepDuration > 0 ? (swingDuration / stepDuration) * 100.0 : 0,
                    StepLength = stepLength
                };

                result.Steps.Add(step);
            }

            if (result.Steps.Count > 0)
            {
                result.AverageStepTimeMs = result.Steps.Average(s => s.StepDurationMs);
                result.AverageStanceTimeMs = result.Steps.Average(s => s.StanceDurationMs);
                result.AverageSwingTimeMs = result.Steps.Average(s => s.SwingDurationMs);
                result.AverageStancePercentage = result.Steps.Average(s => s.StancePercentage);
                result.AverageSwingPercentage = result.Steps.Average(s => s.SwingPercentage);
                result.AverageStepLength = result.Steps.Average(s => s.StepLength);

                if (result.AverageStepTimeMs > 0)
                {
                    result.CadenceStepsPerMinute = (60000.0 / result.AverageStepTimeMs);
                }
            }

            return result;
        }

        private static double[] Smooth(double[] input, int radius)
        {
            int n = input.Length;
            double[] output = new double[n];
            for (int i = 0; i < n; i++)
            {
                int min = Math.Max(0, i - radius);
                int max = Math.Min(n - 1, i + radius);
                double sum = 0;
                for (int j = min; j <= max; j++)
                    sum += input[j];
                output[i] = sum / (max - min + 1);
            }
            return output;
        }

        private static double[] ComputeFirstDerivative(double[] values, double[] times)
        {
            int n = values.Length;
            double[] deriv = new double[n];
            if (n < 2)
                return deriv;

            for (int i = 1; i < n - 1; i++)
            {
                double dt = (times[i + 1] - times[i - 1]) / 1000.0;
                if (Math.Abs(dt) > 1e-6)
                    deriv[i] = (values[i + 1] - values[i - 1]) / dt;
            }
            deriv[0] = deriv[1];
            deriv[n - 1] = deriv[n - 2];
            return deriv;
        }
    }
}
