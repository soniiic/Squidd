using System;
using System.Net;
using Squidd.Runner.ConsoleApp.Config;
using Squidd.Runner.ConsoleApp.Handlers;

namespace Squidd.Runner.ConsoleApp
{
    class Program
    {
        private const int Port = 13000;

        private const string IpAddress = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing runner...");
            var runManager = new RunManager();
            var applicationSettings = new ApplicationSettings();

            runManager.AddHandler(new FileStorageHandler(applicationSettings));
            runManager.AddHandler(new InfoHandler());
            runManager.AddHandler(new PowershellHandler(applicationSettings));
            runManager.Listen(IPAddress.Parse(IpAddress), Port);
            Console.WriteLine("Initialised!");
            Console.WriteLine("Press enter to close");
            Console.ReadLine();
        }
    }
}
