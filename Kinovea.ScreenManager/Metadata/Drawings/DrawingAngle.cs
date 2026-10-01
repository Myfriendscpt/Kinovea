
/*
Copyright � Joan Charmant 2008.
jcharmant@gmail.com 
 
This file is part of Kinovea.

Kinovea is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License version 2 
as published by the Free Software Foundation.

Kinovea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Kinovea. If not, see http://www.gnu.org/licenses/.

*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using Kinovea.ScreenManager.Languages;
using Kinovea.Services;

namespace Kinovea.ScreenManager
{
    [XmlType ("Angle")]
    public class DrawingAngle : AbstractDrawing, IKvaSerializable, IDecorable, IInitializable, ITrackable, IMeasurable
    {
        #region Events
        public event EventHandler<TrackablePointMovedEventArgs> TrackablePointMoved;
        public event EventHandler<EventArgs<MeasureLabelType>> ShowMeasurableInfoChanged;
        #endregion
        
        #region Properties
        public override string ToolDisplayName
        {
            get { return ScreenManagerLang.ToolTip_DrawingToolAngle2D; }
        }
        public override int ContentHash
        {
            get 
            {
                int hash = 0;
                hash ^= styleData.ContentHash;
                hash ^= infosFading.ContentHash;
                hash ^= signedAngle.GetHashCode();
                hash ^= counterClockwise.GetHashCode();
                hash ^= supplementaryAngle.GetHashCode();
                hash ^= showCircle.GetHashCode();
                hash ^= targetRangeEnabled.GetHashCode();
                hash ^= targetAngle.GetHashCode();
                hash ^= targetTolerance.GetHashCode();
                hash ^= targetAudioAlert.GetHashCode();
                return hash; 
            }
        } 
        public StyleElements StyleElements
        {
            get { return styleElements;}
        }
        public override InfosFading InfosFading
        {
            get{ return infosFading;}
            set{ infosFading = value;}
        }
        public override DrawingCapabilities Caps
        {
            get { return DrawingCapabilities.ConfigureColor | DrawingCapabilities.Fading | DrawingCapabilities.Track | DrawingCapabilities.CopyPaste; }
        }
        public override List<ToolStripItem> ContextMenu
        {
            get 
            {
                List<ToolStripItem> contextMenu = new List<ToolStripItem>();
                ReloadMenusCulture();

                contextMenu.AddRange(new ToolStripItem[] {
                    mnuOptions
                });

                mnuSignedAngle.Checked = signedAngle;
                mnuCounterClockwise.Checked = counterClockwise;
                mnuSupplementaryAngle.Checked = supplementaryAngle;
                mnuShowCircle.Checked = showCircle;
                mnuTargetRange.Checked = targetRangeEnabled;

                return contextMenu; 
            }
        }
        public bool Initializing
        {
            get { return initializing; }
        }
        public AngleOptions AngleOptions
        {
            get { return new AngleOptions(signedAngle, counterClockwise, supplementaryAngle); }
        }
        public CalibrationHelper CalibrationHelper { get; set; }

        public bool TargetRangeEnabled
        {
            get { return targetRangeEnabled; }
            set { targetRangeEnabled = value; }
        }
        public float TargetAngle
        {
            get { return targetAngle; }
            set { targetAngle = value; }
        }
        public float TargetTolerance
        {
            get { return targetTolerance; }
            set { targetTolerance = value; }
        }
        public bool TargetAudioAlert
        {
            get { return targetAudioAlert; }
            set { targetAudioAlert = value; }
        }
        public Color TargetInRangeColor
        {
            get { return targetInRangeColor; }
            set { targetInRangeColor = value; }
        }
        public Color TargetOutOfRangeColor
        {
            get { return targetOutOfRangeColor; }
            set { targetOutOfRangeColor = value; }
        }
        #endregion

        #region Members
        private Dictionary<string, PointF> points = new Dictionary<string, PointF>();
        private long trackingTimestamps = -1;
        private bool initializing = true;
        
        private AngleHelper angleHelper = new AngleHelper();
        private StyleElements styleElements = new StyleElements();
        private StyleData styleData = new StyleData();
        private InfosFading infosFading;

        // Options
        private bool signedAngle = true;
        private bool counterClockwise = true;
        private bool supplementaryAngle = false;
        private bool showCircle = false;

        // Target Range & Biofeedback
        private bool targetRangeEnabled = false;
        private float targetAngle = 90.0f;
        private float targetTolerance = 5.0f;
        private bool targetAudioAlert = false;
        private Color targetInRangeColor = Color.LimeGreen;
        private Color targetOutOfRangeColor = Color.Tomato;
        private bool? lastInRangeState = null;

        #region Context menu
        private ToolStripMenuItem mnuOptions = new ToolStripMenuItem();
        private ToolStripMenuItem mnuSignedAngle = new ToolStripMenuItem();
        private ToolStripMenuItem mnuCounterClockwise = new ToolStripMenuItem();
        private ToolStripMenuItem mnuSupplementaryAngle = new ToolStripMenuItem();
        private ToolStripMenuItem mnuShowCircle = new ToolStripMenuItem();
        private ToolStripMenuItem mnuTargetRange = new ToolStripMenuItem();
        #endregion

        private static readonly int defaultBackgroundAlpha = 92;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor
        public DrawingAngle(PointF origin, long timestamp, double averageTimeStampsPerFrame, StyleElements preset = null, IImageToViewportTransformer transformer = null)
        {
            int length = 50;
            if (transformer != null)
                length = transformer.Untransform(50);

            points.Add("o", origin);
            points.Add("a", origin.Translate(length, 0));
            points.Add("b", origin.Translate(0, -length));

            SetupStyle(preset);
            
            // Fading
            infosFading = new InfosFading(timestamp, averageTimeStampsPerFrame);

            InitializeMenus();
        }
        public DrawingAngle(XmlReader xmlReader, PointF scale, TimestampMapper timestampMapper, Metadata parent)
            : this(PointF.Empty, 0, 0)
        {
            ReadXml(xmlReader, scale, timestampMapper);
        }

        private void InitializeMenus()
        {
            mnuOptions.Image = Properties.Resources.equalizer;
            // TODO: images.

            mnuShowCircle.Image = Properties.Drawings.circle;

            mnuSignedAngle.Click += mnuSignedAngle_Click;
            mnuCounterClockwise.Click += mnuCounterClockwise_Click;
            mnuSupplementaryAngle.Click += mnuSupplementaryAngle_Click;
            mnuShowCircle.Click += mnuShowCircle_Click;
            mnuTargetRange.Click += mnuTargetRange_Click;

            mnuOptions.DropDownItems.AddRange(new ToolStripItem[] {
                mnuSignedAngle,
                mnuCounterClockwise,
                mnuSupplementaryAngle,
                mnuShowCircle,
                new ToolStripSeparator(),
                mnuTargetRange,
            });
        }
        #endregion

        #region AbstractDrawing Implementation
        public override void Draw(Graphics canvas, DistortionHelper distorter, CameraTransformer cameraTransformer, IImageToViewportTransformer transformer, bool selected, long currentTimestamp)
        {
            double opacityFactor = infosFading.GetOpacityTrackable(trackingTimestamps, currentTimestamp);
            if (opacityFactor <= 0)
                return;
            
            ComputeValues(transformer);
            
            Point pointO = transformer.Transform(points["o"]);
            Point pointA = transformer.Transform(points["a"]);
            Point pointB = transformer.Transform(points["b"]);
            Rectangle boundingBox = transformer.Transform(angleHelper.SweepAngle.BoundingBox);

            PointF arrowStart = transformer.Transform(angleHelper.SweepAngle.ArrowStart);
            PointF arrowEnd = transformer.Transform(angleHelper.SweepAngle.ArrowEnd);

            if (boundingBox.Size == Size.Empty)
                return;

            float measuredAngleDegrees = CalibrationHelper != null 
                ? CalibrationHelper.ConvertAngle(angleHelper.CalibratedAngle) 
                : (float)(Math.Abs(angleHelper.CalibratedAngle) * 180.0 / Math.PI);
            bool inRange = Math.Abs(measuredAngleDegrees - targetAngle) <= targetTolerance;

            if (targetRangeEnabled && targetAudioAlert)
            {
                if (lastInRangeState.HasValue && lastInRangeState.Value != inRange)
                {
                    TriggerBiofeedbackAudio(inRange);
                }
                lastInRangeState = inRange;
            }

            Color activeEdgeBase = targetRangeEnabled ? (inRange ? targetInRangeColor : targetOutOfRangeColor) : styleData.GetBackgroundColor();
            Color activeFillBase = targetRangeEnabled ? (inRange ? targetInRangeColor : targetOutOfRangeColor) : styleData.GetBackgroundColor();

            using(Pen penEdges = new Pen(Color.FromArgb((int)(opacityFactor * 255), activeEdgeBase)))
            using(SolidBrush brushEdges = new SolidBrush(Color.FromArgb((int)(opacityFactor * 255), activeEdgeBase)))
            using(SolidBrush brushFill = new SolidBrush(Color.FromArgb((int)(opacityFactor * defaultBackgroundAlpha), activeFillBase)))
            {
                penEdges.Width = 2.0f;
                
                // Disk section
                canvas.FillPie(brushFill, boundingBox, angleHelper.SweepAngle.Start, angleHelper.SweepAngle.Sweep);

                // Arc or full circle.
                if (showCircle)
                {
                    canvas.DrawEllipse(penEdges, boundingBox);
                }
                else
                {
                    canvas.DrawArc(penEdges, boundingBox, angleHelper.SweepAngle.Start, angleHelper.SweepAngle.Sweep);
                    //canvas.DrawArc(Pens.Red, boundingBox, angleHelper.SweepAngle.Start, angleHelper.SweepAngle.Sweep);
                }
                
                // Debug: reference plane.
                //canvas.DrawRectangle(Pens.Violet, boundingBox);

                // Reference leg.
                penEdges.DashStyle = DashStyle.Dash;
                canvas.DrawLine(penEdges, pointO, pointA);

                // Measurement leg.
                penEdges.DashStyle = DashStyle.Solid;
                canvas.DrawLine(penEdges, pointO, pointB);
    
                // Handlers.
                canvas.DrawEllipse(penEdges, pointO.Box(3));
                canvas.FillEllipse(brushEdges, pointA.Box(3));
                canvas.FillEllipse(brushEdges, pointB.Box(3));

                // Arrow
                if (boundingBox.Size.Width > 100)
                {
                    //Debug: arrow dir
                    //canvas.DrawLine(Pens.Blue, arrowStart, arrowEnd);
                    PointF arrowOffsetEnd = ArrowHelper.GetOffset(penEdges.Width, arrowEnd, arrowStart);
                    arrowEnd = new PointF(arrowEnd.X + arrowOffsetEnd.X, arrowEnd.Y + arrowOffsetEnd.Y);
                    ArrowHelper.Draw(canvas, penEdges, arrowEnd, arrowStart);
                    // Debug: arrow dir
                    //canvas.DrawLine(Pens.Red, arrowStart, arrowEnd);
                }

                // Value
                angleHelper.DrawText(canvas, opacityFactor, brushFill, pointO, transformer, CalibrationHelper, styleData);

                // Target Biofeedback Indicator Badge
                if (targetRangeEnabled)
                {
                    string statusText = inRange 
                        ? "✓ IN TARGET" 
                        : string.Format("{0:0.0}° {1}", Math.Abs(measuredAngleDegrees - targetAngle), measuredAngleDegrees < targetAngle ? "LOW" : "HIGH");
                    using (Font badgeFont = new Font("Segoe UI", 9f, FontStyle.Bold))
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    using (SolidBrush badgeBgBrush = new SolidBrush(Color.FromArgb((int)(opacityFactor * 220), activeEdgeBase)))
                    {
                        SizeF badgeSize = canvas.MeasureString(statusText, badgeFont);
                        PointF badgePos = new PointF(pointO.X + 20, pointO.Y - 28);
                        RectangleF badgeRect = new RectangleF(badgePos.X - 4, badgePos.Y - 2, badgeSize.Width + 8, badgeSize.Height + 4);
                        canvas.FillRectangle(badgeBgBrush, badgeRect);
                        canvas.DrawRectangle(penEdges, Rectangle.Round(badgeRect));
                        canvas.DrawString(statusText, badgeFont, textBrush, badgePos);
                    }
                }
            }
        }
        public override int HitTest(PointF point, long currentTimestamp, DistortionHelper distorter, IImageToViewportTransformer transformer)
        {
            // Convention: miss = -1, object = 0, handle = n.
            int result = -1;
            double opacityFactor = infosFading.GetOpacityTrackable(trackingTimestamps, currentTimestamp);
            if (opacityFactor > 0)
            {
                if (HitTester.HitPoint(point, points["o"], transformer))
                    result = 1;
                else if (HitTester.HitPoint(point, points["a"], transformer))
                    result = 2;
                else if (HitTester.HitPoint(point, points["b"], transformer))
                    result = 3;
                else if (IsPointInObject(point))
                    result = 0;
            }
            
            return result;
        }
        public override void MoveHandle(PointF point, int handle, Keys modifiers)
        {
            int constraintAngleSubdivisions = 8; // (Constraint by 45� steps).
            switch (handle)
            {
                case 1:
                    points["o"] = point;
                    SignalTrackablePointMoved("o");
                    break;
                case 2:
                    if((modifiers & Keys.Shift) == Keys.Shift)
                        points["a"] = GeometryHelper.GetPointAtClosestRotationStepCardinal(points["o"], point, constraintAngleSubdivisions);
                    else
                        points["a"] = point;
                    
                    SignalTrackablePointMoved("a");
                    break;
                case 3:
                    if((modifiers & Keys.Shift) == Keys.Shift)
                        points["b"] = GeometryHelper.GetPointAtClosestRotationStepCardinal(points["o"], point, constraintAngleSubdivisions);
                    else
                        points["b"] = point;
                    
                    SignalTrackablePointMoved("b");
                    break;
                default:
                    break;
            }
        }
        public override void MoveDrawing(float dx, float dy, Keys modifierKeys)
        {
            points["o"] = points["o"].Translate(dx, dy);
            points["a"] = points["a"].Translate(dx, dy);
            points["b"] = points["b"].Translate(dx, dy);
            SignalAllTrackablePointsMoved();
        }
        public override PointF GetCopyPoint()
        {
            return points["o"];
        }
        #endregion
            
        #region KVA Serialization
        public void ReadXml(XmlReader xmlReader, PointF scale, TimestampMapper timestampMapper)
        {
            if (xmlReader.MoveToAttribute("id"))
                identifier = new Guid(xmlReader.ReadContentAsString());

            if (xmlReader.MoveToAttribute("name"))
                name = xmlReader.ReadContentAsString();

            xmlReader.ReadStartElement();
            
            while(xmlReader.NodeType == XmlNodeType.Element)
            {
                switch(xmlReader.Name)
                {
                    case "PointO":
                        points["o"] = XmlHelper.ParsePointF(xmlReader.ReadElementContentAsString());
                        break;
                    case "PointA":
                        points["a"] = XmlHelper.ParsePointF(xmlReader.ReadElementContentAsString());
                        break;
                    case "PointB":
                        points["b"] = XmlHelper.ParsePointF(xmlReader.ReadElementContentAsString());
                        break;
                    case "ReferenceTimestamp":
                        referenceTimestamp = XmlHelper.ParseTimestamp(xmlReader.ReadElementContentAsString());
                        break;
                    case "Signed":
                        signedAngle = XmlHelper.ParseBoolean(xmlReader.ReadElementContentAsString());
                        break;
                    case "CCW":
                        counterClockwise = XmlHelper.ParseBoolean(xmlReader.ReadElementContentAsString());
                        break;
                    case "Supplementary":
                        supplementaryAngle = XmlHelper.ParseBoolean(xmlReader.ReadElementContentAsString());
                        break;
                    case "ShowCircle":
                        showCircle = XmlHelper.ParseBoolean(xmlReader.ReadElementContentAsString());
                        break;
                    case "TargetRangeEnabled":
                        targetRangeEnabled = XmlHelper.ParseBoolean(xmlReader.ReadElementContentAsString());
                        break;
                    case "TargetAngle":
                        targetAngle = XmlHelper.ParseFloat(xmlReader.ReadElementContentAsString());
                        break;
                    case "TargetTolerance":
                        targetTolerance = XmlHelper.ParseFloat(xmlReader.ReadElementContentAsString());
                        break;
                    case "TargetAudioAlert":
                        targetAudioAlert = XmlHelper.ParseBoolean(xmlReader.ReadElementContentAsString());
                        break;
                    case "TargetInRangeColor":
                        targetInRangeColor = XmlHelper.ParseColor(xmlReader.ReadElementContentAsString(), Color.LimeGreen);
                        break;
                    case "TargetOutOfRangeColor":
                        targetOutOfRangeColor = XmlHelper.ParseColor(xmlReader.ReadElementContentAsString(), Color.Tomato);
                        break;
                    case "DrawingStyle":
                        styleElements.ImportXML(xmlReader);
                        BindStyle();
                        break;
                    case "InfosFading":
                        infosFading.ReadXml(xmlReader);
                        break;
                    case "Measure":
                        xmlReader.ReadOuterXml();
                        break;
                    default:
                        string unparsed = xmlReader.ReadOuterXml();
                        log.DebugFormat("Unparsed content in KVA XML: {0}", unparsed);
                        break;
                }
            }
            
            xmlReader.ReadEndElement();
            initializing = false;

            points["o"] = points["o"].Scale(scale.X, scale.Y);
            points["a"] = points["a"].Scale(scale.X, scale.Y);
            points["b"] = points["b"].Scale(scale.X, scale.Y);
            SignalAllTrackablePointsMoved();
        }
        public void WriteXml(XmlWriter w, SerializationFilter filter)
        {
            if (ShouldSerializeCore(filter))
            {
                PointF o = parentMetadata.TrackabilityManager.GetReferenceValue(Id, "o");
                PointF a = parentMetadata.TrackabilityManager.GetReferenceValue(Id, "a");
                PointF b = parentMetadata.TrackabilityManager.GetReferenceValue(Id, "b");
                w.WriteElementString("PointO", XmlHelper.WritePointF(o));
                w.WriteElementString("PointA", XmlHelper.WritePointF(a));
                w.WriteElementString("PointB", XmlHelper.WritePointF(b));
                w.WriteElementString("ReferenceTimestamp", XmlHelper.WriteTimestamp(referenceTimestamp));

                w.WriteElementString("Signed", XmlHelper.WriteBoolean(signedAngle));
                w.WriteElementString("CCW", XmlHelper.WriteBoolean(counterClockwise));
                w.WriteElementString("Supplementary", XmlHelper.WriteBoolean(supplementaryAngle));
                w.WriteElementString("ShowCircle", XmlHelper.WriteBoolean(showCircle));
                w.WriteElementString("TargetRangeEnabled", XmlHelper.WriteBoolean(targetRangeEnabled));
                w.WriteElementString("TargetAngle", XmlHelper.WriteFloat(targetAngle));
                w.WriteElementString("TargetTolerance", XmlHelper.WriteFloat(targetTolerance));
                w.WriteElementString("TargetAudioAlert", XmlHelper.WriteBoolean(targetAudioAlert));
                w.WriteElementString("TargetInRangeColor", XmlHelper.WriteColor(targetInRangeColor, true));
                w.WriteElementString("TargetOutOfRangeColor", XmlHelper.WriteColor(targetOutOfRangeColor, true));
            }

            if (ShouldSerializeStyle(filter))
            {
                w.WriteStartElement("DrawingStyle");
                styleElements.WriteXml(w);
                w.WriteEndElement();
            }

            if (ShouldSerializeFading(filter))
            {
                w.WriteStartElement("InfosFading");
                infosFading.WriteXml(w);
                w.WriteEndElement();
            }
        }
        public MeasuredDataAngle CollectMeasuredData()
        {
            angleHelper.Update(points["o"], points["a"], points["b"], signedAngle, counterClockwise, supplementaryAngle, CalibrationHelper);
            return MeasurementSerializationHelper.CollectAngle(name, angleHelper, CalibrationHelper);
        }
        #endregion
        
        #region IInitializable implementation
        public void InitializeMove(PointF point, Keys modifiers)
        {
            MoveHandle(point, 3, modifiers);
        }
        public string InitializeCommit(PointF point)
        {
            initializing = false;
            return null;
        }
        public string InitializeEnd(bool cancelCurrentPoint)
        {
            return null;
        }
        #endregion
        
        #region ITrackable implementation and support.
        public Color Color
        {
            get { return styleData.GetBackgroundColor(); }
        }
        public TrackingParameters CustomTrackingParameters
        {
            get { return null; }
        }
        public Dictionary<string, PointF> GetTrackablePoints()
        {
            return points;
        }
        public void SetTrackablePointValue(string name, PointF value, long trackingTimestamps)
        {
            if(!points.ContainsKey(name))
                throw new ArgumentException("This point is not bound.");
            
            points[name] = value;
            this.trackingTimestamps = trackingTimestamps;
        }
        private void SignalAllTrackablePointsMoved()
        {
            if(TrackablePointMoved == null)
                return;
            
            foreach(KeyValuePair<string, PointF> p in points)
                TrackablePointMoved(this, new TrackablePointMovedEventArgs(p.Key, p.Value));
        }
        private void SignalTrackablePointMoved(string name)
        {
            if(TrackablePointMoved == null || !points.ContainsKey(name))
                return;
            
            TrackablePointMoved(this, new TrackablePointMovedEventArgs(name, points[name]));
        }
        #endregion
        
        #region Specific context menu
        
        private void mnuSignedAngle_Click(object sender, EventArgs e)
        {
            CaptureMemento(SerializationFilter.Core);
            signedAngle = !mnuSignedAngle.Checked;
            SignalAllTrackablePointsMoved();
            InvalidateFromMenu(sender);
        }

        private void mnuCounterClockwise_Click(object sender, EventArgs e)
        {
            CaptureMemento(SerializationFilter.Core);
            counterClockwise = !mnuCounterClockwise.Checked;
            SignalAllTrackablePointsMoved();
            InvalidateFromMenu(sender);
        }

        private void mnuSupplementaryAngle_Click(object sender, EventArgs e)
        {
            CaptureMemento(SerializationFilter.Core);
            supplementaryAngle = !mnuSupplementaryAngle.Checked;
            SignalAllTrackablePointsMoved();
            InvalidateFromMenu(sender);
        }

        private void mnuShowCircle_Click(object sender, EventArgs e)
        {
            CaptureMemento(SerializationFilter.Core);
            showCircle = !mnuShowCircle.Checked;
            InvalidateFromMenu(sender);
        }

        private void mnuTargetRange_Click(object sender, EventArgs e)
        {
            using (FormConfigureAngleTarget dlg = new FormConfigureAngleTarget(this))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    CaptureMemento(SerializationFilter.Core);
                    lastInRangeState = null;
                    SignalAllTrackablePointsMoved();
                    InvalidateFromMenu(sender);
                }
            }
        }

        private void TriggerBiofeedbackAudio(bool inRange)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (inRange)
                    {
                        Console.Beep(880, 80);
                        Console.Beep(1175, 120);
                    }
                    else
                    {
                        Console.Beep(330, 80);
                        Console.Beep(262, 100);
                    }
                }
                catch
                {
                    try { System.Media.SystemSounds.Beep.Play(); } catch { }
                }
            });
        }

        #endregion

        #region IMeasurable implementation
        public void InitializeMeasurableData(MeasureLabelType measureLabelType)
        {
        }
        #endregion

        #region Lower level helpers
        private void SetupStyle(StyleElements preset)
        {
            // Initialize style data in case we don't import some values.
            // These are the properties we need to paint this drawing.
            styleData.BackgroundColor = Color.Black;
            styleData.Font = new Font("Arial", 12, FontStyle.Bold);
            styleData.DecimalPlaces = 0;

            // Fallback preset in case we don't have one.
            // (new tool but old prefs).
            if (preset == null)
                preset = ToolManager.GetDefaultStyleElements("Angle");

            // Import full style elements (with metadata) from the tool.
            styleElements = preset.Clone();

            // Bind the style elements to the data fields and push the initial values.
            BindStyle();
        }

        private void BindStyle()
        {
            StyleElements.SanityCheck(styleElements, ToolManager.GetDefaultStyleElements("Angle"));
            styleElements.Bind(styleData, "Bicolor", "line color");
            styleElements.Bind(styleData, "Font", "font size");
            styleElements.Bind(styleData, "DecimalPlaces", "DecimalPlaces");
        }
        private void ComputeValues(IImageToViewportTransformer transformer)
        {
            FixIfNull(transformer);
            angleHelper.UpdateTextDistance(styleData.Font.Size / 12.0f);
            angleHelper.Update(points["o"], points["a"], points["b"], signedAngle, counterClockwise, supplementaryAngle, CalibrationHelper);
        }
        private void FixIfNull(IImageToViewportTransformer transformer)
        {
            int length = transformer.Untransform(50);

            if (points["a"].NearlyCoincideWith(points["o"]))
                points["a"] = points["o"].Translate(0, -length);

            if (points["b"].NearlyCoincideWith(points["o"]))
                points["b"] = points["o"].Translate(length, 0);
        }
        private bool IsPointInObject(PointF point)
        {
            return angleHelper.SweepAngle.Hit(point);
        }
        /// <summary>
        /// Capture the current state to the undo/redo stack.
        /// </summary>
        private void CaptureMemento(SerializationFilter filter)
        {
            var memento = new HistoryMementoModifyDrawing(parentMetadata, parentMetadata.SingletonDrawingsManager.Id, this.Id, this.Name, filter);
            parentMetadata.HistoryStack.PushNewCommand(memento);
        }
        private void ReloadMenusCulture()
        {
            mnuOptions.Text = ScreenManagerLang.Generic_Options;
            mnuSignedAngle.Text = ScreenManagerLang.mnuSignedAngle;
            mnuCounterClockwise.Text = ScreenManagerLang.mnuCounterClockwise;
            mnuSupplementaryAngle.Text = ScreenManagerLang.mnuSupplementaryAngle;
            mnuShowCircle.Text = Kinovea.ScreenManager.Languages.ScreenManagerLang.mnuShowCircle;
            mnuTargetRange.Text = "Target Range & Biofeedback...";
        }
        #endregion
    } 
}