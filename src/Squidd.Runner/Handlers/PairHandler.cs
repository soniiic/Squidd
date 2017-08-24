using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Squidd.Runner.Config;
using Squidd.Shared.Models;

namespace Squidd.Runner.Handlers
{
    public class PairHandler : IHandler
    {
        private IApplicationSettings applicationSettings;

        public PairHandler(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public bool RespondsToMethod(string method)
        {
            return method == "PAIR";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            var model = GetModel(data);
            if (Authenticate(model.PairPassphrase))
            {
                var pairId = Guid.NewGuid();
                applicationSettings.SetPairId(pairId);

                responder.Internal("TOK", pairId.ToString());
            }
            else
            {
                responder.Error("Invalid credentials");
            }
        }

        private bool Authenticate(string pairPassphrase)
        {
            return pairPassphrase == applicationSettings.GetPairPassphrase();
        }

        private static AuthenticationInputModel GetModel(byte[] data)
        {
            var rawModel = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<AuthenticationInputModel>(rawModel);
        }

        public bool RequiresSession => false;

        public bool RequiresAuthentication => false;
    }
}