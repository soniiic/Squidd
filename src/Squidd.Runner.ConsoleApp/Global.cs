using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squidd.Runner.ConsoleApp
{
    internal static class Global
    {
        private static readonly object BusyLocker = new object();

        public static bool IsBusy { get; private set; }

        public static bool SetBusy()
        {
            if (!IsBusy)
            {
                lock (BusyLocker)
                {
                    if (!IsBusy)
                    {
                        IsBusy = true;
                        return true;
                    }
                }
            }

            return false;
        }

        public static void ClearBusy()
        {
            IsBusy = false;
        }
    }
}
