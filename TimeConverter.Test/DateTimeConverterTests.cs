using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using System;

namespace TimeConverter.Test
{
    [TestFixture]
    public class DateTimeConverterTests
    {
        private Mock<IDateTimeParser> _mockDateTimeParser;

        [SetUp]
        public void Init()
        {
            _mockDateTimeParser = new Mock<IDateTimeParser>();
        }

        [TearDown]
        public void Finish()
        {
            _mockDateTimeParser.Reset();
        }

        [Test, TestCaseSource(nameof(CorrectTimeToSecondTestCases))]
        public void CorrectTimeToSecond_RightSecondsExpected(UtcTime utcTime, string expectedSeconds)
        {
            _mockDateTimeParser.Setup(p => p.ParseDateTime(It.IsAny<string>())).Returns(utcTime);
            var converter = GetDateTimeConverter();

            Assert.AreEqual(expectedSeconds, converter.ConvertToSeconds("stub_dateTime"));
        }

        public static IEnumerable<TestCaseData> CorrectTimeToSecondTestCases
        {
            get
            {
                yield return new TestCaseData(GetUtcTime(12, 1, 15, 0, 0, 0), "43275");
                yield return new TestCaseData(GetUtcTime(12, 1, 15, +1, 3, 30), "30675");
                yield return new TestCaseData(GetUtcTime(12, 1, 15, -1, 3, 30), "55875");
                yield return new TestCaseData(GetUtcTime(1, 10, 15, +1, 3, 30), "78015");
                yield return new TestCaseData(GetUtcTime(21, 40, 15, -1, 3, 30), "4215");
            }
        }

        [Test, TestCaseSource(nameof(CorrectSecondsToTimeTestCases))]
        public void CorrectSecondsToTime_CorrectLocalTime(
            string seconds, TimeZone timeZone, string expectedLocalTime)
        {
            _mockDateTimeParser.Setup(p => p.ParseTimeZone(It.IsAny<string>())).Returns(timeZone);
            var converter = GetDateTimeConverter();

            Assert.AreEqual(expectedLocalTime, converter.ConvertToLocalTime(seconds, "stub_time_zone"));
        }

        public static IEnumerable<TestCaseData> CorrectSecondsToTimeTestCases
        {
            get
            {
                yield return new TestCaseData("43275", GetTimeZone(0, 0, 0), "12:01:15");
                yield return new TestCaseData("30675", GetTimeZone(+1, 3, 30), "12:01:15");
                yield return new TestCaseData("55875", GetTimeZone(-1, 3, 30), "12:01:15");
                yield return new TestCaseData("78015", GetTimeZone(+1, 3, 30), "01:10:15");
                yield return new TestCaseData("4215", GetTimeZone(-1, 3, 30), "21:40:15");
            }
        }

        [Test]
        public void NullDateTime_ArgumentNullException()
        {
            var converter = GetDateTimeConverter();

            Assert.Throws<ArgumentNullException>(() => converter.ConvertToSeconds(null));
        }

        [Test]
        public void NullTimeZone_ArgumentNullException()
        {
            var converter = GetDateTimeConverter();

            Assert.Throws<ArgumentNullException>(() => converter.ConvertToLocalTime("1", null));
        }

        [TestCase("-1")]
        [TestCase("86400")]
        [Test]
        public void SecondsOutOfDayRange_ArgumentOutOfRangeException(string seconds)
        {
            var converter = GetDateTimeConverter();

            Assert.Throws<ArgumentOutOfRangeException>
                (() => converter.ConvertToLocalTime(seconds, "+1:00"));
        }

        private static TimeZone GetTimeZone(int tzsign, int tzhours, int tzminutes)
        {
            return new TimeZone
            {
                Hours = tzhours,
                Minutes = tzminutes,
                TimeZoneSign = tzsign
            };
        }

        private static UtcTime GetUtcTime(
            int hours, int minutes, int seconds, int tzsign, int tzhours, int tzminutes)
        {
            return new UtcTime
            {
                Hours = hours,
                Minutes = minutes,
                Seconds = seconds,
                TimeZone = new TimeZone
                {
                    Hours = tzhours,
                    Minutes = tzminutes,
                    TimeZoneSign = tzsign
                }
            };
        }

        private DateTimeConverter GetDateTimeConverter()
        {
            return new DateTimeConverter(_mockDateTimeParser.Object);
        }
    }
}
