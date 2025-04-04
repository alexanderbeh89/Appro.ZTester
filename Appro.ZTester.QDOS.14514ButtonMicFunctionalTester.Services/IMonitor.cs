using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common
{
    public interface IMonitor
    {
        event EventHandler<ServiceResultReceivedEventArgs> ResultReceived;
        event EventHandler<ServiceResultReceivedEventArgs> FailReceived;
        void StartMonitoring(object paramObj);
        void PauseMonitoring();
        void ResumeMonitoring();
        Task StopMonitoringAsync();

    }
}
