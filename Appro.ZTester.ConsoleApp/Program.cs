using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appro.ZTester.PythonExecution;

namespace Appro.ZTester.ConsoleApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await ExampleUsage.RunExample();
        }
    }
}
