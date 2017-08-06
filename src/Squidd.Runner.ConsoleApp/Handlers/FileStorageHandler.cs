using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Squidd.Runner.ConsoleApp.Config;

namespace Squidd.Runner.ConsoleApp.Handlers
{
    internal class FileStorageHandler : IHandler
    {
        private readonly IApplicationSettings settings;

        public FileStorageHandler(IApplicationSettings settings)
        {
            this.settings = settings;
        }

        public bool RespondsToMethod(string method)
        {
            return method == "STOR";
        }

        public void Process(byte[] data, StreamResponder responder)
        {
            var fileId = Guid.NewGuid().ToString();
            var fullPath = GetFullPath(fileId);
            try
            {
                File.WriteAllBytes(fullPath, data);
                responder.Log(fileId);
            }
            catch (Exception e)
            {
                responder.Error(e.Message);
            }
        }

        public bool MakesBusy => true;

        private string GetFullPath(string fileId)
        {
            var directoryPath = settings.GetTemporaryDirectoryPath();
            Directory.CreateDirectory(directoryPath);
            return Path.Combine(directoryPath, fileId + ".zip");
        }
    }
}