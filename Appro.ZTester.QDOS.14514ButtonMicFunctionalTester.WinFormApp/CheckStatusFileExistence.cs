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
        public event EventHandler<FileFoundEventArgs> FileFound;
        private Thread _monitorThread;
        private string _folderPath;
        private string _fileName;
        private bool _monitoring;

        public CheckStatusFileExistence(string folderPath, string fileName)
        {
            _folderPath = folderPath;
            _fileName = fileName;

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
