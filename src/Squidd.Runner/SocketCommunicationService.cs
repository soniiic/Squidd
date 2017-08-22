namespace Squidd.Runner
{
    internal class SocketCommunicationService
    {
        private readonly StreamResponder responder;

        internal SocketCommunicationService(StreamResponder responder)
        {
            this.responder = responder;
        }

        public void SubscribeToOutputOf(PowerShellRunner powerShellRunner)
        {
            powerShellRunner.OnOutput += OnOutput;
        }

        private void OnOutput(object sender, PowershellOutputEventArgs args)
        {
            responder.Log($"{args.LineNumber}: {args.Message}");
        }
    }
}