using System.Linq;
using Squidd.Runner.Config;
using Squidd.Runner.Handlers;

namespace Squidd.Runner.Middleware
{
    class RunnerIsBusyMiddleware : IMiddleware
    {
        public int Order => 3;

        public bool Process(dynamic header, StreamResponder responder, bool isAuthenticated)
        {
            if (header.SessionId == Global.SessionId.ToString())
            {
                return false;
            }

            var allHandlers = IoCContainer.Container.ResolveAll<IHandler>();

            var handlers = allHandlers.Where(r => r.RespondsToMethod(header.Method) && (!r.RequiresAuthentication || r.RequiresAuthentication == isAuthenticated)).ToList();

            if (Global.IsBusy && handlers.All(r => !r.RequiresSession))
            {
                responder.Error("Runner is busy");
                return true;
            }

            return false;
        }
    }
}