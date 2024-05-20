using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteServidorProyectoU3.Helpers
{
    public static class CentralStandarTime
    {
        public static DateTime ToMexicoTime(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Central America Standard Time");
        }
    }
}
