namespace Squidd.Runner.Middleware
{
    interface IMiddleware
    {
        int Order { get; }

        bool Process(dynamic header, StreamResponder responder, bool authenticated);
    }
}
