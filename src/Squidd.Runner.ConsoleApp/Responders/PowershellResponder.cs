using System.Net.Sockets;
using System.Text;
using Squidd.Runner.ConsoleApp.Config;

namespace Squidd.Runner.ConsoleApp.Responders
{
    class PowershellResponder : IResponder
    {
        private readonly IApplicationSettings settings;

        public PowershellResponder(IApplicationSettings settings)
        {
            this.settings = settings;
        }

        public bool RespondsToHeader(string header)
        {
            return header == "PS  ";
        }

        public void Process(byte[] data, Socket socket)
        {
            var script = Encoding.UTF8.GetString(data);
            var powerShellRunner = new PowerShellRunner(settings);
            var communicationService = new SocketCommunicationService(socket);
            communicationService.SubscribeToOutputOf(powerShellRunner);
            powerShellRunner.RunScript(script);
        }

        public bool MakesBusy => true;
    }
}