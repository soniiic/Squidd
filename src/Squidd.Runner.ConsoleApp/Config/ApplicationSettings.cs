using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Squidd.Runner.Config;

namespace Squidd.Runner.ConsoleApp.Config
{
    internal class ApplicationSettings : IApplicationSettings
    {
        private readonly dynamic settings;

        public ApplicationSettings()
        {
            var settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
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