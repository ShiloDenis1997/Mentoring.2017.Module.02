using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TimeConverter.Exceptions;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace TimeConverter
{
    public class DateTimeConverter
    {
        private readonly string _dateTimePattern =
            "(19|20)\\d\\d" +    //year
            "\\." + "(1[012]|0?[1-9])" + //month
            "\\." + "([12]\\d|3[01]|0?[1-9])" + //day
            "[ Tt](?<hh>2[0-3]|[01]?\\d):(?<mm>[0-5]?\\d):(?<ss>[0-5]?\\d)" + //hh:mm:ss
            "( ?(?<tzhh>[+-](2[0-3]|[01]?\\d))(:(?<tzmm>[0-5]?\\d))?[Zz]?|[Zz])"; //timezone
        private readonly int secondsInDay = 24 * 60 * 60;

        public int ConvertToSeconds(string dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException($"{dateTime} is null");
            }

            Match match = Regex.Match(dateTime, _dateTimePattern);
            if (!match.Success || match.Length != dateTime.Length)
            {
                throw new InvalidDateTimeFormatException($"{nameof(dateTime)} has invalid format");
            }

            int hours = int.Parse(match.Groups["hh"].Value);
            int minutes = int.Parse(match.Groups["mm"].Value);
            int seconds = int.Parse(match.Groups["ss"].Value);
            int totalSeconds = hours * 3600 + minutes * 60 + seconds;
            int timeZoneHours = int.Parse(match.Groups["tzhh"].Value);
            int timeZoneMinutes = int.Parse(match.Groups["tzmm"].Value);
            int totalTimeZoneSeconds = timeZoneHours * 3600 + timeZoneMinutes * 60;

            int secondsUtc = (totalSeconds - totalTimeZoneSeconds + secondsInDay) % secondsInDay;
            return secondsUtc;
        }
    }
}
