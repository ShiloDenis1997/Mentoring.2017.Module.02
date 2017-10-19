using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TimeConverter;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class ContractExample
    {
        public int Value { get; set; }

        [ContractInvariantMethod]
        public void CheckValue()
        {
            Console.WriteLine("hi");
            Contract.Invariant(Value >= 0 && Value <= 10);
        }
        public int GetMiddleValue(int min, int max)
        {
            Contract.Requires(min < max);
            return (min + max) / 2;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string date = "2017.10.19t22:02:40Z";
            //DateTime dateTime = DateTime.Parse(date);
            //DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

            //Console.WriteLine(dateTime);
            //Array.ForEach(dateTime.GetDateTimeFormats(), format => Console.WriteLine(format));
            DateTimeConverter dtc = new DateTimeConverter();
            Console.WriteLine(dtc.ConvertToSeconds(date));
            TimeSpan time = TimeSpan.FromSeconds(dtc.ConvertToSeconds(date));
            Console.WriteLine(time);
        }
    }
}
