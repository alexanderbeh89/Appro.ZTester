using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common.Utilities
{
    public class Log
    {
        public event EventHandler<ResultReceivedEventArgs> ResultReceived;

        private StreamWriter _logFileWriter;
        private string _logFileName;
        private string _logDirectory = "Log";

        public void Init()
        {
            if (!Directory.Exists(_logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(_logDirectory);
                    OnResultReceived($"Log directory '{_logDirectory}' created.");
                }
                catch (Exception ex)
                {
                    OnResultReceived($"Error creating log directory '{_logDirectory}': {ex.Message}");
                    _logDirectory = "."; // Fallback to the application's root directory
                }
            }

            // Generate the log file name with timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            _logFileName = Path.Combine(_logDirectory, $"{timestamp}.txt");

            try
            {
                // Create and open the log file for writing
                _logFileWriter = new StreamWriter(_logFileName, true); // 'true' for appending if the file exists
                WriteLine($"--- Test Log Started at {DateTime.Now:yyyy-MM-dd HH:mm:ss} (UTC+8) ---"); // Specify UTC+8 for Malaysia
            }
            catch (Exception ex)
            {
                // Handle potential file creation errors
                OnResultReceived($"Error creating log file '{_logFileName}': {ex.Message}");
                _logFileWriter = null;
            }
        }

        public void WriteLine(string message)
        {
            if (_logFileWriter != null)
            {
                try
                {
                    string logEntry = $"{DateTime.Now:HH:mm:ss.fff}: {message}";
                    _logFileWriter.WriteLine(logEntry);
                    _logFileWriter.Flush(); // Immediately write to the file
                }
                catch (Exception ex)
                {
                    OnResultReceived($"Error writing to log file '{_logFileName}': {ex.Message}");
                }
            }
        }

        public void CloseLog(string testResult)
        {
            if (_logFileWriter != null)
            {
                try
                {
                    WriteLine($"--- Test Log Ended at {DateTime.Now:yyyy-MM-dd HH:mm:ss} (UTC+8) - Result: {testResult} ---");
                    _logFileWriter.Close();
                    _logFileWriter.Dispose();
                    _logFileWriter = null;
                    OnResultReceived($"Log file '{_logFileName}' closed successfully.");

                    // Generate the new log file name with the test result
                    string timestamp = Path.GetFileNameWithoutExtension(_logFileName).Split('.')[0]; // Extract timestamp
                    string newLogFileName = Path.Combine(_logDirectory, $"{timestamp}_{testResult}.txt");

                    // Rename the log file
                    if (File.Exists(_logFileName))
                    {
                        File.Move(_logFileName, newLogFileName);
                        _logFileName = newLogFileName; // Update the internal file name
                        OnResultReceived($"Log file renamed to '{_logFileName}'.");
                    }
                }
                catch (Exception ex)
                {
                    OnResultReceived($"Error closing or renaming log file '{_logFileName}': {ex.Message}");
                }
            }
        }

        private void OnResultReceived(string output)
        {
            ResultReceived?.Invoke(this, new ResultReceivedEventArgs(output));
        }
    }
}



