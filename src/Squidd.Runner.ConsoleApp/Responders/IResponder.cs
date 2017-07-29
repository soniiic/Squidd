using System.Net.Sockets;

namespace Squidd.Runner.ConsoleApp.Responders
{
    internal interface IResponder
    {
        bool RespondsToHeader(string header);

        void Process(byte[] data, Socket socket);

        bool MakesBusy { get; }
    }
}