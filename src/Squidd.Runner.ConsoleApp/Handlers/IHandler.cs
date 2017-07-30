using System.IO;

namespace Squidd.Runner.ConsoleApp.Handlers
{
    internal interface IHandler
    {
        bool RespondsToHeader(string header);

        void Process(byte[] data, StreamResponder responder);

        bool MakesBusy { get; }
    }
}