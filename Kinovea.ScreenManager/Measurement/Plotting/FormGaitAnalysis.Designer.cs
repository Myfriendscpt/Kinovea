namespace Kinovea.ScreenManager
{
    partial class FormGaitAnalysis
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.plotGait = new OxyPlot.WindowsForms.PlotView();
            this.grpSummary = new System.Windows.Forms.GroupBox();
            this.lblCadenceTitle = new System.Windows.Forms.Label();
            this.lblCadence = new System.Windows.Forms.Label();
            this.lblStepTimeTitle = new System.Windows.Forms.Label();
            this.lblStepTime = new System.Windows.Forms.Label();
            this.lblStanceTitle = new System.Windows.Forms.Label();
            this.lblStance = new System.Windows.Forms.Label();
            this.lblSwingTitle = new System.Windows.Forms.Label();
            this.lblSwing = new System.Windows.Forms.Label();
            this.lblStepLengthTitle = new System.Windows.Forms.Label();
            this.lblStepLength = new System.Windows.Forms.Label();
            this.lblTotalStepsTitle = new System.Windows.Forms.Label();
            this.lblTotalSteps = new System.Windows.Forms.Label();
            this.grpTrack = new System.Windows.Forms.GroupBox();
            this.lblSelectTrack = new System.Windows.Forms.Label();
            this.cmbTracks = new System.Windows.Forms.ComboBox();
            this.lvSteps = new System.Windows.Forms.ListView();
            this.colStep = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContact = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colToeOff = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStanceTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSwingTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStanceRatio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnApplyKeyframes = new System.Windows.Forms.Button();
            this.btnCopyData = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.grpSummary.SuspendLayout();
            this.grpTrack.SuspendLayout();
            this.SuspendLayout();
            // 
            // plotGait
            // 
            this.plotGait.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotGait.Location = new System.Drawing.Point(12, 12);
            this.plotGait.Name = "plotGait";
            this.plotGait.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotGait.Size = new System.Drawing.Size(560, 360);
            this.plotGait.TabIndex = 0;
            this.plotGait.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotGait.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotGait.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // grpSummary
            // 
            this.grpSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSummary.Controls.Add(this.lblTotalSteps);
            this.grpSummary.Controls.Add(this.lblTotalStepsTitle);
            this.grpSummary.Controls.Add(this.lblStepLength);
            this.grpSummary.Controls.Add(this.lblStepLengthTitle);
            this.grpSummary.Controls.Add(this.lblSwing);
            this.grpSummary.Controls.Add(this.lblSwingTitle);
            this.grpSummary.Controls.Add(this.lblStance);
            this.grpSummary.Controls.Add(this.lblStanceTitle);
            this.grpSummary.Controls.Add(this.lblStepTime);
            this.grpSummary.Controls.Add(this.lblStepTimeTitle);
            this.grpSummary.Controls.Add(this.lblCadence);
            this.grpSummary.Controls.Add(this.lblCadenceTitle);
            this.grpSummary.Location = new System.Drawing.Point(582, 75);
            this.grpSummary.Name = "grpSummary";
            this.grpSummary.Size = new System.Drawing.Size(260, 225);
            this.grpSummary.TabIndex = 1;
            this.grpSummary.TabStop = false;
            this.grpSummary.Text = "Gait Biomechanics Summary";
            // 
            // lblCadenceTitle
            // 
            this.lblCadenceTitle.AutoSize = true;
            this.lblCadenceTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCadenceTitle.Location = new System.Drawing.Point(12, 28);
            this.lblCadenceTitle.Name = "lblCadenceTitle";
            this.lblCadenceTitle.Size = new System.Drawing.Size(56, 15);
            this.lblCadenceTitle.TabIndex = 0;
            this.lblCadenceTitle.Text = "Cadence:";
            // 
            // lblCadence
            // 
            this.lblCadence.AutoSize = true;
            this.lblCadence.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCadence.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblCadence.Location = new System.Drawing.Point(130, 26);
            this.lblCadence.Name = "lblCadence";
            this.lblCadence.Size = new System.Drawing.Size(43, 19);
            this.lblCadence.TabIndex = 1;
            this.lblCadence.Text = "-- /m";
            // 
            // lblStepTimeTitle
            // 
            this.lblStepTimeTitle.AutoSize = true;
            this.lblStepTimeTitle.Location = new System.Drawing.Point(12, 60);
            this.lblStepTimeTitle.Name = "lblStepTimeTitle";
            this.lblStepTimeTitle.Size = new System.Drawing.Size(81, 13);
            this.lblStepTimeTitle.TabIndex = 2;
            this.lblStepTimeTitle.Text = "Avg Step Time:";
            // 
            // lblStepTime
            // 
            this.lblStepTime.AutoSize = true;
            this.lblStepTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepTime.Location = new System.Drawing.Point(130, 58);
            this.lblStepTime.Name = "lblStepTime";
            this.lblStepTime.Size = new System.Drawing.Size(35, 15);
            this.lblStepTime.TabIndex = 3;
            this.lblStepTime.Text = "-- ms";
            // 
            // lblStanceTitle
            // 
            this.lblStanceTitle.AutoSize = true;
            this.lblStanceTitle.Location = new System.Drawing.Point(12, 92);
            this.lblStanceTitle.Name = "lblStanceTitle";
            this.lblStanceTitle.Size = new System.Drawing.Size(76, 13);
            this.lblStanceTitle.TabIndex = 4;
            this.lblStanceTitle.Text = "Stance Phase:";
            // 
            // lblStance
            // 
            this.lblStance.AutoSize = true;
            this.lblStance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStance.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblStance.Location = new System.Drawing.Point(130, 90);
            this.lblStance.Name = "lblStance";
            this.lblStance.Size = new System.Drawing.Size(26, 15);
            this.lblStance.TabIndex = 5;
            this.lblStance.Text = "-- %";
            // 
            // lblSwingTitle
            // 
            this.lblSwingTitle.AutoSize = true;
            this.lblSwingTitle.Location = new System.Drawing.Point(12, 124);
            this.lblSwingTitle.Name = "lblSwingTitle";
            this.lblSwingTitle.Size = new System.Drawing.Size(71, 13);
            this.lblSwingTitle.TabIndex = 6;
            this.lblSwingTitle.Text = "Swing Phase:";
            // 
            // lblSwing
            // 
            this.lblSwing.AutoSize = true;
            this.lblSwing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSwing.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblSwing.Location = new System.Drawing.Point(130, 122);
            this.lblSwing.Name = "lblSwing";
            this.lblSwing.Size = new System.Drawing.Size(26, 15);
            this.lblSwing.TabIndex = 7;
            this.lblSwing.Text = "-- %";
            // 
            // lblStepLengthTitle
            // 
            this.lblStepLengthTitle.AutoSize = true;
            this.lblStepLengthTitle.Location = new System.Drawing.Point(12, 156);
            this.lblStepLengthTitle.Name = "lblStepLengthTitle";
            this.lblStepLengthTitle.Size = new System.Drawing.Size(90, 13);
            this.lblStepLengthTitle.TabIndex = 8;
            this.lblStepLengthTitle.Text = "Avg Step Length:";
            // 
            // lblStepLength
            // 
            this.lblStepLength.AutoSize = true;
            this.lblStepLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepLength.Location = new System.Drawing.Point(130, 154);
            this.lblStepLength.Name = "lblStepLength";
            this.lblStepLength.Size = new System.Drawing.Size(16, 15);
            this.lblStepLength.TabIndex = 9;
            this.lblStepLength.Text = "--";
            // 
            // lblTotalStepsTitle
            // 
            this.lblTotalStepsTitle.AutoSize = true;
            this.lblTotalStepsTitle.Location = new System.Drawing.Point(12, 188);
            this.lblTotalStepsTitle.Name = "lblTotalStepsTitle";
            this.lblTotalStepsTitle.Size = new System.Drawing.Size(65, 13);
            this.lblTotalStepsTitle.TabIndex = 10;
            this.lblTotalStepsTitle.Text = "Total Steps:";
            // 
            // lblTotalSteps
            // 
            this.lblTotalSteps.AutoSize = true;
            this.lblTotalSteps.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalSteps.Location = new System.Drawing.Point(130, 186);
            this.lblTotalSteps.Name = "lblTotalSteps";
            this.lblTotalSteps.Size = new System.Drawing.Size(14, 15);
            this.lblTotalSteps.TabIndex = 11;
            this.lblTotalSteps.Text = "0";
            // 
            // grpTrack
            // 
            this.grpTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTrack.Controls.Add(this.lblSelectTrack);
            this.grpTrack.Controls.Add(this.cmbTracks);
            this.grpTrack.Location = new System.Drawing.Point(582, 12);
            this.grpTrack.Name = "grpTrack";
            this.grpTrack.Size = new System.Drawing.Size(260, 57);
            this.grpTrack.TabIndex = 2;
            this.grpTrack.TabStop = false;
            this.grpTrack.Text = "Target Marker";
            // 
            // lblSelectTrack
            // 
            this.lblSelectTrack.AutoSize = true;
            this.lblSelectTrack.Location = new System.Drawing.Point(12, 24);
            this.lblSelectTrack.Name = "lblSelectTrack";
            this.lblSelectTrack.Size = new System.Drawing.Size(37, 13);
            this.lblSelectTrack.TabIndex = 0;
            this.lblSelectTrack.Text = "Track:";
            // 
            // cmbTracks
            // 
            this.cmbTracks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTracks.FormattingEnabled = true;
            this.cmbTracks.Location = new System.Drawing.Point(65, 21);
            this.cmbTracks.Name = "cmbTracks";
            this.cmbTracks.Size = new System.Drawing.Size(185, 21);
            this.cmbTracks.TabIndex = 1;
            this.cmbTracks.SelectedIndexChanged += new System.EventHandler(this.cmbTracks_SelectedIndexChanged);
            // 
            // lvSteps
            // 
            this.lvSteps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSteps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colStep,
            this.colContact,
            this.colToeOff,
            this.colStanceTime,
            this.colSwingTime,
            this.colStanceRatio,
            this.colLength});
            this.lvSteps.FullRowSelect = true;
            this.lvSteps.GridLines = true;
            this.lvSteps.HideSelection = false;
            this.lvSteps.Location = new System.Drawing.Point(12, 380);
            this.lvSteps.Name = "lvSteps";
            this.lvSteps.Size = new System.Drawing.Size(830, 140);
            this.lvSteps.TabIndex = 3;
            this.lvSteps.UseCompatibleStateImageBehavior = false;
            this.lvSteps.View = System.Windows.Forms.View.Details;
            // 
            // colStep
            // 
            this.colStep.Text = "Step #";
            this.colStep.Width = 60;
            // 
            // colContact
            // 
            this.colContact.Text = "Initial Contact (ms)";
            this.colContact.Width = 120;
            // 
            // colToeOff
            // 
            this.colToeOff.Text = "Toe-Off (ms)";
            this.colToeOff.Width = 110;
            // 
            // colStanceTime
            // 
            this.colStanceTime.Text = "Stance (ms)";
            this.colStanceTime.Width = 100;
            // 
            // colSwingTime
            // 
            this.colSwingTime.Text = "Swing (ms)";
            this.colSwingTime.Width = 100;
            // 
            // colStanceRatio
            // 
            this.colStanceRatio.Text = "Stance / Swing %";
            this.colStanceRatio.Width = 130;
            // 
            // colLength
            // 
            this.colLength.Text = "Step Length";
            this.colLength.Width = 100;
            // 
            // btnApplyKeyframes
            // 
            this.btnApplyKeyframes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyKeyframes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyKeyframes.Location = new System.Drawing.Point(12, 530);
            this.btnApplyKeyframes.Name = "btnApplyKeyframes";
            this.btnApplyKeyframes.Size = new System.Drawing.Size(220, 28);
            this.btnApplyKeyframes.TabIndex = 4;
            this.btnApplyKeyframes.Text = "⏱ Add Gait Events to Timeline";
            this.btnApplyKeyframes.UseVisualStyleBackColor = true;
            this.btnApplyKeyframes.Click += new System.EventHandler(this.btnApplyKeyframes_Click);
            // 
            // btnCopyData
            // 
            this.btnCopyData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopyData.Location = new System.Drawing.Point(240, 530);
            this.btnCopyData.Name = "btnCopyData";
            this.btnCopyData.Size = new System.Drawing.Size(120, 28);
            this.btnCopyData.TabIndex = 5;
            this.btnCopyData.Text = "Copy Metrics";
            this.btnCopyData.UseVisualStyleBackColor = true;
            this.btnCopyData.Click += new System.EventHandler(this.btnCopyData_Click);
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportCsv.Location = new System.Drawing.Point(368, 530);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(120, 28);
            this.btnExportCsv.TabIndex = 6;
            this.btnExportCsv.Text = "Export CSV...";
            this.btnExportCsv.UseVisualStyleBackColor = true;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(747, 530);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 28);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormGaitAnalysis
            // 
            this.ClientSize = new System.Drawing.Size(854, 570);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExportCsv);
            this.Controls.Add(this.btnCopyData);
            this.Controls.Add(this.btnApplyKeyframes);
            this.Controls.Add(this.lvSteps);
            this.Controls.Add(this.grpTrack);
            this.Controls.Add(this.grpSummary);
            this.Controls.Add(this.plotGait);
            this.MinimumSize = new System.Drawing.Size(750, 520);
            this.Name = "FormGaitAnalysis";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gait Cycle & Cadence Phase Analysis";
            this.grpSummary.ResumeLayout(false);
            this.grpSummary.PerformLayout();
            this.grpTrack.ResumeLayout(false);
            this.grpTrack.PerformLayout();
            this.ResumeLayout(false);

        }

        private OxyPlot.WindowsForms.PlotView plotGait;
        private System.Windows.Forms.GroupBox grpSummary;
        private System.Windows.Forms.Label lblCadenceTitle;
        private System.Windows.Forms.Label lblCadence;
        private System.Windows.Forms.Label lblStepTimeTitle;
        private System.Windows.Forms.Label lblStepTime;
        private System.Windows.Forms.Label lblStanceTitle;
        private System.Windows.Forms.Label lblStance;
        private System.Windows.Forms.Label lblSwingTitle;
        private System.Windows.Forms.Label lblSwing;
        private System.Windows.Forms.Label lblStepLengthTitle;
        private System.Windows.Forms.Label lblStepLength;
        private System.Windows.Forms.Label lblTotalStepsTitle;
        private System.Windows.Forms.Label lblTotalSteps;
        private System.Windows.Forms.GroupBox grpTrack;
        private System.Windows.Forms.Label lblSelectTrack;
        private System.Windows.Forms.ComboBox cmbTracks;
        private System.Windows.Forms.ListView lvSteps;
        private System.Windows.Forms.ColumnHeader colStep;
        private System.Windows.Forms.ColumnHeader colContact;
        private System.Windows.Forms.ColumnHeader colToeOff;
        private System.Windows.Forms.ColumnHeader colStanceTime;
        private System.Windows.Forms.ColumnHeader colSwingTime;
        private System.Windows.Forms.ColumnHeader colStanceRatio;
        private System.Windows.Forms.ColumnHeader colLength;
        private System.Windows.Forms.Button btnApplyKeyframes;
        private System.Windows.Forms.Button btnCopyData;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
