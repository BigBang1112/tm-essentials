using Xunit;

namespace TmEssentials.Tests;

public class TimeSingleTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(5.6f, 5.6f)]
    [InlineData(100, 100)]
    public void TotalSeconds_ShouldBeCorrect(float totalSeconds, float expectedTotalSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedTotalSeconds, actual: timeSingle.TotalSeconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1000)]
    [InlineData(5.6f, 5600)]
    [InlineData(100, 100000)]
    public void TotalMilliseconds_ShouldBeCorrect(float totalSeconds, float expectedTotalMilliseconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedTotalMilliseconds, actual: timeSingle.TotalMilliseconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(150, 2.5f)]
    [InlineData(7.5f, 0.125f)]
    [InlineData(1200, 20)]
    public void TotalMinutes_ShouldBeCorrect(float totalSeconds, float expectedTotalMinutes)
    {
        var timeSingle = new TimeSingle(totalSeconds);
        
        Assert.Equal(expectedTotalMinutes, actual: timeSingle.TotalMinutes);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5.4f, 0.0015f)]
    [InlineData(1800, 0.5f)]
    [InlineData(10800, 3)]
    public void TotalHours_ShouldBeCorrect(float totalSeconds, float expectedTotalHours)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedTotalHours, actual: timeSingle.TotalHours);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10627.2f, 0.123f)]
    [InlineData(60480, 0.7f)]
    [InlineData(172800, 2)]
    public void TotalDays_ShouldBeCorrect(float totalSeconds, float expectedTotalDays)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedTotalDays, actual: timeSingle.TotalDays);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.05f, 500_000)]
    [InlineData(0.5f, 5_000_000)]
    [InlineData(1, 10_000_000)]
    public void Ticks_ShouldBeCorrect(float totalSeconds, long expectedTicks)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedTicks, actual: timeSingle.Ticks);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(5.6f, 600)]
    [InlineData(11.12f, 120)]
    [InlineData(100.2455f, 245.5f)]
    public void Milliseconds_ShouldBeCorrect(float totalSeconds, float expectedMilliseconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedMilliseconds, actual: timeSingle.Milliseconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(5.6f, 600)]
    [InlineData(11.12f, 120)]
    [InlineData(100.2455f, 245)]
    public void Milliseconds_ITime_ShouldBeCorrect(float totalSeconds, int expectedMilliseconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedMilliseconds, actual: (timeSingle as ITime).Milliseconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(5.6f, 5)]
    [InlineData(100, 40)]
    [InlineData(1112, 32)]
    public void Seconds_ShouldBeCorrect(float totalSeconds, int expectedSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedSeconds, actual: timeSingle.Seconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(59.99f, 0)]
    [InlineData(60, 1)]
    [InlineData(61, 1)]
    [InlineData(320, 5)]
    [InlineData(3680, 1)]
    public void Minutes_ShouldBeCorrect(float totalSeconds, int expectedMinutes)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedMinutes, actual: timeSingle.Minutes);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(3599.99f, 0)]
    [InlineData(3600, 1)]
    [InlineData(3601, 1)]
    [InlineData(10000, 2)]
    [InlineData(10800, 3)]
    public void Hours_ShouldBeCorrect(float totalSeconds, int expectedHours)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedHours, actual: timeSingle.Hours);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(86399.99f, 0)]
    [InlineData(86400, 1)]
    [InlineData(86401, 1)]
    [InlineData(172799, 1)]
    [InlineData(172800, 2)]
    public void Days_ShouldBeCorrect(float totalSeconds, int expectedDays)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expectedDays, actual: timeSingle.Days);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5.5f)]
    [InlineData(20)]
    public void New_TotalSeconds_ShouldBeEqual(float totalSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        Assert.Equal(expected: totalSeconds, actual: timeSingle.TotalSeconds);
    }

    [Theory]
    [InlineData(-1, -2, -3)]
    [InlineData(0, 0, 0)]
    [InlineData(5, 10, 15)]
    [InlineData(13, 18, 7)]
    public void New_HoursMinutesSeconds_ShouldBeEqual(int hours, int minutes, int seconds)
    {
        var timeSingle = new TimeSingle(hours, minutes, seconds);

        Assert.Equal(expected: hours, actual: timeSingle.Hours);
        Assert.Equal(expected: minutes, actual: timeSingle.Minutes);
        Assert.Equal(expected: seconds, actual: timeSingle.Seconds);
    }

    [Theory]
    [InlineData(0, -2, -3, -4, -5)]
    [InlineData(0, 0, 0, 0, 0)]
    [InlineData(0, 10, 15, 20, 24)]
    [InlineData(0, 18, 7, 1, 8)]
    public void New_DaysHoursMinutesSecondsMilliseconds_ShouldBeEqual(int days, int hours, int minutes, int seconds, float milliseconds)
    {
        var timeSingle = new TimeSingle(days, hours, minutes, seconds, milliseconds);

        Assert.Equal(expected: days, actual: timeSingle.Days);
        Assert.Equal(expected: hours, actual: timeSingle.Hours);
        Assert.Equal(expected: minutes, actual: timeSingle.Minutes);
        Assert.Equal(expected: seconds, actual: timeSingle.Seconds);
        Assert.Equal(expected: milliseconds, actual: timeSingle.Milliseconds);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromDays_TotalDaysShouldBeEqual(float days)
    {
        var timeSingle = TimeSingle.FromDays(days);

        Assert.Equal(expected: days, actual: timeSingle.TotalDays);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromHours_TotalHoursShouldBeEqual(float hours)
    {
        var timeSingle = TimeSingle.FromHours(hours);

        Assert.Equal(expected: hours, actual: timeSingle.TotalHours);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromMilliseconds_TotalMillisecondsShouldBeEqual(float milliseconds)
    {
        var timeSingle = TimeSingle.FromMilliseconds(milliseconds);

        Assert.Equal(expected: milliseconds, actual: timeSingle.TotalMilliseconds);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromMinutes_TotalMinutesShouldBeEqual(float minutes)
    {
        var timeSingle = TimeSingle.FromMinutes(minutes);

        Assert.Equal(expected: minutes, actual: timeSingle.TotalMinutes);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromSeconds_TotalSecondsShouldBeEqual(float seconds)
    {
        var timeSingle = TimeSingle.FromSeconds(seconds);

        Assert.Equal(expected: seconds, actual: timeSingle.TotalSeconds);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1_000)]
    [InlineData(1_000)]
    [InlineData(100_000)]
    [InlineData(5_100_000)]
    public void FromTicks_TicksShouldBeEqual(long ticks)
    {
        var timeSingle = TimeSingle.FromTicks(ticks);

        Assert.Equal(expected: ticks, actual: timeSingle.Ticks);
    }

    [Theory]
    [InlineData(5, 10, 15)]
    [InlineData(-1, -2, -3)]
    [InlineData(0, 0, 0)]
    [InlineData(12, 15.5f, 27.5f)]
    public void Add_ShouldAdd(float totalSeconds1, float totalSeconds2, float totalSecondsResult)
    {
        var timeSingle1 = new TimeSingle(totalSeconds1);
        var timeSingle2 = new TimeSingle(totalSeconds2);

        var result = timeSingle1.Add(timeSingle2);

        Assert.Equal(expected: totalSecondsResult, actual: result.TotalSeconds);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 0.5f, 2)]
    [InlineData(10, 2, 5)]
    public void Divide_Divisor_ShouldDivide(float totalSeconds, float divisor, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var result = timeSingle.Divide(divisor);
        
        Assert.Equal(expected: totalSecondsResult, actual: result.TotalSeconds);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 0.5f, 2)]
    [InlineData(10, 2, 5)]
    public void Divide_Time_ShouldDivide(float totalSeconds, float totalSecondsDivisor, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);
        var divisor = new TimeSingle(totalSecondsDivisor);

        var result = timeSingle.Divide(divisor);

        Assert.Equal(expected: totalSecondsResult, actual: result);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(10, 10)]
    [InlineData(-10, 10)]
    public void Duration_ShouldReturnAbsoluteValue(float totalSeconds, float totalSecondsExpected)
    {
        var timeSingle = new TimeSingle(totalSeconds);
        var timeSingleExpected = new TimeSingle(totalSecondsExpected);

        var result = timeSingle.Duration();

        Assert.Equal(timeSingleExpected, actual: result);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 0.5f, 0.5f)]
    [InlineData(10, 2, 20)]
    public void Multiply_ShouldMultiplyByFactor(float totalSeconds, float totalSecondsFactor, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var result = timeSingle.Multiply(totalSecondsFactor);

        Assert.Equal(expected: totalSecondsResult, actual: result.TotalSeconds);
    }
}
