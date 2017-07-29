using System.IO;

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