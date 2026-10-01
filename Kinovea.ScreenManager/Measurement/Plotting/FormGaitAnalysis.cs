using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Kinovea.Services;

namespace Kinovea.ScreenManager
{
    public partial class FormGaitAnalysis : Form
    {
        private Metadata metadata;
        private List<DrawingTrack> tracks = new List<DrawingTrack>();
        private DrawingTrack currentTrack;
        private GaitAnalysisResult currentResult;

        public FormGaitAnalysis(Metadata metadata, DrawingTrack selectedTrack = null)
        {
            this.metadata = metadata;
            InitializeComponent();

            LoadTracks(selectedTrack);
        }

        private void LoadTracks(DrawingTrack preferredTrack)
        {
            tracks.Clear();
            cmbTracks.Items.Clear();

            // Find all DrawingTrack instances in metadata
            foreach (DrawingTrack track in metadata.Tracks())
            {
                if (track != null && track.PointsCount > 5)
                {
                    tracks.Add(track);
                    cmbTracks.Items.Add(track.Name);
                }
            }

            foreach (ITrackable drawing in metadata.TrackableDrawings())
            {
                Dictionary<string, DrawingTrack> subTracks = metadata.TrackabilityManager.GetTrackingTracks(drawing);
                if (subTracks != null)
                {
                    foreach (var pair in subTracks)
                    {
                        DrawingTrack track = pair.Value;
                        if (track != null && track.PointsCount > 5 && !tracks.Contains(track))
                        {
                            tracks.Add(track);
                            cmbTracks.Items.Add(track.Name);
                        }
                    }
                }
            }

            if (tracks.Count == 0 && preferredTrack != null)
            {
                tracks.Add(preferredTrack);
                cmbTracks.Items.Add(preferredTrack.Name);
            }

            if (tracks.Count > 0)
            {
                int index = preferredTrack != null ? tracks.IndexOf(preferredTrack) : 0;
                if (index < 0) index = 0;
                cmbTracks.SelectedIndex = index;
            }
            else
            {
                lblCadence.Text = "No tracks found";
            }
        }

        private void cmbTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbTracks.SelectedIndex;
            if (index >= 0 && index < tracks.Count)
            {
                currentTrack = tracks[index];
                RunAnalysis();
            }
        }

        private void RunAnalysis()
        {
            if (currentTrack == null)
                return;

            currentResult = GaitDetector.Analyze(currentTrack, metadata.CalibrationHelper);
            UpdateDashboard();
            BuildPlot();
            PopulateTable();
        }

