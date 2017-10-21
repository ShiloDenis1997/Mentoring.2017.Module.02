using System;
using NUnit.Framework;
using TimeConverter.Exceptions;

namespace TimeConverter.Test
{
    [TestFixture]
    public class UtcDateTimeParserTests
    {
        private IDateTimeParser _parser;

        [SetUp]
        public void Init()
        {
            _parser = new UtcDateTimeParser();
        }

        [Test]
        public void ValidDateTimeWithPositiveTimezone_ParsedUtcTime()
        {
            string dateTime = "2017.10.19t22:02:40 +2:30Z";

            var utcTime = _parser.ParseDateTime(dateTime);

            Assert.IsTrue(utcTime.Hours == 22
                && utcTime.Minutes == 2
                && utcTime.Seconds == 40
                && utcTime.TimeZone.Hours == 2
                && utcTime.TimeZone.Minutes == 30
                && utcTime.TimeZone.TimeZoneSign == 1);
        }

        [Test]
        public void ValidDateTimeWithNegativeTimezone_ParsedUtcTime()
        {
            string dateTime = "2017.10.19t22:02:40 -2:30Z";

            var utcTime = _parser.ParseDateTime(dateTime);

            Assert.IsTrue(utcTime.Hours == 22
                && utcTime.Minutes == 2
                && utcTime.Seconds == 40
                && utcTime.TimeZone.Hours == 2
                && utcTime.TimeZone.Minutes == 30
                && utcTime.TimeZone.TimeZoneSign == -1);
        }

        [Test]
        public void ValidDateTimeWithZeroTimezone_ParsedUtcTime()
        {
            string dateTime = "2017.10.19t22:02:40 +0:00Z";

            var utcTime = _parser.ParseDateTime(dateTime);

            Assert.IsTrue(utcTime.Hours == 22
                && utcTime.Minutes == 2
                && utcTime.Seconds == 40
                && utcTime.TimeZone.Hours == 0
                && utcTime.TimeZone.Minutes == 0
                && utcTime.TimeZone.TimeZoneSign == 0);
        }

        [Test]
        public void ValidDateTimeWithImplicitZeroTimezone_ParsedUtcTime()
        {
            string dateTime = "2017.10.19t22:02:40Z";

            var actualUtcTime = _parser.ParseDateTime(dateTime);

            Assert.IsTrue(actualUtcTime.Hours == 22
                && actualUtcTime.Minutes == 2
                && actualUtcTime.Seconds == 40
                && actualUtcTime.TimeZone.Hours == 0
                && actualUtcTime.TimeZone.Minutes == 0
                && actualUtcTime.TimeZone.TimeZoneSign == 0);
        }

        [Test]
        public void NullDateTime_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.ParseDateTime(null));
        }

        [TestCase("2017.10.19t25:02:40 +0:00Z")]
        [TestCase("2017.10.19t22:60:40 +0:00Z")]
        [TestCase("2017.10.19t22:02:60 +0:00Z")]
        [TestCase("2017.10.19t22:02:40 +25:00Z")]
        [TestCase("2017.10.19t22:02:40 +0:61Z")]
        [TestCase("2117.10.19t22:02:40 +0:00Z")]
        [TestCase("3017.10.19t22:02:40 +0:00Z")]
        [TestCase("1817.10.19t22:02:40 +0:00Z")]
        [TestCase("2017.13.19t22:02:40 +0:00Z")]
        [TestCase("2017.00.19t22:02:40 +0:00Z")]
        [TestCase("2017.10.32t22:02:40 +0:00Z")]
        [TestCase("2017.10.19t22:02:40 0:00Z")]
        [Test]
        public void InvalidDateTimeFormat_InvalidDateTimeFormatExceptionThrown(string dateTime)
        {
            Assert.Throws<InvalidDateTimeFormatException>(() => _parser.ParseDateTime(dateTime));
        }

        [Test]
        public void ValidPositiveTimeZone_ParsedTimeZone()
        {
            string timeZone = "+3:45";

            var actualTimeZone = _parser.ParseTimeZone(timeZone);

            Assert.IsTrue(actualTimeZone.Hours == 3
                && actualTimeZone.Minutes == 45
                && actualTimeZone.TimeZoneSign == 1);
        }

        [Test]
        public void ValidNegativeTimeZone_ParsedTimeZone()
        {
            string timeZone = "-3:45";

            var actualTimeZone = _parser.ParseTimeZone(timeZone);

            Assert.IsTrue(actualTimeZone.Hours == 3
                && actualTimeZone.Minutes == 45
                && actualTimeZone.TimeZoneSign == -1);
        }

        [Test]
        public void ValidPlusZeroTimeZone_ParsedTimeZone()
        {
            string timeZone = "+0:00";

            var actualTimeZone = _parser.ParseTimeZone(timeZone);

            Assert.IsTrue(actualTimeZone.Hours == 0
                && actualTimeZone.Minutes == 0
                && actualTimeZone.TimeZoneSign == 0);
        }

        [Test]
        public void ValidMinusZeroTimeZone_ParsedTimeZone()
        {
            string timeZone = "-0:00";

            var actualTimeZone = _parser.ParseTimeZone(timeZone);

            Assert.IsTrue(actualTimeZone.Hours == 0
                && actualTimeZone.Minutes == 0
                && actualTimeZone.TimeZoneSign == 0);
        }

        [TestCase("3:00")]
        [TestCase("+24:00")]
        [TestCase("-24:00")]
        [TestCase("+3:60")]
        [TestCase("-3:60")]
        [TestCase("+023:00")]
        [TestCase("+3:030")]
        [TestCase("+4.00")]
        [Test]
        public void InvalidTimeZoneFormat_InvalidTimeZoneFormatExceptionThrown(string timeZone)
        {
            Assert.Throws<InvalidTimeZoneFormatException>(() => _parser.ParseTimeZone(timeZone));
        }

        [Test]
        public void NullTimeZone_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.ParseTimeZone(null));
        }
    }
}
