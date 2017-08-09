using System;
using System.Net;
using Squidd.Runner.Config;
using Squidd.Runner.ConsoleApp.Config;

namespace Squidd.Runner.ConsoleApp
{
    class Program
    {
        private const int Port = 13000;

        private const string IpAddress = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing runner...");
            IoCContainer.Configure(new ApplicationSettings());
            var runManager = new RunManager();

            runManager.Listen(IPAddress.Parse(IpAddress), Port);
            Console.WriteLine("Initialised!");
            Console.WriteLine("Press enter to close");
            Console.ReadLine();
        }
    }
}
