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
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.WinFormApp
{
    public partial class QDOS14514ButtonMicFunctionalTesterForm : Form
    {
        private IRunTest _upVolumeService;
        private IRunTest _startTestService;

        public QDOS14514ButtonMicFunctionalTesterForm()
        {
            InitializeComponent();
            Init();
        }

        private void InitUpVolumeService()
        {
            _upVolumeService = ServiceFactory.GetIRunTestInstance("UpVolume");
            _upVolumeService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"***UpVolumeService Output***\r\n {e.Output}\r\n")));
            };
            _upVolumeService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"***UpVolumeService Pass***\r\n")));
            };
            _upVolumeService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => MsgTextBox.AppendText($"***UpVolumeService Fail***\r\n")));
            };
        }
        private void InitStartTestService()
        {
            _startTestService = ServiceFactory.GetIRunTestInstance("StartTest");
            _startTestService.ResultReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"***StartTestService Output***\r\n {e.Output}\r\n")));
            };

            _startTestService.PassReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"***StartTestService Pass***\r\n")));
            };
            _startTestService.FailReceived += (sender, e) =>
            {
                Invoke(new Action(() => TestOperationTextBox.AppendText($"***StartTestService Fail***\r\n")));
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

        private void StopButton_Click(object sender, EventArgs e)
        {
            _startTestService.Abort();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            await _startTestService.Run();
            StartButton.Enabled = false;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Init();
        }

        private async void VolumeUpButton_Click(object sender, EventArgs e)
        {
            await _upVolumeService.Run();

            VolumeUpButton.Enabled = false;
        }

        private void PTTButton_Click(object sender, EventArgs e)
        {
            _upVolumeService.Abort();
        }
    }
    
}
