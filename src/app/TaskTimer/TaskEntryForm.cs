using System.Windows.Forms;

namespace TaskTimer
{
    public partial class TaskEntryForm : Form
    {
        public TaskEntryForm()
        {
            InitializeComponent();
        }

        public TaskEntryForm(string caption, string taskname)
        {
            InitializeComponent();
            SetCaptions(caption, taskname);
        }

        private void SetCaptions(string caption, string taskname)
        {
            Text = caption;
            textBoxTaskName.Text = taskname;
        }

        public string Taskname
        {
            get { return textBoxTaskName.Text; }
        }

        private void TaskEntryFormKeyPress(object sender, KeyPressEventArgs e)
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