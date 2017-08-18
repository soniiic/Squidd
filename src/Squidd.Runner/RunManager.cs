using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Squidd.Runner.Config;
using Squidd.Runner.Handlers;
using Squidd.Runner.Helpers;
using Squidd.Runner.Middleware;
using Topshelf;

namespace Squidd.Runner
{
    public class RunManager : ServiceControl
    {
        private readonly List<IMiddleware> middlewares;
        private readonly IPEndPoint ipAddress;
        private readonly CancellationTokenSource cancellationTokenSource;

        private TcpListener tcpListener;
        private Task mainTask;

        public RunManager(int port)
        {
            ipAddress = new IPEndPoint(IPAddress.Any, port);
            middlewares = IoCContainer.Container.ResolveAll<IMiddleware>().OrderBy(m => m.Order).ToList();
            cancellationTokenSource = new CancellationTokenSource();
        }

        public bool Start(HostControl hostControl)
        {
            var cancellationToken = cancellationTokenSource.Token;
            mainTask = Task.Run(() =>
            {
                tcpListener = new TcpListener(ipAddress);
                tcpListener.Start();
                while (true)
                {
                    while (!tcpListener.Pending())
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                        Thread.Sleep(10);
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var client = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Connection accepted.");
                    Task.Run(() => { HandleConnection(client); });
                }

                tcpListener.Stop();
            }, cancellationToken);

            return true;
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
                    handlers = handlers.Union(new[] { allHandlers.Single(h => h.RespondsToMethod("SESC")) }).ToList();
                }

                foreach (var handler in handlers)
                {
                    handler.Process(allData, responder);
                }

                client.Client.Shutdown(SocketShutdown.Send);
            }
        }

        public bool Stop(HostControl hostControl)
        {
            this.cancellationTokenSource.Cancel();
            this.mainTask.Wait();
            return true;
        }
    }
}