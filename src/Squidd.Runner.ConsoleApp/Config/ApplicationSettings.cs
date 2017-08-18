using System;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Squidd.Runner.Config;

namespace Squidd.Runner.Service.Config
{
    internal class ApplicationSettings : IApplicationSettings
    {
        private readonly dynamic settings;

        public ApplicationSettings()
        {
            var settingsFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            var rawSettings = File.ReadAllText(settingsFilePath);
            settings = JsonConvert.DeserializeObject<ExpandoObject>(rawSettings);
        }

        public int Port => (int)settings.Port;

        public string GetTemporaryDirectoryPath()
        {
            return System.Environment.ExpandEnvironmentVariables(settings.TemporaryDirectoryPath);
        }
    }
}