namespace Squidd.Runner.Middleware
{
    class SessionMiddleware : IMiddleware
    {
        public int Order => 2;

        public bool Process(dynamic header, StreamResponder responder)
        {
            //if (Global.IsBusy)
            //{
            //    var sessionMatch = header.SessionId == Global.SessionId.ToString();
            //    if (!sessionMatch)
            //    {
            //        responder.Error("Session is invalid");
            //        return true;
            //    }
            //}
            return false;
        }
    }
}