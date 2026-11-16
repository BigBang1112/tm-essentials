using System;

namespace TmEssentials.Tests;

public class AccountUtilsTests
{
    [Test]
    public async Task ToLogin_ReturnsExpected()
    {
        var guid = Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09");

        var login = AccountUtils.ToLogin(guid);

        await Assert.That(login).IsEqualTo("v89i_w-eQKq5JBG5xwuKCQ");
    }

    [Test]
    public async Task ToAccountId_Null_ThrowsArgumentNullException()
    {
        await Assert.That(() => AccountUtils.ToAccountId(null)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ToAccountId_ReturnsExpected()
    {
        var login = "v89i_w-eQKq5JBG5xwuKCQ";

        var guid = AccountUtils.ToAccountId(login);

        await Assert.That(guid).IsEqualTo(Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09"));
    }

    [Test]
    public async Task ToAccountId_WhenInvalidLogin_ThrowsException()
    {
        var login = "řáčíšřýěščářýéí";

        await Assert.That(() => AccountUtils.ToAccountId(login)).ThrowsExactly<FormatException>();
    }
}
