using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Squidd.Runner.Config;
using Squidd.Runner.Service.Config;
using Topshelf;

namespace Squidd.Runner.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ApplicationSettings();
            settings.SetPairPassphrase(GeneratePassphrase());
            IoCContainer.Configure(settings);

            HostFactory.Run(x =>
            {
                x.Service(() => new RunManager(settings.Port));
                x.SetServiceName("SquiddRunner");
                x.SetDescription("Squidd Runner");
                x.RunAsLocalSystem();
            });
            Console.ReadKey();
        }

        private static string GeneratePassphrase()
        {
            const string chars = "ACDEFGHJKLMNPQRSTUVWXYZ2345679";
            var length = 6;
            var res = new StringBuilder();
            using (var rng = new RNGCryptoServiceProvider())
            {
                var uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    var num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(chars[(int)(num % (uint)chars.Length)]);
                }
            }

            return res.ToString();
        }
    }
}
