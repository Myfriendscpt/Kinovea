namespace Kinovea.ScreenManager
{
    partial class FormConfigureAngleTarget
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.chkEnableTarget = new System.Windows.Forms.CheckBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.nudTargetAngle = new System.Windows.Forms.NumericUpDown();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.nudTolerance = new System.Windows.Forms.NumericUpDown();
            this.grpFeedback = new System.Windows.Forms.GroupBox();
            this.chkAudioAlert = new System.Windows.Forms.CheckBox();
            this.lblInRange = new System.Windows.Forms.Label();
            this.btnInRangeColor = new System.Windows.Forms.Button();
            this.lblOutOfRange = new System.Windows.Forms.Label();
            this.btnOutOfRangeColor = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.grpTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTolerance)).BeginInit();
            this.grpFeedback.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(145, 275);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 26);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(245, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 26);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpTarget
            // 
            this.grpTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTarget.Controls.Add(this.chkEnableTarget);
            this.grpTarget.Controls.Add(this.lblTarget);
            this.grpTarget.Controls.Add(this.nudTargetAngle);
            this.grpTarget.Controls.Add(this.lblTolerance);
            this.grpTarget.Controls.Add(this.nudTolerance);
            this.grpTarget.Location = new System.Drawing.Point(12, 12);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Size = new System.Drawing.Size(323, 120);
            this.grpTarget.TabIndex = 0;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "Angle Target Threshold";
            // 
            // chkEnableTarget
            // 
            this.chkEnableTarget.AutoSize = true;
            this.chkEnableTarget.Location = new System.Drawing.Point(15, 25);
            this.chkEnableTarget.Name = "chkEnableTarget";
            this.chkEnableTarget.Size = new System.Drawing.Size(193, 17);
            this.chkEnableTarget.TabIndex = 1;
            this.chkEnableTarget.Text = "Enable Target Range Assessment";
            this.chkEnableTarget.UseVisualStyleBackColor = true;
            this.chkEnableTarget.CheckedChanged += new System.EventHandler(this.chkEnableTarget_CheckedChanged);
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(15, 56);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(89, 13);
            this.lblTarget.TabIndex = 2;
            this.lblTarget.Text = "Target Angle (°):";
            // 
            // nudTargetAngle
            // 
            this.nudTargetAngle.DecimalPlaces = 1;
            this.nudTargetAngle.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            this.nudTargetAngle.Location = new System.Drawing.Point(180, 54);
            this.nudTargetAngle.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            this.nudTargetAngle.Minimum = new decimal(new int[] { 360, 0, 0, -2147483648 });
            this.nudTargetAngle.Name = "nudTargetAngle";
            this.nudTargetAngle.Size = new System.Drawing.Size(120, 20);
            this.nudTargetAngle.TabIndex = 3;
            this.nudTargetAngle.Value = new decimal(new int[] { 90, 0, 0, 0 });
            // 
            // lblTolerance
            // 
            this.lblTolerance.AutoSize = true;
            this.lblTolerance.Location = new System.Drawing.Point(15, 87);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(87, 13);
            this.lblTolerance.TabIndex = 4;
            this.lblTolerance.Text = "Tolerance (± °):";
            // 
            // nudTolerance
            // 
            this.nudTolerance.DecimalPlaces = 1;
            this.nudTolerance.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudTolerance.Location = new System.Drawing.Point(180, 85);
            this.nudTolerance.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            this.nudTolerance.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            this.nudTolerance.Name = "nudTolerance";
            this.nudTolerance.Size = new System.Drawing.Size(120, 20);
            this.nudTolerance.TabIndex = 5;
            this.nudTolerance.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // grpFeedback
            // 
            this.grpFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFeedback.Controls.Add(this.chkAudioAlert);
            this.grpFeedback.Controls.Add(this.lblInRange);
            this.grpFeedback.Controls.Add(this.btnInRangeColor);
            this.grpFeedback.Controls.Add(this.lblOutOfRange);
            this.grpFeedback.Controls.Add(this.btnOutOfRangeColor);
            this.grpFeedback.Location = new System.Drawing.Point(12, 140);
            this.grpFeedback.Name = "grpFeedback";
            this.grpFeedback.Size = new System.Drawing.Size(323, 120);
            this.grpFeedback.TabIndex = 5;
            this.grpFeedback.TabStop = false;
            this.grpFeedback.Text = "Biofeedback & Indicators";
            // 
            // chkAudioAlert
            // 
            this.chkAudioAlert.AutoSize = true;
            this.chkAudioAlert.Location = new System.Drawing.Point(15, 25);
            this.chkAudioAlert.Name = "chkAudioAlert";
            this.chkAudioAlert.Size = new System.Drawing.Size(200, 17);
            this.chkAudioAlert.TabIndex = 6;
            this.chkAudioAlert.Text = "Auditory Feedback (chime on transition)";
            this.chkAudioAlert.UseVisualStyleBackColor = true;
            // 
            // lblInRange
            // 
            this.lblInRange.AutoSize = true;
            this.lblInRange.Location = new System.Drawing.Point(15, 56);
            this.lblInRange.Name = "lblInRange";
            this.lblInRange.Size = new System.Drawing.Size(78, 13);
            this.lblInRange.TabIndex = 7;
            this.lblInRange.Text = "In-Target Color:";
            // 
            // btnInRangeColor
            // 
            this.btnInRangeColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInRangeColor.Location = new System.Drawing.Point(180, 52);
            this.btnInRangeColor.Name = "btnInRangeColor";
            this.btnInRangeColor.Size = new System.Drawing.Size(120, 23);
            this.btnInRangeColor.TabIndex = 8;
            this.btnInRangeColor.Text = "Sample";
            this.btnInRangeColor.UseVisualStyleBackColor = false;
            this.btnInRangeColor.Click += new System.EventHandler(this.btnInRangeColor_Click);
            // 
            // lblOutOfRange
            // 
            this.lblOutOfRange.AutoSize = true;
            this.lblOutOfRange.Location = new System.Drawing.Point(15, 87);
            this.lblOutOfRange.Name = "lblOutOfRange";
            this.lblOutOfRange.Size = new System.Drawing.Size(96, 13);
            this.lblOutOfRange.TabIndex = 9;
            this.lblOutOfRange.Text = "Off-Target Color:";
            // 
            // btnOutOfRangeColor
            // 
            this.btnOutOfRangeColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutOfRangeColor.Location = new System.Drawing.Point(180, 83);
            this.btnOutOfRangeColor.Name = "btnOutOfRangeColor";
            this.btnOutOfRangeColor.Size = new System.Drawing.Size(120, 23);
            this.btnOutOfRangeColor.TabIndex = 10;
            this.btnOutOfRangeColor.Text = "Sample";
            this.btnOutOfRangeColor.UseVisualStyleBackColor = false;
            this.btnOutOfRangeColor.Click += new System.EventHandler(this.btnOutOfRangeColor_Click);
            // 
            // FormConfigureAngleTarget
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(347, 313);
            this.Controls.Add(this.grpFeedback);
            this.Controls.Add(this.grpTarget);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigureAngleTarget";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Angle Target Range & Biofeedback";
            this.grpTarget.ResumeLayout(false);
            this.grpTarget.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTolerance)).EndInit();
            this.grpFeedback.ResumeLayout(false);
            this.grpFeedback.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpTarget;
        private System.Windows.Forms.CheckBox chkEnableTarget;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.NumericUpDown nudTargetAngle;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.NumericUpDown nudTolerance;
        private System.Windows.Forms.GroupBox grpFeedback;
        private System.Windows.Forms.CheckBox chkAudioAlert;
        private System.Windows.Forms.Label lblInRange;
        private System.Windows.Forms.Button btnInRangeColor;
        private System.Windows.Forms.Label lblOutOfRange;
        private System.Windows.Forms.Button btnOutOfRangeColor;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}

