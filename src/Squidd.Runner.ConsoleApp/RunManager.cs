using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Squidd.Runner.ConsoleApp.Responders;

namespace Squidd.Runner.ConsoleApp
{
    internal class RunManager
    {
        readonly List<IResponder> allResponders;

        public RunManager()
        {
            allResponders = new List<IResponder>();
        }

        public void AddResponder(IResponder responder)
        {
            allResponders.Add(responder);
        }

        public async void ListenAsync(IPAddress ipAddress, int port)
        {
            var listener = new TcpListener(ipAddress, port);
            listener.Start();
            while (true)
            {
                var socket = listener.AcceptSocket();
                Console.WriteLine("Connection accepted.");

                await Task.Run(() =>
                {
                    var rawHeader = new byte[4];
                    socket.Receive(rawHeader, 4, SocketFlags.None);
                    var header = Encoding.UTF8.GetString(rawHeader);

                    byte[] allData = null;
                    var responders = allResponders.Where(r => r.RespondsToHeader(header));
                    if (responders.Any(r => r.MakesBusy))
                    {
                        Global.IsBusy = true;
                    }

                    foreach (var responder in responders)
                    {
                        allData = allData ?? ReceiveAll(socket);
                        responder.Process(allData, socket);
                    }

                    if (responders.Any(r => r.MakesBusy))
                    {
                        Global.IsBusy = false;
                    }

                    socket.Close();
                });
            }
        }

        private static byte[] ReceiveAll(Socket socket)
        {
            var buffer = new List<byte>();

            while (Encoding.ASCII.GetString(buffer.Skip(buffer.Count - 9).Take(9).ToArray()) != "SQUIDDEND")
            {
                var currByte = new byte[socket.ReceiveBufferSize];
                var byteCounter = socket.Receive(currByte, socket.ReceiveBufferSize, SocketFlags.None);

                if (byteCounter > 0)
                {
                    buffer.AddRange(currByte.Take(byteCounter));
                }
            }

            return buffer.Take(buffer.Count - 9).ToArray();
        }
    }
}
