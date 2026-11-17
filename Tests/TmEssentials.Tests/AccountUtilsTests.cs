namespace TmEssentials.Tests;

public class AccountUtilsTests
{
    private static readonly Guid AccountId = Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09");
    private const string Login = "v89i_w-eQKq5JBG5xwuKCQ";

    [Test]
    public async Task ToLogin_WithAccountId_ReturnsLogin()
    {
        // Act
        var login = AccountUtils.ToLogin(AccountId);

        // Assert
        await Assert.That(login).IsEqualTo(Login);
    }

    [Test]
    [Arguments("00000000-0000-0000-0000-000000000000", "AAAAAAAAAAAAAAAAAAAAAA")]
    [Arguments("ffffffff-ffff-ffff-ffff-ffffffffffff", "_____________________w")]
    [Arguments("fbefbefb-efbe-fbef-befb-efbefbefbefb", "---------------------w")]
    public async Task ToLogin_WithAccountId_ReturnsBase64UrlWithoutPadding(string accountId, string expected)
    {
        // Act
        var login = AccountUtils.ToLogin(Guid.Parse(accountId));

        // Assert
        await Assert.That(login).IsEqualTo(expected);
    }

    [Test]
    public async Task ToAccountId_WithLogin_ReturnsAccountId()
    {
        // Act
        var accountId = AccountUtils.ToAccountId(Login);

        // Assert
        await Assert.That(accountId).IsEqualTo(AccountId);
    }

    [Test]
    public async Task ToAccountId_WithNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => AccountUtils.ToAccountId(null!);

        // Assert
        await Assert.That(act).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ToAccountId_WithNonBase64Login_ThrowsFormatException()
    {
        // Arrange
        var login = "řáčíšřýěščářýéí";

        // Act
        var act = () => AccountUtils.ToAccountId(login);

        // Assert
        await Assert.That(act).ThrowsExactly<FormatException>();
    }

    [Test]
    [Arguments("")]
    [Arguments("AAAA")]
    [Arguments("AAAAAAAA")]
    [Arguments("v89i_w-eQKq5JBG5xwuK")]
    [Arguments("v89i_w-eQKq5JBG5xwuKCQAA")]
    [Arguments("v89i_w-eQKq5JBG5xwuKCQAAAAAA")]
    [Arguments("v89i_w-eQKq5JBG5xwuKCQ_")]
    public async Task ToAccountId_WithLoginOfWrongLength_ThrowsFormatException(string login)
    {
        // Act
        var act = () => AccountUtils.ToAccountId(login);

        // Assert
        await Assert.That(act).ThrowsExactly<FormatException>();
    }

    [Test]
    public async Task ToAccountId_WithEmptyAccountLogin_ReturnsEmptyGuid()
    {
        // Act
        var accountId = AccountUtils.ToAccountId("AAAAAAAAAAAAAAAAAAAAAA");

        // Assert
        await Assert.That(accountId).IsEqualTo(Guid.Empty);
    }
}
