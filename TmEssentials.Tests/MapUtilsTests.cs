using System;

namespace TmEssentials.Tests;

public class MapUtilsTests
{
    [Test]
    public async Task GenerateMapUid_ReturnsExpected()
    {
        var actual = MapUtils.GenerateMapUid(1112);

        await Assert.That(actual).IsEqualTo("E51Ufj8ixjwC3ZluiqMSyEVuFJZ");
    }

    [Test]
    public async Task GenerateMapUid_RandomIsNull_ThrowsArgumentNullException()
    {
        await Assert.That(() => MapUtils.GenerateMapUid(random: null)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task GenerateMapUid_ReturnsNonEmptyString()
    {
        var actual = MapUtils.GenerateMapUid();

        await Assert.That(actual).IsNotEmpty();
    }
}
