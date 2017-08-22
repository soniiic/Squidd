using System;
using Squidd.Runner.Config;
using Squidd.Runner.Service.Config;
using Topshelf;

namespace Squidd.Runner.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ApplicationSettings();
            IoCContainer.Configure(settings);

            HostFactory.Run(x =>
            {
                x.Service(() => new RunManager(settings.Port));
                x.SetServiceName("SquiddRunner");
                x.SetDescription("Squidd Runner");
                x.RunAsLocalSystem();
            });
        }
    }
}
