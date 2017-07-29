using System;
using System.Collections.Generic;
using System.IO;
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
            List<IResponder> responders;
            byte[] allData;
            using (var dataReader = new BinaryReader(client.GetStream(), Encoding.UTF8, true))
            {
                var header = dataReader.ReadString();
                Console.WriteLine($"Received {header} command.");

                responders = allResponders.Where(r => r.RespondsToHeader(header)).ToList();

                if (!responders.Any())
                {
                    RespondWithNotSupported(client, header);
                    return;
                }

                var payloadSize = Convert.ToInt32(dataReader.ReadUInt32());
                allData = dataReader.ReadBytes(payloadSize);
            }

            using (var dataWriter = new BinaryWriter(client.GetStream(), Encoding.UTF8, true))
            {
                if (responders.Any(r => r.MakesBusy))
                {
                    if (Global.IsBusy || !Global.SetBusy())
                    {
                        RespondWithBusy(client, dataWriter);
                        return;
                    }
                }

                foreach (var responder in responders)
                {
                    responder.Process(allData, dataWriter);
                }
            }

            if (responders.Any(r => r.MakesBusy))
            {
                Global.ClearBusy();
            }

            client.Close();
        }

        private static void RespondWithBusy(TcpClient client, BinaryWriter dataWriter)
        {
            dataWriter.Write("EROR");
            dataWriter.Write("Runner is busy.");
            client.Close();
        }

        private static void RespondWithNotSupported(TcpClient client, string header)
        {
            client.Client.Send(Encoding.UTF8.GetBytes("EROR"));
            client.Client.Send(Encoding.UTF8.GetBytes($"Header not supported: {header}"));
            client.Close();
        }
    }
}
