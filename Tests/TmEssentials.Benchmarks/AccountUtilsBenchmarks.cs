using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class AccountUtilsBenchmarks
{
    private static readonly Guid AccountId = Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09");

    // Contains both - and _, so that the base64url characters have to be mapped
    private const string Login = "v89i_w-eQKq5JBG5xwuKCQ";

    [Benchmark]
    public string ToLogin() => AccountUtils.ToLogin(AccountId);

    [Benchmark]
    public Guid ToAccountId() => AccountUtils.ToAccountId(Login);
}
