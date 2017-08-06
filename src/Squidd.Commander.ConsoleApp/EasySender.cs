using System;
using System.Dynamic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public void Send(string method, byte[] payload = null)
        {
            Task.Run(() =>
            {
                var client = new TcpClient(ipAddress, port);
                var stream = client.GetStream();
                Console.WriteLine();
                Console.WriteLine($"Sending {method} command...");
                WriteCommand(method, payload ?? new byte[0], stream);
                Console.WriteLine("Waiting for response...");
                ReadResponse(stream, client);
                client.Close();
            });
        }

        private void WriteCommand(string method, byte[] payload, NetworkStream stream)
        {
            payload = payload ?? throw new ArgumentNullException(nameof(payload));

            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                var header = new
                {
                    Method = method,
                    PayloadLength = payload.Length,
                    Token = this.Token
                };

                var jsonHeader = JsonConvert.SerializeObject(header);

                writer.Write(jsonHeader);

                try
                {
                    writer.Write(payload);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void ReadResponse(NetworkStream stream, TcpClient client)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                while (client.Client.IsConnected() || client.Available > 0)
                {
                    try
                    {
                        reader.BaseStream.ReadTimeout = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;
                        var rawHeader = reader.ReadString();

                        dynamic header = JsonConvert.DeserializeObject<ExpandoObject>(rawHeader);

                        var responseBody = Encoding.UTF8.GetString(reader.ReadBytes((int) header.ContentLength));

                        if (header.Type == "LOG" || header.Type == "EROR")
                        {
                            Console.WriteLine(header.Type);
                            Console.WriteLine(responseBody);
                        }

                        if (header.Type == "INT")
                        {
                            if (header.SubType == "TOK")
                            {
                                this.Token = responseBody;
                                Console.WriteLine("Token stored!");
                            }
                            else
                            {
                                Console.WriteLine(responseBody);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
            }
        }

        public string Token { get; set; }

        public void Send(string method, string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload ?? string.Empty);
            Send(method, bytes);
        }
    }
}