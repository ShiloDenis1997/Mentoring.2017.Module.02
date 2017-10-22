using System;
using TimeConverter.Exceptions;
using Task2;

namespace TimeConverter
{
    public class DateTimeConverter
    {
        private readonly IDateTimeParser dateTimeParser;

        public DateTimeConverter(IDateTimeParser dateTimeParser)
        {
            this.dateTimeParser = dateTimeParser;
        }

        public string ConvertToSeconds(string dateTime)
        {
            UtcTime time = ParseDateTime(dateTime);

            int totalSeconds = time.TotalLocalSeconds;
            int totalTimeZoneSeconds = time.TimeZone.TotalSeconds;

            int secondsUtc = (totalSeconds - totalTimeZoneSeconds + TimeConstants.SecondsInDay) 
                % TimeConstants.SecondsInDay;
            return secondsUtc.ToString();
        }

        public string ConvertToLocalTime(string seconds, string timeZone)
        {
            int secondsUtc = ParseSeconds(seconds);
            TimeZone tz = ParseTimeZone(timeZone);

            int timeZoneSeconds = tz.TotalSeconds;
            int totalLocalSeconds = (secondsUtc + timeZoneSeconds + TimeConstants.SecondsInDay) 
                % TimeConstants.SecondsInDay;
            int hours = totalLocalSeconds / TimeConstants.SecondsInHour;
            totalLocalSeconds %= TimeConstants.SecondsInHour;
            int minutes = totalLocalSeconds / TimeConstants.SecondsInMinute;
            int localSeconds = totalLocalSeconds % TimeConstants.SecondsInMinute;
            return $"{hours:00}:{minutes:00}:{localSeconds:00}";
        }

        private int ParseSeconds(string secondsStr)
        {
            int seconds;
            try
            {
                seconds = secondsStr.ParseInt();
            }
            catch (Exception ex)
            {
                throw new TimeConverterException($"Cannot parse {nameof(secondsStr)}", ex);
            }

            if (seconds < 0 || seconds / TimeConstants.SecondsInHour >= TimeConstants.HoursInDay)
            {
                throw new ArgumentOutOfRangeException($"{nameof(secondsStr)} argument is not in 0-24h range");
            }

            return seconds;
        }

        private TimeZone ParseTimeZone(string timeZone)
        {
            if (timeZone == null)
            {
                throw new ArgumentNullException($"{nameof(timeZone)} is null");
            }

            TimeZone tz;
            try
            {
                tz = dateTimeParser.ParseTimeZone(timeZone);
            }
            catch (InvalidTimeZoneFormatException) { throw; }
            catch (ArgumentNullException) { throw; }
            catch (Exception ex)
            {
                throw new TimeConverterException($"Cannot parse {nameof(timeZone)}", ex);
            }

            return tz;
        }

        private UtcTime ParseDateTime(string dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException($"{dateTime} is null");
            }

            UtcTime time;
            try
            {
                time = dateTimeParser.ParseDateTime(dateTime);
            }
            catch (InvalidDateTimeFormatException) { throw; }
            catch (ArgumentNullException) { throw; }
            catch (Exception ex)
            {
                throw new TimeConverterException($"Cannot parse {nameof(dateTime)}", ex);
            }

            return time;
        }
    }
}
