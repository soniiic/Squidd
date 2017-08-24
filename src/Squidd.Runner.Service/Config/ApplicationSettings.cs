using System;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Squidd.Runner.Config;

namespace Squidd.Runner.Service.Config
{
    internal class ApplicationSettings : IApplicationSettings
    {
        private dynamic settings;

        public ApplicationSettings()
        {
            ReadSettings();
        }

        private void ReadSettings()
        {
            var settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            var rawSettings = File.ReadAllText(settingsFilePath);
            settings = JsonConvert.DeserializeObject<ExpandoObject>(rawSettings);
        }

        public int Port => (int)settings.Port;

        public string GetTemporaryDirectoryPath()
        {
            return Environment.ExpandEnvironmentVariables(settings.TemporaryDirectoryPath);
        }

        public string GetPairId()
        {
            return settings.PairId;
        }

        public string GetPairPassphrase()
        {
            return settings.PairPassphrase;
        }

        public void SetPairId(Guid pairId)
        {
            settings.PairId = pairId.ToString();
            WriteSettings();
        }

        public void SetPairPassphrase(string pairPassphrase)
        {
            settings.PairPassphrase = pairPassphrase;
            WriteSettings();
        }

        private void WriteSettings()
        {
            var settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            var newSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(settingsFilePath, newSettings);
        }
    }
}