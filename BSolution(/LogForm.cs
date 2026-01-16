using BSolution_.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BSolution_
{
    public partial class LogForm : DockContent
    {
        public LogForm()
        {
            InitializeComponent();

            this.FormClosed += LogForm_FormClosed;
            Slogger.LogUpdated += OnLogUpdated;

        }

        private void OnLogUpdated(string logMessage)
        {
            if (listBoxLogs.InvokeRequired)
            {
                listBoxLogs.Invoke(new Action(() => AddLog(logMessage)));
            }

            else
            {
                AddLog(logMessage);
            }
        }

        private void AddLog(string logMessage)
        {
            if (listBoxLogs.Items.Count > 1000)
            {
                listBoxLogs.Items.RemoveAt(0);
            }

            listBoxLogs.Items.Add(logMessage);

            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
        }

        private void LogForm_FormClosed(object sender, EventArgs e)
        {
            {
                Slogger.LogUpdated -= OnLogUpdated;
                this.FormClosed -= LogForm_FormClosed;
            }
        }
    }
}