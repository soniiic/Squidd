namespace Squidd.Runner.Handlers
{
    public interface IHandler
    {
        bool RespondsToMethod(string method);

        void Process(byte[] data, StreamResponder responder);

        bool CanRunWhenBusy { get; }

        bool RequiresAuthentication { get; }
    }
}