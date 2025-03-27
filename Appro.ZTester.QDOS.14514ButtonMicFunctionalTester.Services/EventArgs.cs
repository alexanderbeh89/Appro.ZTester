using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services
{
    public class ServiceResultReceivedEventArgs : EventArgs
    {
        public string Output { get; }

        public ServiceResultReceivedEventArgs(string output)
        {
            Output = output;
        }
    }
}
