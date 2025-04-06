using Appro.ZTester.PythonExecution;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks
{
    public class MonitorStartAction : IMonitor
    {
        public event EventHandler<ServiceResultReceivedEventArgs> ResultReceived;
        public event EventHandler<ServiceResultReceivedEventArgs> FailReceived;

        private JObject _jObject;
        private StringBuilder _ret;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _myTask;
        private Engine _engine;
        private bool _isPause;

        private async Task Run()
        {
            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (_isPause)
                        continue;

                    _ret = new StringBuilder();
                    _engine = new Engine(); // Create a new engine for each iteration

                    _engine.PythonOutputReceived += (sender, e) =>
                    {
                        _ret.Append(string.Format("[INFO]{0}\r\n", e.Output));
                    };

                    _engine.PythonErrorReceived += (sender, e) =>
                    {
                        _ret.Append(string.Format("[ERROR]{0}\r\n", e.Error));
                    };

                    string script = _jObject["SCRIPT"]?.ToString() ?? "";
                    string scriptArgument = _jObject["SCRIPT_ARGUMENT"]?.ToString() ?? "";
                    int timeout = (int?)_jObject["TIMEOUT"] ?? 1000;
                    string passCondition = _jObject["PASS_CONDITION"]?.ToString() ?? "";

                    Task pythonTask = _engine.ExecutePythonScriptAsync(_cancellationTokenSource, script, timeout, scriptArgument); // Pass the cancellation token
                    await pythonTask;

                    if (_ret.ToString().Contains(passCondition))
                    {
                        if (_ret.ToString().Contains("MonitorStartAction_ACK_1"))
                        {
                            OnResultReceived(_ret.ToString());
                        }
                        else if (_ret.ToString().Contains("MonitorStartAction_ACK_2"))
                        {
                            OnResultReceived(_ret.ToString());
                        }
                    }
                    else
                    {
                        OnFailReceived(_ret.ToString());
                    }

                    if (!_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        await Task.Delay(1000, _cancellationTokenSource.Token);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                _ret.Append("MonitoStartAction Run Exception: Task was cancelled\r\n");
                OnFailReceived(_ret.ToString());
            }
            catch (Exception ex)
            {
                _ret.Append($"Failed to run {ex.Message}");
                OnFailReceived(_ret.ToString());
            }
        }

        public MonitorStartAction()
        {
            _ret = new StringBuilder();
            _cancellationTokenSource = new CancellationTokenSource();
            _engine = new Engine();
            _isPause = false;
        }

        public void StartMonitoring(object paramObj)
        {
            if (!(paramObj is JObject jObject))
            {
                _ret.Append("Fail to Run, paramObj is not valid JSON Object");
                OnFailReceived(_ret.ToString());
                return;
            }

            _jObject = (JObject)paramObj;

            if (_myTask == null || _myTask.IsCompleted)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _myTask = Task.Run(async () => await Run()); 
            }
        }

        public void PauseMonitoring()
        {
            _isPause = true;
        }

        public void ResumeMonitoring()
        {
            _isPause = false;
        }

        public async Task StopMonitoringAsync()
        {
            _engine.StopPythonProcess();
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                try
                {
                    if (_myTask != null)
                        await _myTask;
                }
                catch (AggregateException ex)
                {
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        if (innerEx is TaskCanceledException)
                        {
                            _ret.Append("StopMonitoringAsync Exception: Task was cancelled\r\n");
                            OnFailReceived(_ret.ToString());
                        }
                        else
                        {
                            _ret.Append($"Task exception: {innerEx.Message}\r\n");
                            OnFailReceived(_ret.ToString());
                        }
                    }
                }
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        public void OnResultReceived(string output)
        {
            ResultReceived?.Invoke(this, new ServiceResultReceivedEventArgs(output));
        }

        public void OnFailReceived(string output)
        {
            FailReceived?.Invoke(this, new ServiceResultReceivedEventArgs(output));
        }
    }
}