using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TaskTimer.Helpers;
using TaskTimer.Model;
using TaskTimer.Properties;

namespace TaskTimer
{
    public partial class MainForm : Form
    {
        private const long TwoSecondsInTicks = TimeSpan.TicksPerSecond*2;
        private readonly int _contextMenuTaskStartIndex;
        private readonly Color _defaultContextMenuStripBackColor;
        private readonly TaskList _taskList = new TaskList();
        private bool _disableAutoHide;
        private string _filename = String.Empty;
        private bool _formIsActive;
        private long _lastInvisibility = DateTime.Now.Ticks;
        private string _lastTaskname = String.Empty;
        private Point _mouseposition;
        private bool _showQuittingTimeMessage = true;
        private DateTime _taskTimerStartTime = DateTime.Now;

        public MainForm()
        {
            InitializeComponent();
            _defaultContextMenuStripBackColor = contextMenuStrip1.Items[0].BackColor;
            var defaultTasks = Settings.Default.DefaultTasks.Split(';');
            foreach (var defaultTask in defaultTasks)
                comboBox1.Items.Add(defaultTask);
            foreach (var item in comboBox1.Items)
            {
                _taskList.AddTask(item.ToString());
                var toolStripItem = new ToolStripMenuItem {Text = item.ToString()};
                toolStripItem.MouseUp += ToolStripItemMouseUp;
                contextMenuStrip1.Items.Add(toolStripItem);
            }
            for (var i = 0; i < contextMenuStrip1.Items.Count; i++)
                if (contextMenuStrip1.Items[i].Text.Equals("Beenden"))
                {
                    _contextMenuTaskStartIndex = i + 2;
                    break;
                }

            SetTrayTaskListColors(defaultTasks[0]);
            TopMostToolStripMenuItem.Checked = Settings.Default.AlwaysOnTop;
            TopMost = TopMostToolStripMenuItem.Checked;
            autoHideToolStripMenuItem.Checked = Settings.Default.AutoHide;

            //set version information
            versionToolStripMenuItem.Text = string.Format("Über {0} v{1}", Application.ProductName,
                                                          Application.ProductVersion);
        }

        private void Form1MouseDown(object sender, MouseEventArgs e)
        {
            _mouseposition = new Point(-e.X, -e.Y);
        }

