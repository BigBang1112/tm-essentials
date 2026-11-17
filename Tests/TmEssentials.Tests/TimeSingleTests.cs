namespace TmEssentials.Tests;

public class TimeSingleTests
{
    private const float HalfMillisecond = 0.0005f;

    [Test]
    [Arguments(0f)]
    [Arguments(1f)]
    [Arguments(2.3f)]
    [Arguments(5.5f)]
    [Arguments(5.6f)]
    [Arguments(20f)]
    [Arguments(100f)]
    public async Task Constructor_WithTotalSeconds_SetsTotalSeconds(float totalSeconds)
    {
        // Act
        var time = new TimeSingle(totalSeconds);

        // Assert
        await Assert.That(time.TotalSeconds).IsEqualTo(totalSeconds);
    }

    [Test]
    public async Task ObjectInitializer_WithTotalSeconds_SetsTotalSeconds()
    {
        // Act
        var time = new TimeSingle { TotalSeconds = 2.3f };

        // Assert
        await Assert.That(time.TotalSeconds).IsEqualTo(2.3f);
    }

    [Test]
    [Arguments(-1, -2, -3)]
    [Arguments(0, 0, 0)]
    [Arguments(5, 10, 15)]
    [Arguments(13, 18, 7)]
    public async Task Constructor_WithHoursMinutesSeconds_SetsComponents(int hours, int minutes, int seconds)
    {
        // Act
        var time = new TimeSingle(hours, minutes, seconds);

        // Assert
        await Assert.That(time.Hours).IsEqualTo(hours);
        await Assert.That(time.Minutes).IsEqualTo(minutes);
        await Assert.That(time.Seconds).IsEqualTo(seconds);
    }

    [Test]
    [Arguments(0, -2, -3, -4, -100f)]
    [Arguments(0, 0, 0, 0, 0f)]
    [Arguments(0, 10, 15, 20, 100f)]
    [Arguments(0, 18, 7, 1, 100f)]
    public async Task Constructor_WithDaysHoursMinutesSecondsMilliseconds_SetsComponents(int days, int hours, int minutes, int seconds, float milliseconds)
    {
        // Act
        var time = new TimeSingle(days, hours, minutes, seconds, milliseconds);

        // Assert
        await Assert.That(time.Days).IsEqualTo(days);
        await Assert.That(time.Hours).IsEqualTo(hours);
        await Assert.That(time.Minutes).IsEqualTo(minutes);
        await Assert.That(time.Seconds).IsEqualTo(seconds);
        await Assert.That(time.Milliseconds).IsEqualTo(milliseconds);
    }

    [Test]
    public async Task Zero_EqualsDefault()
    {
        // Act
        var zero = TimeSingle.Zero;

        // Assert
        await Assert.That(zero).IsEqualTo(default(TimeSingle));
    }

    [Test]
    public async Task MaxValue_HasSingleMaxValueSeconds()
    {
        // Act
        var maxValue = TimeSingle.MaxValue;

        // Assert
        await Assert.That(maxValue.TotalSeconds).IsEqualTo(float.MaxValue);
    }

    [Test]
    public async Task MinValue_HasSingleMinValueSeconds()
    {
        // Act
        var minValue = TimeSingle.MinValue;

        // Assert
        await Assert.That(minValue.TotalSeconds).IsEqualTo(float.MinValue);
    }

