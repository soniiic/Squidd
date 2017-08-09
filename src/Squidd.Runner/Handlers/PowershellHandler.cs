using System.Text;
using Squidd.Runner.Config;

namespace Squidd.Runner.Handlers
{
    public class PowershellHandler : IHandler
    {
        private readonly IApplicationSettings settings;

        public PowershellHandler(IApplicationSettings settings)
        {
            this.settings = settings;
        }

        public bool RespondsToMethod(string method)
        {
            return method == "PS";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            var script = Encoding.UTF8.GetString(data);
            var powerShellRunner = new PowerShellRunner(settings);
            var communicationService = new SocketCommunicationService(responder);
            communicationService.SubscribeToOutputOf(powerShellRunner);
            powerShellRunner.RunScript(script);
            responder.Internal("SC", "Script complete");
        }

        public bool CanRunWhenBusy => false;

        public bool RequiresAuthentication => true;
    }
}