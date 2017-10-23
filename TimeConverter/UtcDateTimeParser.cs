using System;
using System.Text.RegularExpressions;
using Task2;
using TimeConverter.Exceptions;

namespace TimeConverter
{
    public class UtcDateTimeParser : IDateTimeParser
    {
        private readonly string _timeZonePattern;
        private readonly string _dateTimePattern;

        public UtcDateTimeParser()
        {
            _timeZonePattern = "(?<tzhh>[+-](2[0-3]|[01]?\\d))(:(?<tzmm>[0-5]?\\d))?";
            _dateTimePattern =
                "(19|20)\\d\\d" +    //year
                "\\." + "(1[012]|0?[1-9])" + //month
                "\\." + "([12]\\d|3[01]|0?[1-9])" + //day
                "[ Tt](?<hh>2[0-3]|[01]?\\d):(?<mm>[0-5]?\\d):(?<ss>[0-5]?\\d)" + //hh:mm:ss
                "( ?" + _timeZonePattern +"[Zz]?|[Zz])"; //timezone
        }

        public UtcTime ParseDateTime(string dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException($"{nameof(dateTime)} is null");
            }

            Match match = Regex.Match(dateTime, _dateTimePattern);
            if (!match.Success || match.Length != dateTime.Length)
            {
                throw new InvalidDateTimeFormatException($"{nameof(dateTime)} has invalid format. " +
                    " Should be YYYY.MM.DDtHH:MM:SS (+|-)HH:MMZ");
            }

            return new UtcTime
            {
                Hours = ParseGroupValue(match, "hh"),
                Minutes = ParseGroupValue(match, "mm"),
                Seconds = ParseGroupValue(match, "ss"),
                TimeZone = GetTimeZoneFromMatch(match)
            };
        }

        public TimeZone ParseTimeZone(string timeZone)
        {
            if (timeZone == null)
            {
                throw new ArgumentNullException($"{nameof(timeZone)} is null");
            }

            Match match = Regex.Match(timeZone, _timeZonePattern);
            if (!match.Success || match.Length != timeZone.Length)
            {
                throw new InvalidTimeZoneFormatException($"{nameof(timeZone)} has invalid format." +
                    " Should be (+|-)HH:MM");
            }

            return GetTimeZoneFromMatch(match);
        }

        private TimeZone GetTimeZoneFromMatch(Match match)
        {
            int hours = ParseGroupValue(match, "tzhh");
            return new TimeZone
            {
                TimeZoneSign = Math.Sign(hours),
                Hours = Math.Abs(hours),
                Minutes = ParseGroupValue(match, "tzmm")
            };
        }

        private int ParseGroupValue(Match match, string groupName)
        {
            if (match.Groups[groupName].Success)
            {
                return match.Groups[groupName].Value.ParseInt();
            }

            return 0;
        }
    }
}
