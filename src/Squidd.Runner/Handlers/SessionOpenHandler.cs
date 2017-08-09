using System;

namespace Squidd.Runner.Handlers
{
    class SessionOpenHandler : IHandler
    {
        public bool RespondsToMethod(string method)
        {
            return method == "SESO";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            var success = Global.StartSession();
            if (success)
            {
                responder.Internal("SEST", Global.SessionId.ToString());
            }
        }

        public bool CanRunWhenBusy => false;

        public bool RequiresAuthentication => true;
    }
}