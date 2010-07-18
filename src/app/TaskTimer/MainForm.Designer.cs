using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TaskTimer.Properties;

namespace TaskTimer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StartStopButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TopMostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.countdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.createQuickTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyTicketNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.correctBookedTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFiletoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.startStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerAlwaysOnTop = new System.Windows.Forms.Timer(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timerDebug = new System.Windows.Forms.Timer(this.components);
            this.timerAutoClose = new System.Windows.Forms.Timer(this.components);
            this.globalEventProvider1 = new Gma.UserActivityMonitor.GlobalEventProvider();
            this.timerAutosave = new System.Windows.Forms.Timer(this.components);
            this.renameTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartStopButton
            // 
            this.StartStopButton.BackColor = System.Drawing.Color.Red;
            this.StartStopButton.Location = new System.Drawing.Point(10, 0);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(23, 23);
            this.StartStopButton.TabIndex = 1;
            this.StartStopButton.UseVisualStyleBackColor = false;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButtonClick);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(34, 1);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(376, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ComboBox1MouseClick);
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
            this.comboBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox1KeyPress);
            this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ComboBox1KeyDown);
            this.comboBox1.TextChanged += new System.EventHandler(this.ComboBox1TextChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Task Timer";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.NotifyIcon1Click);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon1DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.toolStripSeparator1,
            this.TopMostToolStripMenuItem,
            this.autoHideToolStripMenuItem,
            this.toolStripSeparator3,
            this.countdownToolStripMenuItem,
            this.countupToolStripMenuItem,
            this.toolStripSeparator5,
            this.renameTaskToolStripMenuItem,
            this.createQuickTaskToolStripMenuItem,
            this.copyToClipboardToolStripMenuItem,
            this.copyTicketNumberToolStripMenuItem,
            this.pasteFromClipboardToolStripMenuItem,
            this.openTicketToolStripMenuItem,
            this.correctBookedTimeToolStripMenuItem,
            this.deleteTaskToolStripMenuItem,
            this.debugModeToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveFiletoolStripMenuItem,
            this.openFolderToolStripMenuItem,
            this.toolStripSeparator6,
            this.startStopToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.versionToolStripMenuItem,
            this.beendenToolStripMenuItem1,
            this.toolStripSeparator4});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(217, 486);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1Opening);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.configToolStripMenuItem.Text = "Konfiguration...";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.ConfigToolStripMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(213, 6);
            // 
            // TopMostToolStripMenuItem
            // 
            this.TopMostToolStripMenuItem.Checked = true;
            this.TopMostToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TopMostToolStripMenuItem.Name = "TopMostToolStripMenuItem";
            this.TopMostToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.TopMostToolStripMenuItem.Text = "Im Vordergrund";
            this.TopMostToolStripMenuItem.Click += new System.EventHandler(this.TopMostToolStripMenuItemClick);
            // 
            // autoHideToolStripMenuItem
            // 
            this.autoHideToolStripMenuItem.Checked = true;
            this.autoHideToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoHideToolStripMenuItem.Name = "autoHideToolStripMenuItem";
            this.autoHideToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.autoHideToolStripMenuItem.Text = "Automatisch ausblenden";
            this.autoHideToolStripMenuItem.Click += new System.EventHandler(this.AutoHideToolStripMenuItemClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(213, 6);
            // 
            // countdownToolStripMenuItem
            // 
            this.countdownToolStripMenuItem.Name = "countdownToolStripMenuItem";
            this.countdownToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.countdownToolStripMenuItem.Text = "Count Down";
            this.countdownToolStripMenuItem.Click += new System.EventHandler(this.CountdownToolStripMenuItemClick);
            // 
            // countupToolStripMenuItem
            // 
            this.countupToolStripMenuItem.Name = "countupToolStripMenuItem";
            this.countupToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.countupToolStripMenuItem.Text = "Count Up";
            this.countupToolStripMenuItem.Click += new System.EventHandler(this.CountupToolStripMenuItemClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(213, 6);
            // 
            // createQuickTaskToolStripMenuItem
            // 
            this.createQuickTaskToolStripMenuItem.Name = "createQuickTaskToolStripMenuItem";
            this.createQuickTaskToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.createQuickTaskToolStripMenuItem.Text = "Quick-Task anlegen (F3)";
            this.createQuickTaskToolStripMenuItem.Click += new System.EventHandler(this.CreateQuickTaskToolStripMenuItemClick);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Kopieren";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.CopyToClipboardToolStripMenuItemClick);
            // 
            // copyTicketNumberToolStripMenuItem
            // 
            this.copyTicketNumberToolStripMenuItem.Name = "copyTicketNumberToolStripMenuItem";
            this.copyTicketNumberToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.copyTicketNumberToolStripMenuItem.Text = "Ticketnummer kopieren";
            this.copyTicketNumberToolStripMenuItem.Click += new System.EventHandler(this.CopyTicketNumberToolStripMenuItemClick);
            // 
            // pasteFromClipboardToolStripMenuItem
            // 
            this.pasteFromClipboardToolStripMenuItem.Name = "pasteFromClipboardToolStripMenuItem";
            this.pasteFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.pasteFromClipboardToolStripMenuItem.Text = "Einfügen";
            this.pasteFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.PasteFromClipboardToolStripMenuItemClick);
            // 
            // openTicketToolStripMenuItem
            // 
            this.openTicketToolStripMenuItem.Name = "openTicketToolStripMenuItem";
            this.openTicketToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.openTicketToolStripMenuItem.Text = "Ticket öffnen (F6)";
            this.openTicketToolStripMenuItem.Click += new System.EventHandler(this.OpenTicketToolStripMenuItemClick);
            // 
            // correctBookedTimeToolStripMenuItem
            // 
            this.correctBookedTimeToolStripMenuItem.Name = "correctBookedTimeToolStripMenuItem";
            this.correctBookedTimeToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.correctBookedTimeToolStripMenuItem.Text = "Zeitkorrektur erfassen (F7)";
            this.correctBookedTimeToolStripMenuItem.Click += new System.EventHandler(this.CorrectBookedTimeToolStripMenuItemClick);
            // 
            // deleteTaskToolStripMenuItem
            // 
            this.deleteTaskToolStripMenuItem.Name = "deleteTaskToolStripMenuItem";
            this.deleteTaskToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.deleteTaskToolStripMenuItem.Text = "Task löschen (F8)";
            this.deleteTaskToolStripMenuItem.Click += new System.EventHandler(this.DeleteTaskToolStripMenuItemClick);
            // 
            // debugModeToolStripMenuItem
            // 
            this.debugModeToolStripMenuItem.Name = "debugModeToolStripMenuItem";
            this.debugModeToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.debugModeToolStripMenuItem.Text = "Debug Modus (F11)";
            this.debugModeToolStripMenuItem.Click += new System.EventHandler(this.DebugModeToolStripMenuItemClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(213, 6);
            // 
            // saveFiletoolStripMenuItem
            // 
            this.saveFiletoolStripMenuItem.Name = "saveFiletoolStripMenuItem";
            this.saveFiletoolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.saveFiletoolStripMenuItem.Text = "Auswertung";
            this.saveFiletoolStripMenuItem.Click += new System.EventHandler(this.SaveFiletoolStripMenuItemClick);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.openFolderToolStripMenuItem.Text = "Auswertungsordner öffnen";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(OpenFolderToolStripMenuItemClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(213, 6);
            // 
            // startStopToolStripMenuItem
            // 
            this.startStopToolStripMenuItem.Name = "startStopToolStripMenuItem";
            this.startStopToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.startStopToolStripMenuItem.Text = "Stop";
            this.startStopToolStripMenuItem.Click += new System.EventHandler(this.StartStopToolStripMenuItemClick);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(213, 6);
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.versionToolStripMenuItem.Text = "Task Timer v...";
            this.versionToolStripMenuItem.Click += new System.EventHandler(VersionToolStripMenuItemClick);
            // 
            // beendenToolStripMenuItem1
            // 
            this.beendenToolStripMenuItem1.Name = "beendenToolStripMenuItem1";
            this.beendenToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.beendenToolStripMenuItem1.Text = "Beenden";
            this.beendenToolStripMenuItem1.Click += new System.EventHandler(BeendenToolStripMenuItem1Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(213, 6);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(416, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            this.label1.Click += new System.EventHandler(this.Label1Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 61000;
            this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
            // 
            // timerAlwaysOnTop
            // 
            this.timerAlwaysOnTop.Enabled = true;
            this.timerAlwaysOnTop.Interval = 1000;
            this.timerAlwaysOnTop.Tick += new System.EventHandler(TimerAlwaysOnTopTick);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 29);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(398, 212);
            this.listBox1.TabIndex = 5;
            this.listBox1.Visible = false;
            // 
            // timerDebug
            // 
            this.timerDebug.Interval = 1000;
            this.timerDebug.Tick += new System.EventHandler(this.TimerDebugTick);
            // 
            // timerAutoClose
            // 
            this.timerAutoClose.Enabled = true;
            this.timerAutoClose.Interval = 50000;
            this.timerAutoClose.Tick += new System.EventHandler(TimerAutoCloseTick);
            // 
            // globalEventProvider1
            // 
            this.globalEventProvider1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GlobalEventProvider1MouseMove);
            this.globalEventProvider1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GlobalEventProvider1KeyPress);
            this.globalEventProvider1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalEventProvider1KeyDown);
            this.globalEventProvider1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GlobalEventProvider1KeyUp);
            // 
            // timerAutosave
            // 
            this.timerAutosave.Enabled = true;
            this.timerAutosave.Interval = 300000;
            this.timerAutosave.Tick += new System.EventHandler(this.TimerAutosaveTick);
            // 
            // renameTaskToolStripMenuItem
            // 
            this.renameTaskToolStripMenuItem.Name = "renameTaskToolStripMenuItem";
            this.renameTaskToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.renameTaskToolStripMenuItem.Text = "Umbenennen (F2)";
            this.renameTaskToolStripMenuItem.Click += new System.EventHandler(this.RenameTaskToolStripMenuItemClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(497, 284);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.StartStopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Task Timer";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.MainFormDeactivate);
            this.Load += new System.EventHandler(this.Form1Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Form1MouseDoubleClick);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseClick);
            this.Activated += new System.EventHandler(MainFormActivated);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1MouseDown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1MouseMove);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startStopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator stopToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem countdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem TopMostToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveFiletoolStripMenuItem;
        private System.Windows.Forms.Timer timerAlwaysOnTop;
        private System.Windows.Forms.ToolStripMenuItem autoHideToolStripMenuItem;
        private Gma.UserActivityMonitor.GlobalEventProvider globalEventProvider1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteFromClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem copyTicketNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTicketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createQuickTaskToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer timerDebug;
        private System.Windows.Forms.ToolStripMenuItem deleteTaskToolStripMenuItem;
        private System.Windows.Forms.Timer timerAutoClose;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem correctBookedTimeToolStripMenuItem;
        private System.Windows.Forms.Timer timerAutosave;
        private System.Windows.Forms.ToolStripMenuItem debugModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameTaskToolStripMenuItem;
    }
}

