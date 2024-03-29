﻿using System;
using Xunit;

namespace TmEssentials.Tests;

public class TimeFormatterTests
{
    [Fact]
    public void ToTmString_Null()
    {
        var expected = "-:--.---";

        var actual = default(TimeInt32?).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NullHundredths()
    {
        var expected = "-:--.--";

        var actual = default(TimeInt32?).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_00F()
    {
        var expected = "0:00.003";

        var actual = TimeInt32.FromMilliseconds(3).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_0FF()
    {
        var expected = "0:00.033";

        var actual = TimeInt32.FromMilliseconds(33).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_FFF()
    {
        var expected = "0:00.333";

        var actual = TimeInt32.FromMilliseconds(333).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_FF()
    {
        var expected = "0:00.33";

        var actual = TimeInt32.FromMilliseconds(333).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_SSFFF()
    {
        var expected = "0:03.333";

        var actual = TimeInt32.FromSeconds(3.333f).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_SSFF()
    {
        var expected = "0:03.33";

        var actual = TimeInt32.FromSeconds(3.333f).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_MSSFFF()
    {
        var expected = "3:03.333";

        var actual = new TimeInt32(0, 0, 3, 3, 333).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_MSSFF()
    {
        var expected = "3:03.33";

        var actual = new TimeInt32(0, 0, 3, 3, 333).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_MMSSFFF()
    {
        var expected = "33:03.333";

        var actual = new TimeInt32(0, 0, 33, 3, 333).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_MMSSFF()
    {
        var expected = "33:03.33";

        var actual = new TimeInt32(0, 0, 33, 3, 333).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_HMMSSFFF()
    {
        var expected = "3:33:03.333";

        var actual = new TimeInt32(0, 3, 33, 3, 333).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_HMMSSFF()
    {
        var expected = "3:33:03.33";

        var actual = new TimeInt32(0, 3, 33, 3, 333).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_FullTimeWithoutHours()
    {
        var expected = "1:00:56:43.165";

        var actual = new TimeInt32(1, hours: 0, 56, 43, 165).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_FullTime()
    {
        var expected = "1:15:56:43.165";

        var actual = new TimeInt32(1, 15, 56, 43, 165).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_FullTimeHundredths()
    {
        var expected = "1:15:56:43.16";

        var actual = new TimeInt32(1, 15, 56, 43, 165).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeFFF()
    {
        var expected = "-0:00.333";

        var actual = (-TimeInt32.FromMilliseconds(333)).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeFF()
    {
        var expected = "-0:00.33";

        var actual = (-TimeInt32.FromMilliseconds(333)).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeSSFFF()
    {
        var expected = "-0:03.333";

        var actual = (-TimeInt32.FromSeconds(3.333f)).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeSSFF()
    {
        var expected = "-0:03.33";

        var actual = (-TimeInt32.FromSeconds(3.333f)).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeMSSFFF()
    {
        var expected = "-3:03.333";

        var actual = (-new TimeInt32(0, 0, 3, 3, 333)).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeMSSFF()
    {
        var expected = "-3:03.33";

        var actual = (-new TimeInt32(0, 0, 3, 3, 333)).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeMMSSFFF()
    {
        var expected = "-33:03.333";

        var actual = (-new TimeInt32(0, 0, 33, 3, 333)).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeMMSSFF()
    {
        var expected = "-33:03.33";

        var actual = (-new TimeInt32(0, 0, 33, 3, 333)).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeHMMSSFFF()
    {
        var expected = "-3:33:03.333";

        var actual = (-new TimeInt32(0, 3, 33, 3, 333)).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_NegativeHMMSSFF()
    {
        var expected = "-3:33:03.33";

        var actual = (-new TimeInt32(0, 3, 33, 3, 333)).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_FullNegativeTime()
    {
        var expected = "-1:15:56:43.165";

        var actual = (-new TimeInt32(1, 15, 56, 43, 165)).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_FullNegativeTimeHundredths()
    {
        var expected = "-1:15:56:43.16";

        var actual = (-new TimeInt32(1, 15, 56, 43, 165)).ToTmString(true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_Null()
    {
        var expected = "-'--''---";

        var actual = default(TimeInt32?).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NullHundredths()
    {
        var expected = "-'--''--";

        var actual = default(TimeInt32?).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_FFF()
    {
        var expected = "0'00''333";

        var actual = TimeInt32.FromMilliseconds(333).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_FF()
    {
        var expected = "0'00''33";

        var actual = TimeInt32.FromMilliseconds(333).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_SSFFF()
    {
        var expected = "0'03''333";

        var actual = TimeInt32.FromSeconds(3.333f).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_SSFF()
    {
        var expected = "0'03''33";

        var actual = TimeInt32.FromSeconds(3.333f).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_MSSFFF()
    {
        var expected = "3'03''333";

        var actual = new TimeInt32(0, 0, 3, 3, 333).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_MSSFF()
    {
        var expected = "3'03''33";

        var actual = new TimeInt32(0, 0, 3, 3, 333).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_MMSSFFF()
    {
        var expected = "33'03''333";

        var actual = new TimeInt32(0, 0, 33, 3, 333).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_MMSSFF()
    {
        var expected = "33'03''33";

        var actual = new TimeInt32(0, 0, 33, 3, 333).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_HMMSSFFF()
    {
        var expected = "3'33'03''333";

        var actual = new TimeInt32(0, 3, 33, 3, 333).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_HMMSSFF()
    {
        var expected = "3'33'03''33";

        var actual = new TimeInt32(0, 3, 33, 3, 333).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_FullTimeWithoutHours()
    {
        var expected = "1'00'56'43''165";

        var actual = new TimeInt32(1, hours: 0, 56, 43, 165).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_FullTime()
    {
        var expected = "1'15'56'43''165";

        var actual = new TimeInt32(1, 15, 56, 43, 165).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_FullTimeHundredths()
    {
        var expected = "1'15'56'43''16";

        var actual = new TimeInt32(1, 15, 56, 43, 165).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeFFF()
    {
        var expected = "-0'00''333";

        var actual = (-TimeInt32.FromMilliseconds(333)).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeFF()
    {
        var expected = "-0'00''33";

        var actual = (-TimeInt32.FromMilliseconds(333)).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeSSFFF()
    {
        var expected = "-0'03''333";

        var actual = (-TimeInt32.FromSeconds(3.333f)).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeSSFF()
    {
        var expected = "-0'03''33";

        var actual = (-TimeInt32.FromSeconds(3.333f)).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeMSSFFF()
    {
        var expected = "-3'03''333";

        var actual = (-new TimeInt32(0, 0, 3, 3, 333)).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeMSSFF()
    {
        var expected = "-3'03''33";

        var actual = (-new TimeInt32(0, 0, 3, 3, 333)).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeMMSSFFF()
    {
        var expected = "-33'03''333";

        var actual = (-new TimeInt32(0, 0, 33, 3, 333)).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeMMSSFF()
    {
        var expected = "-33'03''33";

        var actual = (-new TimeInt32(0, 0, 33, 3, 333)).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeHMMSSFFF()
    {
        var expected = "-3'33'03''333";

        var actual = (-new TimeInt32(0, 3, 33, 3, 333)).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_NegativeHMMSSFF()
    {
        var expected = "-3'33'03''33";

        var actual = (-new TimeInt32(0, 3, 33, 3, 333)).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_FullNegativeTime()
    {
        var expected = "-1'15'56'43''165";

        var actual = (-new TimeInt32(1, 15, 56, 43, 165)).ToTmString(useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Apostrophe_FullNegativeTimeHundredths()
    {
        var expected = "-1'15'56'43''16";

        var actual = (-new TimeInt32(1, 15, 56, 43, 165)).ToTmString(true, useApostrophe: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days1Digit()
    {
        var expected = "1:00:00:00.000";

        var actual = new TimeInt32(1, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days2Digit()
    {
        var expected = "10:00:00:00.000";

        var actual = new TimeInt32(10, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days3Digit()
    {
        var expected = "100:00:00:00.000";

        var actual = new TimeSingle(100, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days4Digit()
    {
        var expected = "1000:00:00:00.000";

        var actual = new TimeSingle(1000, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days5Digit()
    {
        var expected = "10000:00:00:00.000";

        var actual = new TimeSpan(10000, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days6Digit()
    {
        var expected = "100000:00:00:00.000";

        var actual = new TimeSpan(100000, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days7Digit()
    {
        var expected = "1000000:00:00:00.000";

        var actual = new TimeSpan(1000000, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTmString_Days8Digit()
    {
        var expected = "10000000:00:00:00.000";

        var actual = new TimeSpan(10000000, 0, 0, 0, 0).ToTmString();

        Assert.Equal(expected, actual);
    }
}