    [Test]
    [Arguments(0f, 0f)]
    [Arguments(1f, 1_000f)]
    [Arguments(5.6f, 5_600f)]
    [Arguments(100f, 100_000f)]
    public async Task TotalMilliseconds_WithTotalSeconds_ReturnsTotalMilliseconds(float totalSeconds, float expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var totalMilliseconds = time.TotalMilliseconds;

        // Assert
        await Assert.That(totalMilliseconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0f)]
    [Arguments(150f, 2.5f)]
    [Arguments(7.5f, 0.125f)]
    [Arguments(1_200f, 20f)]
    public async Task TotalMinutes_WithTotalSeconds_ReturnsTotalMinutes(float totalSeconds, float expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var totalMinutes = time.TotalMinutes;

        // Assert
        await Assert.That(totalMinutes).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0f)]
    [Arguments(5.4f, 0.0015f)]
    [Arguments(1_800f, 0.5f)]
    [Arguments(10_800f, 3f)]
    public async Task TotalHours_WithTotalSeconds_ReturnsTotalHours(float totalSeconds, float expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var totalHours = time.TotalHours;

        // Assert
        await Assert.That(totalHours).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0f)]
    [Arguments(10_627.2f, 0.123f)]
    [Arguments(60_480f, 0.7f)]
    [Arguments(172_800f, 2f)]
    public async Task TotalDays_WithTotalSeconds_ReturnsTotalDays(float totalSeconds, float expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var totalDays = time.TotalDays;

        // Assert
        await Assert.That(totalDays).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0L)]
    [Arguments(0.05f, 500_000L)]
    [Arguments(0.5f, 5_000_000L)]
    [Arguments(1f, 10_000_000L)]
    public async Task Ticks_WithTotalSeconds_ReturnsTicks(float totalSeconds, long expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var ticks = time.Ticks;

        // Assert
        await Assert.That(ticks).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0f)]
    [Arguments(1f, 0f)]
    [Arguments(5.6f, 600f)]
    [Arguments(11.12f, 120f)]
    [Arguments(100.2455f, 245.5f)]
    public async Task Milliseconds_WithTotalSeconds_ReturnsFractionalMillisecondsComponent(float totalSeconds, float expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var milliseconds = time.Milliseconds;

        // Assert
        await Assert.That(milliseconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0)]
    [Arguments(1f, 0)]
    [Arguments(5.5f, 500)]
    [Arguments(11.1f, 100)]
    [Arguments(100.2455f, 245)]
    public async Task ITimeMilliseconds_WithTotalSeconds_ReturnsWholeMillisecondsComponent(float totalSeconds, int expected)
    {
        // Arrange
        ITime time = new TimeSingle(totalSeconds);

        // Act
        var milliseconds = time.Milliseconds;

        // Assert
        await Assert.That(milliseconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0)]
    [Arguments(1f, 1)]
    [Arguments(5.6f, 5)]
    [Arguments(100f, 40)]
    [Arguments(1_112f, 32)]
    public async Task Seconds_WithTotalSeconds_ReturnsSecondsComponent(float totalSeconds, int expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var seconds = time.Seconds;

        // Assert
        await Assert.That(seconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0)]
    [Arguments(59.99f, 0)]
    [Arguments(60f, 1)]
    [Arguments(61f, 1)]
    [Arguments(320f, 5)]
    [Arguments(3_680f, 1)]
    public async Task Minutes_WithTotalSeconds_ReturnsMinutesComponent(float totalSeconds, int expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var minutes = time.Minutes;

        // Assert
        await Assert.That(minutes).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0)]
    [Arguments(3_599.99f, 0)]
    [Arguments(3_600f, 1)]
    [Arguments(3_601f, 1)]
    [Arguments(10_000f, 2)]
    [Arguments(10_800f, 3)]
    public async Task Hours_WithTotalSeconds_ReturnsHoursComponent(float totalSeconds, int expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var hours = time.Hours;

        // Assert
        await Assert.That(hours).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0)]
    [Arguments(86_399.99f, 0)]
    [Arguments(86_400f, 1)]
    [Arguments(86_401f, 1)]
    [Arguments(172_799f, 1)]
    [Arguments(172_800f, 2)]
    public async Task Days_WithTotalSeconds_ReturnsWholeDays(float totalSeconds, int expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var days = time.Days;

        // Assert
        await Assert.That(days).IsEqualTo(expected);
    }

    [Test]
    [Arguments(-0.001f, true)]
    [Arguments(0f, false)]
    [Arguments(0.001f, false)]
    public async Task IsNegative_WithTotalSeconds_ReturnsWhetherBelowZero(float totalSeconds, bool expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var isNegative = time.IsNegative;

        // Assert
        await Assert.That(isNegative).IsEqualTo(expected);
    }

    [Test]
    [Arguments(-2f)]
    [Arguments(0f)]
    [Arguments(5f)]
    [Arguments(6.5f)]
    [Arguments(12f)]
    public async Task FromDays_WithDays_ReturnsTimeWithSameTotalDays(float days)
    {
        // Act
        var time = TimeSingle.FromDays(days);

        // Assert
        await Assert.That(time.TotalDays).IsEqualTo(days);
    }

    [Test]
    [Arguments(-2f)]
    [Arguments(0f)]
    [Arguments(5f)]
    [Arguments(6.5f)]
    [Arguments(12f)]
    public async Task FromHours_WithHours_ReturnsTimeWithSameTotalHours(float hours)
    {
        // Act
        var time = TimeSingle.FromHours(hours);

        // Assert
        await Assert.That(time.TotalHours).IsEqualTo(hours);
    }

    [Test]
    [Arguments(-2f)]
    [Arguments(0f)]
    [Arguments(5f)]
    [Arguments(6.5f)]
    [Arguments(12f)]
    public async Task FromMilliseconds_WithMilliseconds_ReturnsTimeWithSameTotalMilliseconds(float milliseconds)
    {
        // Act
        var time = TimeSingle.FromMilliseconds(milliseconds);

        // Assert
        await Assert.That(time.TotalMilliseconds).IsEqualTo(milliseconds);
    }

    [Test]
    [Arguments(-2f)]
    [Arguments(0f)]
    [Arguments(5f)]
    [Arguments(6.5f)]
    [Arguments(12f)]
    public async Task FromMinutes_WithMinutes_ReturnsTimeWithSameTotalMinutes(float minutes)
    {
        // Act
        var time = TimeSingle.FromMinutes(minutes);

        // Assert
        await Assert.That(time.TotalMinutes).IsEqualTo(minutes);
    }

    [Test]
    [Arguments(-2f)]
    [Arguments(0f)]
    [Arguments(5f)]
    [Arguments(6.5f)]
    [Arguments(12f)]
    public async Task FromSeconds_WithSeconds_ReturnsTimeWithSameTotalSeconds(float seconds)
    {
        // Act
        var time = TimeSingle.FromSeconds(seconds);

        // Assert
        await Assert.That(time.TotalSeconds).IsEqualTo(seconds);
    }

    [Test]
    [Arguments(0L)]
    [Arguments(-10_000L)]
    [Arguments(10_000L)]
    [Arguments(100_000L)]
    [Arguments(5_100_000L)]
    public async Task FromTicks_WithWholeMillisecondTicks_ReturnsTimeWithSameTicks(long ticks)
    {
        // Act
        var time = TimeSingle.FromTicks(ticks);

        // Assert
        await Assert.That(time.Ticks).IsEqualTo(ticks);
    }

    [Test]
    [Arguments(5f, 10f, 15f)]
    [Arguments(-1f, -2f, -3f)]
    [Arguments(0f, 0f, 0f)]
    [Arguments(12f, 15.5f, 27.5f)]
    public async Task Add_WithTime_ReturnsSum(float totalSeconds, float otherTotalSeconds, float expectedTotalSeconds)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);
        var other = new TimeSingle(otherTotalSeconds);

        // Act
        var result = time.Add(other);

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(expectedTotalSeconds));
    }

    [Test]
    [Arguments(0f, 1f, 0f)]
    [Arguments(1f, 0.5f, 2f)]
    [Arguments(10f, 2f, 5f)]
    public async Task Divide_WithSingleDivisor_ReturnsQuotient(float totalSeconds, float divisor, float expectedTotalSeconds)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var result = time.Divide(divisor);

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(expectedTotalSeconds));
    }

    [Test]
    [Arguments(0f, 1f, 0f)]
    [Arguments(1f, 0.5f, 2f)]
    [Arguments(10f, 2f, 5f)]
    public async Task Divide_WithTime_ReturnsRatio(float totalSeconds, float divisorTotalSeconds, float expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);
        var divisor = new TimeSingle(divisorTotalSeconds);

        // Act
        var result = time.Divide(divisor);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0f, 0f)]
    [Arguments(-1f, 1f)]
    [Arguments(10f, 10f)]
    [Arguments(-10f, 10f)]
    public async Task Duration_WithTime_ReturnsAbsoluteValue(float totalSeconds, float expectedTotalSeconds)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var result = time.Duration();

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(expectedTotalSeconds));
    }

    [Test]
    [Arguments(0f, 1f, 0f)]
    [Arguments(1f, 0.5f, 0.5f)]
    [Arguments(10f, 2f, 20f)]
    public async Task Multiply_WithSingleFactor_ReturnsProduct(float totalSeconds, float factor, float expectedTotalSeconds)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var result = time.Multiply(factor);

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(expectedTotalSeconds));
    }

    [Test]
    [Arguments(0f, 0f)]
    [Arguments(1f, -1f)]
    [Arguments(5.5f, -5.5f)]
    [Arguments(-8.5f, 8.5f)]
    public async Task Negate_WithTime_ReturnsOppositeSign(float totalSeconds, float expectedTotalSeconds)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var result = time.Negate();

        // Assert
        await Assert.That(result.TotalSeconds).IsEqualTo(expectedTotalSeconds);
    }

    [Test]
    [Arguments(0f)]
    [Arguments(1f)]
    [Arguments(5.5f)]
    [Arguments(-8.5f)]
    public async Task ToTimeInt32_WithWholeMillisecondTime_PreservesTotalSeconds(float totalSeconds)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var result = time.ToTimeInt32();

        // Assert
        await Assert.That(result.TotalSeconds).IsEqualTo(totalSeconds);
    }

    [Test]
    [Arguments(0.251f, 251)]
    [Arguments(0.009f, 9)]
    [Arguments(-0.251f, -251)]
    [Arguments(0.9996f, 1000)]
    [Arguments(0.0004f, 0)]
    public async Task ToTimeInt32_WithFloatError_RoundsToNearestMillisecond(float totalSeconds, int expectedMilliseconds)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var result = time.ToTimeInt32();
        var castResult = (TimeInt32)time;

        // Assert
        await Assert.That(result.TotalMilliseconds).IsEqualTo(expectedMilliseconds);
        await Assert.That(castResult.TotalMilliseconds).IsEqualTo(expectedMilliseconds);
    }

    [Test]
    public async Task ToTimeInt32_ForEveryMillisecondUnderOneMinute_ReturnsSameMilliseconds()
    {
        // Arrange
        var mismatch = -1;

        // Act
        for (var ms = 0; ms < 60_000; ms++)
        {
            if (new TimeSingle(ms / 1000f).ToTimeInt32().TotalMilliseconds != ms)
            {
                mismatch = ms;
                break;
            }
        }

        // Assert
        await Assert.That(mismatch).IsEqualTo(-1);
    }

    [Test]
    public async Task GreaterThanOperator_WhenLeftIsGreater_ReturnsTrue()
    {
        // Arrange
        var left = new TimeSingle(2f);
        var right = new TimeSingle(1.5f);

        // Act
        var result = left > right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(1.5f, 2f)]
    [Arguments(2f, 2f)]
    public async Task GreaterThanOperator_WhenLeftIsNotGreater_ReturnsFalse(float leftTotalSeconds, float rightTotalSeconds)
    {
        // Arrange
        var left = new TimeSingle(leftTotalSeconds);
        var right = new TimeSingle(rightTotalSeconds);

        // Act
        var result = left > right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(2f, 2f)]
    [Arguments(2f, 1.5f)]
    public async Task GreaterThanOrEqualOperator_WhenLeftIsGreaterOrEqual_ReturnsTrue(float leftTotalSeconds, float rightTotalSeconds)
    {
        // Arrange
        var left = new TimeSingle(leftTotalSeconds);
        var right = new TimeSingle(rightTotalSeconds);

        // Act
        var result = left >= right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task GreaterThanOrEqualOperator_WhenLeftIsLess_ReturnsFalse()
    {
        // Arrange
        var left = new TimeSingle(1.5f);
        var right = new TimeSingle(2f);

        // Act
        var result = left >= right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task LessThanOperator_WhenLeftIsLess_ReturnsTrue()
    {
        // Arrange
        var left = new TimeSingle(1.5f);
        var right = new TimeSingle(2f);

        // Act
        var result = left < right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(2f, 1.5f)]
    [Arguments(2f, 2f)]
    public async Task LessThanOperator_WhenLeftIsNotLess_ReturnsFalse(float leftTotalSeconds, float rightTotalSeconds)
    {
        // Arrange
        var left = new TimeSingle(leftTotalSeconds);
        var right = new TimeSingle(rightTotalSeconds);

        // Act
        var result = left < right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(2f, 2f)]
    [Arguments(2f, 2.5f)]
    public async Task LessThanOrEqualOperator_WhenLeftIsLessOrEqual_ReturnsTrue(float leftTotalSeconds, float rightTotalSeconds)
    {
        // Arrange
        var left = new TimeSingle(leftTotalSeconds);
        var right = new TimeSingle(rightTotalSeconds);

        // Act
        var result = left <= right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task LessThanOrEqualOperator_WhenLeftIsGreater_ReturnsFalse()
    {
        // Arrange
        var left = new TimeSingle(2.5f);
        var right = new TimeSingle(2f);

        // Act
        var result = left <= right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task EqualityOperator_WithEqualTimeInt32_ReturnsTrue()
    {
        // Arrange
        var time = new TimeSingle(1f);
        var other = new TimeInt32(1_000);

        // Act
        var result = time == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task InequalityOperator_WithDifferentTimeInt32_ReturnsTrue()
    {
        // Arrange
        var time = new TimeSingle(1.5f);
        var other = new TimeInt32(1_000);

        // Act
        var result = time != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task UnaryPlusOperator_WithTime_ReturnsSameTime()
    {
        // Arrange
        var time = new TimeSingle(1.5f);

        // Act
        var result = +time;

        // Assert
        await Assert.That(result).IsEqualTo(time);
    }

    [Test]
    public async Task UnaryMinusOperator_WithTime_ReturnsOppositeSign()
    {
        // Arrange
        var time = new TimeSingle(1.5f);

        // Act
        var result = -time;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(-1.5f));
    }

    [Test]
    public async Task AdditionOperator_WithTimeSingle_ReturnsSum()
    {
        // Arrange
        var left = new TimeSingle(1f);
        var right = new TimeSingle(2.5f);

        // Act
        var result = left + right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(3.5f));
    }

    [Test]
    public async Task AdditionOperator_WithTimeSpan_ReturnsSum()
    {
        // Arrange
        var left = new TimeSingle(1f);
        var right = TimeSpan.FromSeconds(2.5);

        // Act
        var result = left + right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(3.5f));
    }

    [Test]
    public async Task SubtractionOperator_WithTimeSingle_ReturnsDifference()
    {
        // Arrange
        var left = new TimeSingle(3.5f);
        var right = new TimeSingle(1f);

        // Act
        var result = left - right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(2.5f));
    }

    [Test]
    public async Task SubtractionOperator_WithTimeSpan_ReturnsDifference()
    {
        // Arrange
        var left = new TimeSingle(3.5f);
        var right = TimeSpan.FromSeconds(1);

        // Act
        var result = left - right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(2.5f));
    }

    [Test]
    public async Task MultiplicationOperator_SingleByTime_ReturnsProduct()
    {
        // Arrange
        var time = new TimeSingle(2f);

        // Act
        var result = 2.5f * time;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(5f));
    }

    [Test]
    public async Task MultiplicationOperator_TimeBySingle_ReturnsProduct()
    {
        // Arrange
        var time = new TimeSingle(1.5f);

        // Act
        var result = time * 2f;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(3f));
    }

    [Test]
    public async Task DivisionOperator_TimeBySingle_ReturnsQuotient()
    {
        // Arrange
        var time = new TimeSingle(3f);

        // Act
        var result = time / 2f;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(1.5f));
    }

    [Test]
    public async Task DivisionOperator_TimeByTime_ReturnsRatio()
    {
        // Arrange
        var dividend = new TimeSingle(3f);
        var divisor = new TimeSingle(1.5f);

        // Act
        var result = dividend / divisor;

        // Assert
        await Assert.That(result).IsEqualTo(2f);
    }

    [Test]
    public async Task ImplicitConversionToTimeSpan_WithTime_ReturnsEquivalentTimeSpan()
    {
        // Arrange
        var time = new TimeSingle(1.5f);

        // Act
        TimeSpan result = time;

        // Assert
        await Assert.That(result).IsEqualTo(TimeSpan.FromSeconds(1.5));
    }

    [Test]
    public async Task ImplicitConversionFromTimeSpan_WithTimeSpan_ReturnsEquivalentTime()
    {
        // Arrange
        var timeSpan = TimeSpan.FromSeconds(2);

        // Act
        TimeSingle result = timeSpan;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(2f));
    }

    [Test]
    public async Task ImplicitConversionFromTimeInt32_WithTimeInt32_ReturnsEquivalentTime()
    {
        // Arrange
        var timeInt32 = new TimeInt32(3_000);

        // Act
        TimeSingle result = timeInt32;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(3f));
    }

    [Test]
    [Arguments(1f, 1.5f, -1)]
    [Arguments(1.5f, 1f, 1)]
    [Arguments(1f, 1f, 0)]
    public async Task CompareTo_WithTimeSingle_ReturnsRelativeOrder(float totalSeconds, float otherTotalSeconds, int expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);
        var other = new TimeSingle(otherTotalSeconds);

        // Act
        var result = time.CompareTo(other);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(1f, 1_500, -1)]
    [Arguments(1.5f, 1_000, 1)]
    [Arguments(1f, 1_000, 0)]
    public async Task CompareTo_WithTimeInt32_ReturnsRelativeOrder(float totalSeconds, int otherTotalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);
        var other = new TimeInt32(otherTotalMilliseconds);

        // Act
        var result = time.CompareTo(other);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(1f, 1.5f, -1)]
    [Arguments(1.5f, 1f, 1)]
    [Arguments(1f, 1f, 0)]
    public async Task CompareTo_WithTimeAsObject_ReturnsRelativeOrder(float totalSeconds, float otherTotalSeconds, int expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);
        object other = new TimeSingle(otherTotalSeconds);

        // Act
        var result = time.CompareTo(other);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task CompareTo_WithNullITime_ReturnsPositive()
    {
        // Arrange
        var time = new TimeSingle(1f);

        // Act
        var result = time.CompareTo((ITime?)null);

        // Assert
        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithNullObject_ReturnsPositive()
    {
        // Arrange
        var time = new TimeSingle(1f);

        // Act
        var result = time.CompareTo((object?)null);

        // Assert
        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithNonTimeObject_ThrowsArgumentException()
    {
        // Arrange
        var time = new TimeSingle(1f);

        // Act
        var act = () => time.CompareTo(new object());

        // Assert
        await Assert.That(act).ThrowsExactly<ArgumentException>();
    }

    [Test]
    public async Task Equals_WithNullITime_ReturnsFalse()
    {
        // Arrange
        var time = new TimeSingle(1f);

        // Act
        var result = time.Equals((ITime?)null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WithDifferentTimeInt32_ReturnsFalse()
    {
        // Arrange
        var time = new TimeSingle(2f);
        var other = new TimeInt32(1_000);

        // Act
        var result = time.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("1.876", 1.876f)]
    [Arguments("00:00.00", 0f)]
    [Arguments("0:00.69", 0.69f)]
    [Arguments("0:01.00", 1f)]
    [Arguments("0:01.000", 1f)]
    [Arguments("0:43.256", 43.256f)]
    [Arguments("1:00.000", 60f)]
    [Arguments("1:50.011", 110.011f)]
    public async Task Parse_WithTimeString_ReturnsTime(string input, float expectedTotalSeconds)
    {
        // Act
        var time = TimeSingle.Parse(input);

        // Assert
        await Assert.That(time.TotalSeconds).IsEqualTo(expectedTotalSeconds).Within(HalfMillisecond);
    }

    [Test]
    [Arguments("-1.876", -1.876f)]
    [Arguments("-0:00.69", -0.69f)]
    [Arguments("-0:01.00", -1f)]
    [Arguments("-0:43.256", -43.256f)]
    [Arguments("-1:00.000", -60f)]
    [Arguments("-1:50.011", -110.011f)]
    [Arguments("-2:15.789", -135.789f)]
    [Arguments("-1:23:45.678", -5_025.678f)]
    public async Task Parse_WithNegativeTimeString_ReturnsNegativeTime(string input, float expectedTotalSeconds)
    {
        // Act
        var time = TimeSingle.Parse(input);

        // Assert
        await Assert.That(time.TotalSeconds).IsEqualTo(expectedTotalSeconds).Within(HalfMillisecond);
    }

    [Test]
    public async Task Parse_WithInvalidString_ThrowsFormatException()
    {
        // Act
        var act = () => TimeSingle.Parse("invalid");

        // Assert
        await Assert.That(act).ThrowsExactly<FormatException>();
    }

    [Test]
    public async Task Parse_WithValidSpan_ReturnsTime()
    {
        // Act
        var time = TimeSingle.Parse("0:01.000".AsSpan());

        // Assert
        await Assert.That(time).IsEqualTo(new TimeSingle(1f));
    }

    [Test]
    public async Task Parse_WithInvalidSpan_ThrowsFormatException()
    {
        // Act
        var act = () => TimeSingle.Parse("invalid".AsSpan());

        // Assert
        await Assert.That(act).ThrowsExactly<FormatException>();
    }

    [Test]
    [Arguments("0:01.000", 1f)]
    [Arguments("-1:23.456", -83.456f)]
    public async Task TryParse_WithValidString_ReturnsTrueAndTime(string input, float expectedTotalSeconds)
    {
        // Act
        var success = TimeSingle.TryParse(input, out var time);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(time.TotalSeconds).IsEqualTo(expectedTotalSeconds).Within(HalfMillisecond);
    }

    [Test]
    [Arguments("invalid")]
    [Arguments(null)]
    public async Task TryParse_WithInvalidString_ReturnsFalseAndZero(string? input)
    {
        // Act
        var success = TimeSingle.TryParse(input!, out var time);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(time).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task TryParse_WithValidSpan_ReturnsTrueAndTime()
    {
        // Act
        var success = TimeSingle.TryParse("0:01.000".AsSpan(), out var time);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(time).IsEqualTo(new TimeSingle(1f));
    }

    [Test]
    [Arguments("invalid")]
    [Arguments("")]
    public async Task TryParse_WithInvalidSpan_ReturnsFalseAndZero(string input)
    {
        // Act
        var success = TimeSingle.TryParse(input.AsSpan(), out var time);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(time).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task ToString_WithoutArguments_ReturnsTimeWithMilliseconds()
    {
        // Arrange
        var time = new TimeSingle(3_723.5f);

        // Act
        var result = time.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("1:02:03.500");
    }

    [Test]
    public async Task ToString_WithNegativeTime_PrefixesMinusSign()
    {
        // Arrange
        var time = new TimeSingle(-83.456f);

        // Act
        var result = time.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("-1:23.456");
    }

    [Test]
    [Arguments(0.251f, false, "0:00.251")]
    [Arguments(0.251f, true, "0:00.25")]
    [Arguments(-0.251f, false, "-0:00.251")]
    [Arguments(0.9996f, false, "0:01.000")]
    [Arguments(59.9999f, false, "1:00.000")]
    [Arguments(3_599.9996f, false, "1:00:00.000")]
    [Arguments(-0.0001f, false, "0:00.000")]
    public async Task ToString_WithFloatError_RoundsToNearestMillisecond(float totalSeconds, bool useHundredths, string expected)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var result = time.ToString(useHundredths);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task ToString_ForEveryMillisecondUnderOneMinute_MatchesTimeInt32()
    {
        // Arrange
        var mismatch = -1;

        // Act
        for (var ms = 0; ms < 60_000; ms++)
        {
            if (new TimeSingle(ms / 1000f).ToString() != new TimeInt32(ms).ToString())
            {
                mismatch = ms;
                break;
            }
        }

        // Assert
        await Assert.That(mismatch).IsEqualTo(-1);
    }

    [Test]
    [Arguments("60.0")]
    [Arguments("1:60.0")]
    [Arguments("60:00.0")]
    [Arguments("24:00:00.0")]
    [Arguments("1:24:00:00.0")]
    [Arguments("10675200:00:00:00.0")]
    [Arguments("10675199:23:59:59.9")]
    [Arguments("10675199:02:48:05.4775808")]
    public async Task TryParse_WithPartOutOfRange_ReturnsFalse(string input)
    {
        // Act
        var successFromString = TimeSingle.TryParse(input, out _);
        var successFromSpan = TimeSingle.TryParse(input.AsSpan(), out _);

        // Assert
        await Assert.That(successFromString).IsFalse();
        await Assert.That(successFromSpan).IsFalse();
    }

    [Test]
    public async Task TryParse_WithLargestTimeSpan_ReturnsTime()
    {
        // Act
        var success = TimeSingle.TryParse("10675199:02:48:05.4775807", out var time);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(time.TotalSeconds).IsEqualTo((float)TimeSpan.MaxValue.TotalSeconds);
    }

    [Test]
    public async Task ToString_AsObject_ReturnsTmFormat()
    {
        // Arrange
        object time = new TimeSingle(83.456f);

        // Act
        var result = time.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("1:23.456");
    }

    [Test]
    public async Task ITimeAdd_WithTimeInt32_ReturnsTimeSingleSum()
    {
        // Arrange
        ITime time = new TimeSingle(1f);

        // Act
        var result = time.Add(new TimeInt32(500));

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(1.5f));
    }

    [Test]
    public async Task Add_WithITime_ReturnsTimeSingleSum()
    {
        // Act
        var result = new TimeSingle(1f).Add((ITime)new TimeInt32(500));

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(1.5f));
    }

    [Test]
    public async Task ITimeDivide_WithFloatDivisor_ReturnsTimeSingleQuotient()
    {
        // Arrange
        ITime time = new TimeSingle(1f);

        // Act
        var result = time.Divide(4f);

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(0.25f));
    }

    [Test]
    public async Task Divide_WithITime_ReturnsRatio()
    {
        // Act
        var result = new TimeSingle(3f).Divide((ITime)new TimeInt32(1_500));

        // Assert
        await Assert.That(result).IsEqualTo(2f);
    }

    [Test]
    public async Task ITimeDuration_WithNegativeTime_ReturnsPositiveTimeSingle()
    {
        // Arrange
        ITime time = new TimeSingle(-1.5f);

        // Act
        var result = time.Duration();

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(1.5f));
    }

    [Test]
    public async Task ITimeMultiply_WithFloatFactor_ReturnsTimeSingleProduct()
    {
        // Arrange
        ITime time = new TimeSingle(1f);

        // Act
        var result = time.Multiply(2.5f);

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(2.5f));
    }

    [Test]
    public async Task ITimeNegate_WithPositiveTime_ReturnsNegativeTimeSingle()
    {
        // Arrange
        ITime time = new TimeSingle(1.5f);

        // Act
        var result = time.Negate();

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(-1.5f));
    }

    [Test]
    [Arguments(1.5f, true)]
    [Arguments(2.5f, false)]
    public async Task Equals_WithTimeSingleAsITime_ComparesTotalSeconds(float otherTotalSeconds, bool expected)
    {
        // Arrange
        var time = new TimeSingle(1.5f);
        ITime other = new TimeSingle(otherTotalSeconds);

        // Act
        var result = time.Equals(other);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task TryParse_WithNegativeSpan_ReturnsTrueAndNegativeTime()
    {
        // Act
        var success = TimeSingle.TryParse("-1:23.456".AsSpan(), out var time);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(time.TotalSeconds).IsEqualTo(-83.456f);
    }
}
