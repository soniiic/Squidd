using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
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

    internal class CommunicationService
    {
        private readonly Socket socket;

        private CommunicationService(PowerShellRunner runner, Socket socket)
        {
            this.socket = socket;
            runner.OnOutput += OnOutput;
        }

        private void OnOutput(object sender, PowershellOutputEventArgs args)
        {
            socket.Send(Encoding.ASCII.GetBytes($"{args.LineNumber}: {args.Message}\n"));
        }

        public static void SubscribeToOutput(PowerShellRunner powerShellRunner, Socket socket)
        {
            new CommunicationService(powerShellRunner, socket);
        }
    }

    internal class PowerShellRunner
    {
        public event PowershellOutputEvent OnOutput;

        public void RunScript(string script)
        {
            using (var instance = PowerShell.Create())
            {
                instance.AddScript(script);
                var outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += OnDataAdded;
                var result = instance.BeginInvoke<PSObject, PSObject>(null, outputCollection);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void OnDataAdded(object sender, DataAddedEventArgs e)
        {
            var messages = (PSDataCollection<PSObject>)sender;
            var args = new PowershellOutputEventArgs
            {
                LineNumber = e.Index,
                Message = messages.ElementAt(e.Index).BaseObject.ToString()
            };
            OnOutput?.Invoke(this, args);
        }

        internal delegate void PowershellOutputEvent(object sender, PowershellOutputEventArgs args);
    }


    internal class PowershellOutputEventArgs
    {
        public int LineNumber { get; set; }

        public string Message { get; set; }
    }
}
