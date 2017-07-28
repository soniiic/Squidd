using System.Net.Sockets;
using System.Text;

namespace Squidd.Runner.ConsoleApp
{
    internal class SocketCommunicationService
    {
        private readonly Socket socket;

        internal SocketCommunicationService(Socket socket)
        {
            this.socket = socket;
        }

        public void SubscribeToOutputOf(PowerShellRunner powerShellRunner)
        {
            powerShellRunner.OnOutput += OnOutput;
        }

        private void OnOutput(object sender, PowershellOutputEventArgs args)
        {
            socket.Send(Encoding.UTF8.GetBytes($"{args.LineNumber}: {args.Message}\n"));
        }
    }
}