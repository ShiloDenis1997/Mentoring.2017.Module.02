using System;
using TimeConverter.Exceptions;
using Task2;

namespace TimeConverter
{
    public class DateTimeConverter
    {
        private const int secondsInDay = 24 * 60 * 60;
        private const int secondsInHour = 3600;
        private const int secondsInMinute = 60;
        private const int hoursInDay = 24;
        private readonly IDateTimeParser dateTimeParser;

        public DateTimeConverter(IDateTimeParser dateTimeParser)
        {
            this.dateTimeParser = dateTimeParser;
        }

        public string ConvertToSeconds(string dateTime)
        {
            UtcTime time = ParseDateTime(dateTime);
            TimeZone timeZone = time.TimeZone;

            int totalSeconds = time.Hours * secondsInHour + time.Minutes * secondsInMinute + time.Seconds;
            int totalTimeZoneSeconds = 
                (timeZone.Hours * secondsInHour + timeZone.Minutes * secondsInMinute) * timeZone.TimeZoneSign;

            int secondsUtc = (totalSeconds - totalTimeZoneSeconds + secondsInDay) % secondsInDay;
            return secondsUtc.ToString();
        }

        public string ConvertToLocalTime(string seconds, string timeZone)
        {
            int secondsUtc = ParseSeconds(seconds);
            TimeZone tz = ParseTimeZone(timeZone);

            int timeZoneSeconds = (tz.Hours * secondsInHour + tz.Minutes * secondsInMinute) * tz.TimeZoneSign;
            int totalLocalSeconds = (secondsUtc + timeZoneSeconds + secondsInDay) % secondsInDay;
            int hours = totalLocalSeconds / secondsInHour;
            totalLocalSeconds %= secondsInHour;
            int minutes = totalLocalSeconds / secondsInMinute;
            int localSeconds = totalLocalSeconds % secondsInMinute;
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

            if (seconds < 0 || seconds / secondsInHour >= hoursInDay)
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
