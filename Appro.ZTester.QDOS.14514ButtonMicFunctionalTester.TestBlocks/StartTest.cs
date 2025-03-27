using Appro.ZTester.PythonExecution;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks
{
    namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks
    {
        public class StartTest : IRunTest
        {
            public event EventHandler<ServiceResultReceivedEventArgs> ResultReceived;
            public event EventHandler<ServiceResultReceivedEventArgs> PassReceived;
            public event EventHandler<ServiceResultReceivedEventArgs> FailReceived;

            private const string PYTHON_TEST = "start_test";
            private const string PYTHON_TEST_SCRIPT = PYTHON_TEST + ".py";
            private const string PYTHON_TEST_PASS = "TESTPASS";
            private const int PYTHON_TEST_TIMEOUT = 12000;

            private StringBuilder _ret;
            private Engine _engine;
            private Task _pythonTask;

            public StartTest()
            {
                _ret = new StringBuilder();
                _engine = new Engine();

                _engine.PythonOutputReceived += (sender, e) =>
                {
                    _ret.Append(string.Format("{0} Output:{1}", PYTHON_TEST, e.Output));
                };

                _engine.PythonErrorReceived += (sender, e) =>
                {
                    _ret.Append(string.Format("{0} Error:{1}", PYTHON_TEST, e.Error));
                };

                _engine.PythonProcessCompleted += (sender, e) =>
                {
                    _ret.Append(string.Format("{0} Timeout:{1}", PYTHON_TEST, e.TimedOut));
                };

            }
            public async Task Run()
            {
                string executableLocation = Assembly.GetExecutingAssembly().Location;
                string executableDirectory = Path.GetDirectoryName(executableLocation) + "\\Scripts\\Python\\";

                string script = executableDirectory + PYTHON_TEST_SCRIPT;

                _pythonTask = _engine.ExecutePythonScriptAsync(script, PYTHON_TEST_TIMEOUT);

                await _pythonTask;

                OnResultReceived(_ret.ToString());

                if (_ret.ToString().Contains(PYTHON_TEST_PASS))
                {
                    OnPassReceived(_ret.ToString());
                }
                else
                {
                    OnFailReceived(_ret.ToString());
                }
            }

            public void Abort()
            {
                _engine.StopPythonProcess();
            }
            protected virtual void OnResultReceived(string output)
            {
                ResultReceived?.Invoke(this, new ServiceResultReceivedEventArgs(output));
            }
            protected virtual void OnPassReceived(string output)
            {
                PassReceived?.Invoke(this, new ServiceResultReceivedEventArgs(output));
            }
            protected virtual void OnFailReceived(string output)
            {
                FailReceived?.Invoke(this, new ServiceResultReceivedEventArgs(output));
            }
        }
    }
}
