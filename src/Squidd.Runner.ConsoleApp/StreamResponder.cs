using System;
using System.IO;
using System.Text;

namespace Squidd.Runner.ConsoleApp
{
    internal class StreamResponder : IDisposable
    {
        private readonly BinaryWriter writer;

        public StreamResponder(Stream stream)
        {
            writer = new BinaryWriter(stream, Encoding.UTF8, true);
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void Error(string message)
        {
            writer.Write("EROR");
            writer.Write(message + "\n");
        }

        public void Log(string message)
        {
            writer.Write(message + "\n");
        }
    }
}