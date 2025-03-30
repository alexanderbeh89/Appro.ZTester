using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.WinFormApp
{
    public class CheckStatusFileExistence
    {
        // FileFound Event:
        // Purpose:
        // The event is for notifying other parts of your application (including the WinForms form) that a file has been found.
        // It's a more general notification mechanism.
        // Use Cases:
        // Triggering other actions in the WinForms form or other classes based on the file being found (e.g., enabling buttons, starting other processes, logging).
        // Allowing multiple subscribers to react to the file found event.
        // Allowing the form class to have more control over the handling of the event.
        public event EventHandler<FileFoundEventArgs> FileFound;

        // Delegate (Action<string>, Action, Custom Delegate):
        // Purpose:
        // The delegate is primarily for communicating UI-specific updates directly from the CheckStatusFileExistence class.
        // It's used to pass messages or data related to the file monitoring result that are intended for immediate UI display.
        // Use Cases:
        // Directly updating a text box or other UI elements with specific messages.
        // Encapsulating UI logic within the CheckStatusFileExistence class to keep it decoupled from WinForms.
        private Action<string> _updateUI; // Delegate with string parameter

        private Thread _monitorThread;
        private string _folderPath;
        private string _fileName;
        private bool _monitoring;

        public CheckStatusFileExistence(string folderPath, string fileName, Action<string> updateUI)
        {
            _folderPath = folderPath;
            _fileName = fileName;
            _updateUI = updateUI; // Store the delegate

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        public void StartMonitoring()
        {
            _monitoring = true;
            _monitorThread = new Thread(MonitorFiles);
            _monitorThread.Start();
        }

        private void MonitorFiles()
        {
            string filePath = Path.Combine(_folderPath, _fileName);

            while (_monitoring)
            {
                if (File.Exists(filePath))
                {
                    _updateUI?.Invoke($"File {_fileName} found."); // Invoke delegate with message
                    OnFileFound();
                    //break; // Stop after finding the file
                }

                Thread.Sleep(1000); // Check every 1 second
            }
        }

        public void StopMonitoring()
        {
            _monitoring = false;
            if (_monitorThread != null && _monitorThread.IsAlive)
            {
                _monitorThread.Join(); // Wait for the thread to finish
            }
        }
        protected virtual void OnFileFound()
        {
            FileFound?.Invoke(this, new FileFoundEventArgs(_fileName));
        }
    }
    public class FileFoundEventArgs : EventArgs
    {
        public string FileName { get; }

        public FileFoundEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
