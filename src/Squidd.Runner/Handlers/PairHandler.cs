using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Squidd.Shared.Models;

namespace Squidd.Runner.Handlers
{
    public class PairHandler : IHandler
    {
        public bool RespondsToMethod(string method)
        {
            return method == "PAIR";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            var model = GetModel(data);
            if (Authenticate(model.Username, model.Password))
            {
                Global.PairId = Guid.NewGuid();

                // todo make this an actual token rather than GUID
                responder.Internal("TOK", Global.PairId.ToString());
            }
            else
            {
                responder.Error("Invalid credentials");
            }
        }

        private static AuthenticationInputModel GetModel(byte[] data)
        {
            var rawModel = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<AuthenticationInputModel>(rawModel);
        }

        private bool Authenticate(string username, string password)
        {
            // todo better authentication
            return username == "admin" && password == "password";
        }

        public bool MakesBusy => false;

        public bool RequiresAuthentication => false;
    }
}