using System;
using TimeConverter;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string date = "2017.10.19t22:02:40 -2:20Z";
            DateTimeConverter dtc = new DateTimeConverter(new UtcDateTimeParser());
            string seconds = dtc.ConvertToSeconds(date);
            Console.WriteLine(seconds);

            Console.WriteLine(dtc.ConvertToLocalTime(seconds, "+3:30"));
        }
    }
}
