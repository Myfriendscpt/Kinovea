using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kinovea.ScreenManager
{
    public partial class FormConfigureAngleTarget : Form
    {
        private DrawingAngle drawingAngle;

        public FormConfigureAngleTarget(DrawingAngle drawingAngle)
        {
            this.drawingAngle = drawingAngle;
            InitializeComponent();
            LoadValues();
        }

        private void LoadValues()
        {
            chkEnableTarget.Checked = drawingAngle.TargetRangeEnabled;
            nudTargetAngle.Value = Math.Max(nudTargetAngle.Minimum, Math.Min(nudTargetAngle.Maximum, (decimal)drawingAngle.TargetAngle));
            nudTolerance.Value = Math.Max(nudTolerance.Minimum, Math.Min(nudTolerance.Maximum, (decimal)drawingAngle.TargetTolerance));
            chkAudioAlert.Checked = drawingAngle.TargetAudioAlert;

            UpdateColorButton(btnInRangeColor, drawingAngle.TargetInRangeColor);
            UpdateColorButton(btnOutOfRangeColor, drawingAngle.TargetOutOfRangeColor);

            UpdateEnabledControls();
        }

        private void UpdateColorButton(Button btn, Color color)
        {
            btn.BackColor = color;
            // Contrast text
            int brightness = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
            btn.ForeColor = brightness > 128 ? Color.Black : Color.White;
        }

        private void UpdateEnabledControls()
        {
            bool enabled = chkEnableTarget.Checked;
            lblTarget.Enabled = enabled;
            nudTargetAngle.Enabled = enabled;
            lblTolerance.Enabled = enabled;
            nudTolerance.Enabled = enabled;
            chkAudioAlert.Enabled = enabled;
            lblInRange.Enabled = enabled;
            btnInRangeColor.Enabled = enabled;
            lblOutOfRange.Enabled = enabled;
            btnOutOfRangeColor.Enabled = enabled;
        }

        private void chkEnableTarget_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabledControls();
        }

        private void btnInRangeColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btnInRangeColor.BackColor;
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                UpdateColorButton(btnInRangeColor, colorDialog.Color);
            }
        }

        private void btnOutOfRangeColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btnOutOfRangeColor.BackColor;
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                UpdateColorButton(btnOutOfRangeColor, colorDialog.Color);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            drawingAngle.TargetRangeEnabled = chkEnableTarget.Checked;
            drawingAngle.TargetAngle = (float)nudTargetAngle.Value;
            drawingAngle.TargetTolerance = (float)nudTolerance.Value;
            drawingAngle.TargetAudioAlert = chkAudioAlert.Checked;
            drawingAngle.TargetInRangeColor = btnInRangeColor.BackColor;
            drawingAngle.TargetOutOfRangeColor = btnOutOfRangeColor.BackColor;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

