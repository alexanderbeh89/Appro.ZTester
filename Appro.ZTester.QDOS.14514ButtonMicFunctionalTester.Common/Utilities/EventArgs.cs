using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common.Utilities
{
    public class ResultReceivedEventArgs : EventArgs
    {
        public string Output { get; }

        public ResultReceivedEventArgs(string output)
        {
            Output = output;
        }
    }
}
