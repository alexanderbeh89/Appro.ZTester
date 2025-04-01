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
        private IRunTest _startTestService;
        private IRunTest _upVolumeService;

        private void InitStartTestService()
        {
            _startTestService = ServiceFactory.GetIRunTestInstance("StartTest");
            _startTestService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"{e.Output}\r\n")));
            };

            _startTestService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"[Test Pass from GUI]\r\n")));
                Thread.Sleep(1000);
                TestOperationTextBox.Clear();
            };
            _startTestService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"[Test Fail from GUI]\r\n")));  // Dont call TestOperationTextBox.Clear(); to let user troubleshoot the issue 1st before clicking the Reset Button
            };
        }

        private void InitUpVolumeService()
        {
            _upVolumeService = ServiceFactory.GetIRunTestInstance("UpVolume");
            _upVolumeService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"{e.Output}\r\n")));
            };
            _upVolumeService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"[UpVolume Pass from GUI]\r\n")));
                Thread.Sleep(1000);
                MsgTextBox.Clear();
            };
            _upVolumeService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"[UpVolume Fail from GUI]\r\n")));  // Dont call MsgTextBox.Clear(); to let user troubleshoot the issue 1st before clicking the Reset Button
            };
        }

        private void Init()
        {
            InitStartTestService();
            InitUpVolumeService();

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
            try
            {
                _startTestService.Abort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            TestOperationTextBox.Clear();

            try
            {
                string executableLocation = Assembly.GetExecutingAssembly().Location;
                string executablePath = Path.GetDirectoryName(executableLocation) + "\\" + "TestFlow.json";
                string jsonString = File.ReadAllText(executablePath);
                JArray jArray = JArray.Parse(jsonString);

                await _startTestService.Run(jArray);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            StartButton.Enabled = true;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            try
            {
                AbortAllServices();
                Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       

        private async void VolumeUpButton_Click(object sender, EventArgs e)
        {
            VolumeUpButton.Enabled = false;
            MsgTextBox.Clear();

            try
            {
                string executableLocation = Assembly.GetExecutingAssembly().Location;
                string executablePath = Path.GetDirectoryName(executableLocation) + "\\" + "UpVolume.json";
                string jsonString = File.ReadAllText(executablePath);
                JObject jObject = JObject.Parse(jsonString);

                await _upVolumeService.Run(jObject);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            VolumeUpButton.Enabled = true;
        }

        private void PTTButton_Click(object sender, EventArgs e)
        {
            PTTButton.Enabled = false;

            try
            {
                _upVolumeService.Abort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            PTTButton.Enabled = true;
        }

        public QDOS14514ButtonMicFunctionalTesterForm()
        {
            InitializeComponent();
            Init();
        }

        private CheckStatusFileExistence _fileMonitor;
        
        private void FileMonitor_FileFound(object sender, FileFoundEventArgs e)
        {
            Invoke(new Action(() => MsgTextBox.AppendText($"{e.FileName}\r\n")));
            Thread.Sleep(1000);
            Invoke(new Action(() => MsgTextBox.Clear()));
        }
        
        private void QDOS14514ButtonMicFunctionalTesterForm_Load(object sender, EventArgs e)
        {
            try
            {
                string folderPath = Path.Combine(Environment.CurrentDirectory, "Observation");
                _fileMonitor = new CheckStatusFileExistence(folderPath, "abc.txt");
                _fileMonitor.FileFound += FileMonitor_FileFound;

                _fileMonitor.StartMonitoring();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void QDOS14514ButtonMicFunctionalTesterForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            try
            {
                _fileMonitor.StopMonitoring();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}
