using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Squidd.Shared.Models;

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

            Console.WriteLine("1 INFO");
            Console.WriteLine("2 STOR (empty file)");
            Console.WriteLine("3 UNSP");
            Console.WriteLine("4 PS (fib)");
            Console.WriteLine("5 PS (delay)");
            Console.WriteLine("6 PAIR (valid)");
            Console.WriteLine("8 SESO");
            Console.WriteLine("9 SESC");

            while (true)
            {
                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        easySender.Send("INFO");
                        break;
                    case '2':
                        easySender.Send("STOR", new byte[0]);
                        break;
                    case '3':
                        easySender.Send("UNSP");
                        break;
                    case '4':
                        easySender.Send("PS", Fibonacci);
                        break;
                    case '5':
                        easySender.Send("PS", Sleep);
                        break;
                    case '6':
                        easySender.Send("PAIR", Auth(Console.ReadLine()));
                        break;
                    case '8':
                        easySender.Send("SESO");
                        break;
                    case '9':
                        easySender.Send("SESC");
                        break;
                    default:
                        continue;

                }
            }
        }

        private static string Auth(string passphrase)
        {
            return JsonConvert.SerializeObject(new AuthenticationInputModel()
            {
                PairPassphrase = passphrase
            });
        }

        public const string Sleep = @"Start-Sleep -s 10";

        public const string Fibonacci = @"Function Get-Fib ($n) {
     $current = $previous = 1;
     while ($current -lt $n) {
           $current;
           $current,$previous = ($current + $previous),$current}
     }
Get-Fib 100";
    }
}