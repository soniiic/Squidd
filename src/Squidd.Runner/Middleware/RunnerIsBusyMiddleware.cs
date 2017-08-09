using System.Linq;
using Squidd.Runner.Config;
using Squidd.Runner.Handlers;
using Squidd.Runner.Helpers;

namespace Squidd.Runner.Middleware
{
    class RunnerIsBusyMiddleware : IMiddleware
    {
        public int Order => 3;

        public bool Process(dynamic header, StreamResponder responder)
        {
            if (header.SessionId == Global.SessionId.ToString())
            {
                return false;
            }

            var isAuthenticated = Authentication.IsAuthenticated(header);

            var allHandlers = IoCContainer.Container.ResolveAll<IHandler>();

            var handlers = allHandlers.Where(r => r.RespondsToMethod(header.Method) && (!r.RequiresAuthentication || r.RequiresAuthentication == isAuthenticated)).ToList();

            if (Global.IsBusy && handlers.All(r => !r.CanRunWhenBusy))
            {
                responder.Error("Runner is busy");
                return true;
            }

            return false;
        }
    }
}