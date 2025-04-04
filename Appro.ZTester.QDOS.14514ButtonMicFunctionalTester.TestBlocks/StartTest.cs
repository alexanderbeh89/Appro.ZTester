using Appro.ZTester.PythonExecution;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common;
using Newtonsoft.Json;
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
            private BaseTest _currentBaseTest;
            private bool _isTestFlowFail;

            private async Task RunTestFlow(List<TestFlowItem> testFlowItems)
            {
                foreach (TestFlowItem item in testFlowItems)
                {
                    if (_isTestFlowFail)
                        break;


                    JObject paramObj = new JObject
                    {
                        ["TEST_NAME"] = item.TEST_NAME,
                        ["SCRIPT"] = item.SCRIPT,
                        ["SCRIPT_ARGUMENT"] = item.SCRIPT_ARGUMENT,
                        ["TIMEOUT"] = item.TIMEOUT,
                        ["PASS_CONDITION"] = item.PASS_CONDITION,
                        ["COMPORT"] = item.COMPORT,                     
                        ["FAIL_CONDITION"] = item.FAIL_CONDITION
                    };

                    _currentBaseTest = new BaseTest();
                    _currentBaseTest.ResultReceived += (sender, e) =>
                    {
                        OnResultReceived(string.Format("{0}\r\n", e.Output));
                    };
                    _currentBaseTest.PassReceived += (sender, e) =>
                    {
                        if (_isTestFlowFail)
                            OnResultReceived(string.Format("[{0} Test ABORTED]\r\n", item.TEST_NAME));
                        else
                            OnResultReceived(string.Format("[{0} Test PASS]\r\n", item.TEST_NAME));
                    };
                    _currentBaseTest.FailReceived += (sender, e) =>
                    {
                        _isTestFlowFail = true;                       
                        OnResultReceived(string.Format("[{0} Test FAIL]\r\n", item.TEST_NAME));
                        _currentBaseTest.Abort();
                    };

                    OnResultReceived(string.Format("[{0} Test Start]\r\n", item.TEST_NAME));

                    await _currentBaseTest.Run(paramObj);
                }

                if (_isTestFlowFail)
                    OnFailReceived("TEST FLOW FAIL\r\n");

                else
                    OnPassReceived("TEST FLOW PASS\r\n");
            }

            public override async Task Run(object jArrayObject)
            {
                try
                {
                    if (!(jArrayObject is JArray jArray))
                    {
                        _ret.Append("Fail to Run, jArrayObject is not valid JSON Array Object");
                        OnFailReceived(_ret.ToString());
                        return;
                    }

                    _isTestFlowFail = false;

                    List<TestFlowItem> testFlowItems = JsonConvert.DeserializeObject<List<TestFlowItem>>(jArray.ToString());

                    await RunTestFlow(testFlowItems);
                }
                catch (Exception ex)
                {
                    _ret.Append($"Failed to run {ex.Message}");
                    OnFailReceived(_ret.ToString());
                }
            }

            public override void Abort()
            {
                _isTestFlowFail = true;
                if (_currentBaseTest != null)
                    _currentBaseTest.Abort();
            }
        }

        public class TestFlowItem
        {
            public string TEST_NAME { get; set; }
            public string SCRIPT { get; set; }
            public string SCRIPT_ARGUMENT { get; set; }
            public int TIMEOUT { get; set; }
            public string PASS_CONDITION { get; set; }
            public int COMPORT { get; set; }
            public string FAIL_CONDITION { get; set; } // Make it nullable
        }
    }
}
