namespace TmEssentials.Tests;

public class TimeInt32Tests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(50)]
    [Arguments(2_000)]
    [Arguments(234_567_890)]
    public async Task Constructor_WithTotalMilliseconds_SetsTotalMilliseconds(int totalMilliseconds)
    {
        // Act
        var time = new TimeInt32(totalMilliseconds);

        // Assert
        await Assert.That(time.TotalMilliseconds).IsEqualTo(totalMilliseconds);
    }

    [Test]
    public async Task ObjectInitializer_WithTotalMilliseconds_SetsTotalMilliseconds()
    {
        // Act
        var time = new TimeInt32 { TotalMilliseconds = 234_567_890 };

        // Assert
        await Assert.That(time.TotalMilliseconds).IsEqualTo(234_567_890);
    }

    [Test]
    [Arguments(-1, -2, -3)]
    [Arguments(0, 0, 0)]
    [Arguments(5, 10, 15)]
    [Arguments(13, 18, 7)]
    public async Task Constructor_WithHoursMinutesSeconds_SetsComponents(int hours, int minutes, int seconds)
    {
        // Act
        var time = new TimeInt32(hours, minutes, seconds);

        // Assert
        await Assert.That(time.Hours).IsEqualTo(hours);
        await Assert.That(time.Minutes).IsEqualTo(minutes);
        await Assert.That(time.Seconds).IsEqualTo(seconds);
    }

    [Test]
    [Arguments(0, -2, -3, -4, -5)]
    [Arguments(0, 0, 0, 0, 0)]
    [Arguments(0, 10, 15, 20, 24)]
    [Arguments(0, 18, 7, 1, 8)]
    public async Task Constructor_WithDaysHoursMinutesSecondsMilliseconds_SetsComponents(int days, int hours, int minutes, int seconds, int milliseconds)
    {
        // Act
        var time = new TimeInt32(days, hours, minutes, seconds, milliseconds);

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
        var zero = TimeInt32.Zero;

        // Assert
        await Assert.That(zero).IsEqualTo(default(TimeInt32));
    }

    [Test]
    public async Task MaxValue_HasInt32MaxValueMilliseconds()
    {
        // Act
        var maxValue = TimeInt32.MaxValue;

        // Assert
        await Assert.That(maxValue.TotalMilliseconds).IsEqualTo(int.MaxValue);
    }

    [Test]
    public async Task MinValue_HasInt32MinValueMilliseconds()
    {
        // Act
        var minValue = TimeInt32.MinValue;

        // Assert
        await Assert.That(minValue.TotalMilliseconds).IsEqualTo(int.MinValue);
    }

    [Test]
    [Arguments(0, 0f)]
    [Arguments(900, 0.9f)]
    [Arguments(990, 0.99f)]
    [Arguments(1_000, 1f)]
    [Arguments(3_200, 3.2f)]
    [Arguments(5_500, 5.5f)]
    public async Task TotalSeconds_WithTotalMilliseconds_ReturnsTotalSeconds(int totalMilliseconds, float expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var totalSeconds = time.TotalSeconds;

        // Assert
        await Assert.That(totalSeconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0f)]
    [Arguments(66_000, 1.1f)]
    [Arguments(192_000, 3.2f)]
    public async Task TotalMinutes_WithTotalMilliseconds_ReturnsTotalMinutes(int totalMilliseconds, float expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var totalMinutes = time.TotalMinutes;

        // Assert
        await Assert.That(totalMinutes).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0f)]
    [Arguments(1_800_000, 0.5f)]
    [Arguments(3_600_000, 1f)]
    [Arguments(5_400_000, 1.5f)]
    public async Task TotalHours_WithTotalMilliseconds_ReturnsTotalHours(int totalMilliseconds, float expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var totalHours = time.TotalHours;

        // Assert
        await Assert.That(totalHours).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0f)]
    [Arguments(43_200_000, 0.5f)]
    [Arguments(86_400_000, 1f)]
    public async Task TotalDays_WithTotalMilliseconds_ReturnsTotalDays(int totalMilliseconds, float expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var totalDays = time.TotalDays;

        // Assert
        await Assert.That(totalDays).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0L)]
    [Arguments(-2, -20_000L)]
    [Arguments(1, 10_000L)]
    [Arguments(3, 30_000L)]
    public async Task Ticks_WithTotalMilliseconds_ReturnsTicks(int totalMilliseconds, long expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var ticks = time.Ticks;

        // Assert
        await Assert.That(ticks).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(-1_200, -200)]
    [Arguments(500, 500)]
    [Arguments(1_200, 200)]
    [Arguments(2_600, 600)]
    public async Task Milliseconds_WithTotalMilliseconds_ReturnsMillisecondsComponent(int totalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var milliseconds = time.Milliseconds;

        // Assert
        await Assert.That(milliseconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(3_000, 3)]
    [Arguments(59_999, 59)]
    [Arguments(60_000, 0)]
    [Arguments(60_999, 0)]
    [Arguments(61_000, 1)]
    [Arguments(16_777_999, 37)]
    [Arguments(36_000_999, 0)]
    [Arguments(86_399_999, 59)]
    [Arguments(int.MaxValue, 23)]
    [Arguments(-36_000_999, 0)]
    public async Task Seconds_WithTotalMilliseconds_ReturnsSecondsComponent(int totalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var seconds = time.Seconds;

        // Assert
        await Assert.That(seconds).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(59_999, 0)]
    [Arguments(60_000, 1)]
    [Arguments(61_000, 1)]
    [Arguments(320_000, 5)]
    [Arguments(3_680_000, 1)]
    [Arguments(86_399_999, 59)]
    [Arguments(int.MaxValue, 31)]
    public async Task Minutes_WithTotalMilliseconds_ReturnsMinutesComponent(int totalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var minutes = time.Minutes;

        // Assert
        await Assert.That(minutes).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(3_599_999, 0)]
    [Arguments(3_600_000, 1)]
    [Arguments(3_601_000, 1)]
    [Arguments(10_000_000, 2)]
    [Arguments(10_800_000, 3)]
    [Arguments(86_399_999, 23)]
    [Arguments(int.MaxValue, 20)]
    public async Task Hours_WithTotalMilliseconds_ReturnsHoursComponent(int totalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var hours = time.Hours;

        // Assert
        await Assert.That(hours).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(86_399_990, 0)]
    [Arguments(86_399_999, 0)]
    [Arguments(86_400_000, 1)]
    [Arguments(int.MaxValue, 24)]
    [Arguments(int.MinValue, -24)]
    [Arguments(86_401_000, 1)]
    [Arguments(172_799_000, 1)]
    [Arguments(172_800_000, 2)]
    public async Task Days_WithTotalMilliseconds_ReturnsWholeDays(int totalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var days = time.Days;

        // Assert
        await Assert.That(days).IsEqualTo(expected);
    }

    [Test]
    [Arguments(-1, true)]
    [Arguments(0, false)]
    [Arguments(1, false)]
    public async Task IsNegative_WithTotalMilliseconds_ReturnsWhetherBelowZero(int totalMilliseconds, bool expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

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
        var time = TimeInt32.FromDays(days);

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
        var time = TimeInt32.FromHours(hours);

        // Assert
        await Assert.That(time.TotalHours).IsEqualTo(hours);
    }

    [Test]
    [Arguments(-2)]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(12)]
    public async Task FromMilliseconds_WithMilliseconds_ReturnsTimeWithSameTotalMilliseconds(int milliseconds)
    {
        // Act
        var time = TimeInt32.FromMilliseconds(milliseconds);

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
        var time = TimeInt32.FromMinutes(minutes);

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
        var time = TimeInt32.FromSeconds(seconds);

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
        var time = TimeInt32.FromTicks(ticks);

        // Assert
        await Assert.That(time.Ticks).IsEqualTo(ticks);
    }

    [Test]
    [Arguments(5, 10, 15)]
    [Arguments(-1, -2, -3)]
    [Arguments(0, 0, 0)]
    [Arguments(12, 15, 27)]
    public async Task Add_WithTime_ReturnsSum(int totalMilliseconds, int otherTotalMilliseconds, int expectedTotalMilliseconds)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);
        var other = new TimeInt32(otherTotalMilliseconds);

        // Act
        var result = time.Add(other);

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(expectedTotalMilliseconds));
    }

    [Test]
    [Arguments(0, 1f, 0f)]
    [Arguments(1, 0.5f, 2f)]
    [Arguments(10, 2f, 5f)]
    public async Task Divide_WithSingleDivisor_ReturnsQuotient(int totalMilliseconds, float divisor, float expectedTotalMilliseconds)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var result = time.Divide(divisor);

        // Assert
        await Assert.That(result.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
    }

    [Test]
    [Arguments(0, 1, 0f)]
    [Arguments(6, 2, 3f)]
    [Arguments(14, 2, 7f)]
    public async Task Divide_WithTime_ReturnsRatio(int totalMilliseconds, int divisorTotalMilliseconds, float expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);
        var divisor = new TimeInt32(divisorTotalMilliseconds);

        // Act
        var result = time.Divide(divisor);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(-1, 1)]
    [Arguments(10, 10)]
    [Arguments(-10, 10)]
    public async Task Duration_WithTime_ReturnsAbsoluteValue(int totalMilliseconds, int expectedTotalMilliseconds)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var result = time.Duration();

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(expectedTotalMilliseconds));
    }

    [Test]
    [Arguments(0, 1f, 0f)]
    [Arguments(2, 0.5f, 1f)]
    [Arguments(10, 2f, 20f)]
    public async Task Multiply_WithSingleFactor_ReturnsProduct(int totalMilliseconds, float factor, float expectedTotalMilliseconds)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var result = time.Multiply(factor);

        // Assert
        await Assert.That(result.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, -1)]
    [Arguments(5, -5)]
    [Arguments(-8, 8)]
    public async Task Negate_WithTime_ReturnsOppositeSign(int totalMilliseconds, int expectedTotalMilliseconds)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var result = time.Negate();

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(expectedTotalMilliseconds));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(5)]
    [Arguments(-8)]
    public async Task ToTimeSingle_WithTime_PreservesTotalMilliseconds(int totalMilliseconds)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var result = time.ToTimeSingle();

        // Assert
        await Assert.That(result.TotalMilliseconds).IsEqualTo(totalMilliseconds);
    }

    [Test]
    public async Task GreaterThanOperator_WhenLeftIsGreater_ReturnsTrue()
    {
        // Arrange
        var left = new TimeInt32(1_000);
        var right = new TimeInt32(500);

        // Act
        var result = left > right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(500, 1_000)]
    [Arguments(1_000, 1_000)]
    public async Task GreaterThanOperator_WhenLeftIsNotGreater_ReturnsFalse(int leftTotalMilliseconds, int rightTotalMilliseconds)
    {
        // Arrange
        var left = new TimeInt32(leftTotalMilliseconds);
        var right = new TimeInt32(rightTotalMilliseconds);

        // Act
        var result = left > right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(1_000, 1_000)]
    [Arguments(1_000, 500)]
    public async Task GreaterThanOrEqualOperator_WhenLeftIsGreaterOrEqual_ReturnsTrue(int leftTotalMilliseconds, int rightTotalMilliseconds)
    {
        // Arrange
        var left = new TimeInt32(leftTotalMilliseconds);
        var right = new TimeInt32(rightTotalMilliseconds);

        // Act
        var result = left >= right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task GreaterThanOrEqualOperator_WhenLeftIsLess_ReturnsFalse()
    {
        // Arrange
        var left = new TimeInt32(500);
        var right = new TimeInt32(1_000);

        // Act
        var result = left >= right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task LessThanOperator_WhenLeftIsLess_ReturnsTrue()
    {
        // Arrange
        var left = new TimeInt32(500);
        var right = new TimeInt32(1_000);

        // Act
        var result = left < right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(1_000, 500)]
    [Arguments(1_000, 1_000)]
    public async Task LessThanOperator_WhenLeftIsNotLess_ReturnsFalse(int leftTotalMilliseconds, int rightTotalMilliseconds)
    {
        // Arrange
        var left = new TimeInt32(leftTotalMilliseconds);
        var right = new TimeInt32(rightTotalMilliseconds);

        // Act
        var result = left < right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(1_000, 1_000)]
    [Arguments(1_000, 1_500)]
    public async Task LessThanOrEqualOperator_WhenLeftIsLessOrEqual_ReturnsTrue(int leftTotalMilliseconds, int rightTotalMilliseconds)
    {
        // Arrange
        var left = new TimeInt32(leftTotalMilliseconds);
        var right = new TimeInt32(rightTotalMilliseconds);

        // Act
        var result = left <= right;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task LessThanOrEqualOperator_WhenLeftIsGreater_ReturnsFalse()
    {
        // Arrange
        var left = new TimeInt32(1_500);
        var right = new TimeInt32(1_000);

        // Act
        var result = left <= right;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task EqualityOperator_WithEqualTimeSingle_ReturnsTrue()
    {
        // Arrange
        var time = new TimeInt32(1_000);
        var other = new TimeSingle(1f);

        // Act
        var result = time == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task InequalityOperator_WithDifferentTimeSingle_ReturnsTrue()
    {
        // Arrange
        var time = new TimeInt32(1_000);
        var other = new TimeSingle(0.5f);

        // Act
        var result = time != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task UnaryPlusOperator_WithTime_ReturnsSameTime()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = +time;

        // Assert
        await Assert.That(result).IsEqualTo(time);
    }

    [Test]
    public async Task UnaryMinusOperator_WithTime_ReturnsOppositeSign()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = -time;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(-1_000));
    }

    [Test]
    public async Task AdditionOperator_WithTimeInt32_ReturnsSum()
    {
        // Arrange
        var left = new TimeInt32(1_000);
        var right = new TimeInt32(500);

        // Act
        var result = left + right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(1_500));
    }

    [Test]
    public async Task AdditionOperator_WithTimeSpan_ReturnsSum()
    {
        // Arrange
        var left = new TimeInt32(1_000);
        var right = TimeSpan.FromMilliseconds(500);

        // Act
        var result = left + right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(1_500));
    }

    [Test]
    public async Task SubtractionOperator_WithTimeInt32_ReturnsDifference()
    {
        // Arrange
        var left = new TimeInt32(1_500);
        var right = new TimeInt32(500);

        // Act
        var result = left - right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(1_000));
    }

    [Test]
    public async Task SubtractionOperator_WithTimeSpan_ReturnsDifference()
    {
        // Arrange
        var left = new TimeInt32(1_500);
        var right = TimeSpan.FromMilliseconds(500);

        // Act
        var result = left - right;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(1_000));
    }

    [Test]
    public async Task MultiplicationOperator_SingleByTime_ReturnsTimeSingleProduct()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = 2.5f * time;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(2.5f));
    }

    [Test]
    public async Task MultiplicationOperator_TimeBySingle_ReturnsTimeSingleProduct()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = time * 0.5f;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(0.5f));
    }

    [Test]
    public async Task MultiplicationOperator_Int32ByTime_ReturnsTimeInt32Product()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = 3 * time;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(3_000));
    }

    [Test]
    public async Task MultiplicationOperator_TimeByInt32_ReturnsTimeInt32Product()
    {
        // Arrange
        var time = new TimeInt32(500);

        // Act
        var result = time * 4;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(2_000));
    }

    [Test]
    public async Task DivisionOperator_TimeBySingle_ReturnsTimeSingleQuotient()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = time / 2f;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeSingle(0.5f));
    }

    [Test]
    public async Task DivisionOperator_TimeByTime_ReturnsRatio()
    {
        // Arrange
        var dividend = new TimeInt32(1_500);
        var divisor = new TimeInt32(500);

        // Act
        var result = dividend / divisor;

        // Assert
        await Assert.That(result).IsEqualTo(3f);
    }

    [Test]
    public async Task ImplicitConversionToTimeSpan_WithTime_ReturnsEquivalentTimeSpan()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        TimeSpan result = time;

        // Assert
        await Assert.That(result).IsEqualTo(TimeSpan.FromMilliseconds(1_000));
    }

    [Test]
    public async Task ExplicitConversionFromTimeSpan_WithTimeSpan_ReturnsEquivalentTime()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMilliseconds(500);

        // Act
        var result = (TimeInt32)timeSpan;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(500));
    }

    [Test]
    [Arguments(180_000_010_000L, 18_000_001)]
    [Arguments(180_000_019_999L, 18_000_001)]
    [Arguments(-180_000_019_999L, -18_000_001)]
    [Arguments(864_000_000_000L - 10_000L, 86_399_999)]
    [Arguments(int.MaxValue * 10_000L + 9_999L, int.MaxValue)]
    [Arguments(int.MinValue * 10_000L - 9_999L, int.MinValue)]
    public async Task ExplicitConversionFromTimeSpan_WithLargeTimeSpan_KeepsEveryMillisecond(long ticks, int expectedTotalMilliseconds)
    {
        // Act
        var result = (TimeInt32)TimeSpan.FromTicks(ticks);

        // Assert
        await Assert.That(result.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
    }

    [Test]
    [Arguments(int.MaxValue + 1L)]
    [Arguments(int.MinValue - 1L)]
    [Arguments(long.MaxValue / 10_000)]
    [Arguments(long.MinValue / 10_000)]
    public async Task ExplicitConversionFromTimeSpan_WithTimeSpanOutOfRange_ThrowsOverflowException(long totalMilliseconds)
    {
        // Arrange
        var timeSpan = TimeSpan.FromTicks(totalMilliseconds * 10_000);

        // Act
        var act = () => (TimeInt32)timeSpan;

        // Assert
        await Assert.That(act).ThrowsExactly<OverflowException>();
    }

    [Test]
    public async Task ExplicitConversionFromTimeSingle_WithTimeSingle_ReturnsEquivalentTime()
    {
        // Arrange
        var timeSingle = new TimeSingle(1.5f);

        // Act
        var result = (TimeInt32)timeSingle;

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(1_500));
    }

    [Test]
    [Arguments(1_000, 1_500, -1)]
    [Arguments(1_500, 1_000, 1)]
    [Arguments(1_000, 1_000, 0)]
    public async Task CompareTo_WithTimeInt32_ReturnsRelativeOrder(int totalMilliseconds, int otherTotalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);
        var other = new TimeInt32(otherTotalMilliseconds);

        // Act
        var result = time.CompareTo(other);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(1_000, 1.5f, -1)]
    [Arguments(1_500, 1f, 1)]
    [Arguments(1_000, 1f, 0)]
    public async Task CompareTo_WithTimeSingle_ReturnsRelativeOrder(int totalMilliseconds, float otherTotalSeconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);
        var other = new TimeSingle(otherTotalSeconds);

        // Act
        var result = time.CompareTo(other);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(1_000, 1_500, -1)]
    [Arguments(1_500, 1_000, 1)]
    [Arguments(1_000, 1_000, 0)]
    public async Task CompareTo_WithTimeAsObject_ReturnsRelativeOrder(int totalMilliseconds, int otherTotalMilliseconds, int expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);
        object other = new TimeInt32(otherTotalMilliseconds);

        // Act
        var result = time.CompareTo(other);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task CompareTo_WithNullITime_ReturnsPositive()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = time.CompareTo((ITime?)null);

        // Assert
        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithNullObject_ReturnsPositive()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = time.CompareTo((object?)null);

        // Assert
        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task CompareTo_WithNonTimeObject_ThrowsArgumentException()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var act = () => time.CompareTo(new object());

        // Assert
        await Assert.That(act).ThrowsExactly<ArgumentException>();
    }

    [Test]
    public async Task Equals_WithNullITime_ReturnsFalse()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var result = time.Equals((ITime?)null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WithDifferentTimeSingle_ReturnsFalse()
    {
        // Arrange
        var time = new TimeInt32(1_000);
        var other = new TimeSingle(2f);

        // Act
        var result = time.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("1.876", 1_876)]
    [Arguments("00:00.00", 0)]
    [Arguments("0:00.69", 690)]
    [Arguments("0:01.00", 1_000)]
    [Arguments("0:01.000", 1_000)]
    [Arguments("0:43.256", 43_256)]
    [Arguments("1:00.000", 60_000)]
    [Arguments("1:50.011", 110_011)]
    public async Task Parse_WithTimeString_ReturnsTime(string input, int expectedTotalMilliseconds)
    {
        // Act
        var time = TimeInt32.Parse(input);

        // Assert
        await Assert.That(time).IsEqualTo(new TimeInt32(expectedTotalMilliseconds));
    }

    [Test]
    [Arguments("-1.876", -1_876)]
    [Arguments("-0:00.69", -690)]
    [Arguments("-0:01.00", -1_000)]
    [Arguments("-0:43.256", -43_256)]
    [Arguments("-1:00.000", -60_000)]
    [Arguments("-1:50.011", -110_011)]
    [Arguments("-2:15.789", -135_789)]
    [Arguments("-1:23:45.678", -5_025_678)]
    public async Task Parse_WithNegativeTimeString_ReturnsNegativeTime(string input, int expectedTotalMilliseconds)
    {
        // Act
        var time = TimeInt32.Parse(input);

        // Assert
        await Assert.That(time).IsEqualTo(new TimeInt32(expectedTotalMilliseconds));
    }

    [Test]
    public async Task Parse_WithInvalidString_ThrowsFormatException()
    {
        // Act
        var act = () => TimeInt32.Parse("invalid");

        // Assert
        await Assert.That(act).ThrowsExactly<FormatException>();
    }

    [Test]
    public async Task Parse_WithValidSpan_ReturnsTime()
    {
        // Act
        var time = TimeInt32.Parse("0:01.000".AsSpan());

        // Assert
        await Assert.That(time).IsEqualTo(new TimeInt32(1_000));
    }

    [Test]
    public async Task Parse_WithInvalidSpan_ThrowsFormatException()
    {
        // Act
        var act = () => TimeInt32.Parse("invalid".AsSpan());

        // Assert
        await Assert.That(act).ThrowsExactly<FormatException>();
    }

    [Test]
    [Arguments("0:01.000", 1_000)]
    [Arguments("-1:23.456", -83_456)]
    public async Task TryParse_WithValidString_ReturnsTrueAndTime(string input, int expectedTotalMilliseconds)
    {
        // Act
        var success = TimeInt32.TryParse(input, out var time);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(time).IsEqualTo(new TimeInt32(expectedTotalMilliseconds));
    }

    [Test]
    [Arguments("invalid")]
    [Arguments(null)]
    public async Task TryParse_WithInvalidString_ReturnsFalseAndZero(string? input)
    {
        // Act
        var success = TimeInt32.TryParse(input!, out var time);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(time).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task TryParse_WithValidSpan_ReturnsTrueAndTime()
    {
        // Act
        var success = TimeInt32.TryParse("0:01.000".AsSpan(), out var time);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(time).IsEqualTo(new TimeInt32(1_000));
    }

    [Test]
    [Arguments("invalid")]
    [Arguments("")]
    public async Task TryParse_WithInvalidSpan_ReturnsFalseAndZero(string input)
    {
        // Act
        var success = TimeInt32.TryParse(input.AsSpan(), out var time);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(time).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    [Arguments("4:39:37.999", 16_777_999)]
    [Arguments("5:00:00.001", 18_000_001)]
    [Arguments("23:59:59.999", 86_399_999)]
    [Arguments("-23:59:59.999", -86_399_999)]
    [Arguments("24:20:31:23.647", int.MaxValue)]
    [Arguments("-24:20:31:23.648", int.MinValue)]
    public async Task Parse_WithLargeTimeString_KeepsEveryMillisecond(string input, int expectedTotalMilliseconds)
    {
        // Act
        var fromString = TimeInt32.Parse(input);
        var fromSpan = TimeInt32.Parse(input.AsSpan());

        // Assert
        await Assert.That(fromString.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
        await Assert.That(fromSpan.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
        await Assert.That(fromString.ToString()).IsEqualTo(input);
    }

    [Test]
    [Arguments("24:20:31:23.648")]
    [Arguments("-24:20:31:23.649")]
    [Arguments("30:00:00:00.000")]
    public async Task TryParse_WithTimeOutOfRange_ReturnsFalseAndZero(string input)
    {
        // Act
        var successFromString = TimeInt32.TryParse(input, out var fromString);
        var successFromSpan = TimeInt32.TryParse(input.AsSpan(), out var fromSpan);

        // Assert
        await Assert.That(successFromString).IsFalse();
        await Assert.That(fromString).IsEqualTo(TimeInt32.Zero);
        await Assert.That(successFromSpan).IsFalse();
        await Assert.That(fromSpan).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task Parse_WithTimeOutOfRange_ThrowsFormatException()
    {
        // Act
        var act = () => TimeInt32.Parse("30:00:00:00.000");

        // Assert
        await Assert.That(act).ThrowsExactly<FormatException>();
    }

    [Test]
    public async Task ToString_WithoutArguments_ReturnsTimeWithMilliseconds()
    {
        // Arrange
        var time = new TimeInt32(234_567_890);

        // Act
        var result = time.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("2:17:09:27.890");
    }

    [Test]
    [Arguments(16_777_999, "4:39:37.999")]
    [Arguments(36_000_999, "10:00:00.999")]
    [Arguments(86_399_999, "23:59:59.999")]
    [Arguments(-86_399_999, "-23:59:59.999")]
    [Arguments(int.MaxValue, "24:20:31:23.647")]
    [Arguments(int.MinValue, "-24:20:31:23.648")]
    public async Task ToString_WithLargeTime_DoesNotRoundUp(int totalMilliseconds, string expected)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var result = time.ToString();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task ToString_WithNegativeTime_PrefixesMinusSign()
    {
        // Arrange
        var time = new TimeInt32(-83_456);

        // Act
        var result = time.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("-1:23.456");
    }

    [Test]
    [Arguments(25, 0, 0, 0, 0)]
    [Arguments(-25, 0, 0, 0, 0)]
    [Arguments(24, 20, 31, 23, 648)]
    [Arguments(0, 0, 0, int.MaxValue, 0)]
    public async Task Constructor_WithTimeOutOfRange_ThrowsOverflowException(int days, int hours, int minutes, int seconds, int milliseconds)
    {
        // Act
        var act = () => new TimeInt32(days, hours, minutes, seconds, milliseconds);

        // Assert
        await Assert.That(act).ThrowsExactly<OverflowException>();
    }

    [Test]
    public async Task Constructor_WithMaxValueComponents_ReturnsMaxValue()
    {
        // Act
        var time = new TimeInt32(24, 20, 31, 23, 647);

        // Assert
        await Assert.That(time).IsEqualTo(TimeInt32.MaxValue);
    }

    [Test]
    public async Task Duration_WithMinValue_ThrowsOverflowException()
    {
        // Act
        var act = () => TimeInt32.MinValue.Duration();

        // Assert
        await Assert.That(act).ThrowsExactly<OverflowException>();
    }

    [Test]
    public async Task Negate_WithMinValue_ThrowsOverflowException()
    {
        // Act
        var negate = () => TimeInt32.MinValue.Negate();
        var unaryMinus = () => -TimeInt32.MinValue;

        // Assert
        await Assert.That(negate).ThrowsExactly<OverflowException>();
        await Assert.That(unaryMinus).ThrowsExactly<OverflowException>();
    }

    [Test]
    public async Task ArithmeticOperators_WhenResultIsOutOfRange_ThrowOverflowException()
    {
        // Act
        var add = () => TimeInt32.MaxValue + new TimeInt32(1);
        var addTimeSpan = () => TimeInt32.MaxValue + TimeSpan.FromMilliseconds(1);
        var addMethod = () => TimeInt32.MaxValue.Add((ITime)new TimeInt32(1));
        var subtract = () => TimeInt32.MinValue - new TimeInt32(1);
        var subtractTimeSpan = () => TimeInt32.MinValue - TimeSpan.FromMilliseconds(1);
        var multiply = () => TimeInt32.MaxValue * 2;

        // Assert
        await Assert.That(add).ThrowsExactly<OverflowException>();
        await Assert.That(addTimeSpan).ThrowsExactly<OverflowException>();
        await Assert.That(addMethod).ThrowsExactly<OverflowException>();
        await Assert.That(subtract).ThrowsExactly<OverflowException>();
        await Assert.That(subtractTimeSpan).ThrowsExactly<OverflowException>();
        await Assert.That(multiply).ThrowsExactly<OverflowException>();
    }

    [Test]
    [Arguments(251, 0.251f)]
    [Arguments(9, 0.009f)]
    [Arguments(-83_456, -83.456f)]
    public async Task EqualityWithTimeSingle_IsConsistentAcrossOperatorsAndMethods(int totalMilliseconds, float totalSeconds)
    {
        // Arrange
        var timeInt32 = new TimeInt32(totalMilliseconds);
        var timeSingle = new TimeSingle(totalSeconds);

        // Assert
        await Assert.That(timeInt32 == timeSingle).IsTrue();
        await Assert.That(timeInt32 != timeSingle).IsFalse();
        await Assert.That(timeInt32.Equals(timeSingle)).IsTrue();
        await Assert.That(timeInt32.Equals((ITime)timeSingle)).IsTrue();
        await Assert.That(timeInt32.CompareTo(timeSingle)).IsEqualTo(0);
        await Assert.That(timeInt32.CompareTo((ITime)timeSingle)).IsEqualTo(0);
        await Assert.That(timeSingle == timeInt32).IsTrue();
        await Assert.That(timeSingle.Equals(timeInt32)).IsTrue();
    }

    [Test]
    public async Task EqualsAndCompareTo_WithLargeTimeInt32AsITime_UseExactMilliseconds()
    {
        // Arrange - one millisecond apart, but the same float number of seconds
        var time = new TimeInt32(16_777_217);
        ITime other = new TimeInt32(16_777_216);

        // Assert
        await Assert.That(time.TotalSeconds).IsEqualTo(other.TotalSeconds);
        await Assert.That(time.Equals(other)).IsFalse();
        await Assert.That(time.CompareTo(other)).IsEqualTo(1);
    }

    [Test]
    public async Task Add_WithTimeSingleAsITime_RoundsToNearestMillisecond()
    {
        // Act
        var result = new TimeInt32(1_000).Add((ITime)new TimeSingle(0.251f));

        // Assert
        await Assert.That(result.TotalMilliseconds).IsEqualTo(1_251);
    }

    [Test]
    [Arguments("5", 500)]
    [Arguments("05", 50)]
    [Arguments("500", 500)]
    [Arguments("-5", -500)]
    public async Task Parse_WithBareNumber_ReadsFractionOfSecond(string input, int expectedTotalMilliseconds)
    {
        // Act
        var time = TimeInt32.Parse(input);

        // Assert
        await Assert.That(time.TotalMilliseconds).IsEqualTo(expectedTotalMilliseconds);
    }
}
