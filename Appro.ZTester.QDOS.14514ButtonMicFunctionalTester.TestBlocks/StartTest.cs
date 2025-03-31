using Appro.ZTester.PythonExecution;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
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

            private string ParsePythonFileNameWithoutExtension(string rawString)
            {
                if (string.IsNullOrEmpty(rawString))
                {
                    return null; // Or throw an exception, depending on your needs.
                }

                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(rawString);
                    return fileName;
                }
                catch (ArgumentException)
                {
                    return null; // Or throw a more specific exception.
                }
            }
            private async Task RunTestFlow(List<TestFlowItem> testFlowItems)
            {
                foreach (TestFlowItem item in testFlowItems)
                {
                    if (_isTestFlowFail)
                        break;

                    JObject paramObj = new JObject
                    {
                        ["SCRIPT"] = item.SCRIPT,
                        ["TIMEOUT"] = item.TIMEOUT,
                        ["PASS_CONDITION"] = item.PASS_CONDITION,
                        ["COMPORT"] = item.COMPORT
                    };

                    _currentBaseTest = new BaseTest();
                    _currentBaseTest.ResultReceived += (sender, e) =>
                    {
                        OnResultReceived(string.Format("{0}\r\n", e.Output));
                    };
                    _currentBaseTest.PassReceived += (sender, e) =>
                    {
                        if (_isTestFlowFail)
                            OnResultReceived(string.Format("[{0} Test ABORTED]\r\n", ParsePythonFileNameWithoutExtension(item.SCRIPT)));
                        else
                            OnResultReceived(string.Format("[{0} Test PASS]\r\n", ParsePythonFileNameWithoutExtension(item.SCRIPT)));
                    };
                    _currentBaseTest.FailReceived += (sender, e) =>
                    {
                        _isTestFlowFail = true;                       
                        OnResultReceived(string.Format("[{0} Test FAIL]\r\n", ParsePythonFileNameWithoutExtension(item.SCRIPT)));
                        _currentBaseTest.Abort();
                    };

                    OnResultReceived(string.Format("[{0} Test Start]\r\n", ParsePythonFileNameWithoutExtension(item.SCRIPT)));

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
            public string SCRIPT { get; set; }
            public int TIMEOUT { get; set; }
            public string PASS_CONDITION { get; set; }
            public int COMPORT { get; set; }
            public string FAIL_CONDITION { get; set; } // Make it nullable
        }
    }
}
