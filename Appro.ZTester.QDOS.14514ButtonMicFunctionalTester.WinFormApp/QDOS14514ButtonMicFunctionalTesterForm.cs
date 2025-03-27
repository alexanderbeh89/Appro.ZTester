using Appro.ZTester.PythonExecution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.WinFormApp
{
    public partial class QDOS14514ButtonMicFunctionalTesterForm : Form
    {
        private Engine _engine;
        private Task _pythonTask;
        public QDOS14514ButtonMicFunctionalTesterForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _engine = new Engine();

            _engine.PythonOutputReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"Output: {e.Output}\r\n")));
            };

            _engine.PythonErrorReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"Error: {e.Error}\r\n")));
            };

            _engine.PythonProcessCompleted += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"Process Completed. Timed Out: {e.TimedOut}\r\n")));
            };

            StartButton.Enabled = true;
            StopButton.Enabled = true;
            ResetButton.Enabled = true;
            MsgTextBox.Clear();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _engine.StopPythonProcess();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executableLocation);

            string script = executableDirectory + "\\" + "t.py";
            int timeout = 15000;

            _pythonTask = _engine.ExecutePythonScriptAsync(script, timeout);

            try
            {
                await _pythonTask;
            }
            catch (Exception ex)
            {
                MsgTextBox.AppendText($"Exception: {ex.Message}\r\n");
            }

            StartButton.Enabled = false;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _engine.StopPythonProcess();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Init();
        }
    }
    
}
