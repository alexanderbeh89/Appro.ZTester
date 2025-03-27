using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services
{
    public interface IRunTest
    {
        Task Run();
        void Abort();

        event EventHandler <ServiceResultReceivedEventArgs> ResultReceived;
        event EventHandler<ServiceResultReceivedEventArgs> PassReceived;
        event EventHandler<ServiceResultReceivedEventArgs> FailReceived;
    }
}
