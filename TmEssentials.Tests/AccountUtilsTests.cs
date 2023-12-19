using System;
using Xunit;

namespace TmEssentials.Tests;

public class AccountUtilsTests
{
    [Fact]
    public void ToLogin_Guid_ReturnsExpected()
    {
        var guid = Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09");

        var login = AccountUtils.ToLogin(guid);

        Assert.Equal(expected: "v89i_w-eQKq5JBG5xwuKCQ", actual: login);
    }
}
