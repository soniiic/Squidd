﻿using System;
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
        readonly List<IResponder> responders;

        public RunManager()
        {
            responders = new List<IResponder>();
        }

        public void AddResponder(IResponder responder)
        {
            responders.Add(responder);
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
                    var header = Encoding.ASCII.GetString(rawHeader);

                    byte[] allData = null;
                    foreach (var responder in responders.Where(r => r.RespondsToHeader(header)))
                    {
                        allData = allData ?? ReceiveAll(socket);
                        responder.Process(allData, socket);
                    }
                    
                    socket.Close();
                });
            }
        }

        private static byte[] ReceiveAll(Socket socket)
        {
            var buffer = new List<byte>();

            while (socket.Available > 0)
            {
                var currByte = new byte[socket.ReceiveBufferSize];
                var byteCounter = socket.Receive(currByte, socket.ReceiveBufferSize, SocketFlags.None);

                if (byteCounter > 0)
                {
                    buffer.AddRange(currByte.Take(byteCounter));
                }
            }

            return buffer.ToArray();
        }
    }
}
