using System;
using System.Windows.Forms;

namespace TaskTimer
{
    public partial class CorrectBookedTimeForm : Form
    {
        public CorrectBookedTimeForm()
        {
            InitializeComponent();
        }

        public CorrectBookedTimeForm(long minutes)
        {
            InitializeComponent();
            labelTaskTime.Text += minutes + " Minute";
            if (minutes == 0 || minutes > 1)
                labelTaskTime.Text += "n";
        }

        public int TimeCorrection
        {
            get { return Convert.ToInt32(numericUpDownCorrection.Value); }
        }

        private void CorrectBookedTimeFormLoad(object sender, EventArgs e)
        {
            numericUpDownCorrection.Select(0, 1);
        }

        private void CorrectBookedTimeFormKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            if (e.KeyChar != 27) return;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}