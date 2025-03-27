using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.PythonExecution
{
    public class PythonOutputReceivedEventArgs : EventArgs
    {
        public string Output { get; }

        public PythonOutputReceivedEventArgs(string output)
        {
            Output = output;
        }
    }

    public class PythonErrorReceivedEventArgs : EventArgs
    {
        public string Error { get; }

        public PythonErrorReceivedEventArgs(string error)
        {
            Error = error;
        }
    }

    public class PythonProcessCompletedEventArgs : EventArgs
    {
        public bool TimedOut { get; }

        public PythonProcessCompletedEventArgs(bool timedOut)
        {
            TimedOut = timedOut;
        }
    }
}
