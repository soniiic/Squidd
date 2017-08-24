using System;
using Squidd.Runner.Config;

namespace Squidd.Runner.Helpers
{
    class Authentication
    {
        private readonly IApplicationSettings settings;

        public Authentication(IApplicationSettings settings)
        {
            this.settings = settings;
        }

        internal bool IsAuthenticated(dynamic header)
        {
            return header.Token != null && new Guid(header.Token).ToString() == settings.GetPairId();
        }
    }
}
