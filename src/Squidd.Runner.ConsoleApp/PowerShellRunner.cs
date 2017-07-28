using System.Linq;
using System.Management.Automation;

namespace Squidd.Runner.ConsoleApp
{
    internal class PowerShellRunner
    {
        public event PowershellOutputEvent OnOutput;

        public void RunScript(string script)
        {
            using (var instance = PowerShell.Create())
            {
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