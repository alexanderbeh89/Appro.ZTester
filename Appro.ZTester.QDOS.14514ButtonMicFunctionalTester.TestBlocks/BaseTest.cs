using Appro.ZTester.PythonExecution;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks
{
    public class BaseTest : IRunTest
    {
        public event EventHandler<ServiceResultReceivedEventArgs> ResultReceived;
        public event EventHandler<ServiceResultReceivedEventArgs> PassReceived;
        public event EventHandler<ServiceResultReceivedEventArgs> FailReceived;

        protected StringBuilder _ret;
        protected Engine _engine;
        protected Task _pythonTask;

        protected bool _isDebug;

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

        public BaseTest()
        {
            _ret = new StringBuilder();
            _engine = new Engine();
            _isDebug = false;

            _engine.PythonOutputReceived += (sender, e) =>
            {
                _ret.Append(string.Format("[INFO]{0}\r\n", e.Output));
            };

            _engine.PythonErrorReceived += (sender, e) =>
            {
                _ret.Append(string.Format("[ERROR]{0}\r\n", e.Error));
            };

            _engine.PythonProcessCompleted += (sender, e) =>
            {
                if (_isDebug)
                    _ret.Append(string.Format("[TIMEOUT]{0}\r\n", e.TimedOut));
            };
        }
        public virtual async Task Run(object paramObj)
        {
            try
            {
                if (!(paramObj is JObject jObject))
                {
                    _ret.Append("Fail to Run, paramObj is not valid JSON Object");
                    OnFailReceived(_ret.ToString());
                    return;
                }

                string script = jObject["SCRIPT"]?.ToString() ?? "";
                string scriptArgument = jObject["SCRIPT_ARGUMENT"]?.ToString() ?? "";
                int timeout = (int?)jObject["TIMEOUT"] ?? 1000;
                string passCondition = jObject["PASS_CONDITION"]?.ToString() ?? "";

                _pythonTask = _engine.ExecutePythonScriptAsync(script, timeout, scriptArgument);
                await _pythonTask;

                OnResultReceived(_ret.ToString());

                if (_ret.ToString().Contains(passCondition))
                {
                    OnPassReceived(_ret.ToString());
                }
                else
                {
                    OnFailReceived(_ret.ToString());
                }
            }
            catch (Exception ex)
            {
                _ret.Append($"Failed to run {ex.Message}");
                OnFailReceived(_ret.ToString());
            }
        }
        public virtual void Abort()
        {
            _engine.StopPythonProcess();
        }
    }
}
