using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Appro.ZTester.PythonExecution
{
    public class ExampleUsage
    {
        public static async Task RunExample()
        {
            Engine engine = new Engine();

            engine.PythonOutputReceived += (sender, e) =>
            {
                Console.WriteLine($"Python Output: {e.Output}");
            };

            engine.PythonErrorReceived += (sender, e) =>
            {
                Console.WriteLine($"Python Error: {e.Error}");
            };

            engine.PythonProcessCompleted += (sender, e) =>
            {
                Console.WriteLine($"Python Process Completed. Timed Out: {e.TimedOut}");
            };

            string executableLocation = Assembly.GetExecutingAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executableLocation);

            string filePath = executableDirectory + "\\" + "t.py";

            await engine.ExecutePythonScriptAsync(filePath, 8000); // 3 seconds timeout.

            Console.WriteLine("Stopping process...");
            engine.StopPythonProcess();

            /*
            string script = "import time; print('Start'); time.sleep(5); print('End')";

            await engine.ExecutePythonScriptAsync(script, 3000); // 3 seconds timeout.

            Console.WriteLine("Stopping process...");
            engine.StopPythonProcess();

            await engine.ExecutePythonScriptAsync(script, 10000);
            */
        }
    }
}