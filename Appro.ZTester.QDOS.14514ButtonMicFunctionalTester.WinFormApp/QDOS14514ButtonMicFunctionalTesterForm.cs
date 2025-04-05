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
using System.Threading;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.WinFormApp
{
    public partial class QDOS14514ButtonMicFunctionalTesterForm : Form
    {
        private IMonitor _monitorStartActionService;
        private IRunTest _startTestService;

        private void InitMonitorStartActionService()
        {
            _monitorStartActionService = ServiceFactory.GetIMonitorInstance("MonitorStartAction");
            
            _monitorStartActionService.ResultReceived += async (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"{e.Output}\r\n")));
                
                _monitorStartActionService.PauseMonitoring();

                await StartTest();
            };
            
            _monitorStartActionService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"{e.Output}\r\n")));

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
            };

            _startTestService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"[Test Pass from GUI]\r\n")));
                Thread.Sleep(1000);
                Invoke(new Action(() => TestOperationTextBox.Clear()));

                ResetUIAttr();
                ResetServices();
            };

            _startTestService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"[Test Fail from GUI]\r\n")));  // Dont call TestOperationTextBox.Clear(); to let user troubleshoot the issue 1st before clicking the Reset Button

                _monitorStartActionService.PauseMonitoring();
            };
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
            Invoke(new Action(() => MsgTextBox.Clear()));
            Invoke(new Action(() => TestOperationTextBox.Clear()));
        }

        private void ResetServices()
        {
            AbortStartTestService();
            _monitorStartActionService.ResumeMonitoring();
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
            MonitorStartAction();
            ResetUIAttr();
        }

        private async Task StartTest()
        {
            Invoke(new Action(() => StartButton.Enabled = false));
            Invoke(new Action(() => TestOperationTextBox.Clear()));

            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executablePath = Path.GetDirectoryName(executableLocation) + "\\" + "TestFlow.json";
            string jsonString = File.ReadAllText(executablePath);
            JArray jArray = JArray.Parse(jsonString);

            await _startTestService.Run(jArray);

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
        /*
        private async void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                await StartTest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            StartButton.Enabled = true;
        }
        */

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
