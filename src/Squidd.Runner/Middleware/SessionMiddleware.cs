using System.Linq;
using Squidd.Runner.Config;
using Squidd.Runner.Handlers;
using Squidd.Runner.Helpers;

namespace Squidd.Runner.Middleware
{
    class SessionMiddleware : IMiddleware
    {
        public int Order => 2;

        public bool Process(dynamic header, StreamResponder responder)
        {
            var sessionMatch = header.SessionId == Global.SessionId.ToString();
            if (sessionMatch)
            {
                return false;
            }

            var isAuthenticated = Authentication.IsAuthenticated(header);

            var allHandlers = IoCContainer.Container.ResolveAll<IHandler>();

            var handlers = allHandlers.Where(r => r.RespondsToMethod(header.Method) && (!r.RequiresAuthentication || r.RequiresAuthentication == isAuthenticated)).ToList();

            if (handlers.Any(r => r.RequiresSession))
            {
                responder.Error("Session is invalid");
                return true;
            }

            return false;
        }
    }
}