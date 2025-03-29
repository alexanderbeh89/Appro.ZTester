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
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
using System.Threading;


namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.WinFormApp
{
    public partial class QDOS14514ButtonMicFunctionalTesterForm : Form
    {
        private IRunTest _upVolumeService;
        private IRunTest _startTestService;

        private void InitUpVolumeService()
        {
            _upVolumeService = ServiceFactory.GetIRunTestInstance("UpVolume");
            _upVolumeService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"***UpVolume Output***\r\n {e.Output}\r\n")));
            };
            _upVolumeService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"UpVolume Successful\r\n")));
                Thread.Sleep(1000);
                MsgTextBox.Clear();
            };
            _upVolumeService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"UpVolume Fail\r\n")));  // Dont call MsgTextBox.Clear(); to let user troubleshoot the issue 1st before clicking the Reset Button
            };
        }
        private void InitStartTestService()
        {
            _startTestService = ServiceFactory.GetIRunTestInstance("StartTest");
            _startTestService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"***Test Output***\r\n {e.Output}\r\n")));
            };

            _startTestService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"Result: Test Pass\r\n")));
                Thread.Sleep(1000);
                TestOperationTextBox.Clear();
            };
            _startTestService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"Result: Test Fail\r\n")));  // Dont call TestOperationTextBox.Clear(); to let user troubleshoot the issue 1st before clicking the Reset Button
            };
        }

        private void Init()
        {
            InitUpVolumeService();
            InitStartTestService();

            PTTButton.Enabled = true;
            VolumeUpButton.Enabled = true;
            VolumeDownButton.Enabled = true;          
            StartButton.Enabled = true;
            StopButton.Enabled = true;
            ResetButton.Enabled = true;

            MsgTextBox.Clear();
            TestOperationTextBox.Clear();
        }

        private void AbortAllServices()
        {
            if (_startTestService != null)
            {
                _startTestService.Abort();
            }

            if (_upVolumeService != null)
            {
                _upVolumeService.Abort();
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _startTestService.Abort();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;

            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executableLocation) + "\\Scripts\\Python\\";

            JObject paramObj = new JObject
            {
                ["SCRIPT"] = executableDirectory + "StartTest.py",
                ["TIMEOUT"] = 20000,
                ["PASS_CONDITION"] = "TESTPASS",
                ["COMPORT"] = 1,
                ["SCRIPT1"] = executableDirectory + "UpVolume.py",

            };
            
            await _startTestService.Run(paramObj);

            StartButton.Enabled = true;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            AbortAllServices();
            Init();
        }

        private async void VolumeUpButton_Click(object sender, EventArgs e)
        {
            VolumeUpButton.Enabled = false;

            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executableLocation) + "\\Scripts\\Python\\";

            JObject paramObj = new JObject
            {
                ["SCRIPT"] = executableDirectory + "UpVolume.py",
                ["TIMEOUT"] = 10000,
                ["PASS_CONDITION"] = "TESTPASS",
                ["COMPORT"] = 1
            };

            await _upVolumeService.Run(paramObj);

            VolumeUpButton.Enabled = true;
        }

        private void PTTButton_Click(object sender, EventArgs e)
        {
            _upVolumeService.Abort();
        }

        public QDOS14514ButtonMicFunctionalTesterForm()
        {
            InitializeComponent();
            Init();
        }
    }
    
}
