using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ModalHandler.Tools
{
    /// <summary>
    /// Misc helper class.
    /// </summary>
    internal static class Util
    {
        private static readonly AutoResetEvent Timer = new AutoResetEvent(false);

        public static void Wait(Func<bool> func, TimeSpan timeout)
        {
            var tick = TimeSpan.FromMilliseconds(100);
            while (!func() && !timeout.Equals(TimeSpan.Zero))
            {
                timeout = timeout.Subtract(tick);
                Timer.WaitOne(tick);
            }
        }

        public static string Aggregate(this IEnumerable<string> collection, params char[] separators)
        {
            if (!separators.Any())
                separators = new[] {' '};
            var separatorLine = separators.Aggregate(string.Empty, (str, sep) => str + sep);
            return collection.Aggregate(string.Empty, (tot, cur) => $"{tot}{separatorLine}{cur}").Trim(separators);
        }
    }
}