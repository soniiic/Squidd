using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Squidd.Commander.ConsoleApp.Extensions;

namespace Squidd.Commander.ConsoleApp
{
    internal class EasySender
    {
        private readonly string ipAddress;
        private readonly int port;

        public EasySender(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void Send(string header, string payload = null)
        {
            var client = new TcpClient(ipAddress, port);
            var stream = client.GetStream();
            var bytesToSend = Encoding.ASCII.GetBytes(header + (payload ?? string.Empty));

            Console.WriteLine("Sending script...");
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            Console.WriteLine("Waiting for response...");

            while (client.Client.IsConnected())
            {
                while (stream.DataAvailable)
                {
                    var bytesToRead = new byte[client.ReceiveBufferSize];
                    var bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.Write(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                }
                Thread.Sleep(1000);
            }
            
            client.Close();
        }
    }
}