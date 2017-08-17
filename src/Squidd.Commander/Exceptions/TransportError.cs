using System;

namespace Squidd.Commander.Exceptions
{
    internal class TransportError : Exception
    {
        public TransportError(string message, Exception exception) : base(message, exception)
        {
        }

        public TransportError(string message) : base(message)
        {
        }
    }
}