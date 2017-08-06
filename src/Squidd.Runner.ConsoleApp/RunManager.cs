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
using Squidd.Runner.ConsoleApp.Handlers;

namespace Squidd.Runner.ConsoleApp
{
    internal class RunManager
    {
        readonly List<IHandler> allHandlers;

        public RunManager()
        {
            allHandlers = new List<IHandler>();
        }

        public void AddHandler(IHandler handler)
        {
            allHandlers.Add(handler);
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
            List<IHandler> handlers;
            byte[] allData;
            using (var dataReader = new BinaryReader(client.GetStream(), Encoding.UTF8, true))
            {
                dynamic header = JsonConvert.DeserializeObject<ExpandoObject>(dataReader.ReadString());
                Console.WriteLine($"Received {header.Method} command.");

                handlers = allHandlers.Where(r => r.RespondsToMethod(header.Method)).ToList();

                if (!handlers.Any())
                {
                    using (var responder = new StreamResponder(client.GetStream()))
                    {
                        responder.Log($"Method not supported: {header.Method}.");
                    }
                    client.Close();
                    return;
                }

                allData = dataReader.ReadBytes((int)header.PayloadLength);
            }

            using (var responder = new StreamResponder(client.GetStream()))
            {
                if (handlers.Any(r => r.MakesBusy))
                {
                    if (Global.IsBusy || !Global.SetBusy())
                    {
                        responder.Error("Runner is busy");
                        client.Close();
                        return;
                    }
                }

                foreach (var handler in handlers)
                {
                    handler.Process(allData, responder);
                }
            }

            if (handlers.Any(r => r.MakesBusy))
            {
                Global.ClearBusy();
            }

            client.Close();
        }
    }
}
