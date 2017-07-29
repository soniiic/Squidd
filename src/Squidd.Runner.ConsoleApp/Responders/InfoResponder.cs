using System;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Squidd.Runner.ConsoleApp.Responders
{
    class InfoResponder : IResponder
    {
        public bool RespondsToHeader(string header)
        {
            return header == "INFO";
        }

        public void Process(byte[] data, Socket socket)
        {
            socket.Send(Encoding.UTF8.GetBytes($"Name: {Environment.MachineName}\n" +
                                                $"Squidd Runner Version: {Assembly.GetExecutingAssembly().GetName().Version}\n" +
                                                $"Windows: {Environment.OSVersion}\n" +
                                               $"Busy: false"));
        }
    }
}