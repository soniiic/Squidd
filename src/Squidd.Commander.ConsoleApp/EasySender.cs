using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            Task.Run(() =>
            {
                var client = new TcpClient(ipAddress, port);
                var stream = client.GetStream();
                Console.WriteLine();
                Console.WriteLine($"Sending {header} command...");
                WriteCommand(header, payload, stream);
                Console.WriteLine("Waiting for response...");
                ReadResponse(stream, client);
                client.Close();
            });
        }

        private static void WriteCommand(string header, byte[] payload, NetworkStream stream)
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(header);

                payload = payload ?? new byte[0];
                var payLoadLength = BitConverter.GetBytes(Convert.ToUInt32(payload.Length));
                try
                {
                    writer.Write(payLoadLength);
                    writer.Write(payload);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static void ReadResponse(NetworkStream stream, TcpClient client)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                while (client.Client.IsConnected() || client.Available > 0)
                {
                    try
                    {
                        Console.Write(reader.ReadString());
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        public void Send(string header, string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload ?? string.Empty);
            Send(header, bytes);
        }
    }
}