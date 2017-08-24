using System;

namespace Squidd.Runner
{
    internal static class Global
    {
        private static readonly object BusyLocker = new object();

        public static bool IsBusy { get; private set; }

        public static Guid? SessionId { get; set; }

        public static bool StartSession()
        {
            if (!IsBusy)
            {
                lock (BusyLocker)
                {
                    if (!IsBusy)
                    {
                        IsBusy = true;
                        SessionId = Guid.NewGuid();
                        return true;
                    }
                }
            }

            return false;
        }

        public static void EndSession()
        {
            IsBusy = false;
            SessionId = null;
        }
    }
}
