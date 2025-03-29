using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Appro.ZTester.PythonExecution;

namespace Appro.ZTester.ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main thread started.");

            Task.Run(async () =>
            {
                Console.WriteLine("Sub-thread: Start running ExampleUsage...");
                await ExampleUsage.RunExample();
                Console.WriteLine("Sub-thread: Finish running ExampleUsage...");
            });

            // Main thread continues to perform other tasks
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($"Main thread: Processing task {i}");
                Thread.Sleep(1000); // Simulate some work
            }

            Console.WriteLine("Main thread: Press any key to exit...");
            Console.ReadKey();
        }
    }
}
