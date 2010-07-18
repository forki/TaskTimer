using System;
using System.Windows.Forms;
using TaskTimer.Properties;

namespace TaskTimer
{
    public partial class ConfigForm : Form
    {
        private readonly NotifyIcon _notifyIcon;

        public ConfigForm()
        {
            InitializeComponent();
        }

        public ConfigForm(NotifyIcon notifyIcon)
        {
            InitializeComponent();
            _notifyIcon = notifyIcon;
        }

        private void ConfigFormLoad(object sender, EventArgs e)
        {
            numericUpDown1.Value = Settings.Default.WorkHours;
            radioButtonHoursDecimal.Checked = Settings.Default.TimeDisplayDecimal;
            radioButtonHoursHrMin.Checked = !Settings.Default.TimeDisplayDecimal;
            checkBoxUseBATTTime.Checked = Settings.Default.UseBATTTime;
            textBoxDefaultTasks.Text = Settings.Default.DefaultTasks;
            checkBoxQuittingMessage.Checked = Settings.Default.QuittingTimeMessage;
            textBoxQuittingMessage.Text = Settings.Default.QuittingTimeMessageText;
            numericUpDownOffsetRight.Maximum = Screen.PrimaryScreen.Bounds.Right - 10;
            numericUpDownOffsetRight.Value = Settings.Default.OffsetRight;
            numericUpDownAutoHideOffset.Value = Settings.Default.AutoHideOffset;
            checkBoxCopyOnTrayMenuClick.Checked = Settings.Default.CopyOnTrayClick;
            checkBoxOpenBATTOnTrayMenuClick.Checked = Settings.Default.OpenBATTOnTrayClick;
            checkBoxSaveOnStop.Checked = Settings.Default.SaveOnStop;
            checkBoxExperimental.Checked = Settings.Default.Experimental;
            checkBoxDebugMode.Checked = Settings.Default.DebugMode;

            CheckBoxQuittingMessageCheckedChanged(null, null);
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            Settings.Default.WorkHours = numericUpDown1.Value;
            Settings.Default.TimeDisplayDecimal = radioButtonHoursDecimal.Checked;
            Settings.Default.UseBATTTime = checkBoxUseBATTTime.Checked;
            if (textBoxDefaultTasks.Text.Equals(String.Empty))
                textBoxDefaultTasks.Text = "Sonstiges";
            Settings.Default.DefaultTasks = textBoxDefaultTasks.Text;
            Settings.Default.QuittingTimeMessage = checkBoxQuittingMessage.Checked;
            Settings.Default.QuittingTimeMessageText = textBoxQuittingMessage.Text;
            Settings.Default.OffsetRight = (int) numericUpDownOffsetRight.Value;
            Settings.Default.AutoHideOffset = (int) numericUpDownAutoHideOffset.Value;
            Settings.Default.CopyOnTrayClick = checkBoxCopyOnTrayMenuClick.Checked;
            Settings.Default.OpenBATTOnTrayClick = checkBoxOpenBATTOnTrayMenuClick.Checked;
            Settings.Default.SaveOnStop = checkBoxSaveOnStop.Checked;
            Settings.Default.Experimental = checkBoxExperimental.Checked;
            Settings.Default.DebugMode = checkBoxDebugMode.Checked;
            Settings.Default.Save();
        }

        private void ConfigFormKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ButtonOkClick(null, null);
                DialogResult = DialogResult.OK;
                Close();
            }
            if (e.KeyChar != 27) return;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckBoxQuittingMessageCheckedChanged(object sender, EventArgs e)
        {
            textBoxQuittingMessage.Enabled = checkBoxQuittingMessage.Checked;
        }

        private void ButtonTestQuittingMessageClick(object sender, EventArgs e)
        {
            var text = textBoxQuittingMessage.Text.Replace("\\", "\r\n");
            _notifyIcon.ShowBalloonTip(15000, "Feierabend", text, ToolTipIcon.Info);
        }

        private void CheckBoxUseBattTimeCheckedChanged(object sender, EventArgs e)
        {
            radioButtonHoursDecimal.Enabled = !checkBoxUseBATTTime.Checked;
            radioButtonHoursHrMin.Enabled = !checkBoxUseBATTTime.Checked;
        }
    }
}