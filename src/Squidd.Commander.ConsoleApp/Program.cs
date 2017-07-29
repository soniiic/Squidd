using System;
using System.Threading;
using System.Threading.Tasks;

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

            var easySender = new EasySender(IpAddress, Port);

            easySender.Send("INFO");
            easySender.Send("STOR", new byte[0]);

            easySender.Send("UNSP");

            Task.Run(() => easySender.Send("PS", "Start-Sleep -s 10"));
            Thread.Sleep(1000);
            easySender.Send("INFO");


            while (true)
            {
                var header = "PS";
                var payload = @"Function Get-Fib ($n) {
     $current = $previous = 1;
     while ($current -lt $n) {
           $current;
           $current,$previous = ($current + $previous),$current}
     }
Get-Fib 100";

                easySender.Send(header, payload);
                Console.WriteLine("Press enter to send again...");
                Console.ReadLine();
            }
        }
    }
}