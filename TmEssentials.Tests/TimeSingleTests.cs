using System;
using Xunit;

namespace TmEssentials.Tests;

public class TimeSingleTests
{
    [Fact]
    public void Constructor_WithTotalSeconds()
    {
        var expected = 2.3f;

        var actual = new TimeSingle(expected).TotalSeconds;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Constructor_InitTotalSeconds()
    {
        var expected = 2.3f;

        var actual = new TimeSingle() { TotalSeconds = expected }.TotalSeconds;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Zero_IsZero()
    {
        var expected = new TimeSingle();

        var actual = TimeSingle.Zero;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Zero_IsMaxSingleValue()
    {
        var expected = new TimeSingle(float.MaxValue);

        var actual = TimeSingle.MaxValue;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Zero_IsMinSingleValue()
    {
        var expected = new TimeSingle(float.MinValue);

        var actual = TimeSingle.MinValue;

        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ToString_Override_UsesDefaultParamValues()
    {
        var expected = ((object)new TimeSingle(234567890)).ToString();

        var actual = new TimeSingle(234567890).ToString(useHundredths: false, useApostrophe: false);

        Assert.Equal(expected, actual);
    }
    
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
    [InlineData(5.5f, 500)]
    [InlineData(11.1f, 100)]
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
    [InlineData(0, -2, -3, -4, -100)]
    [InlineData(0, 0, 0, 0, 0)]
    [InlineData(0, 10, 15, 20, 100)]
    [InlineData(0, 18, 7, 1, 100)]
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
    [InlineData(-10_000)]
    [InlineData(10_000)]
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

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    [InlineData(5.5f, -5.5f)]
    [InlineData(-8.5f, 8.5f)]
    public void Negate_ShouldNegate(float totalSeconds, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var result = timeSingle.Negate();

        Assert.Equal(expected: totalSecondsResult, actual: result.TotalSeconds);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5.5f)]
    [InlineData(-8.5f)]
    public void ToTimeInt32_ConvertsCorrectly(float totalSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var timeInt = timeSingle.ToTimeInt32();

        Assert.Equal(expected: timeSingle.TotalSeconds, actual: timeInt.TotalSeconds);
    }

    [Fact]
    public void GreaterThanOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(2.0f); // 2 seconds
        var time2 = new TimeSingle(1.5f); // 1.5 seconds

        Assert.True(time1 > time2);
        Assert.False(time2 > time1);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(2.0f);
        var time2 = new TimeSingle(2.0f);
        var time3 = new TimeSingle(1.5f);

        Assert.True(time1 >= time2);
        Assert.True(time1 >= time3);
        Assert.False(time3 >= time1);
    }

    [Fact]
    public void LessThanOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(1.5f);
        var time2 = new TimeSingle(2.0f);

        Assert.True(time1 < time2);
        Assert.False(time2 < time1);
    }

    [Fact]
    public void LessThanOrEqualOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(2.0f);
        var time2 = new TimeSingle(2.0f);
        var time3 = new TimeSingle(2.5f);

        Assert.True(time1 <= time2);
        Assert.True(time1 <= time3);
        Assert.False(time3 <= time1);
    }

    [Fact]
    public void EqualityOperator_TimeSingleAndTimeInt32_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(1.0f); // 1 second
        var timeInt32 = new TimeInt32(1000); // 1000 milliseconds

        Assert.True(timeSingle == timeInt32);
    }

    [Fact]
    public void InequalityOperator_TimeSingleAndTimeInt32_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(1.5f); // 1.5 seconds
        var timeInt32 = new TimeInt32(1000); // 1000 milliseconds

