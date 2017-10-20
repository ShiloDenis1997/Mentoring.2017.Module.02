using TimeConverter.Exceptions;
using System;

namespace TimeConverter
{
    public interface IDateTimeParser
    {
        /// <summary>
        /// Extracts time with timzone params from date time string
        /// </summary>
        /// <param name="dateTime">date and time as string</param>
        /// <returns>Parsed time with timezone values</returns>
        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="dateTime"/> is null</exception>
        /// <exception cref="InvalidDateTimeFormatException">Throws if 
        /// <paramref name="dateTime"/> has invalid format </exception>
        UtcTime ParseDateTime(string dateTime);

        TimeZone ParseTimeZone(string timeZone);
    }
}
