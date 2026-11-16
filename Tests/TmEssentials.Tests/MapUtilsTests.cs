namespace TmEssentials.Tests;

public class MapUtilsTests
{
    [Test]
    public async Task GenerateMapUid_WithSeed_ReturnsDeterministicUid()
    {
        // Arrange
        var seed = 1112;

        // Act
        var uid = MapUtils.GenerateMapUid(seed);

        // Assert
        await Assert.That(uid).IsEqualTo("E51Ufj8ixjwC3ZluiqMSyEVuFJZ");
    }

    [Test]
    public async Task GenerateMapUid_WithNullRandom_ThrowsArgumentNullException()
    {
        // Act
        var act = () => MapUtils.GenerateMapUid(random: null!);

        // Assert
        await Assert.That(act).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task GenerateMapUid_WithoutArguments_ReturnsNonEmptyUid()
    {
        // Act
        var uid = MapUtils.GenerateMapUid();

        // Assert
        await Assert.That(uid).IsNotEmpty();
    }
}
