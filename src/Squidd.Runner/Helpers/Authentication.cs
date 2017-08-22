using System;

namespace Squidd.Runner.Helpers
{
    static class Authentication
    {
        internal static bool IsAuthenticated(dynamic header)
        {
            return header.Token != null && new Guid(header.Token) == Global.PairId;
        }
    }
}
