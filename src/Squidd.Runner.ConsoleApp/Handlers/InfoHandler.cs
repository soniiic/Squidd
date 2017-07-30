using System;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Squidd.Runner.ConsoleApp.Handlers
{
    class InfoHandler : IHandler
    {
        public bool RespondsToHeader(string header)
        {
            return header == "INFO";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            dynamic info = new ExpandoObject();
            info.Name = Environment.MachineName;
            info.RunnerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            info.WindowsVersion = Environment.OSVersion.ToString();
            info.Bits = Environment.Is64BitOperatingSystem ? 64 : 32;
            info.IsBusy = Global.IsBusy;

            responder.Log(JsonConvert.SerializeObject(info));
        }

        public bool MakesBusy => false;
    }
}