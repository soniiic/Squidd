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

        public void Send(string header, byte[] payload = null)
        {
            var client = new TcpClient(ipAddress, port);
            var stream = client.GetStream();

            Console.WriteLine("Sending script...");
            var headerBytes = Encoding.UTF8.GetBytes(header);
            stream.Write(headerBytes, 0, headerBytes.Length);
            if (payload != null)
            {
                stream.Write(payload, 0, payload.Length);
            }
            var endOfStream = Encoding.ASCII.GetBytes("SQUIDDEND");
            stream.Write(endOfStream, 0, endOfStream.Length);
            Console.WriteLine("Waiting for response...");

            while (client.Client.IsConnected())
            {
                while (stream.DataAvailable)
                {
                    var bytesToRead = new byte[client.ReceiveBufferSize];
                    var bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.Write(Encoding.UTF8.GetString(bytesToRead, 0, bytesRead));
                }
                Thread.Sleep(100);
            }

            client.Close();
        }

        public void Send(string header, string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload ?? string.Empty);
            Send(header, bytes);
        }
    }
}