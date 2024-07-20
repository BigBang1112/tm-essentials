using System;
using Xunit;

namespace TmEssentials.Tests;

public class MapUtilsTests
{
    [Fact]
    public void GenerateMapUid_ReturnsExpected()
    {
        var actual = MapUtils.GenerateMapUid(1112);

        Assert.Equal(expected: "E51Ufj8ixjwC3ZluiqMSyEVuFJZ", actual);
    }

    [Fact]
    public void GenerateMapUid_RandomIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => MapUtils.GenerateMapUid(random: null));
    }

    [Fact]
    public void GenerateMapUid_ReturnsNonEmptyString()
    {
        var actual = MapUtils.GenerateMapUid();

        Assert.NotEmpty(actual);
    }
}