        Assert.True(timeSingle != timeInt32);
    }

    [Fact]
    public void UnaryPlusOperator_ReturnsSameValue()
    {
        var time = new TimeSingle(1.5f); // 1.5 seconds
        var result = +time;

        Assert.Equal(time.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void AddTwoTimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(1.0f); // 1 second
        var time2 = new TimeSingle(2.5f); // 2.5 seconds
        var expected = new TimeSingle(3.5f);

        var result = time1 + time2;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void AddTimeSingleAndTimeSpan_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(1.0f); // 1 second
        var timeSpan = TimeSpan.FromSeconds(2.5); // 2.5 seconds
        var expected = new TimeSingle(3.5f);

        var result = timeSingle + timeSpan;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void UnaryMinusOperator_ReturnsNegatedValue()
    {
        var time = new TimeSingle(1.5f); // 1.5 seconds
        var expected = new TimeSingle(-1.5f);

        var result = -time;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void SubtractTwoTimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(3.5f); // 3.5 seconds
        var time2 = new TimeSingle(1.0f); // 1 second
        var expected = new TimeSingle(2.5f);

        var result = time1 - time2;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void SubtractTimeSpanFromTimeSingle_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(3.5f); // 3.5 seconds
        var timeSpan = TimeSpan.FromSeconds(1.0); // 1 second
        var expected = new TimeSingle(2.5f);

        var result = timeSingle - timeSpan;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void MultiplyFloatAndTimeSingle_ReturnsTimeSingle()
    {
        var time = new TimeSingle(2.0f); // 2 seconds
        float factor = 2.5f;
        var expected = new TimeSingle(5.0f); // 5 seconds

        var result = factor * time;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void MultiplyTimeSingleAndFloat_ReturnsTimeSingle()
    {
        var time = new TimeSingle(1.5f); // 1.5 seconds
        float factor = 2f;
        var expected = new TimeSingle(3.0f); // 3 seconds

        var result = time * factor;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void DivideTimeSingleByFloat_ReturnsTimeSingle()
    {
        var time = new TimeSingle(3.0f); // 3 seconds
        float divisor = 2f;
        var expected = new TimeSingle(1.5f); // 1.5 seconds

        var result = time / divisor;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void DivideTimeSingleByTimeSingle_ReturnsFloat()
    {
        var time1 = new TimeSingle(3.0f); // 3 seconds
        var time2 = new TimeSingle(1.5f); // 1.5 seconds
        float expected = 2.0f; // 3 / 1.5

        var result = time1 / time2;

        Assert.Equal(expected, result, precision: 3);
    }

    [Fact]
    public void ImplicitConversionFromTimeSingleToTimeSpan()
    {
        var timeSingle = new TimeSingle(1.5f); // 1.5 seconds
        TimeSpan expected = TimeSpan.FromSeconds(1.5);

        TimeSpan result = timeSingle;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ImplicitConversionFromTimeSpanToTimeSingle()
    {
        var timeSpan = TimeSpan.FromSeconds(2.0); // 2 seconds
        var expected = new TimeSingle(2.0f);

        TimeSingle result = timeSpan;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void ImplicitConversionFromTimeInt32ToTimeSingle()
    {
        var timeInt32 = new TimeInt32(3000); // 3000 milliseconds
        var expected = new TimeSingle(3.0f); // 3 seconds

        TimeSingle result = timeInt32;

        Assert.Equal(expected.TotalSeconds, result.TotalSeconds, precision: 3);
    }

    [Fact]
    public void CompareTo_WithNull_ReturnsPositive()
    {
        var time = new TimeSingle(1); // Assuming this class implements ITime
        int result = time.CompareTo(null);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareTo_WithGreaterTotalSeconds_ReturnsNegative()
    {
        var time = new TimeSingle(1f);
        var other = new TimeSingle(1.5f);

        int result = time.CompareTo(other);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_WithLesserTotalSeconds_ReturnsPositive()
    {
        var time = new TimeSingle(1.5f);
        var other = new TimeSingle(1);

        int result = time.CompareTo(other);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareTo_WithEqualTotalSeconds_ReturnsZero()
    {
        var time = new TimeSingle(1);
        var other = new TimeSingle(1);

        int result = time.CompareTo(other);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareToObject_WithNull_ReturnsPositive()
    {
        var time = new TimeSingle(1.0f); // Replace with your class that implements ITime
        int result = time.CompareTo(default(object));

        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData(1f, 1.5f, -1)]
    [InlineData(1.5f, 1f, 1)]
    [InlineData(1f, 1f, 0)]
    public void CompareToObject_WithITimeObject_ReturnsCorrectComparison(float timeFloat, float otherFloat, int expected)
    {
        var time = new TimeSingle(timeFloat);
        object other = new TimeSingle(otherFloat); // other implements ITime

        int result = time.CompareTo(other);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CompareToObject_WithNonITimeObject_ThrowsArgumentException()
    {
        var time = new TimeSingle(1.0f);
        object nonTimeObject = new Random(); // An object that does not implement ITime

        Assert.Throws<ArgumentException>(() => time.CompareTo(nonTimeObject));
    }

    [Theory]
    [InlineData(1f, 1500, -1)]
    [InlineData(1.5f, 1000, 1)]
    [InlineData(1f, 1000, 0)]
    public void CompareTo_WithTimeInt32_ReturnsCorrectComparison(float timeFloat, int otherInt, int expected)
    {
        var time = new TimeSingle(timeFloat);
        var other = new TimeInt32(otherInt);

        int result = time.CompareTo(other);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equals_WithNullITime_ReturnsFalse()
    {
        var time = new TimeSingle(1.0f);

        Assert.False(time.Equals(null));
    }

    [Fact]
    public void Equals_WithTimeInt32_ReturnsFalse()
    {
        var time = new TimeSingle(2);
        var other = new TimeInt32(1000);
        Assert.False(time.Equals(other));
    }

    [Fact]
    public void TryParse_WithValidString_ReturnsTrue()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var result = TimeSingle.TryParse(str, out var parsed);
        Assert.True(result);
        Assert.Equal(time, parsed);
    }

    [Fact]
    public void TryParse_WithInvalidString_ReturnsFalse()
    {
        var str = "invalid";
        var result = TimeSingle.TryParse(str, out var parsed);
        Assert.False(result);
        Assert.Equal(TimeSingle.Zero, parsed);
    }

    [Fact]
    public void TryParse_WithNullString_ReturnsFalse()
    {
        var result = TimeSingle.TryParse(null, out var parsed);
        Assert.False(result);
        Assert.Equal(TimeSingle.Zero, parsed);
    }

    [Fact]
    public void Parse_WithValidString_ReturnsTimeSingle()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var parsed = TimeSingle.Parse(str);
        Assert.Equal(time, parsed);
    }

    [Fact]
    public void Parse_WithInvalidString_ThrowsFormatException()
    {
        var str = "invalid";
        Assert.Throws<FormatException>(() => TimeSingle.Parse(str));
    }

    [Fact]
    public void TryParse_WithValidSpan_ReturnsTrue()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var result = TimeSingle.TryParse(str.AsSpan(), out var parsed);
        Assert.True(result);
        Assert.Equal(time, parsed);
    }

    [Fact]
    public void TryParse_WithInvalidSpan_ReturnsFalse()
    {
        var str = "invalid";
        var result = TimeSingle.TryParse(str.AsSpan(), out var parsed);
        Assert.False(result);
        Assert.Equal(TimeSingle.Zero, parsed);
    }

    [Fact]
    public void TryParse_WithEmptySpan_ReturnsFalse()
    {
        var result = TimeSingle.TryParse(ReadOnlySpan<char>.Empty, out var parsed);
        Assert.False(result);
        Assert.Equal(TimeSingle.Zero, parsed);
    }

    [Fact]
    public void Parse_WithValidSpan_ReturnsTimeSingle()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var parsed = TimeSingle.Parse(str.AsSpan());
        Assert.Equal(time, parsed);
    }

    [Fact]
    public void Parse_WithInvalidSpan_ThrowsFormatException()
    {
        var str = "invalid";
        Assert.Throws<FormatException>(() => TimeSingle.Parse(str.AsSpan()));
    }

    [Theory]
    [InlineData("1.876", 1.876f)]
    [InlineData("00:00.00", 0f)]
    [InlineData("0:00.69", 0.690f)]
    [InlineData("0:01.00", 1.000f)]
    [InlineData("0:43.256", 43.256f)]
    [InlineData("1:00.000", 60.000f)]
    [InlineData("1:50.011", 110.011f)]
    public void Parse_VariousTimes_ReturnsTimeSingle(string str, float expected)
    {
        var parsed = TimeSingle.Parse(str);
        Assert.Equal(expected, parsed.TotalSeconds);
    }

    [Theory]
    [InlineData("-1.876", -1.876f)]
    [InlineData("-0:00.69", -0.690f)]
    [InlineData("-0:01.00", -1.000f)]
    [InlineData("-0:43.256", -43.256f)]
    [InlineData("-1:00.000", -60.000f)]
    [InlineData("-1:50.011", -110.011f)]
    [InlineData("-1:23:45.678", -5025.678f)] // 1h 23m 45s 678ms = 5025.678s
    public void Parse_NegativeTimes_ReturnsNegativeTimeSingle(string str, float expected)
    {
        var parsed = TimeSingle.Parse(str);
        Assert.Equal(expected, parsed.TotalSeconds, precision: 3);
    }

    [Fact]
    public void TryParse_WithNegativeTime_ReturnsTrue()
    {
        var result = TimeSingle.TryParse("-1:23.456", out var parsed);
        Assert.True(result);
        Assert.Equal(-83.456f, parsed.TotalSeconds, precision: 3);
    }

    [Fact]
    public void Parse_WithNegativeTime_ReturnsNegativeTimeSingle()
    {
        var parsed = TimeSingle.Parse("-2:15.789");
        Assert.Equal(-135.789f, parsed.TotalSeconds, precision: 3);
        Assert.True(parsed.IsNegative);
    }

    [Fact]
    public void ToString_NegativeTime_IncludesNegativeSign()
    {
        var negativeTime = new TimeSingle(-83.456f); // -1:23.456
        var timeString = negativeTime.ToString();
        Assert.StartsWith("-", timeString);
        Assert.Equal("-1:23.456", timeString);
    }
}
