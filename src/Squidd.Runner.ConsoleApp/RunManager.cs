using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Squidd.Runner.ConsoleApp
{
    internal class RunManager
    {
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
                    var data = ReceiveAll(socket);
                    var script = Encoding.ASCII.GetString(data);
                    var powerShellRunner = new PowerShellRunner();
                    CommunicationService.SubscribeToOutput(powerShellRunner, socket);
                    powerShellRunner.RunScript(script);
                    socket.Close();
                });
            }
        }

        static byte[] ReceiveAll(Socket socket)
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
