using System;

namespace TmEssentials.Tests;

public class TimeInt32Tests
{
    [Test]
    public async Task Constructor_WithTotalMilliseconds()
    {
        var expected = 234567890;

        var actual = new TimeInt32(expected).TotalMilliseconds;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Constructor_InitTotalMilliseconds()
    {
        var expected = 234567890;

        var actual = new TimeInt32() { TotalMilliseconds = expected }.TotalMilliseconds;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Zero_IsZero()
    {
        var expected = new TimeInt32();
        
        var actual = TimeInt32.Zero;

        await Assert.That(actual).IsEqualTo(expected);
    }
    
    [Test]
    public async Task Zero_IsMaxInt32Value()
    {
        var expected = new TimeInt32(int.MaxValue);

        var actual = TimeInt32.MaxValue;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Zero_IsMinInt32Value()
    {
        var expected = new TimeInt32(int.MinValue);

        var actual = TimeInt32.MinValue;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task ToString_Override_UsesDefaultParamValues()
    {
        var expected = ((object)new TimeInt32(234567890)).ToString();

        var actual = new TimeInt32(234567890).ToString(useHundredths: false, useApostrophe: false);

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 1)]
    [Arguments(50, 50)]
    [Arguments(2000, 2000)]
    public async Task TotalMilliseconds_ShouldBeCorrect(int totalMilliseconds, int expectedTotalMilliseconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(900, 0.9f)]
    [Arguments(1000, 1)]
    [Arguments(3200, 3.2f)]
    public async Task TotalSeconds_ShouldBeCorrect(int totalMilliseconds, float expectedTotalSeconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.TotalSeconds).IsEqualTo(expectedTotalSeconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(66_000, 1.1f)]
    [Arguments(192_000, 3.2f)]
    public async Task TotalMinutes_ShouldBeCorrect(int totalMilliseconds, float expectedTotalMinutes)
    {
        var timeInt = new TimeInt32(totalMilliseconds);
        
        await Assert.That(timeInt.TotalMinutes).IsEqualTo(expectedTotalMinutes);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1_800_000, 0.5f)]
    [Arguments(3_600_000, 1)]
    [Arguments(5_400_000, 1.5f)]
    public async Task TotalHours_ShouldBeCorrect(int totalMilliseconds, float expectedTotalHours)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.TotalHours).IsEqualTo(expectedTotalHours);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(86_400_000, 1)]
    [Arguments(43_200_000, 0.5f)]
    public async Task TotalDays_ShouldBeCorrect(int totalMilliseconds, float expectedTotalDays)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.TotalDays).IsEqualTo(expectedTotalDays);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(-2, -20_000)]
    [Arguments(1, 10_000)]
    [Arguments(3, 30_000)]
    public async Task Ticks_ShouldBeCorrect(int totalMilliseconds, long expectedTicks)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.Ticks).IsEqualTo(expectedTicks);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(-1200, -200)]
    [Arguments(500, 500)]
    [Arguments(1200, 200)]
    [Arguments(2600, 600)]
    public async Task Milliseconds_ShouldBeCorrect(int totalMilliseconds, float expectedMilliseconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.Milliseconds).IsEqualTo(expectedMilliseconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(3_000, 3)]
    [Arguments(59_999, 59)]
    [Arguments(60_000, 0)]
    [Arguments(60_999, 0)]
    [Arguments(61_000, 1)]
    public async Task Seconds_ShouldBeCorrect(int totalMilliseconds, int expectedSeconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.Seconds).IsEqualTo(expectedSeconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(59_999, 0)]
    [Arguments(60_000, 1)]
    [Arguments(61_000, 1)]
    [Arguments(320_000, 5)]
    [Arguments(3_680_000, 1)]
    public async Task Minutes_ShouldBeCorrect(int totalMilliseconds, int expectedMinutes)
    {
        var timeSingle = new TimeInt32(totalMilliseconds);

        await Assert.That(timeSingle.Minutes).IsEqualTo(expectedMinutes);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(3_599_999, 0)]
    [Arguments(3_600_000, 1)]
    [Arguments(3_601_000, 1)]
    [Arguments(10_000_000, 2)]
    [Arguments(10_800_000, 3)]
    public async Task Hours_ShouldBeCorrect(int totalMilliseconds, int expectedHours)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.Hours).IsEqualTo(expectedHours);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(86_399_990, 0)]
    [Arguments(86_400_000, 1)]
    [Arguments(86_401_000, 1)]
    [Arguments(172_799_000, 1)]
    [Arguments(172_800_000, 2)]
    public async Task Days_ShouldBeCorrect(int totalMilliseconds, int expectedDays)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.Days).IsEqualTo(expectedDays);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(990, 0.99f)]
    [Arguments(1000, 1)]
    [Arguments(5500, 5.5f)]
    public async Task New_TotalSeconds_ShouldBeEqual(int totalMilliseconds, float expectedTotalSeconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        await Assert.That(timeInt.TotalSeconds).IsEqualTo(expectedTotalSeconds);
    }

    [Test]
    [Arguments(-1, -2, -3)]
    [Arguments(0, 0, 0)]
    [Arguments(5, 10, 15)]
    [Arguments(13, 18, 7)]
    public async Task New_HoursMinutesSeconds_ShouldBeEqual(int hours, int minutes, int seconds)
    {
        var timeInt = new TimeInt32(hours, minutes, seconds);

        await Assert.That(timeInt.Hours).IsEqualTo(hours);
        await Assert.That(timeInt.Minutes).IsEqualTo(minutes);
        await Assert.That(timeInt.Seconds).IsEqualTo(seconds);
    }

    [Test]
    [Arguments(0, -2, -3, -4, -5)]
    [Arguments(0, 0, 0, 0, 0)]
    [Arguments(0, 10, 15, 20, 24)]
    [Arguments(0, 18, 7, 1, 8)]
    public async Task New_DaysHoursMinutesSecondsMilliseconds_ShouldBeEqual(int days, int hours, int minutes, int seconds, int milliseconds)
    {
        var timeInt = new TimeInt32(days, hours, minutes, seconds, milliseconds);

        await Assert.That(timeInt.Days).IsEqualTo(days);
        await Assert.That(timeInt.Hours).IsEqualTo(hours);
        await Assert.That(timeInt.Minutes).IsEqualTo(minutes);
        await Assert.That(timeInt.Seconds).IsEqualTo(seconds);
        await Assert.That(timeInt.Milliseconds).IsEqualTo(milliseconds);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromDays_TotalDaysShouldBeEqual(float days)
    {
        var timeInt = TimeInt32.FromDays(days);

        await Assert.That(timeInt.TotalDays).IsEqualTo(days);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromHours_TotalHoursShouldBeEqual(float hours)
    {
        var timeInt = TimeInt32.FromHours(hours);

        await Assert.That(timeInt.TotalHours).IsEqualTo(hours);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(12)]
    public async Task FromMilliseconds_TotalMillisecondsShouldBeEqual(int milliseconds)
    {
        var timeInt = TimeInt32.FromMilliseconds(milliseconds);

        await Assert.That(timeInt.TotalMilliseconds).IsEqualTo(milliseconds);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromMinutes_TotalMinutesShouldBeEqual(float minutes)
    {
        var timeInt = TimeInt32.FromMinutes(minutes);

        await Assert.That(timeInt.TotalMinutes).IsEqualTo(minutes);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6.5f)]
    [Arguments(12)]
    public async Task FromSeconds_TotalSecondsShouldBeEqual(float seconds)
    {
        var timeInt = TimeInt32.FromSeconds(seconds);

        await Assert.That(timeInt.TotalSeconds).IsEqualTo(seconds);
    }

    [Test]
    [Arguments(0)]
    [Arguments(-10_000)]
    [Arguments(10_000)]
    [Arguments(100_000)]
    [Arguments(5_100_000)]
    public async Task FromTicks_TicksShouldBeEqual(long ticks)
    {
        var timeInt = TimeInt32.FromTicks(ticks);

        await Assert.That(timeInt.Ticks).IsEqualTo(ticks);
    }

    [Test]
    [Arguments(5, 10, 15)]
    [Arguments(-1, -2, -3)]
    [Arguments(0, 0, 0)]
    [Arguments(12, 15.5f, 27.5f)]
    public async Task Add_ShouldAdd(int totalMilliseconds1, int totalMilliseconds2, int totalMillisecondsResult)
    {
        var timeInt1 = new TimeInt32(totalMilliseconds1);
        var timeInt2 = new TimeInt32(totalMilliseconds2);

        var result = timeInt1.Add(timeInt2);

        await Assert.That(result.TotalMilliseconds).IsEqualTo(totalMillisecondsResult);
    }

    [Test]
    [Arguments(0, 1, 0)]
    [Arguments(1, 0.5f, 2)]
    [Arguments(10, 2, 5)]
    public async Task Divide_Divisor_ShouldDivide(int totalMilliseconds, float divisor, float totalMillisecondsResult)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        var result = timeInt.Divide(divisor);
        
        await Assert.That(result.TotalMilliseconds).IsEqualTo(totalMillisecondsResult);
    }

    [Test]
    [Arguments(0, 1, 0)]
    [Arguments(6, 2, 3)]
    [Arguments(14, 2, 7)]
    public async Task Divide_Time_ShouldDivide(int totalMilliseconds, int totalMillisecondsDivisor, float totalSecondsResult)
    {
        var timeInt = new TimeInt32(totalMilliseconds);
        var divisor = new TimeInt32(totalMillisecondsDivisor);

        var result = timeInt.Divide(divisor);

        await Assert.That(result).IsEqualTo(totalSecondsResult);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(-1, 1)]
    [Arguments(10, 10)]
    [Arguments(-10, 10)]
    public async Task Duration_ShouldReturnAbsoluteValue(int totalMilliseconds, int totalMillisecondsExpected)
    {
        var timeInt = new TimeInt32(totalMilliseconds);
        var timeIntExpected = new TimeInt32(totalMillisecondsExpected);

        var result = timeInt.Duration();

        await Assert.That(result).IsEqualTo(timeIntExpected);
    }

    [Test]
    [Arguments(0, 1, 0)]
    [Arguments(2, 0.5f, 1)]
    [Arguments(10, 2, 20)]
    public async Task Multiply_ShouldMultiplyByFactor(int totalMilliseconds, float factor, int totalMillisecondsResult)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        var result = timeInt.Multiply(factor);

        await Assert.That(result.TotalMilliseconds).IsEqualTo(totalMillisecondsResult);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, -1)]
    [Arguments(5, -5)]
    [Arguments(-8, 8)]
    public async Task Negate_ShouldNegate(int totalMilliseconds, int totalMillisecondsResult)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        var result = timeInt.Negate();

        await Assert.That(result.TotalMilliseconds).IsEqualTo(totalMillisecondsResult);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(5)]
    [Arguments(-8)]
    public async Task ToTimeSingle_ConvertsCorrectly(int totalMilliseconds)
    {
        var timeInt = new TimeInt32(totalMilliseconds);

        var timeSingle = timeInt.ToTimeSingle();

        await Assert.That(timeSingle.TotalMilliseconds).IsEqualTo(timeInt.TotalMilliseconds);
    }

    [Test]
    public async Task GreaterThanOperator_ReturnsCorrectResult()
    {
        var time1 = new TimeInt32(1000); // Example values
        var time2 = new TimeInt32(500);

        await Assert.That(time1 > time2).IsTrue();
        await Assert.That(time2 > time1).IsFalse();
    }

    [Test]
    public async Task GreaterThanOrEqualOperator_ReturnsCorrectResult()
    {
        var time1 = new TimeInt32(1000);
        var time2 = new TimeInt32(1000);
        var time3 = new TimeInt32(500);

        await Assert.That(time1 >= time2).IsTrue();
        await Assert.That(time1 >= time3).IsTrue();
        await Assert.That(time3 >= time1).IsFalse();
    }

    [Test]
    public async Task LessThanOperator_ReturnsCorrectResult()
    {
        var time1 = new TimeInt32(500);
        var time2 = new TimeInt32(1000);

        await Assert.That(time1 < time2).IsTrue();
        await Assert.That(time2 < time1).IsFalse();
    }

    [Test]
    public async Task LessThanOrEqualOperator_ReturnsCorrectResult()
    {
        var time1 = new TimeInt32(1000);
        var time2 = new TimeInt32(1000);
        var time3 = new TimeInt32(1500);

        await Assert.That(time1 <= time2).IsTrue();
        await Assert.That(time1 <= time3).IsTrue();
        await Assert.That(time3 <= time1).IsFalse();
    }

    [Test]
    public async Task EqualityOperator_ReturnsCorrectResult()
    {
        var timeInt32 = new TimeInt32(1000);
        var timeSingle = new TimeSingle(1);

        await Assert.That(timeInt32 == timeSingle).IsTrue();
    }

    [Test]
    public async Task InequalityOperator_ReturnsCorrectResult()
    {
        var timeInt32 = new TimeInt32(1000);
        var timeSingle = new TimeSingle(0.5f);

        await Assert.That(timeInt32 != timeSingle).IsTrue();
    }

    [Test]
    public async Task UnaryPlusOperator_ReturnsSameValue()
    {
        var time = new TimeInt32(1000);
        var result = +time;

        await Assert.That(result).IsEqualTo(time);
    }

    [Test]
    public async Task AddTwoTimeInt32_ReturnsCorrectResult()
    {
        var time1 = new TimeInt32(1000);
        var time2 = new TimeInt32(500);
        var expected = new TimeInt32(1500);

        var result = time1 + time2;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task AddTimeInt32AndTimeSpan_ReturnsCorrectResult()
    {
        var timeInt32 = new TimeInt32(1000);
        var timeSpan = TimeSpan.FromMilliseconds(500);
        var expected = new TimeInt32(1500);

        var result = timeInt32 + timeSpan;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task UnaryMinusOperator_ReturnsNegatedValue()
    {
        var time = new TimeInt32(1000);
        var expected = new TimeInt32(-1000);

        var result = -time;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task SubtractTwoTimeInt32_ReturnsCorrectResult()
    {
        var time1 = new TimeInt32(1500);
        var time2 = new TimeInt32(500);
        var expected = new TimeInt32(1000);

        var result = time1 - time2;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task SubtractTimeSpanFromTimeInt32_ReturnsCorrectResult()
    {
        var timeInt32 = new TimeInt32(1500);
        var timeSpan = TimeSpan.FromMilliseconds(500);
        var expected = new TimeInt32(1000);

        var result = timeInt32 - timeSpan;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task MultiplyFloatAndTimeInt32_ReturnsTimeSingle()
    {
        var time = new TimeInt32(1000); // 1000 milliseconds
        float factor = 2.5f;
        var expected = new TimeSingle(2.5f * 1); // Assuming TotalSeconds = TotalMilliseconds / 1000

        var result = factor * time;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f); // Adjust precision as needed
    }

    [Test]
    public async Task MultiplyIntAndTimeInt32_ReturnsTimeInt32()
    {
        var time = new TimeInt32(1000);
        int factor = 3;
        var expected = new TimeInt32(3000);

        var result = factor * time;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task MultiplyTimeInt32AndFloat_ReturnsTimeSingle()
    {
        var time = new TimeInt32(1000);
        float factor = 0.5f;
        var expected = new TimeSingle(0.5f * 1); // Assuming TotalSeconds = TotalMilliseconds / 1000

        var result = time * factor;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task MultiplyTimeInt32AndInt_ReturnsTimeInt32()
    {
        var time = new TimeInt32(500);
        int factor = 4;
        var expected = new TimeInt32(2000);

        var result = time * factor;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task DivideTimeInt32ByFloat_ReturnsTimeSingle()
    {
        var time = new TimeInt32(1000);
        float divisor = 2f;
        var expected = new TimeSingle(0.5f); // Assuming TotalSeconds = TotalMilliseconds / 1000

        var result = time / divisor;

        await Assert.That(result.TotalSeconds).IsEqualTo(expected.TotalSeconds).Within(0.001f);
    }

    [Test]
    public async Task DivideTimeInt32ByTimeInt32_ReturnsFloat()
    {
        var time1 = new TimeInt32(1500);
        var time2 = new TimeInt32(500);
        float expected = 3f; // 1500 / 500

        var result = time1 / time2;

        await Assert.That(result).IsEqualTo(expected).Within(0.001f);
    }

    [Test]
    public async Task ImplicitConversionFromTimeInt32ToTimeSpan()
    {
        var timeInt32 = new TimeInt32(1000); // 1000 milliseconds
        TimeSpan expected = TimeSpan.FromMilliseconds(1000);

        TimeSpan result = timeInt32;

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task ImplicitConversionFromTimeSpanToTimeInt32()
    {
        var timeSpan = TimeSpan.FromMilliseconds(500);
        var expected = new TimeInt32(500);

        TimeInt32 result = timeSpan;

        await Assert.That(result.TotalMilliseconds).IsEqualTo(expected.TotalMilliseconds);
    }

    [Test]
    public async Task ImplicitConversionFromTimeSingleToTimeInt32()
    {
        var timeSingle = new TimeSingle(1.5f); // 1.5 seconds
        var expected = new TimeInt32(1500); // Assuming FromSeconds converts seconds to milliseconds

        TimeInt32 result = timeSingle;

        await Assert.That(result.TotalMilliseconds).IsEqualTo(expected.TotalMilliseconds);
    }

    [Test]
    public async Task CompareTo_WithNull_ReturnsPositive()
    {
        var time = new TimeInt32(1000); // Assuming this class implements ITime
        int result = time.CompareTo(null);

        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithGreaterTotalMilliseconds_ReturnsNegative()
    {
        var time = new TimeInt32(1000);
        var other = new TimeInt32(1500); // other has more milliseconds

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task CompareTo_WithLesserTotalMilliseconds_ReturnsPositive()
    {
        var time = new TimeInt32(1500);
        var other = new TimeInt32(1000); // other has fewer milliseconds

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithEqualTotalMilliseconds_ReturnsZero()
    {
        var time = new TimeInt32(1000);
        var other = new TimeInt32(1000); // other has equal milliseconds

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_WithNullObject_ReturnsPositive()
    {
        var time = new TimeInt32(1000); // Replace with your class that implements ITime
        int result = time.CompareTo(default(object));

        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    [Arguments(1000, 1500, -1)]
    [Arguments(1500, 1000, 1)]
    [Arguments(1000, 1000, 0)]
    public async Task CompareTo_WithITimeObject_ReturnsCorrectComparison(int timeInt, int otherInt, int expected)
    {
        var time = new TimeInt32(timeInt);
        object other = new TimeInt32(otherInt); // other implements ITime

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task CompareTo_WithNonITimeObject_ThrowsArgumentException()
    {
        var time = new TimeInt32(1000);
        object nonTimeObject = new Random(); // An object that does not implement ITime

        await Assert.That(() => time.CompareTo(nonTimeObject)).ThrowsExactly<ArgumentException>();
    }

    [Test]
    [Arguments(1000, 1.5f, -1)]
    [Arguments(1500, 1f, 1)]
    [Arguments(1000, 1f, 0)]
    public async Task CompareTo_WithTimeSingle_ReturnsCorrectComparison(int timeInt, float otherFloat, int expected)
    {
        var time = new TimeInt32(timeInt);
        var other = new TimeSingle(otherFloat);

        int result = time.CompareTo(other);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Equals_WithNullITime_ReturnsFalse()
    {
        var time = new TimeInt32(1000);

        await Assert.That(time.Equals(null)).IsFalse();
    }

    [Test]
    public async Task Equals_WithTimeSingle_ReturnsFalse()
    {
        var time = new TimeInt32(1000);
        var other = new TimeSingle(2f);
        await Assert.That(time.Equals(other)).IsFalse();
    }

    [Test]
    public async Task TryParse_WithValidString_ReturnsTrue()
    {
        var time = new TimeInt32(1000);
        var str = time.ToString();
        var result = TimeInt32.TryParse(str, out var parsed);
        await Assert.That(result).IsTrue();
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task TryParse_WithInvalidString_ReturnsFalse()
    {
        var str = "invalid";
        var result = TimeInt32.TryParse(str, out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task TryParse_WithNullString_ReturnsFalse()
    {
        var result = TimeInt32.TryParse(null, out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task Parse_WithValidString_ReturnsTimeInt32()
    {
        var time = new TimeInt32(1000);
        var str = time.ToString();
        var parsed = TimeInt32.Parse(str);
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task Parse_WithInvalidString_ThrowsFormatException()
    {
        var str = "invalid";
        await Assert.That(() => TimeInt32.Parse(str)).ThrowsExactly<FormatException>();
    }

    [Test]
    public async Task TryParse_WithValidSpan_ReturnsTrue()
    {
        var time = new TimeInt32(1000);
        var str = time.ToString();
        var result = TimeInt32.TryParse(str.AsSpan(), out var parsed);
        await Assert.That(result).IsTrue();
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task TryParse_WithInvalidSpan_ReturnsFalse()
    {
        var str = "invalid";
        var result = TimeInt32.TryParse(str.AsSpan(), out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task TryParse_WithEmptySpan_ReturnsFalse()
    {
        var result = TimeInt32.TryParse(ReadOnlySpan<char>.Empty, out var parsed);
        await Assert.That(result).IsFalse();
        await Assert.That(parsed).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task Parse_WithValidSpan_ReturnsTimeInt32()
    {
        var time = new TimeInt32(1000);
        var str = time.ToString();
        var parsed = TimeInt32.Parse(str.AsSpan());
        await Assert.That(parsed).IsEqualTo(time);
    }

    [Test]
    public async Task Parse_WithInvalidSpan_ThrowsFormatException()
    {
        var str = "invalid";
        await Assert.That(() => TimeInt32.Parse(str.AsSpan())).ThrowsExactly<FormatException>();
    }

    [Test]
    [Arguments("1.876", 1876)]
    [Arguments("00:00.00", 0)]
    [Arguments("0:00.69", 690)]
    [Arguments("0:01.00", 1000)]
    [Arguments("0:43.256", 43256)]
    [Arguments("1:00.000", 60000)]
    [Arguments("1:50.011", 110011)]
    public async Task Parse_VariousTimes_ReturnsTimeInt32(string str, int expected)
    {
        var parsed = TimeInt32.Parse(str);
        await Assert.That(parsed.TotalMilliseconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments("-1.876", -1876)]
    [Arguments("-0:00.69", -690)]
    [Arguments("-0:01.00", -1000)]
    [Arguments("-0:43.256", -43256)]
    [Arguments("-1:00.000", -60000)]
    [Arguments("-1:50.011", -110011)]
    [Arguments("-1:23:45.678", -5025678)] // 1h 23m 45s 678ms = 5025678ms
    public async Task Parse_NegativeTimes_ReturnsNegativeTimeInt32(string str, int expected)
    {
        var parsed = TimeInt32.Parse(str);
        await Assert.That(parsed.TotalMilliseconds).IsEqualTo(expected);
    }

    [Test]
    public async Task TryParse_WithNegativeTime_ReturnsTrue()
    {
        var result = TimeInt32.TryParse("-1:23.456", out var parsed);
        await Assert.That(result).IsTrue();
        await Assert.That(parsed.TotalMilliseconds).IsEqualTo(-83456);
    }

    [Test]
    public async Task Parse_WithNegativeTime_ReturnsNegativeTimeInt32()
    {
        var parsed = TimeInt32.Parse("-2:15.789");
        await Assert.That(parsed.TotalMilliseconds).IsEqualTo(-135789);
        await Assert.That(parsed.IsNegative).IsTrue();
    }

    [Test]
    public async Task ToString_NegativeTime_IncludesNegativeSign()
    {
        var negativeTime = new TimeInt32(-83456); // -1:23.456
        var timeString = negativeTime.ToString();
        await Assert.That(timeString).StartsWith("-");
        await Assert.That(timeString).IsEqualTo("-1:23.456");
    }
}
