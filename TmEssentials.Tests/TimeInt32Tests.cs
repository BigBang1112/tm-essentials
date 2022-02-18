using Xunit;

namespace TmEssentials.Tests;

public class TimeInt32Tests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(50, 50)]
    [InlineData(2000, 2000)]
    public void TotalMilliseconds_ShouldBeCorrect(int totalMilliseconds, int expectedTotalMilliseconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedTotalMilliseconds, actual: timeInt.TotalMilliseconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(900, 0.9f)]
    [InlineData(1000, 1)]
    [InlineData(3200, 3.2f)]
    public void TotalSeconds_ShouldBeCorrect(int totalMilliseconds, float expectedTotalSeconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedTotalSeconds, actual: timeInt.TotalSeconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(66_000, 1.1f)]
    [InlineData(192_000, 3.2f)]
    public void TotalMinutes_ShouldBeCorrect(int totalMilliseconds, float expectedTotalMinutes)
    {
        var timeInt = new TimeInt32(totalMilliseconds);
        
        Assert.Equal(expectedTotalMinutes, actual: timeInt.TotalMinutes);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1_800_000, 0.5f)]
    [InlineData(3_600_000, 1)]
    [InlineData(5_400_000, 1.5f)]
    public void TotalHours_ShouldBeCorrect(int totalMilliseconds, float expectedTotalHours)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedTotalHours, actual: timeInt.TotalHours);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(86_400_000, 1)]
    [InlineData(43_200_000, 0.5f)]
    public void TotalDays_ShouldBeCorrect(int totalMilliseconds, float expectedTotalDays)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedTotalDays, actual: timeInt.TotalDays);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-2, -20_000)]
    [InlineData(1, 10_000)]
    [InlineData(3, 30_000)]
    public void Ticks_ShouldBeCorrect(int totalMilliseconds, long expectedTicks)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedTicks, actual: timeInt.Ticks);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1200, -200)]
    [InlineData(500, 500)]
    [InlineData(1200, 200)]
    [InlineData(2600, 600)]
    public void Milliseconds_ShouldBeCorrect(int totalMilliseconds, float expectedMilliseconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedMilliseconds, actual: timeInt.Milliseconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(3_000, 3)]
    [InlineData(59_999, 59)]
    [InlineData(60_000, 0)]
    [InlineData(60_999, 0)]
    [InlineData(61_000, 1)]
    public void Seconds_ShouldBeCorrect(int totalMilliseconds, int expectedSeconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedSeconds, actual: timeInt.Seconds);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(59_999, 0)]
    [InlineData(60_000, 1)]
    [InlineData(61_000, 1)]
    [InlineData(320_000, 5)]
    [InlineData(3_680_000, 1)]
    public void Minutes_ShouldBeCorrect(int totalMilliseconds, int expectedMinutes)
    {
        var timeSingle = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedMinutes, actual: timeSingle.Minutes);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(3_599_999, 0)]
    [InlineData(3_600_000, 1)]
    [InlineData(3_601_000, 1)]
    [InlineData(10_000_000, 2)]
    [InlineData(10_800_000, 3)]
    public void Hours_ShouldBeCorrect(int totalMilliseconds, int expectedHours)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedHours, actual: timeInt.Hours);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(86_399_990, 0)]
    [InlineData(86_400_000, 1)]
    [InlineData(86_401_000, 1)]
    [InlineData(172_799_000, 1)]
    [InlineData(172_800_000, 2)]
    public void Days_ShouldBeCorrect(int totalMilliseconds, int expectedDays)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedDays, actual: timeInt.Days);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(990, 0.99f)]
    [InlineData(1000, 1)]
    [InlineData(5500, 5.5f)]
    public void New_TotalSeconds_ShouldBeEqual(int totalMilliseconds, float expectedTotalSeconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        Assert.Equal(expectedTotalSeconds, actual: timeInt.TotalSeconds);
    }

    [Theory]
    [InlineData(-1, -2, -3)]
    [InlineData(0, 0, 0)]
    [InlineData(5, 10, 15)]
    [InlineData(13, 18, 7)]
    public void New_HoursMinutesSeconds_ShouldBeEqual(int hours, int minutes, int seconds)
    {
        var timeInt = new TimeInt32(hours, minutes, seconds);

        Assert.Equal(expected: hours, actual: timeInt.Hours);
        Assert.Equal(expected: minutes, actual: timeInt.Minutes);
        Assert.Equal(expected: seconds, actual: timeInt.Seconds);
    }

    [Theory]
    [InlineData(0, -2, -3, -4, -5)]
    [InlineData(0, 0, 0, 0, 0)]
    [InlineData(0, 10, 15, 20, 24)]
    [InlineData(0, 18, 7, 1, 8)]
    public void New_DaysHoursMinutesSecondsMilliseconds_ShouldBeEqual(int days, int hours, int minutes, int seconds, int milliseconds)
    {
        var timeInt = new TimeInt32(days, hours, minutes, seconds, milliseconds);

        Assert.Equal(expected: days, actual: timeInt.Days);
        Assert.Equal(expected: hours, actual: timeInt.Hours);
        Assert.Equal(expected: minutes, actual: timeInt.Minutes);
        Assert.Equal(expected: seconds, actual: timeInt.Seconds);
        Assert.Equal(expected: milliseconds, actual: timeInt.Milliseconds);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromDays_TotalDaysShouldBeEqual(float days)
    {
        var timeInt = TimeInt32.FromDays(days);

        Assert.Equal(expected: days, actual: timeInt.TotalDays);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromHours_TotalHoursShouldBeEqual(float hours)
    {
        var timeInt = TimeInt32.FromHours(hours);

        Assert.Equal(expected: hours, actual: timeInt.TotalHours);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(12)]
    public void FromMilliseconds_TotalMillisecondsShouldBeEqual(int milliseconds)
    {
        var timeInt = TimeInt32.FromMilliseconds(milliseconds);

        Assert.Equal(expected: milliseconds, actual: timeInt.TotalMilliseconds);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromMinutes_TotalMinutesShouldBeEqual(float minutes)
    {
        var timeInt = TimeInt32.FromMinutes(minutes);

        Assert.Equal(expected: minutes, actual: timeInt.TotalMinutes);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6.5f)]
    [InlineData(12)]
    public void FromSeconds_TotalSecondsShouldBeEqual(float seconds)
    {
        var timeInt = TimeInt32.FromSeconds(seconds);

        Assert.Equal(expected: seconds, actual: timeInt.TotalSeconds);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10_000)]
    [InlineData(10_000)]
    [InlineData(100_000)]
    [InlineData(5_100_000)]
    public void FromTicks_TicksShouldBeEqual(long ticks)
    {
        var timeInt = TimeInt32.FromTicks(ticks);

        Assert.Equal(expected: ticks, actual: timeInt.Ticks);
    }

    [Theory]
    [InlineData(5, 10, 15)]
    [InlineData(-1, -2, -3)]
    [InlineData(0, 0, 0)]
    [InlineData(12, 15.5f, 27.5f)]
    public void Add_ShouldAdd(int totalMilliseconds1, int totalMilliseconds2, int totalMillisecondsResult)
    {
        var timeInt1 = new TimeInt32(totalMilliseconds1);
        var timeInt2 = new TimeInt32(totalMilliseconds2);

        var result = timeInt1.Add(timeInt2);

        Assert.Equal(expected: totalMillisecondsResult, actual: result.TotalMilliseconds);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 0.5f, 2)]
    [InlineData(10, 2, 5)]
    public void Divide_Divisor_ShouldDivide(int totalMilliseconds, float divisor, float totalMillisecondsResult)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        var result = timeInt.Divide(divisor);
        
        Assert.Equal(expected: totalMillisecondsResult, actual: result.TotalMilliseconds);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(6, 2, 3)]
    [InlineData(14, 2, 7)]
    public void Divide_Time_ShouldDivide(int totalMilliseconds, int totalMillisecondsDivisor, float totalSecondsResult)
    {
        var timeInt = new TimeInt32(totalMilliseconds);
        var divisor = new TimeInt32(totalMillisecondsDivisor);

        var result = timeInt.Divide(divisor);

        Assert.Equal(expected: totalSecondsResult, actual: result);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(10, 10)]
    [InlineData(-10, 10)]
    public void Duration_ShouldReturnAbsoluteValue(int totalMilliseconds, int totalMillisecondsExpected)
    {
        var timeInt = new TimeInt32(totalMilliseconds);
        var timeIntExpected = new TimeInt32(totalMillisecondsExpected);

        var result = timeInt.Duration();

        Assert.Equal(timeIntExpected, actual: result);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(2, 0.5f, 1)]
    [InlineData(10, 2, 20)]
    public void Multiply_ShouldMultiplyByFactor(int totalMilliseconds, float factor, int totalMillisecondsResult)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        var result = timeInt.Multiply(factor);

        Assert.Equal(expected: totalMillisecondsResult, actual: result.TotalMilliseconds);
    }
}
