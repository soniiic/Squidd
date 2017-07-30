using System.IO;
using System.Net.Sockets;
using System.Text;
using Squidd.Runner.ConsoleApp.Config;

namespace Squidd.Runner.ConsoleApp.Handlers
{
    class PowershellHandler : IHandler
    {
        private readonly IApplicationSettings settings;

        public PowershellHandler(IApplicationSettings settings)
        {
            this.settings = settings;
        }

        public bool RespondsToHeader(string header)
        {
            return header == "PS";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            var script = Encoding.UTF8.GetString(data);
            var powerShellRunner = new PowerShellRunner(settings);
            var communicationService = new SocketCommunicationService(responder);
            communicationService.SubscribeToOutputOf(powerShellRunner);
            powerShellRunner.RunScript(script);
        }

        public bool MakesBusy => true;
    }
}