using System;
using Xunit;

namespace TmEssentials.Tests;

public class AccountUtilsTests
{
    [Fact]
    public void ToLogin_ReturnsExpected()
    {
        var guid = Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09");

        var login = AccountUtils.ToLogin(guid);

        Assert.Equal(expected: "v89i_w-eQKq5JBG5xwuKCQ", actual: login);
    }

    [Fact]
    public void ToAccountId_ReturnsExpected()
    {
        var login = "v89i_w-eQKq5JBG5xwuKCQ";

        var guid = AccountUtils.ToAccountId(login);

        Assert.Equal(expected: Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09"), actual: guid);
    }

    [Fact]
    public void ToAccountId_ThrowsException_WhenInvalidLogin()
    {
        var login = "řáčíšřýěščářýéí";

        Assert.Throws<FormatException>(() => AccountUtils.ToAccountId(login));
    }
}
