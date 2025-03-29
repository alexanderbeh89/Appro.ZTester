using Appro.ZTester.PythonExecution;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks
{
    namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks
    {
        public class StartTest : BaseTest, IRunTest
        {
            public override async Task Run(object paramObj)
            {
                try
                {
                    if (!(paramObj is JObject jObject))
                    {
                        _ret.Append("Fail to Run, paramObj is not valid JSON Object");
                        OnFailReceived(_ret.ToString());
                        return;
                    }

                    string script = (string)jObject["SCRIPT"];
                    int timeout = (int)jObject["TIMEOUT"];
                    string passCondition = (string)jObject["PASS_CONDITION"];
                    int comPort = (int)jObject["COMPORT"];
                    string script1 = (string)jObject["SCRIPT1"];

                    _pythonTask = _engine.ExecutePythonScriptAsync(script, timeout);
                    await _pythonTask;

                    OnResultReceived(_ret.ToString());

                    if (_ret.ToString().Contains(passCondition))
                    {
                        OnPassReceived(_ret.ToString());
                    }
                    else
                    {
                        OnFailReceived(_ret.ToString());
                    }
                }
                catch (Exception ex)
                {
                    _ret.Append($"Failed to run {ex.Message}");
                    OnFailReceived(_ret.ToString());
                }
            }
        }
    }
}
