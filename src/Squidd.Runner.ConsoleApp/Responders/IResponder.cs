using System.IO;

namespace Squidd.Runner.ConsoleApp.Responders
{
    internal interface IResponder
    {
        bool RespondsToHeader(string header);

        void Process(byte[] data, BinaryWriter writer);

        bool MakesBusy { get; }
    }
}