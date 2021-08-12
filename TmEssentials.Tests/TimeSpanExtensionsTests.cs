using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TmEssentials.Tests
{
    public class TimeSpanExtensionsTests
    {
        [Fact]
        public void ToMilliseconds_WithoutTicks()
        {
            var expected = 23456;

            var actual = new TimeSpan(0, 0, 0, 23, 456).ToMilliseconds();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToMilliseconds_WithTicks()
        {
            var expected = 23456;

            var actual = new TimeSpan(234567890).ToMilliseconds();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_Null()
        {
            var expected = "-:--.---";

            var actual = default(TimeSpan?).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NullHundredths()
        {
            var expected = "-:--.--";

            var actual = default(TimeSpan?).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_FFF()
        {
            var expected = "0:00.333";

            var actual = TimeSpan.FromMilliseconds(333).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_FF()
        {
            var expected = "0:00.33";

            var actual = TimeSpan.FromMilliseconds(333).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_SSFFF()
        {
            var expected = "0:03.333";

            var actual = TimeSpan.FromSeconds(3.333).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_SSFF()
        {
            var expected = "0:03.33";

            var actual = TimeSpan.FromSeconds(3.333).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_MSSFFF()
        {
            var expected = "3:03.333";

            var actual = new TimeSpan(0, 0, 3, 3, 333).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_MSSFF()
        {
            var expected = "3:03.33";

            var actual = new TimeSpan(0, 0, 3, 3, 333).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_MMSSFFF()
        {
            var expected = "33:03.333";

            var actual = new TimeSpan(0, 0, 33, 3, 333).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_MMSSFF()
        {
            var expected = "33:03.33";

            var actual = new TimeSpan(0, 0, 33, 3, 333).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_HMMSSFFF()
        {
            var expected = "3:33:03.333";

            var actual = new TimeSpan(0, 3, 33, 3, 333).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_HMMSSFF()
        {
            var expected = "3:33:03.33";

            var actual = new TimeSpan(0, 3, 33, 3, 333).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_FullTimeWithoutHours()
        {
            var expected = "1:00:56:43.165";

            var actual = new TimeSpan(1, hours: 0, 56, 43, 165).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_FullTime()
        {
            var expected = "1:15:56:43.165";

            var actual = new TimeSpan(1, 15, 56, 43, 165).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_FullTimeHundredths()
        {
            var expected = "1:15:56:43.16";

            var actual = new TimeSpan(1, 15, 56, 43, 165).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeFFF()
        {
            var expected = "-0:00.333";

            var actual = (-TimeSpan.FromMilliseconds(333)).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeFF()
        {
            var expected = "-0:00.33";

            var actual = (-TimeSpan.FromMilliseconds(333)).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeSSFFF()
        {
            var expected = "-0:03.333";

            var actual = (-TimeSpan.FromSeconds(3.333)).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeSSFF()
        {
            var expected = "-0:03.33";

            var actual = (-TimeSpan.FromSeconds(3.333)).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeMSSFFF()
        {
            var expected = "-3:03.333";

            var actual = (-new TimeSpan(0, 0, 3, 3, 333)).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeMSSFF()
        {
            var expected = "-3:03.33";

            var actual = (-new TimeSpan(0, 0, 3, 3, 333)).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeMMSSFFF()
        {
            var expected = "-33:03.333";

            var actual = (-new TimeSpan(0, 0, 33, 3, 333)).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeMMSSFF()
        {
            var expected = "-33:03.33";

            var actual = (-new TimeSpan(0, 0, 33, 3, 333)).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeHMMSSFFF()
        {
            var expected = "-3:33:03.333";

            var actual = (-new TimeSpan(0, 3, 33, 3, 333)).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_NegativeHMMSSFF()
        {
            var expected = "-3:33:03.33";

            var actual = (-new TimeSpan(0, 3, 33, 3, 333)).ToTmString(true);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_FullNegativeTime()
        {
            var expected = "-1:15:56:43.165";

            var actual = (-new TimeSpan(1, 15, 56, 43, 165)).ToTmString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToTmString_FullNegativeTimeHundredths()
        {
            var expected = "-1:15:56:43.16";

            var actual = (-new TimeSpan(1, 15, 56, 43, 165)).ToTmString(true);

            Assert.Equal(expected, actual);
        }
    }
}
