using System;

namespace TmEssentials.Tests;

public class TimeSingleTests
{
    [Test]
    public async Task Constructor_WithTotalSeconds()
    {
        var expected = 2.3f;

        var actual = new TimeSingle(expected).TotalSeconds;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Constructor_InitTotalSeconds()
    {
        var expected = 2.3f;

        var actual = new TimeSingle() { TotalSeconds = expected }.TotalSeconds;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Zero_IsZero()
    {
        var expected = new TimeSingle();

        var actual = TimeSingle.Zero;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Zero_IsMaxSingleValue()
    {
        var expected = new TimeSingle(float.MaxValue);

        var actual = TimeSingle.MaxValue;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Zero_IsMinSingleValue()
    {
        var expected = new TimeSingle(float.MinValue);

        var actual = TimeSingle.MinValue;

        await Assert.That(actual).IsEqualTo(expected);
    }
    
    [Test]
    public async Task ToString_Override_UsesDefaultParamValues()
    {
        var expected = ((object)new TimeSingle(234567890)).ToString();

        var actual = new TimeSingle(234567890).ToString(useHundredths: false, useApostrophe: false);

        await Assert.That(actual).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 1)]
    [Arguments(5.6f, 5.6f)]
    [Arguments(100, 100)]
    public async Task TotalSeconds_ShouldBeCorrect(float totalSeconds, float expectedTotalSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.TotalSeconds).IsEqualTo(expectedTotalSeconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 1000)]
    [Arguments(5.6f, 5600)]
    [Arguments(100, 100000)]
    public async Task TotalMilliseconds_ShouldBeCorrect(float totalSeconds, float expectedTotalMilliseconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(150, 2.5f)]
    [Arguments(7.5f, 0.125f)]
    [Arguments(1200, 20)]
    public async Task TotalMinutes_ShouldBeCorrect(float totalSeconds, float expectedTotalMinutes)
    {
        var timeSingle = new TimeSingle(totalSeconds);
        
        await Assert.That(timeSingle.TotalMinutes).IsEqualTo(expectedTotalMinutes);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(5.4f, 0.0015f)]
    [Arguments(1800, 0.5f)]
    [Arguments(10800, 3)]
    public async Task TotalHours_ShouldBeCorrect(float totalSeconds, float expectedTotalHours)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.TotalHours).IsEqualTo(expectedTotalHours);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(10627.2f, 0.123f)]
    [Arguments(60480, 0.7f)]
    [Arguments(172800, 2)]
    public async Task TotalDays_ShouldBeCorrect(float totalSeconds, float expectedTotalDays)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.TotalDays).IsEqualTo(expectedTotalDays);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0.05f, 500_000)]
    [Arguments(0.5f, 5_000_000)]
    [Arguments(1, 10_000_000)]
    public async Task Ticks_ShouldBeCorrect(float totalSeconds, long expectedTicks)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.Ticks).IsEqualTo(expectedTicks);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(5.6f, 600)]
    [Arguments(11.12f, 120)]
    [Arguments(100.2455f, 245.5f)]
    public async Task Milliseconds_ShouldBeCorrect(float totalSeconds, float expectedMilliseconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.Milliseconds).IsEqualTo(expectedMilliseconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(5.5f, 500)]
    [Arguments(11.1f, 100)]
    [Arguments(100.2455f, 245)]
    public async Task Milliseconds_ITime_ShouldBeCorrect(float totalSeconds, int expectedMilliseconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That((timeSingle as ITime).Milliseconds).IsEqualTo(expectedMilliseconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 1)]
    [Arguments(5.6f, 5)]
    [Arguments(100, 40)]
    [Arguments(1112, 32)]
    public async Task Seconds_ShouldBeCorrect(float totalSeconds, int expectedSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.Seconds).IsEqualTo(expectedSeconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(59.99f, 0)]
    [Arguments(60, 1)]
    [Arguments(61, 1)]
    [Arguments(320, 5)]
    [Arguments(3680, 1)]
    public async Task Minutes_ShouldBeCorrect(float totalSeconds, int expectedMinutes)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.Minutes).IsEqualTo(expectedMinutes);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(3599.99f, 0)]
    [Arguments(3600, 1)]
    [Arguments(3601, 1)]
    [Arguments(10000, 2)]
    [Arguments(10800, 3)]
    public async Task Hours_ShouldBeCorrect(float totalSeconds, int expectedHours)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.Hours).IsEqualTo(expectedHours);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(86399.99f, 0)]
    [Arguments(86400, 1)]
    [Arguments(86401, 1)]
    [Arguments(172799, 1)]
    [Arguments(172800, 2)]
    public async Task Days_ShouldBeCorrect(float totalSeconds, int expectedDays)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.Days).IsEqualTo(expectedDays);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(5.5f)]
    [Arguments(20)]
    public async Task New_TotalSeconds_ShouldBeEqual(float totalSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        await Assert.That(timeSingle.TotalSeconds).IsEqualTo(totalSeconds);
    }

    [Test]
    [Arguments(-1, -2, -3)]
    [Arguments(0, 0, 0)]
    [Arguments(5, 10, 15)]
    [Arguments(13, 18, 7)]
    public async Task New_HoursMinutesSeconds_ShouldBeEqual(int hours, int minutes, int seconds)
    {
        var timeSingle = new TimeSingle(hours, minutes, seconds);

        await Assert.That(timeSingle.Hours).IsEqualTo(hours);
        await Assert.That(timeSingle.Minutes).IsEqualTo(minutes);
        await Assert.That(timeSingle.Seconds).IsEqualTo(seconds);
    }

    [Test]
    [Arguments(0, -2, -3, -4, -100)]
    [Arguments(0, 0, 0, 0, 0)]
    [Arguments(0, 10, 15, 20, 100)]
    [Arguments(0, 18, 7, 1, 100)]
    public async Task New_DaysHoursMinutesSecondsMilliseconds_ShouldBeEqual(int days, int hours, int minutes, int seconds, float milliseconds)
    {
        var timeSingle = new TimeSingle(days, hours, minutes, seconds, milliseconds);

        await Assert.That(timeSingle.Days).IsEqualTo(days);
        await Assert.That(timeSingle.Hours).IsEqualTo(hours);
        await Assert.That(timeSingle.Minutes).IsEqualTo(minutes);
        await Assert.That(timeSingle.Seconds).IsEqualTo(seconds);
        await Assert.That(timeSingle.Milliseconds).IsEqualTo(milliseconds);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromDays_TotalDaysShouldBeEqual(float days)
    {
        var timeSingle = TimeSingle.FromDays(days);

        await Assert.That(timeSingle.TotalDays).IsEqualTo(days);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromHours_TotalHoursShouldBeEqual(float hours)
    {
        var timeSingle = TimeSingle.FromHours(hours);

        await Assert.That(timeSingle.TotalHours).IsEqualTo(hours);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromMilliseconds_TotalMillisecondsShouldBeEqual(float milliseconds)
    {
        var timeSingle = TimeSingle.FromMilliseconds(milliseconds);

        await Assert.That(timeSingle.TotalMilliseconds).IsEqualTo(milliseconds);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromMinutes_TotalMinutesShouldBeEqual(float minutes)
    {
        var timeSingle = TimeSingle.FromMinutes(minutes);

        await Assert.That(timeSingle.TotalMinutes).IsEqualTo(minutes);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromSeconds_TotalSecondsShouldBeEqual(float seconds)
    {
        var timeSingle = TimeSingle.FromSeconds(seconds);

        await Assert.That(timeSingle.TotalSeconds).IsEqualTo(seconds);
    }

    [Test]
    [Arguments(0)]
    [Arguments(-10_000)]
    [Arguments(10_000)]
    [Arguments(100_000)]
    [Arguments(5_100_000)]
    public async Task FromTicks_TicksShouldBeEqual(long ticks)
    {
        var timeSingle = TimeSingle.FromTicks(ticks);

        await Assert.That(timeSingle.Ticks).IsEqualTo(ticks);
    }

    [Test]
    [Arguments(5, 10, 15)]
    [Arguments(-1, -2, -3)]
    [Arguments(0, 0, 0)]
    [Arguments(12, 15.5f, 27.5f)]
    public async Task Add_ShouldAdd(float totalSeconds1, float totalSeconds2, float totalSecondsResult)
    {
        var timeSingle1 = new TimeSingle(totalSeconds1);
        var timeSingle2 = new TimeSingle(totalSeconds2);

        var result = timeSingle1.Add(timeSingle2);

        await Assert.That(result.TotalSeconds).IsEqualTo(totalSecondsResult);
    }

    [Test]
    [Arguments(0, 1, 0)]
    [Arguments(1, 0.5f, 2)]
    [Arguments(10, 2, 5)]
    public async Task Divide_Divisor_ShouldDivide(float totalSeconds, float divisor, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var result = timeSingle.Divide(divisor);
        
        await Assert.That(result.TotalSeconds).IsEqualTo(totalSecondsResult);
    }

    [Test]
    [Arguments(0, 1, 0)]
    [Arguments(1, 0.5f, 2)]
    [Arguments(10, 2, 5)]
    public async Task Divide_Time_ShouldDivide(float totalSeconds, float totalSecondsDivisor, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);
        var divisor = new TimeSingle(totalSecondsDivisor);

        var result = timeSingle.Divide(divisor);

        await Assert.That(result).IsEqualTo(totalSecondsResult);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(-1, 1)]
    [Arguments(10, 10)]
    [Arguments(-10, 10)]
    public async Task Duration_ShouldReturnAbsoluteValue(float totalSeconds, float totalSecondsExpected)
    {
        var timeSingle = new TimeSingle(totalSeconds);
        var timeSingleExpected = new TimeSingle(totalSecondsExpected);

        var result = timeSingle.Duration();

        await Assert.That(result).IsEqualTo(timeSingleExpected);
    }

    [Test]
    [Arguments(0, 1, 0)]
    [Arguments(1, 0.5f, 0.5f)]
    [Arguments(10, 2, 20)]
    public async Task Multiply_ShouldMultiplyByFactor(float totalSeconds, float totalSecondsFactor, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var result = timeSingle.Multiply(totalSecondsFactor);

        await Assert.That(result.TotalSeconds).IsEqualTo(totalSecondsResult);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, -1)]
    [Arguments(5.5f, -5.5f)]
    [Arguments(-8.5f, 8.5f)]
    public async Task Negate_ShouldNegate(float totalSeconds, float totalSecondsResult)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var result = timeSingle.Negate();

        await Assert.That(result.TotalSeconds).IsEqualTo(totalSecondsResult);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(5.5f)]
    [Arguments(-8.5f)]
    public async Task ToTimeInt32_ConvertsCorrectly(float totalSeconds)
    {
        var timeSingle = new TimeSingle(totalSeconds);

        var timeInt = timeSingle.ToTimeInt32();

        await Assert.That(timeInt.TotalSeconds).IsEqualTo(timeSingle.TotalSeconds);
    }

    [Test]
    public async Task GreaterThanOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(2.0f); // 2 seconds
        var time2 = new TimeSingle(1.5f); // 1.5 seconds

        await Assert.That(time1 > time2).IsTrue();
        await Assert.That(time2 > time1).IsFalse();
    }

    [Test]
    public async Task GreaterThanOrEqualOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(2.0f);
        var time2 = new TimeSingle(2.0f);
        var time3 = new TimeSingle(1.5f);

        await Assert.That(time1 >= time2).IsTrue();
        await Assert.That(time1 >= time3).IsTrue();
        await Assert.That(time3 >= time1).IsFalse();
    }

    [Test]
    public async Task LessThanOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(1.5f);
        var time2 = new TimeSingle(2.0f);

        await Assert.That(time1 < time2).IsTrue();
        await Assert.That(time2 < time1).IsFalse();
    }

    [Test]
    public async Task LessThanOrEqualOperator_TimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(2.0f);
        var time2 = new TimeSingle(2.0f);
        var time3 = new TimeSingle(2.5f);

        await Assert.That(time1 <= time2).IsTrue();
        await Assert.That(time1 <= time3).IsTrue();
        await Assert.That(time3 <= time1).IsFalse();
    }

    [Test]
    public async Task EqualityOperator_TimeSingleAndTimeInt32_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(1.0f); // 1 second
        var timeInt32 = new TimeInt32(1000); // 1000 milliseconds

        await Assert.That(timeSingle == timeInt32).IsTrue();
    }

    [Test]
    public async Task InequalityOperator_TimeSingleAndTimeInt32_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(1.5f); // 1.5 seconds
        var timeInt32 = new TimeInt32(1000); // 1000 milliseconds

        await Assert.That(timeSingle != timeInt32).IsTrue();
    }

    [Test]
    public async Task UnaryPlusOperator_ReturnsSameValue()
    {
        var time = new TimeSingle(1.5f); // 1.5 seconds
        var result = +time;

        await Assert.That(result.TotalSeconds).IsEqualTo(time.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task AddTwoTimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(1.0f); // 1 second
        var time2 = new TimeSingle(2.5f); // 2.5 seconds
        var expected = new TimeSingle(3.5f);

        var result = time1 + time2;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task AddTimeSingleAndTimeSpan_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(1.0f); // 1 second
        var timeSpan = TimeSpan.FromSeconds(2.5); // 2.5 seconds
        var expected = new TimeSingle(3.5f);

        var result = timeSingle + timeSpan;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task UnaryMinusOperator_ReturnsNegatedValue()
    {
        var time = new TimeSingle(1.5f); // 1.5 seconds
        var expected = new TimeSingle(-1.5f);

        var result = -time;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task SubtractTwoTimeSingle_ReturnsCorrectResult()
    {
        var time1 = new TimeSingle(3.5f); // 3.5 seconds
        var time2 = new TimeSingle(1.0f); // 1 second
        var expected = new TimeSingle(2.5f);

        var result = time1 - time2;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task SubtractTimeSpanFromTimeSingle_ReturnsCorrectResult()
    {
        var timeSingle = new TimeSingle(3.5f); // 3.5 seconds
        var timeSpan = TimeSpan.FromSeconds(1.0); // 1 second
        var expected = new TimeSingle(2.5f);

        var result = timeSingle - timeSpan;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task MultiplyFloatAndTimeSingle_ReturnsTimeSingle()
    {
        var time = new TimeSingle(2.0f); // 2 seconds
        float factor = 2.5f;
        var expected = new TimeSingle(5.0f); // 5 seconds

        var result = factor * time;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task MultiplyTimeSingleAndFloat_ReturnsTimeSingle()
    {
        var time = new TimeSingle(1.5f); // 1.5 seconds
        float factor = 2f;
        var expected = new TimeSingle(3.0f); // 3 seconds

        var result = time * factor;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task DivideTimeSingleByFloat_ReturnsTimeSingle()
    {
        var time = new TimeSingle(3.0f); // 3 seconds
        float divisor = 2f;
        var expected = new TimeSingle(1.5f); // 1.5 seconds

        var result = time / divisor;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task DivideTimeSingleByTimeSingle_ReturnsFloat()
    {
        var time1 = new TimeSingle(3.0f); // 3 seconds
        var time2 = new TimeSingle(1.5f); // 1.5 seconds
        float expected = 2.0f; // 3 / 1.5

        var result = time1 / time2;

        await Assert.That(result).IsEqualTo(expected).Within(0.001f);
    }

    [Test]
    public async Task ImplicitConversionFromTimeSingleToTimeSpan()
    {
        var timeSingle = new TimeSingle(1.5f); // 1.5 seconds
        TimeSpan expected = TimeSpan.FromSeconds(1.5);

        TimeSpan result = timeSingle;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task ImplicitConversionFromTimeSpanToTimeSingle()
    {
        var timeSpan = TimeSpan.FromSeconds(2.0); // 2 seconds
        var expected = new TimeSingle(2.0f);

        TimeSingle result = timeSpan;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task ImplicitConversionFromTimeInt32ToTimeSingle()
    {
        var timeInt32 = new TimeInt32(3000); // 3000 milliseconds
        var expected = new TimeSingle(3.0f); // 3 seconds

        TimeSingle result = timeInt32;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task CompareTo_WithNull_ReturnsPositive()
    {
        var time = new TimeSingle(1); // Assuming this class implements ITime
        int result = time.CompareTo(null);

        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithGreaterTotalSeconds_ReturnsNegative()
    {
        var time = new TimeSingle(1f);
        var other = new TimeSingle(1.5f);

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task CompareTo_WithLesserTotalSeconds_ReturnsPositive()
    {
        var time = new TimeSingle(1.5f);
        var other = new TimeSingle(1);

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithEqualTotalSeconds_ReturnsZero()
    {
        var time = new TimeSingle(1);
        var other = new TimeSingle(1);

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareToObject_WithNull_ReturnsPositive()
    {
        var time = new TimeSingle(1.0f); // Replace with your class that implements ITime
        int result = time.CompareTo(default(object));

        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    [Arguments(1f, 1.5f, -1)]
    [Arguments(1.5f, 1f, 1)]
    [Arguments(1f, 1f, 0)]
    public async Task CompareToObject_WithITimeObject_ReturnsCorrectComparison(float timeFloat, float otherFloat, int expected)
    {
        var time = new TimeSingle(timeFloat);
        object other = new TimeSingle(otherFloat); // other implements ITime

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task CompareToObject_WithNonITimeObject_ThrowsArgumentException()
    {
        var time = new TimeSingle(1.0f);
        object nonTimeObject = new Random(); // An object that does not implement ITime

        await Assert.That(() => time.CompareTo(nonTimeObject)).ThrowsExactly<ArgumentException>();
    }

    [Test]
    [Arguments(1f, 1500, -1)]
    [Arguments(1.5f, 1000, 1)]
    [Arguments(1f, 1000, 0)]
    public async Task CompareTo_WithTimeInt32_ReturnsCorrectComparison(float timeFloat, int otherInt, int expected)
    {
        var time = new TimeSingle(timeFloat);
        var other = new TimeInt32(otherInt);

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Equals_WithNullITime_ReturnsFalse()
    {
        var time = new TimeSingle(1.0f);

        await Assert.That(time.Equals(null)).IsFalse();
    }

    [Test]
    public async Task Equals_WithTimeInt32_ReturnsFalse()
    {
        var time = new TimeSingle(2);
        var other = new TimeInt32(1000);
        await Assert.That(time.Equals(other)).IsFalse();
    }

    [Test]
    public async Task TryParse_WithValidString_ReturnsTrue()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var result = TimeSingle.TryParse(str, out var parsed);
        await Assert.That(result).IsTrue();
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task TryParse_WithInvalidString_ReturnsFalse()
    {
        var str = "invalid";
        var result = TimeSingle.TryParse(str, out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task TryParse_WithNullString_ReturnsFalse()
    {
        var result = TimeSingle.TryParse(null, out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task Parse_WithValidString_ReturnsTimeSingle()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var parsed = TimeSingle.Parse(str);
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task Parse_WithInvalidString_ThrowsFormatException()
    {
        var str = "invalid";
        await Assert.That(() => TimeSingle.Parse(str)).ThrowsExactly<FormatException>();
    }

    [Test]
    public async Task TryParse_WithValidSpan_ReturnsTrue()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var result = TimeSingle.TryParse(str.AsSpan(), out var parsed);
        await Assert.That(result).IsTrue();
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task TryParse_WithInvalidSpan_ReturnsFalse()
    {
        var str = "invalid";
        var result = TimeSingle.TryParse(str.AsSpan(), out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task TryParse_WithEmptySpan_ReturnsFalse()
    {
        var result = TimeSingle.TryParse(ReadOnlySpan<char>.Empty, out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task Parse_WithValidSpan_ReturnsTimeSingle()
    {
        var time = new TimeSingle(1);
        var str = time.ToString();
        var parsed = TimeSingle.Parse(str.AsSpan());
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task Parse_WithInvalidSpan_ThrowsFormatException()
    {
        var str = "invalid";
        await Assert.That(() => TimeSingle.Parse(str.AsSpan())).ThrowsExactly<FormatException>();
    }

    [Test]
    [Arguments("1.876", 1.876f)]
    [Arguments("00:00.00", 0f)]
    [Arguments("0:00.69", 0.690f)]
    [Arguments("0:01.00", 1.000f)]
    [Arguments("0:43.256", 43.256f)]
    [Arguments("1:00.000", 60.000f)]
    [Arguments("1:50.011", 110.011f)]
    public async Task Parse_VariousTimes_ReturnsTimeSingle(string str, float expected)
    {
        var parsed = TimeSingle.Parse(str);
        await Assert.That(parsed.TotalSeconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments("-1.876", -1.876f)]
    [Arguments("-0:00.69", -0.690f)]
    [Arguments("-0:01.00", -1.000f)]
    [Arguments("-0:43.256", -43.256f)]
    [Arguments("-1:00.000", -60.000f)]
    [Arguments("-1:50.011", -110.011f)]
    [Arguments("-1:23:45.678", -5025.678f)] // 1h 23m 45s 678ms = 5025.678s
    public async Task Parse_NegativeTimes_ReturnsNegativeTimeSingle(string str, float expected)
    {
        var parsed = TimeSingle.Parse(str);
        await Assert.That(parsed.TotalSeconds).IsEqualTo(expected).Within(0.001f);
    }

    [Test]
    public async Task TryParse_WithNegativeTime_ReturnsTrue()
    {
        var result = TimeSingle.TryParse("-1:23.456", out var parsed);
        await Assert.That(result).IsTrue();
        await Assert.That(parsed.TotalSeconds).IsEqualTo(-83.456f).Within(0.001f);
    }

    [Test]
    public async Task Parse_WithNegativeTime_ReturnsNegativeTimeSingle()
    {
        var parsed = TimeSingle.Parse("-2:15.789");
        await Assert.That(parsed.TotalSeconds).IsEqualTo(-135.789f).Within(0.001f);
        await Assert.That(parsed.IsNegative).IsTrue();
    }

    [Test]
    public async Task ToString_NegativeTime_IncludesNegativeSign()
    {
        var negativeTime = new TimeSingle(-83.456f); // -1:23.456
        var timeString = negativeTime.ToString();
        await Assert.That(timeString).StartsWith("-");
        await Assert.That(timeString).IsEqualTo("-1:23.456");
    }
}
