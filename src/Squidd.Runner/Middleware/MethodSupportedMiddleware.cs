﻿using System.Linq;
using Squidd.Runner.Config;
using Squidd.Runner.Handlers;

namespace Squidd.Runner.Middleware
{
    class MethodSupportedMiddleware : IMiddleware
    {
        public int Order => 1;

        public bool Process(dynamic header, StreamResponder responder, bool isAuthenticated)
        {
            var allHandlers = IoCContainer.Container.ResolveAll<IHandler>();

            var handlers = allHandlers.Where(r => r.RespondsToMethod(header.Method) && (!r.RequiresAuthentication || r.RequiresAuthentication == isAuthenticated)).ToList();

            if (!handlers.Any())
            {
                responder.Error($"Method not supported: {header.Method} or you are not paired.");
                return true;
            }

            return false;
        }
    }
}