using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Squidd.Shared.Models;

namespace Squidd.Runner.ConsoleApp.Handlers
{
    class PairHandler : IHandler
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
                var token = JToken.FromObject(Global.PairId);
                responder.Internal("TOK", token.ToString());
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