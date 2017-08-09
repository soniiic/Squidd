namespace Squidd.Runner.Handlers
{
    class SessionCloseHandler : IHandler
    {
        public bool RespondsToMethod(string method)
        {
            return method == "SESC";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            Global.EndSession();
        }

        public bool CanRunWhenBusy => true;

        public bool RequiresAuthentication => true;
    }
}