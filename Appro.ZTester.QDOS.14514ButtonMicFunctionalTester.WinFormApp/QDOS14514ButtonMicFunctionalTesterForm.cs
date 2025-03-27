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

        private Engine _engine1;
        private Task _pythonTask1;
        public QDOS14514ButtonMicFunctionalTesterForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _engine1 = new Engine();

            _engine1.PythonOutputReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"Output: {e.Output}\r\n")));
            };

            _engine1.PythonErrorReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"Error: {e.Error}\r\n")));
            };

            _engine1.PythonProcessCompleted += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"Process Completed. Timed Out: {e.TimedOut}\r\n")));
            };

            _engine = new Engine();

            _engine.PythonOutputReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"Output: {e.Output}\r\n")));
            };

            _engine.PythonErrorReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"Error: {e.Error}\r\n")));
            };

            _engine.PythonProcessCompleted += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"Process Completed. Timed Out: {e.TimedOut}\r\n")));
            };

            PTTButton.Enabled = true;
            VolumeUpButton.Enabled = true;
            VolumeDownButton.Enabled = true;          
            StartButton.Enabled = true;
            StopButton.Enabled = true;
            ResetButton.Enabled = true;

            MsgTextBox.Clear();
            TestOperationTextBox.Clear();
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
                TestOperationTextBox.AppendText($"Exception: {ex.Message}\r\n");
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

        private async void VolumeUpButton_Click(object sender, EventArgs e)
        {
            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executableLocation);

            string script = executableDirectory + "\\" + "tt.py";
            int timeout = 20000;

            _pythonTask1 = _engine1.ExecutePythonScriptAsync(script, timeout);

            try
            {
                await _pythonTask1;
            }
            catch (Exception ex)
            {
                MsgTextBox.AppendText($"Exception: {ex.Message}\r\n");
            }

            VolumeUpButton.Enabled = false;
        }

        private void PTTButton_Click(object sender, EventArgs e)
        {
            _engine1.StopPythonProcess();
        }
    }
    
}
