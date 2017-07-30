using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Squidd.Runner.ConsoleApp.Config;

namespace Squidd.Runner.ConsoleApp.Responders
{
    internal class FileStorageResponder : IResponder
    {
        private readonly IApplicationSettings settings;

        public FileStorageResponder(IApplicationSettings settings)
        {
            this.settings = settings;
        }

        public bool RespondsToHeader(string header)
        {
            return header == "STOR";
        }

        public void Process(byte[] data, BinaryWriter writer)
        {
            var fileId = Guid.NewGuid().ToString();
            var fullPath = GetFullPath(fileId);
            try
            {
                File.WriteAllBytes(fullPath, data);
                writer.Write(fileId + "\n");
            }
            catch (Exception e)
            {
                writer.Write("EROR");
                writer.Write(e.Message + "\n");
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