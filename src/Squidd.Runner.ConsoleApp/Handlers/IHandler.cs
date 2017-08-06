namespace Squidd.Runner.ConsoleApp.Handlers
{
    internal interface IHandler
    {
        bool RespondsToMethod(string method);

        void Process(byte[] data, StreamResponder responder);

        bool MakesBusy { get; }

        bool RequiresAuthentication { get; }
    }
}