using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaskTimer
{
    public partial class TransferTimeForm : Form
    {
        private readonly long _duration;
        private readonly string _srcTask;
        private readonly List<String> _taskList;

        public TransferTimeForm()
        {
            InitializeComponent();
        }

        public TransferTimeForm(List<String> taskList, string srcTask, long duration)
        {
            InitializeComponent();
            _taskList = taskList;
            _srcTask = srcTask;
            _duration = duration;
        }

        public string DestinationTask
        {
            get { return comboBoxTransferTimeTo.Text; }
        }

        private void TransferTimeFormLoad(object sender, EventArgs e)
        {
            label1.Text += "(" + _duration + " Minute";
            label1.Text += _duration == 1 ? "):" : "n):";
            foreach (var task in _taskList.Where(task => !task.Equals(_srcTask)))
                comboBoxTransferTimeTo.Items.Add(task);

            comboBoxTransferTimeTo.Text = comboBoxTransferTimeTo.Items[0].ToString();
            if (comboBoxTransferTimeTo.Items.Count == 1)
                comboBoxTransferTimeTo.Enabled = false;
        }

        private void TransferTimeFormKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
            if (e.KeyChar != 27) return;
            DialogResult = DialogResult.No;
            Close();
        }
    }
}