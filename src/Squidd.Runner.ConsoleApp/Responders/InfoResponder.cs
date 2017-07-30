using System;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Squidd.Runner.ConsoleApp.Responders
{
    class InfoResponder : IResponder
    {
        public bool RespondsToHeader(string header)
        {
            return header == "INFO";
        }

        public void Process(byte[] data, BinaryWriter writer)
        {
            dynamic info = new ExpandoObject();
            info.Name = Environment.MachineName;
            info.RunnerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            info.WindowsVersion = Environment.OSVersion.ToString();
            info.Bits = Environment.Is64BitOperatingSystem ? 64 : 32;
            info.IsBusy = Global.IsBusy;

            writer.Write(JsonConvert.SerializeObject(info) + "\n");
        }

        public bool MakesBusy => false;
    }
}