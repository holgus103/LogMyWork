using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public static class UnixTime
    {
        public static UInt64 ToUnixTimestamp(this DateTime utcDate)
        {
            return (UInt64)(utcDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime ParseUnitTimestamp(UInt64 timestamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(timestamp);
        }
    }
}
