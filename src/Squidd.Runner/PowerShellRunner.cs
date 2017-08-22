using System.IO;
using System.Linq;
using System.Management.Automation;
using Squidd.Runner.Config;

namespace Squidd.Runner
{
    internal class PowerShellRunner
    {
        private readonly IApplicationSettings settings;

        public PowerShellRunner(IApplicationSettings settings)
        {
            this.settings = settings;
        }

        public event PowershellOutputEvent OnOutput;

        public void RunScript(string script)
        {
            var workingDirectory = Path.Combine(settings.GetTemporaryDirectoryPath(), "work");
            Directory.CreateDirectory(workingDirectory);
            using (var instance = PowerShell.Create(RunspaceMode.NewRunspace))
            {
                instance.Runspace.SessionStateProxy.Path.SetLocation(workingDirectory);
                instance.AddScript(script);
                var outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += OnDataAdded;
                var result = instance.BeginInvoke<PSObject, PSObject>(null, outputCollection);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void OnDataAdded(object sender, DataAddedEventArgs e)
        {
            var messages = (PSDataCollection<PSObject>)sender;
            var args = new PowershellOutputEventArgs
            {
                LineNumber = e.Index,
                Message = messages.ElementAt(e.Index).BaseObject.ToString()
            };
            OnOutput?.Invoke(this, args);
        }

        internal delegate void PowershellOutputEvent(object sender, PowershellOutputEventArgs args);
    }
}