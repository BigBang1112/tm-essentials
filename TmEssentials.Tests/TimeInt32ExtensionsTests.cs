using Xunit;

namespace TmEssentials.Tests;

public class TimeInt32ExtensionsTests
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void ToTmString_CallsToString(bool useHundredths, bool useApostrophe)
    {
        var expected = new TimeInt32(234567890).ToString(useHundredths, useApostrophe);
        
        var actual = new TimeInt32(234567890).ToTmString(useHundredths, useApostrophe);

        Assert.Equal(expected, actual);
    }
}
