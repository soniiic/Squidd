using System;

namespace Squidd.Commander.Exceptions
{
    internal class RunnerError : Exception
    {
        public RunnerError(string message) : base(message)
        {
        }
    }
}