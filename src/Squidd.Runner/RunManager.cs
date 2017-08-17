using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Squidd.Runner.Config;
using Squidd.Runner.Handlers;
using Squidd.Runner.Helpers;
using Squidd.Runner.Middleware;

namespace Squidd.Runner
{
    public class RunManager
    {
        private readonly List<IMiddleware> middlewares;

        public RunManager()
        {
            middlewares = IoCContainer.Container.ResolveAll<IMiddleware>().OrderBy(m => m.Order).ToList();
        }

        public void Listen(IPAddress ipAddress, int port)
        {
            var listener = new TcpListener(ipAddress, port);
            listener.Start();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                Console.WriteLine("Connection accepted.");
                Task.Run(() => { HandleConnection(client); });
            }
        }

        private void HandleConnection(TcpClient client)
        {
            dynamic header;
            using (var responder = new StreamResponder(client.GetStream()))
            {
                byte[] allData;
                using (var dataReader = new BinaryReader(client.GetStream(), Encoding.UTF8, true))
                {
                    header = JsonConvert.DeserializeObject<ExpandoObject>(dataReader.ReadString());
                    Console.WriteLine($"Received {header.Method} command.");

                    if (middlewares.Any(m => m.Process(header, responder)))
                    {
                        client.Client.Shutdown(SocketShutdown.Send);
                        return;
                    }

                    allData = dataReader.ReadBytes((int)header.PayloadLength);
                }

                var isAuthenticated = Authentication.IsAuthenticated(header);

                var allHandlers = IoCContainer.Container.ResolveAll<IHandler>();

                var handlers = allHandlers
                    .Where(r => r.RespondsToMethod(header.Method) &&
                                (!r.RequiresAuthentication || r.RequiresAuthentication == isAuthenticated))
                    .ToList();

                if (((IDictionary<string, object>)header).ContainsKey("CloseSession") && header.CloseSession == true)
                {
                    handlers = handlers.Union(new[] { allHandlers.Single(h => h.RespondsToMethod("SESC"))}).ToList();
                }

                foreach (var handler in handlers)
                {
                    handler.Process(allData, responder);
                }

                client.Client.Shutdown(SocketShutdown.Send);
            }
        }
    }
}