using System;
using Squidd.Runner.Config;
using Squidd.Runner.ConsoleApp.Config;
using Topshelf;

namespace Squidd.Runner.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ApplicationSettings();
            IoCContainer.Configure(settings);
            var host = HostFactory.New(x =>
            {
                x.Service(() => new RunManager(settings.Port));
                x.SetServiceName("SquiddRunner");
                x.SetDescription("Squidd Runner");
            });

            host.Run();
        }
    }
}
