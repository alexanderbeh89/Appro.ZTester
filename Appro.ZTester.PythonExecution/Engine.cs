using Appro.ZTester.PythonExecution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Appro.ZTester.PythonExecution
{
    public class Engine
    {
        public delegate void PythonOutputReceivedEventHandler(object sender, PythonOutputReceivedEventArgs e);
        public delegate void PythonErrorReceivedEventHandler(object sender, PythonErrorReceivedEventArgs e);
        public delegate void PythonProcessCompletedEventHandler(object sender, PythonProcessCompletedEventArgs e);

        public event PythonOutputReceivedEventHandler PythonOutputReceived;
        public event PythonErrorReceivedEventHandler PythonErrorReceived;
        public event PythonProcessCompletedEventHandler PythonProcessCompleted;

        private CancellationTokenSource _cancellationTokenSource;
        private Process _pythonProcess;

        public async Task ExecutePythonScriptContentAsync(string scriptContent, int timeoutMilliseconds, string arguments = "")
        {
            _cancellationTokenSource = new CancellationTokenSource();
            string tempFilePath = Path.GetTempFileName() + ".py";
            File.WriteAllText(tempFilePath, scriptContent);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{tempFilePath}\" {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            _pythonProcess = new Process();
            _pythonProcess.StartInfo = startInfo;

            _pythonProcess.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    OnPythonOutputReceived(e.Data);
                }
            };

            _pythonProcess.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    OnPythonErrorReceived(e.Data);
                }
            };

            try
            {
                _pythonProcess.Start();
                _pythonProcess.BeginOutputReadLine();
                _pythonProcess.BeginErrorReadLine();

                bool completed = await Task.Run(() => _pythonProcess.WaitForExit(timeoutMilliseconds), _cancellationTokenSource.Token);

                if (!completed)
                {
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        _pythonProcess.Kill(); // Timeout occurred
                        OnPythonProcessCompleted(true); // Timed out.
                    }
                    else
                    {
                        OnPythonProcessCompleted(false); // Cancelled by user.
                    }
                }
                else
                {
                    OnPythonProcessCompleted(false); // Normal completion.
                }

            }
            catch (OperationCanceledException)
            {
                OnPythonProcessCompleted(false); // Process was cancelled.
            }
            finally
            {
                File.Delete(tempFilePath);
                _pythonProcess?.Dispose();
            }
        }

        public async Task ExecutePythonScriptAsync(string filePath, int timeoutMilliseconds, string arguments = "")
        {
            _cancellationTokenSource = new CancellationTokenSource();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{filePath}\" {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            _pythonProcess = new Process();
            _pythonProcess.StartInfo = startInfo;

            _pythonProcess.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    OnPythonOutputReceived(e.Data);
                }
            };

            _pythonProcess.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    OnPythonErrorReceived(e.Data);
                }
            };

            try
            {
                _pythonProcess.Start();
                _pythonProcess.BeginOutputReadLine();
                _pythonProcess.BeginErrorReadLine();

                bool completed = await Task.Run(() => _pythonProcess.WaitForExit(timeoutMilliseconds), _cancellationTokenSource.Token);

                if (!completed)
                {
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        _pythonProcess.Kill(); // Timeout occurred
                        OnPythonProcessCompleted(true); // Timed out.
                    }
                    else
                    {
                        OnPythonProcessCompleted(false); // Cancelled by user.
                    }
                }
                else
                {
                    OnPythonProcessCompleted(false); // Normal completion.
                }

            }
            catch (OperationCanceledException)
            {
                OnPythonProcessCompleted(false); // Process was cancelled.
            }
            finally
            {
                _pythonProcess?.Dispose();
            }
        }
        public void StopPythonProcess()
        {
            _cancellationTokenSource?.Cancel();
            try
            {
                _pythonProcess?.Kill();
            }
            catch (InvalidOperationException)
            {
                // Process already exited or not started.
            }
        }

        protected virtual void OnPythonOutputReceived(string output)
        {
            PythonOutputReceived?.Invoke(this, new PythonOutputReceivedEventArgs(output));
        }

        protected virtual void OnPythonErrorReceived(string error)
        {
            PythonErrorReceived?.Invoke(this, new PythonErrorReceivedEventArgs(error));
        }

        protected virtual void OnPythonProcessCompleted(bool timedOut)
        {
            PythonProcessCompleted?.Invoke(this, new PythonProcessCompletedEventArgs(timedOut));
        }
    }
}