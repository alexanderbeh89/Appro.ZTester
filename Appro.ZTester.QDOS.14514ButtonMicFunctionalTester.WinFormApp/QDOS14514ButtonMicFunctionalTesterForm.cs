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
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common.Utilities;
using System.Threading;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.WinFormApp
{
    public partial class QDOS14514ButtonMicFunctionalTesterForm : Form
    {
        private IMonitor _monitorStartActionService;
        private IRunTest _startTestService;
        private IRunTest _completeTestService;
        private Log _log;

        private void InitMonitorStartActionService()
        {
            _monitorStartActionService = ServiceFactory.GetIMonitorInstance("MonitorStartAction");
            
            _monitorStartActionService.ResultReceived += async (sender, e) =>
            {
                Invoke(new Action(() => labelInterfaceBoardState.Text = "Start"));
                Thread.Sleep(1000);
                Invoke(new Action(() => labelInterfaceBoardState.Text = "OK"));
                
                _monitorStartActionService.PauseMonitoring();

                await StartTest();
            };
            
            _monitorStartActionService.FailReceived += (sender, e) =>
            {
                MessageBox.Show(e.Output);
                Invoke(new Action(() => labelInterfaceBoardState.Text = "FAIL"));

                _monitorStartActionService.PauseMonitoring();
            };

            _monitorStartActionService.PauseMonitoring();
        }

        private void InitStartTestService()
        {
            _startTestService = ServiceFactory.GetIRunTestInstance("StartTest");
            
            _startTestService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"{e.Output}\r\n")));
                _log.WriteLine($"{e.Output}\r\n");
            };

            _startTestService.PassReceived += async (sender, e) =>
            {
                InitCompleteTestService();
                await CompleteTest();
            };

            _startTestService.FailReceived += async (sender, e) =>
            {
                InitCompleteTestService();
                await CompleteTest();
            };
        }

        private void InitCompleteTestService()
        {
            _completeTestService = ServiceFactory.GetIRunTestInstance("CompleteTest");

            _completeTestService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"{e.Output}\r\n")));
                _log.WriteLine($"{e.Output}\r\n");
            };

            _completeTestService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"[Test Pass from GUI]\r\n")));
                Invoke(new Action(() => labelTestStatus.Text = "PASS"));
                Invoke(new Action(() => labelTestStatus.BackColor = Color.LimeGreen));
                _log.WriteLine($"[Test Pass from GUI]\r\n");
                _log.CloseLog("PASS");

                Thread.Sleep(1000);

                ResetUIAttr();
                ResetServices();
            };

            _completeTestService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"[Test Fail from GUI]\r\n")));  // Dont call TestOperationTextBox.Clear(); to let user troubleshoot the issue 1st before clicking the Reset Button
                Invoke(new Action(() => labelTestStatus.Text = "FAIL"));
                Invoke(new Action(() => labelTestStatus.BackColor = Color.Red));
                _log.WriteLine($"[Test Fail from GUI]\r\n");
                _log.CloseLog("FAIL");
            };
        }

        private void InitLog()
        {
            _log = new Log();
            _log.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"{e.Output}\r\n")));
            };

            _log.Init();

            return;
        }

        private void MonitorStartAction()
        {
            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executablePath = Path.GetDirectoryName(executableLocation) + "\\" + "MonitorStartAction.json";
            string jsonString = File.ReadAllText(executablePath);
            JObject jObject = JObject.Parse(jsonString);

            _monitorStartActionService.StartMonitoring(jObject);
        }

        private void ResetUIAttr()
        {
            Invoke(new Action(() => StartButton.Enabled = true));
            Invoke(new Action(() => StopButton.Enabled = true));
            Invoke(new Action(() => ResetButton.Enabled = true));
            Invoke(new Action(() => labelInterfaceBoardState.Text = "OK"));
            Invoke(new Action(() => TestOperationTextBox.Clear()));

            return;
        }

        private void ResetServices()
        {
            AbortStartTestService();
            _monitorStartActionService.ResumeMonitoring();
            
            return;
        }

        private void ResetTestStatus()
        {
            Invoke(new Action(() => labelTestStatus.Text = "IDLE"));
            Invoke(new Action(() => labelTestStatus.BackColor = Color.LightCyan));

            return;
        }

        private void AbortStartTestService()
        {
            if (_startTestService != null)
            {
                _startTestService.Abort();
            }
        }

        private void Init()
        {
            InitMonitorStartActionService();
            InitStartTestService();
            InitCompleteTestService();
            MonitorStartAction();
            ResetUIAttr();
            ResetTestStatus();
        }

        private async Task StartTest()
        {
            Invoke(new Action(() => StartButton.Enabled = false));
            Invoke(new Action(() => TestOperationTextBox.Clear()));
            Invoke(new Action(() => labelTestStatus.Text = "TESTING"));
            Invoke(new Action(() => labelTestStatus.BackColor = Color.Yellow));

            InitLog();

            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executablePath = Path.GetDirectoryName(executableLocation) + "\\" + "TestFlow.json";
            string jsonString = File.ReadAllText(executablePath);
            JArray jArray = JArray.Parse(jsonString);

            await _startTestService.Run(jArray);
        }

        private async Task CompleteTest()
        {
            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executablePath = Path.GetDirectoryName(executableLocation) + "\\" + "CompleteTest.json";
            string jsonString = File.ReadAllText(executablePath);
            JObject jobject = JObject.Parse(jsonString);

            await _completeTestService.Run(jobject);

            Invoke(new Action(() => StartButton.Enabled = true));
        }

        private void QDOS14514ButtonMicFunctionalTesterForm_Load(object sender, EventArgs e)
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }           
        }

        private async void QDOS14514ButtonMicFunctionalTesterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                await _monitorStartActionService.StopMonitoringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                _monitorStartActionService.ResumeMonitoring();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            StartButton.Enabled = true;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                AbortStartTestService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            try
            {
                ResetTestStatus();
                ResetUIAttr();
                ResetServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       

        public QDOS14514ButtonMicFunctionalTesterForm()
        {
            InitializeComponent();
        }
    }
    
}
