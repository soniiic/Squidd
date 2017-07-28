using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Squidd.Commander.ConsoleApp
{
    class Program
    {
        private const int Port = 13000;

        private const string IpAddress = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing commander...");
            Thread.Sleep(1000);

            while (true)
            {
                var textToSend = @"Get-Date
echo $env:computername
Function Get-Fib ($n) {
     $current = $previous = 1;
     while ($current -lt $n) {
           $current;
           $current,$previous = ($current + $previous),$current}
     }
Get-Fib 100";

                var client = new TcpClient(IpAddress, Port);
                var stream = client.GetStream();
                var bytesToSend = Encoding.ASCII.GetBytes(textToSend);

                Console.WriteLine("Sending script...");
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                Console.WriteLine("Waiting for response...");

                var socket = client.Client;
                while (!(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0))
                {
                    while (stream.DataAvailable)
                    {
                        var bytesToRead = new byte[client.ReceiveBufferSize];
                        var bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                        Console.Write(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    }
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Finished executing PS!");

                Console.WriteLine("Press enter to send again...");

                Console.ReadLine();
                client.Close();
            }
        }
    }
}