        private void UpdateDashboard()
        {
            if (currentResult == null || currentResult.TotalSteps == 0)
            {
                lblCadence.Text = "N/A";
                lblStepTime.Text = "N/A";
                lblStance.Text = "N/A";
                lblSwing.Text = "N/A";
                lblStepLength.Text = "N/A";
                lblTotalSteps.Text = "0";
                return;
            }

            lblCadence.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.0} /min", currentResult.CadenceStepsPerMinute);
            lblStepTime.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.0} ms", currentResult.AverageStepTimeMs);
            lblStance.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.0}% ({1:0.0} ms)", currentResult.AverageStancePercentage, currentResult.AverageStanceTimeMs);
            lblSwing.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.0}% ({1:0.0} ms)", currentResult.AverageSwingPercentage, currentResult.AverageSwingTimeMs);
            lblStepLength.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.00} {1}", currentResult.AverageStepLength, currentResult.LengthUnit);
            lblTotalSteps.Text = currentResult.TotalSteps.ToString();
        }

        private void BuildPlot()
        {
            if (currentTrack == null || currentTrack.FilteredTrajectory == null)
                return;

            FilteredTrajectory traj = currentTrack.FilteredTrajectory;
            int count = traj.Length;
            if (count == 0)
                return;

            PlotModel model = new PlotModel
            {
                Title = string.Format("Vertical Foot Trajectory & Gait Events ({0})", currentTrack.Name),
                PlotType = PlotType.XY
            };

            LinearAxis xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Time (ms)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };
            LinearAxis yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = string.Format("Vertical Position ({0})", currentResult.LengthUnit),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };
            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);

            // Trajectory Line
            LineSeries trajectorySeries = new LineSeries
            {
                Title = "Trajectory",
                Color = OxyColor.FromArgb(200, 30, 144, 255),
                StrokeThickness = 2.0
            };

            long startTimestamp = traj.Times[0];
            for (int i = 0; i < count; i++)
            {
                double timeMs = (traj.Times[i] - startTimestamp) / 10000.0;
                PointF pt = traj.CanFilter ? traj.Coordinates(i) : traj.RawCoordinates(i);
                double yVal = metadata.CalibrationHelper != null ? metadata.CalibrationHelper.GetPoint(pt).Y : pt.Y;
                trajectorySeries.Points.Add(new DataPoint(timeMs, yVal));
            }
            model.Series.Add(trajectorySeries);

            // Initial Contact (Heel Strike) Event Markers
            ScatterSeries heelStrikeSeries = new ScatterSeries
            {
                Title = "Initial Contact (Heel Strike)",
                MarkerType = MarkerType.Circle,
                MarkerSize = 7.0,
                MarkerFill = OxyColors.ForestGreen,
                MarkerStroke = OxyColors.White,
                MarkerStrokeThickness = 1.5
            };

            // Toe-Off Event Markers
            ScatterSeries toeOffSeries = new ScatterSeries
            {
                Title = "Toe-Off",
                MarkerType = MarkerType.Triangle,
                MarkerSize = 7.0,
                MarkerFill = OxyColors.Crimson,
                MarkerStroke = OxyColors.White,
                MarkerStrokeThickness = 1.5
            };

            foreach (var ev in currentResult.Events)
            {
                double eventTimeMs = (ev.Timestamp - startTimestamp) / 10000.0;
                if (ev.Type == GaitEventType.InitialContact)
                {
                    heelStrikeSeries.Points.Add(new ScatterPoint(eventTimeMs, ev.VerticalPosition));
                }
                else if (ev.Type == GaitEventType.ToeOff)
                {
                    toeOffSeries.Points.Add(new ScatterPoint(eventTimeMs, ev.VerticalPosition));
                }
            }

            model.Series.Add(heelStrikeSeries);
            model.Series.Add(toeOffSeries);

            plotGait.Model = model;
        }

        private void PopulateTable()
        {
            lvSteps.Items.Clear();
            if (currentResult == null || currentResult.Steps.Count == 0)
                return;

            long startTimestamp = currentTrack.FilteredTrajectory.Times[0];

            foreach (var step in currentResult.Steps)
            {
                double contactTime = (step.InitialContact.Timestamp - startTimestamp) / 10000.0;
                string toeOffStr = step.ToeOff != null 
                    ? string.Format(CultureInfo.InvariantCulture, "{0:0.0}", (step.ToeOff.Timestamp - startTimestamp) / 10000.0) 
                    : "--";

                ListViewItem item = new ListViewItem(step.StepNumber.ToString());
                item.SubItems.Add(string.Format(CultureInfo.InvariantCulture, "{0:0.0}", contactTime));
                item.SubItems.Add(toeOffStr);
                item.SubItems.Add(string.Format(CultureInfo.InvariantCulture, "{0:0.0}", step.StanceDurationMs));
                item.SubItems.Add(string.Format(CultureInfo.InvariantCulture, "{0:0.0}", step.SwingDurationMs));
                item.SubItems.Add(string.Format(CultureInfo.InvariantCulture, "{0:0.0}% / {1:0.0}%", step.StancePercentage, step.SwingPercentage));
                item.SubItems.Add(string.Format(CultureInfo.InvariantCulture, "{0:0.00} {1}", step.StepLength, currentResult.LengthUnit));

                lvSteps.Items.Add(item);
            }
        }

        private void btnApplyKeyframes_Click(object sender, EventArgs e)
        {
            if (currentResult == null || currentResult.Events.Count == 0)
            {
                MessageBox.Show(this, "No gait events detected to add.", "Gait Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int countAdded = 0;
            foreach (var ev in currentResult.Events)
            {
                Color kfColor = ev.Type == GaitEventType.InitialContact ? Color.FromArgb(46, 139, 87) : Color.FromArgb(220, 20, 60);
                string label = string.Format("{0} [{1}]", ev.DisplayName, currentTrack.Name);

                Keyframe kf = new Keyframe(ev.Timestamp, label, kfColor, metadata);
                metadata.AddKeyframe(kf);
                countAdded++;
            }

            MessageBox.Show(this, string.Format("Successfully added {0} gait events as keyframes to the timeline!", countAdded), "Gait Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCopyData_Click(object sender, EventArgs e)
        {
            if (currentResult == null)
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Gait & Cadence Biomechanical Analysis");
            sb.AppendLine(string.Format("Track: {0}", currentResult.TrackName));
            sb.AppendLine(string.Format("Cadence: {0:0.0} steps/min", currentResult.CadenceStepsPerMinute));
            sb.AppendLine(string.Format("Avg Step Time: {0:0.0} ms", currentResult.AverageStepTimeMs));
            sb.AppendLine(string.Format("Avg Stance Phase: {0:0.0}% ({1:0.0} ms)", currentResult.AverageStancePercentage, currentResult.AverageStanceTimeMs));
            sb.AppendLine(string.Format("Avg Swing Phase: {0:0.0}% ({1:0.0} ms)", currentResult.AverageSwingPercentage, currentResult.AverageSwingTimeMs));
            sb.AppendLine(string.Format("Avg Step Length: {0:0.00} {1}", currentResult.AverageStepLength, currentResult.LengthUnit));
            sb.AppendLine();
            sb.AppendLine("Step\tInitial Contact (ms)\tToe-Off (ms)\tStance (ms)\tSwing (ms)\tStance %\tStep Length");

            long startTimestamp = currentTrack.FilteredTrajectory.Times[0];
            foreach (var step in currentResult.Steps)
            {
                double contactTime = (step.InitialContact.Timestamp - startTimestamp) / 10000.0;
                double toeOffTime = step.ToeOff != null ? (step.ToeOff.Timestamp - startTimestamp) / 10000.0 : 0;
                sb.AppendLine(string.Format("{0}\t{1:0.0}\t{2:0.0}\t{3:0.0}\t{4:0.0}\t{5:0.0}%\t{6:0.00}",
                    step.StepNumber, contactTime, toeOffTime, step.StanceDurationMs, step.SwingDurationMs, step.StancePercentage, step.StepLength));
            }

            Clipboard.SetText(sb.ToString());
            MessageBox.Show(this, "Gait metrics copied to clipboard!", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (currentResult == null)
                return;

            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            saveFileDialog.FileName = string.Format("{0}_GaitAnalysis.csv", currentResult.TrackName);
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Step,InitialContact_ms,ToeOff_ms,StanceDuration_ms,SwingDuration_ms,StancePercentage,SwingPercentage,StepLength");

                long startTimestamp = currentTrack.FilteredTrajectory.Times[0];
                foreach (var step in currentResult.Steps)
                {
                    double contactTime = (step.InitialContact.Timestamp - startTimestamp) / 10000.0;
                    double toeOffTime = step.ToeOff != null ? (step.ToeOff.Timestamp - startTimestamp) / 10000.0 : 0;
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0},{1:0.0},{2:0.0},{3:0.0},{4:0.0},{5:0.0},{6:0.0},{7:0.00}",
                        step.StepNumber, contactTime, toeOffTime, step.StanceDurationMs, step.SwingDurationMs, step.StancePercentage, step.SwingPercentage, step.StepLength));
                }

                File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show(this, "CSV exported successfully!", "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