        private void Form1MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var mousePos = MousePosition;
                mousePos.Offset(_mouseposition.X, _mouseposition.Y);
                Location = mousePos;
            }
        }

        /// <summary>
        ///   Start/Stop button
        /// </summary>
        private void StartStopButtonClick(object sender, EventArgs e)
        {
            _formIsActive = true;

            if (!comboBox1.Text.Equals(_lastTaskname)) // set combobox to current task
                comboBox1.Text = _lastTaskname;

            if (comboBox1.Text.Equals(String.Empty))
                return;

            if (StartStopButton.BackColor == Color.Red)
            {
                StartStopButton.BackColor = Color.Lime;
                if (!_taskList.Contains(comboBox1.Text))
                    _taskList.AddTask(comboBox1.Text);
                _taskList.Start(comboBox1.Text);
                Timer1Tick(null, null);
                timer1.Enabled = true;
            }
            else
            {
                StartStopButton.BackColor = Color.Red;
                _taskList.Stop(comboBox1.Text);
                timer1.Enabled = false;
            }

            startStopToolStripMenuItem.Text = StartStopButton.BackColor == Color.Red ? "Start" : "Stop";

            SetTrayTaskListColors(_lastTaskname);

            if (StartStopButton.BackColor == Color.Red && Settings.Default.SaveOnStop)
                SaveFile(String.Empty);
        }

        private void ComboBox1KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                if (!comboBox1.Text.Equals(String.Empty))
                    AddOrChangeTask(comboBox1.Text);
                else
                    comboBox1.Text = _lastTaskname;
        }

        private void AddOrChangeTask(string newTask)
        {
            comboBox1.BackColor = Color.White;
            if (!_taskList.Contains(newTask))
            {
                _taskList.AddTask(newTask);
                var toolStripItem = new ToolStripMenuItem {Text = newTask};
                toolStripItem.MouseUp += ToolStripItemMouseUp;
                contextMenuStrip1.Items.Add(toolStripItem);
                comboBox1.Items.Add(newTask);
            }
            if (StartStopButton.BackColor == Color.Lime)
            {
                _taskList.Stop(_lastTaskname);
                _taskList.Start(newTask);
            }
            SetTrayTaskListColors(newTask);
            _lastTaskname = newTask;
            SetNotifyIconToolTip(_lastTaskname);

            Timer1Tick(null, null); // Update label
        }

        private void ComboBox1TextChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = comboBox1.Text.Equals(_lastTaskname) ? Color.White : Color.LightPink;
        }

        private void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox1TextChanged(null, null);
        }

        private static double GetBATTFromMinutes(long minutes)
        {
            if (minutes%15 == 0)
                return minutes/15.0/4D;

            return ((minutes/15) + 1)/4D;
        }


        private static void BeendenToolStripMenuItem1Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFile(String.Empty);
        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            if (Settings.Default.Countdown)
                SetLabelCountdown();
            else
                SetLabelCountup();
        }

        private void SetLabelCountup()
        {
            var totalMinutes = _taskList.Tasks.Sum(task => task.DurationInMinutes(DateTime.Today));
            if (Settings.Default.UseBATTTime)
            {
                label1.Text = GetTotalBATTBookingDuration().ToString();
                if (label1.Text.Length == 1)
                    label1.Text += ",0";

                MacheDichHeemeMeiner(totalMinutes);
                SetLabelColor(totalMinutes);
            }
            else
            {
                if (Settings.Default.TimeDisplayDecimal)
                {
                    var duration = (Math.Round(((double) totalMinutes/60), 1)).ToString();
                    label1.Text = duration.Length > 1 ? duration : duration + ",0";
                }
                else
                {
                    var hours = (int) totalMinutes/60;
                    var minutes = (int) totalMinutes - hours*60;
                    label1.Text = hours + ":" + minutes.ToString().PadLeft(2, '0');
                }

                MacheDichHeemeMeiner(totalMinutes);
                SetLabelColor(totalMinutes);
            }

            AppendCurrentTaskDuration();
        }

        private void SetLabelCountdown()
        {
            var totalMinutes = _taskList.Tasks.Sum(task => task.DurationInMinutes(DateTime.Today));
            if (Settings.Default.UseBATTTime)
            {
                label1.Text = GetTotalBATTBookingDuration().ToString();
                if (label1.Text.Length == 1)
                    label1.Text += ",0";

                MacheDichHeemeMeiner(totalMinutes);
                SetLabelColor(totalMinutes);
            }
            else
            {
                if (Settings.Default.TimeDisplayDecimal)
                {
                    var hours = Math.Round(((decimal) totalMinutes/60), 1);
                    var hoursRemaining = Settings.Default.WorkHours - hours;
                    var duration = Math.Abs(hoursRemaining).ToString();
                    label1.Text = duration.Length > 1 ? duration : duration + ",0";
                }
                else
                {
                    var minutesRemaining = Math.Abs((int) (Settings.Default.WorkHours*60) - (int) totalMinutes);
                    var hours = minutesRemaining/60;
                    var minutes = minutesRemaining - hours*60;
                    label1.Text = hours + ":" + minutes.ToString().PadLeft(2, '0');
                }

                MacheDichHeemeMeiner(totalMinutes);
                SetLabelColor(totalMinutes);
            }

            AppendCurrentTaskDuration();
        }

        private void AppendCurrentTaskDuration()
        {
            var appendix = " (";
            if (Settings.Default.UseBATTTime)
            {
                var currentTaskDuration =
                    GetBATTFromMinutes(_taskList.GetByName(_lastTaskname).DurationInMinutes(DateTime.Now)).ToString();
                if (currentTaskDuration.Length == 1)
                    currentTaskDuration += ",0";
                appendix += currentTaskDuration;
            }
            else if (Settings.Default.TimeDisplayDecimal)
            {
                var hours = Math.Round(((decimal) _taskList.GetByName(_lastTaskname).DurationInMinutes(DateTime.Now)/60), 1);
                var duration = hours.ToString();
                appendix += duration.Length > 1 ? duration : duration + ",0";
            }
            else
            {
                var minutes = (int) _taskList.GetByName(_lastTaskname).DurationInMinutes(DateTime.Now);
                var hours = minutes/60;
                minutes = minutes - hours*60;
                appendix += hours + ":" + minutes.ToString().PadLeft(2, '0');
            }
            appendix += ")";
            label1.Text += appendix;
        }

        private void MacheDichHeemeMeiner(long duration)
        {
            if (!Settings.Default.QuittingTimeMessage ||
                !_showQuittingTimeMessage ||
                duration != Settings.Default.WorkHours*60)
                return;
            var text = Settings.Default.QuittingTimeMessageText.Replace("\\", "\r\n");

            notifyIcon1.ShowBalloonTip(15000, "Feierabend", text, ToolTipIcon.Info);
            _showQuittingTimeMessage = false;
            SetLabelColor(Color.Red);
        }

        private void SetLabelColor(Color color)
        {
            label1.ForeColor = color;
        }

        private void SetLabelColor(long totalMinutes)
        {
            if (Settings.Default.TimeDisplayDecimal)
            {
                var hours = Math.Round(((decimal) totalMinutes/60), 1);
                var color = hours > Settings.Default.WorkHours ? Color.Red : Color.Black;
                SetLabelColor(color);
            }
            else
            {
                var color = totalMinutes > Settings.Default.WorkHours*60 ? Color.Red : Color.Black;
                SetLabelColor(color);
            }
        }

        private void ConfigToolStripMenuItemClick(object sender, EventArgs e)
        {
            var form = new ConfigForm(notifyIcon1);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            Timer1Tick(null, null);
            // TODO - position merken und zurücksetzen nur noch auf wunsch
            SetDesktopLocation(
                Screen.PrimaryScreen.Bounds.Right -
                Size.Width -
                Settings.Default.OffsetRight, 0);
            DebugMode();
        }

        private void DebugMode()
        {
            TimerDebugTick(null, null);
            timerDebug.Enabled = Settings.Default.DebugMode;
            listBox1.Visible = Settings.Default.DebugMode;
        }

        private void CountdownToolStripMenuItemClick(object sender, EventArgs e)
        {
            Settings.Default.Countdown = true;
            Settings.Default.Save();
            SetCountDownCountUp();
            Timer1Tick(null, null);
        }

        private void CountupToolStripMenuItemClick(object sender, EventArgs e)
        {
            Settings.Default.Countdown = false;
            Settings.Default.Save();
            SetCountDownCountUp();
            Timer1Tick(null, null);
        }

        private void SetCountDownCountUp()
        {
            if (Settings.Default.Countdown)
            {
                countdownToolStripMenuItem.Checked = true;
                countupToolStripMenuItem.Checked = false;
            }
            else
            {
                countdownToolStripMenuItem.Checked = false;
                countupToolStripMenuItem.Checked = true;
            }
        }

        private void TopMostToolStripMenuItemClick(object sender, EventArgs e)
        {
            TopMostToolStripMenuItem.Checked = !TopMostToolStripMenuItem.Checked;
            TopMost = TopMostToolStripMenuItem.Checked;
            Settings.Default.AlwaysOnTop = TopMostToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private void StartStopToolStripMenuItemClick(object sender, EventArgs e)
        {
            StartStopButtonClick(null, null);
            startStopToolStripMenuItem.Text = StartStopButton.BackColor == Color.Red ? "Start" : "Stop";
        }

        private void SaveFiletoolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveFile(String.Empty);
            if (!_filename.Equals(String.Empty))
                Process.Start(_filename);
        }

        private void SaveFile(string filename)
        {
            var directory = Settings.Default.StorageLocation;
            try
            {
                FileSystemHelper.CreateDirectory(directory);
                _filename = directory + "\\";
                if (!filename.Equals(String.Empty))
                    _filename += filename;
                else
                    _filename +=
                        string.Format("{0}{1}{2}_{3}{4}{5}_{6}_({7}h).csv",
                                      DateTime.Now.Year,
                                      DateTime.Now.Month.ToString().PadLeft(2, '0'),
                                      DateTime.Now.Day.ToString().PadLeft(2, '0'),
                                      DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                                      DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                                      DateTime.Now.Second.ToString().PadLeft(2, '0'),
                                      CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int) (DateTime.Now.DayOfWeek)],
                                      GetTotalBATTBookingDuration());

                using (var writer = new StreamWriter(_filename, false, Encoding.Default))
                {
                    writer.WriteLine(User.Current);
                    writer.WriteLine();
                    writer.WriteLine(DateTime.Now.ToLongDateString());
                    writer.WriteLine("Beginn:;" + _taskTimerStartTime.ToShortTimeString() + " Uhr");
                    writer.WriteLine("Ende:;" + DateTime.Now.ToShortTimeString() + " Uhr");
                    writer.WriteLine();
                    writer.WriteLine("BATT;Minuten;Eintrag");

                    var sortedTasks = _taskList.Tasks.ToDictionary(task => task.Name,
                                                                   task => task.DurationInSeconds());
                    var items = from k in sortedTasks.Keys
                                orderby sortedTasks[k] descending
                                select k;

                    long totalMinutes = 0;
                    double totalBatt = 0;
                    foreach (var item in items)
                    {
                        var task = _taskList.GetByName(item);
                        var minutes = task.DurationInMinutes(DateTime.Today);
                        totalMinutes += minutes;
                        var batt = GetBATTFromMinutes(minutes);
                        totalBatt += batt;
                        if (minutes > 0)
                            writer.WriteLine(batt + ";" + minutes + ";" + task.Name);
                    }
                    writer.WriteLine("Summe");
                    writer.WriteLine(totalBatt + ";" + totalMinutes);
                }
            }
            catch (Exception ex)
            {
                if (_filename.EndsWith("autosave.csv"))
                    notifyIcon1.ShowBalloonTip(15000, "Autosave fehlgeschlagen",
                                               "Ist die Autosave-Datei geöffnet?",
                                               ToolTipIcon.Error);
                else
                    MessageBox.Show(string.Format("{0}\r\n\r\nExistiert das Verzeichnis {1} ?",
                                                  ex.Message, directory),
                                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double GetTotalBATTBookingDuration()
        {
            return _taskList.Tasks
                .Select(task => task.DurationInMinutes(DateTime.Today))
                .Select(GetBATTFromMinutes)
                .Sum();
        }

        private IEnumerable<string> GetTaskListStringsByDuration()
        {
            var sortedTasks = _taskList.Tasks.ToDictionary(task => task.Name, task => task.DurationInSeconds());

            return (from key in sortedTasks.Keys
                    orderby sortedTasks[key] descending
                    select _taskList.GetByName(key)
                    into task
                    let minutes = task.DurationInMinutes(DateTime.Today)
                    let seconds = task.DurationInSeconds(DateTime.Today)
                    let correction = task.TimeDifferenceInMinutes
                    let batt = GetBATTFromMinutes(minutes)
                    select string.Format("{0}\t{1}\t{2}\t{3}\t{4}", batt, minutes, seconds, correction, task.Name))
                .ToList();
        }


        private void ComboBox1KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) // reset combobox value on ESC
            {
                if (!comboBox1.Text.Equals(_lastTaskname))
                    comboBox1.Text = _lastTaskname;
            }

            if (e.KeyValue == 113) // F2
            {
                RenameTask();
            }
            if (e.KeyValue == 114) // F3
            {
                CreateUnspecificTask();
            }
            if (e.KeyValue == 117) // F6
            {
                // open ticket
                OpenTicketToolStripMenuItemClick(null, null);
            }
            if (e.KeyValue == 118) // F7
            {
                // correct booked time
                var task = _taskList.GetByName(comboBox1.Text);
                if (task != null)
                    CorrectBookedTime(task);
            }
            if (e.KeyValue == 119) // F8
            {
                // delete task
                DeleteTask();
            }
            if (e.KeyValue == 122) // F11
            {
                Settings.Default.DebugMode = !Settings.Default.DebugMode;
                Settings.Default.Save();
                DebugMode();
            }
            if (e.KeyValue == 123) // F12
            {
                // show task info in balloon tip
                ShowTaskInfo(comboBox1.Text);
            }
        }

        private void RenameTask()
        {
            // rename current task
            if (!_taskList.Contains(comboBox1.Text))
                return;

            var tmpLastTaskname = comboBox1.Text;
            var setLastTaskname = _lastTaskname.Equals(comboBox1.Text);
            var renameForm = new TaskEntryForm("Task umbenennen", comboBox1.Text);
            if (renameForm.ShowDialog() == DialogResult.OK)
            {
                if (renameForm.Taskname.Equals(String.Empty))
                {
                    MessageBox.Show("Unbenannte Tasks sind nicht erlaubt.", "Task Timer", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                if (_taskList.Contains(renameForm.Taskname))
                {
                    MessageBox.Show("Dieser Task existiert bereits.", "Task Timer", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }
                _taskList.GetByName(comboBox1.Text).Name = renameForm.Taskname;
                if (setLastTaskname)
                    _lastTaskname = renameForm.Taskname;
                for (var i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (!comboBox1.Items[i].Equals(comboBox1.Text))
                        continue;
                    comboBox1.Items[i] = "";
                    comboBox1.Items[i] = renameForm.Taskname;
                }
                comboBox1.Text = renameForm.Taskname;
                for (var i = _contextMenuTaskStartIndex; i < contextMenuStrip1.Items.Count; i++)
                {
                    if (contextMenuStrip1.Items[i].Text.Equals(tmpLastTaskname))
                        contextMenuStrip1.Items[i].Text = renameForm.Taskname;
                }
                SetNotifyIconToolTip(comboBox1.Text);
            }
            ActivateComboBox();
        }

        private void CorrectBookedTime(Task task)
        {
            var correctBookedTimeForm = new CorrectBookedTimeForm(task.DurationInMinutes(DateTime.Now));
            if (correctBookedTimeForm.ShowDialog() != DialogResult.OK || correctBookedTimeForm.TimeCorrection == 0)
                return;

            task.AddMinutes(correctBookedTimeForm.TimeCorrection);
            Timer1Tick(null, null);
        }

        private void RefreshTrayTaskItems()
        {
            // remove all tasks from tray menu
            for (var i = contextMenuStrip1.Items.Count - 1; i > _contextMenuTaskStartIndex - 1; i--)
                contextMenuStrip1.Items.RemoveAt(i);

            // add all tasks to tray menu
            foreach (var toolStripItem in _taskList.Tasks.Select(task => new ToolStripMenuItem {Text = task.Name}))
            {
                // TODO - Superfeature
                /*
                string _battDuration = GetBATTFromMinutes(_task.DurationInMinutes()).ToString();
                if (_battDuration.Length == 1)
                    _battDuration += ",";
                toolStripItem.Text = "[" + _battDuration.PadRight(4,'0') + "] " + _task.Name;
                */
                toolStripItem.MouseUp += ToolStripItemMouseUp;
                contextMenuStrip1.Items.Add(toolStripItem);
            }
            SetTrayTaskListColors(comboBox1.Text.Equals(String.Empty) ? _lastTaskname : comboBox1.Text);
        }

        private void DeleteTask()
        {
            if (!UseExperimentalFeatures())
                return;

            if (comboBox1.Text.Equals(comboBox1.Items[0]))
                return;

            var task = _taskList.GetByName(comboBox1.Text);
            if (task == null)
                return;

            if (MessageBox.Show(string.Format("Diesen Task wirklich löschen?\r\n\r\n{0}", task.Name),
                                "Task Timer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            // TODO: versucht bereits gelöschten task zu stoppen
            if (DeleteTask(task))
            {
                comboBox1.Text = _lastTaskname;
                comboBox1.Items.Remove(task.Name);
                if (comboBox1.Text.Equals(String.Empty))
                    AddOrChangeTask(comboBox1.Items[0].ToString());
                comboBox1.Text = _lastTaskname;
                RefreshTrayTaskItems();
            }
            else
                MessageBox.Show("Fehler bei löschen von Task:\r\n\r\n" + task.Name,
                                "Task Timer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool DeleteTask(Task task)
        {
            if (task.Name.Equals(comboBox1.Items[0]))
                //throw new NotSupportedException(); //TODO
                return false;
            TransferTaskTime(task);
            return _taskList.Remove(task);
        }

        private void TransferTaskTime(Task srcTask)
        {
            var transferTimeForm = new TransferTimeForm(GetTaskListAsStringList(), srcTask.Name,
                                                        srcTask.DurationInMinutes());
            if (transferTimeForm.ShowDialog() == DialogResult.Yes)
                TransferTaskTime(srcTask, _taskList.GetByName(transferTimeForm.DestinationTask));
        }

        private static void TransferTaskTime(Task srcTask, Task dstTask)
        {
            dstTask.AddSeconds(srcTask.DurationInSeconds());
        }

        private List<String> GetTaskListAsStringList()
        {
            return _taskList.Tasks.Select(item => item.Name).ToList();
        }

        private void CreateUnspecificTask()
        {
            if (MessageBox.Show("Neuen Task anlegen?", "Task Timer",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var newTask = string.Format("Unbestimmte Aufgabe (angelegt um {0} Uhr)", DateTime.Now.ToShortTimeString());
            if (_taskList.GetByName(newTask) != null)
            {
                newTask += " " + DateTime.Now.Ticks;
            }
            comboBox1.Text = newTask;
            AddOrChangeTask(newTask);
        }

        private void NotifyIcon1Click(object sender, EventArgs e)
        {
            var mouseEvent = (MouseEventArgs) e;
            if (mouseEvent.Button == MouseButtons.Left)
                ActivateComboBox();
        }

        private void NotifyIcon1DoubleClick(object sender, EventArgs e)
        {
            var mouseEvent = (MouseEventArgs) e;
            if (mouseEvent.Button != MouseButtons.Left)
                return;

            // create new task (TODO: dialog is not active after double click)
            //var newTaskForm = new TaskEntryForm("Neuer Task", "");
            //if (newTaskForm.ShowDialog() == DialogResult.OK)
            //{
            //    AddOrChangeTask(newTaskForm.Name);
            //    comboBox1.Text = newTaskForm.Name;
            //}

            TopMost = true;
            TopMost = TopMostToolStripMenuItem.Checked;

            StartStopButtonClick(null, null); // start/stop
        }

        private void AutoHideToolStripMenuItemClick(object sender, EventArgs e)
        {
            ToggleAutoHide();
        }

        private void GlobalEventProvider1MouseMove(object sender, MouseEventArgs e)
        {
            if (autoHideToolStripMenuItem.Checked &&
                TopMostToolStripMenuItem.Checked &&
                !_disableAutoHide &&
                !_formIsActive)
            {
                var offset = Settings.Default.AutoHideOffset;
                var dblOffset = offset*2;
                var rect = new Rectangle(
                    Location.X - offset,
                    Location.Y - offset,
                    Size.Width + dblOffset,
                    Size.Height + dblOffset);
                Visible = !rect.Contains(e.Location);
            }

            if (!Visible)
                _lastInvisibility = DateTime.Now.Ticks;

            if (DateTime.Now.Ticks - _lastInvisibility < TwoSecondsInTicks &&
                e.X < 10)
                ActivateComboBox();


            if (Visible || e.Delta <= 0)
                return;

            Visible = true;
            ActivateComboBox();
        }

        private void GlobalEventProvider1KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 163:
                case 162:
                    _disableAutoHide = true;
                    break;
                case 91:
                    break;
            }
        }

        private void GlobalEventProvider1KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 163:
                case 162:
                    _disableAutoHide = false;
                    break;
                case 91:
                    break;
            }
        }

        private void MainFormDeactivate(object sender, EventArgs e)
        {
            label1.Focus();
            _formIsActive = false;
        }

        private void MainFormMouseClick(object sender, MouseEventArgs e)
        {
            _formIsActive = true;
        }

        private void ComboBox1MouseClick(object sender, MouseEventArgs e)
        {
            _formIsActive = true;
        }

        private void Label1Click(object sender, EventArgs e)
        {
            _formIsActive = true;
        }

        private void Form1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ToggleAutoHide();
            if (autoHideToolStripMenuItem.Checked)
                Visible = false;
        }

        private void ToggleAutoHide()
        {
            autoHideToolStripMenuItem.Checked = !autoHideToolStripMenuItem.Checked;
            Settings.Default.AutoHide = autoHideToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private static void GlobalEventProvider1KeyPress(object sender, KeyPressEventArgs e)
        {
            // Ctrl+Win+T (T=116?)
            //if (e.KeyChar == 20 && IsCtrlDown && IsWinDown)
            //{
            //    this.BringToFront();
            //    this.Activate();
            //    comboBox1.Focus();
            //}
        }

        private void ActivateComboBox()
        {
            BringToFront();
            Activate();
            comboBox1.Focus();
            _formIsActive = true;
        }

        private static void MainFormActivated(object sender, EventArgs e)
        {
            // bug: shouldn't run when form is only made visible
            //formIsActive = true;
        }

        private void CopyToClipboardToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(comboBox1.Text, true);
            }
            catch (Exception)
            {
                MessageBox.Show("Text konnte nicht in Zwischenablage geschrieben werden.", "Task Timer",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripItemMouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    comboBox1.Text = sender.ToString();
                    AddOrChangeTask(sender.ToString());
                    break;
                case MouseButtons.Right:
                    if (Settings.Default.CopyOnTrayClick)
                    {
                        try
                        {
                            Clipboard.SetDataObject(sender.ToString(), true);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Text konnte nicht in Zwischenablage geschrieben werden.", "Task Timer",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (Settings.Default.OpenBATTOnTrayClick)
                    {
                        // REDUNDANT!
                        // TODO: Experimental
                        if (UseExperimentalFeatures())
                        {
                            var list = GetTicketNumbers(sender.ToString());
                            if (list.Count > 0)
                                OpenTicket(list[0]);
                        }
                    }
                    ShowTaskInfo(sender.ToString());
                    break;
            }
        }

        private void SetTrayTaskListColors(string task)
        {
            var trayItemBackColor = StartStopButton.BackColor == Color.Red ? Color.Red : Color.LightGreen;

            for (var i = _contextMenuTaskStartIndex; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].BackColor =
                    contextMenuStrip1.Items[i].Text.Equals(task) ? trayItemBackColor : _defaultContextMenuStripBackColor;
            }
        }

        private void ShowTaskInfo(string taskname)
        {
            var task = _taskList.GetByName(taskname);
            if (task == null)
                return;
            var minutes = task.DurationInMinutes(DateTime.Today);
            var batt = GetBATTFromMinutes(minutes);
            var correction = task.TimeDifferenceInMinutes;
            notifyIcon1.ShowBalloonTip(15000, task.Name,
                                       string.Format("Erfasste Zeit: {0} Std. ({1} Min.)\r\nKorrekturwert: {2} Min.",
                                                     batt, minutes, correction),
                                       ToolTipIcon.Info);
        }

        private void SetNotifyIconToolTip(string newToolTip)
        {
            var length = newToolTip.Length > 63 ? 63 : newToolTip.Length;
            notifyIcon1.Text = newToolTip.Substring(0, length);
        }

        private void CopyTicketNumberToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO: Experimental
            if (UseExperimentalFeatures())
                CopyTicketNumberToClipboard(comboBox1.Text);
        }

        private static void CopyTicketNumberToClipboard(string taskname)
        {
            var list = GetTicketNumbers(taskname);
            if (!list.Any())
                return;

            try
            {
                Clipboard.SetDataObject(list.First(), true);
            }
            catch (Exception)
            {
                MessageBox.Show("Text konnte nicht in Zwischenablage geschrieben werden.", "Task Timer",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenTicketToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO: Experimental
            if (!UseExperimentalFeatures()) return;
            var list = GetTicketNumbers(comboBox1.Text);
            if (list.Count > 0)
                OpenTicket(list[0]);
        }

        private static List<String> GetTicketNumbers(string taskname)
        {
            var regex = new Regex(@"((I|S)\d{8})|((P)\d{5})");
            return
                regex.Matches(taskname)
                    .Cast<Match>()
                    .Select(match => match.Value)
                    .ToList();
        }

        private static void OpenTicket(string ticketNumber)
        {
            var type = ticketNumber.Substring(0, 1);
            var command = String.Empty;
            if (type.Equals("I"))
                command =
                    string.Format(
                        "navision://client/run?ntauthentication=ja&servername=zeus&database=development&company=Development&target=Form 79201&view=SORTING(Field10)&position=Field10=0({0})&servertype=MSSQL",
                        ticketNumber);
            if (type.Equals("S"))
                command =
                    string.Format(
                        "navision://client/run?ntauthentication=ja&servername=zeus&database=development&company=Development&target=Form 79237&view=SORTING(Field1)&position=Field1=0({0})&servertype=MSSQL",
                        ticketNumber);
            if (type.Equals("P"))
                command =
                    string.Format(
                        "navision://client/run?ntauthentication=ja&servername=zeus&database=development&company=Development&target=Form 79274&view=SORTING(Field1)&position=Field1=0({0})&servertype=MSSQL",
                        ticketNumber);
            if (command != String.Empty)
                try
                {
                    Process.Start(command);
                }
                catch (Exception)
                {
                    MessageBox.Show("Navision konnte nicht gestartet werden.", "Task Timer", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
        }

        private static bool UseExperimentalFeatures()
        {
            if (Settings.Default.Experimental)
                return true;

            MessageBox.Show(
                "Diese Funktion ist noch in Entwicklung und kann fehlerhaft sein oder zu Abstürzen führen!\r\n\n" +
                "Bitte im Konfigurationsdialog freischalten.", "Task Timer", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        private void CreateQuickTaskToolStripMenuItemClick(object sender, EventArgs e)
        {
            CreateUnspecificTask();
        }

        private void TimerDebugTick(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var list = GetTaskListStringsByDuration();
            listBox1.Items.Add("BATT\tMin\tSec\tCorr\tTask");
            foreach (var task in list)
            {
                var x = task.Split('\t');
                if (_taskList.GetByName(x[4]).IsActive)
                    listBox1.Items.Add("*" + task);
                else
                    listBox1.Items.Add(task);
            }
        }

        private void DeleteTaskToolStripMenuItemClick(object sender, EventArgs e)
        {
            DeleteTask();
        }

        private static void TimerAutoCloseTick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute > 58)
                Application.Exit();
        }

        private void CorrectBookedTimeToolStripMenuItemClick(object sender, EventArgs e)
        {
            // correct booked time
            var task = _taskList.GetByName(comboBox1.Text);
            if (task != null)
                CorrectBookedTime(task);
        }

        private static void VersionToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void TimerAutosaveTick(object sender, EventArgs e)
        {
            SaveFile("autosave.csv");
        }

        private void ContextMenuStrip1Opening(object sender, CancelEventArgs e)
        {
            RefreshTrayTaskItems();
        }

        private void DebugModeToolStripMenuItemClick(object sender, EventArgs e)
        {
            Settings.Default.DebugMode = !Settings.Default.DebugMode;
            Settings.Default.Save();
            DebugMode();
        }

        private void RenameTaskToolStripMenuItemClick(object sender, EventArgs e)
        {
            RenameTask();
        }

        private void Form1Load(object sender, EventArgs e)
        {
            DebugMode();

            SetCountDownCountUp();

            // autostart
            comboBox1.Text = Settings.Default.DefaultTasks.Split(';')[0];
            SetNotifyIconToolTip(comboBox1.Text);
            _taskList.Start(comboBox1.Text);
            _lastTaskname = comboBox1.Text;
            StartStopButton.BackColor = Color.Lime;
            comboBox1.BackColor = Color.White;
            SetTrayTaskListColors(comboBox1.Text);
            Timer1Tick(null, null);
            timer1.Enabled = true;

            SetDesktopLocation(
                Screen.PrimaryScreen.Bounds.Right -
                Size.Width -
                Settings.Default.OffsetRight, 0);
        }

        private static void TimerAlwaysOnTopTick(object sender, EventArgs e)
        {
            // TODO
            //if (!this.Focused)
            //    this.TopMost = TopMostToolStripMenuItem.Checked;
        }

        private void PasteFromClipboardToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                try
                {
                    comboBox1.Text = Clipboard.GetText();
                    ActivateComboBox();
                }
                catch (Exception)
                {
                    MessageBox.Show("Text konnte nicht aus Zwischenablage gelesen werden.", "Task Timer",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void OpenFolderToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Settings.Default.StorageLocation);
            }
            catch (Exception)
            {
                MessageBox.Show("Fehler bei Öffnen des Auswertungsordners.", "Task Timer", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}