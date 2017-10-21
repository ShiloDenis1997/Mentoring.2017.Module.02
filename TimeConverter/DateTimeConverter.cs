﻿using System;
using TimeConverter.Exceptions;
using Task2;

namespace TimeConverter
{
    public class DateTimeConverter
    {
        private readonly int secondsInDay = 24 * 60 * 60;
        private readonly IDateTimeParser dateTimeParser;

        public DateTimeConverter(IDateTimeParser dateTimeParser)
        {
            this.dateTimeParser = dateTimeParser;
        }

        public string ConvertToSeconds(string dateTime)
        {
            UtcTime time = ParseDateTime(dateTime);
            TimeZone timeZone = time.TimeZone;

            int totalSeconds = time.Hours * 3600 + time.Minutes * 60 + time.Seconds;
            int totalTimeZoneSeconds = (timeZone.Hours * 3600 + timeZone.Minutes * 60) * timeZone.TimeZoneSign;

            int secondsUtc = (totalSeconds - totalTimeZoneSeconds + secondsInDay) % secondsInDay;
            return secondsUtc.ToString();
        }

        public string ConvertToLocalTime(string seconds, string timeZone)
        {
            int secondsUtc = ParseSeconds(seconds);
            TimeZone tz = ParseTimeZone(timeZone);

            int timeZoneSeconds = (tz.Hours * 3600 + tz.Minutes * 60) * tz.TimeZoneSign;
            int totalLocalSeconds = (secondsUtc + timeZoneSeconds + secondsInDay) % secondsInDay;
            int hours = totalLocalSeconds / 3600;
            totalLocalSeconds %= 3600;
            int minutes = totalLocalSeconds / 60;
            int localSeconds = totalLocalSeconds % 60;
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

            if (seconds < 0 || seconds / 3600 >= 24)
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
