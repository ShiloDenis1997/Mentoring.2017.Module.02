using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Task2.Test
{
    [TestFixture]
    public class StringToNumberParsersTests
    {
        [TestCase("-1", -1)]
        [TestCase("1", 1)]
        [TestCase("+1", 1)]
        [TestCase("0", 0)]
        [TestCase("-1234", -1234)]
        [TestCase("1234", 1234)]
        [TestCase("+1234", 1234)]
        [TestCase("2147483647", int.MaxValue)]
        [TestCase("-2147483648", int.MinValue)]
        [Test]
        public void ValidIntString_ParsedIntResult(string number, int expected)
        {
            int actual = number.ParseInt();

            Assert.AreEqual(expected, actual);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [Test]
        public void InvalidArgument_ArgumentExceptionThrown(string number)
        {
            Assert.Throws<ArgumentException>(() => number.ParseInt());
        }

        [TestCase("2147483648")]
        [TestCase("-2147483649")]
        [Test]
        public void TooBigNumber_OverflowExceptionThrown(string number)
        {
            Assert.Throws<OverflowException>(() => number.ParseInt());
        }

        [TestCase("++123")]
        [TestCase("--123")]
        [TestCase("12.3")]
        [TestCase("1,23")]
        [Test]
        public void InvalidNumberFormat_FormatExceptionThrown(string number)
        {
            Assert.Throws<FormatException>(() => number.ParseInt());
        }
    }
}
