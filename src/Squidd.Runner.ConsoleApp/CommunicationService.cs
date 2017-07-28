using System.Net.Sockets;
using System.Text;

namespace Squidd.Runner.ConsoleApp
{
    internal class CommunicationService
    {
        private readonly Socket socket;

        private CommunicationService(PowerShellRunner runner, Socket socket)
        {
            this.socket = socket;
            runner.OnOutput += OnOutput;
        }

        private void OnOutput(object sender, PowershellOutputEventArgs args)
        {
            socket.Send(Encoding.ASCII.GetBytes($"{args.LineNumber}: {args.Message}\n"));
        }

        public static void SubscribeToOutput(PowerShellRunner powerShellRunner, Socket socket)
        {
            new CommunicationService(powerShellRunner, socket);
        }
    }
}