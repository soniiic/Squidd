using System.Net.Sockets;
using System.Text;

namespace Squidd.Runner.ConsoleApp.Responders
{
    class PowershellResponder : IResponder
    {
        public bool RespondsToHeader(string header)
        {
            return header == "PS  ";
        }

        public void Process(byte[] data, Socket socket)
        {
            var script = Encoding.ASCII.GetString(data);
            var powerShellRunner = new PowerShellRunner();
            var communicationService = new SocketCommunicationService(socket);
            communicationService.SubscribeToOutputOf(powerShellRunner);
            powerShellRunner.RunScript(script);
        }
    }
}