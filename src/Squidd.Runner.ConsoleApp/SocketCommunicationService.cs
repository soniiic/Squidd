using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Squidd.Runner.ConsoleApp
{
    internal class SocketCommunicationService
    {
        private readonly BinaryWriter writer;

        internal SocketCommunicationService(BinaryWriter writer)
        {
            this.writer = writer;
        }

        public void SubscribeToOutputOf(PowerShellRunner powerShellRunner)
        {
            powerShellRunner.OnOutput += OnOutput;
        }

        private void OnOutput(object sender, PowershellOutputEventArgs args)
        {
            writer.Write($"{args.LineNumber}: {args.Message}\n");
        }
    }
}