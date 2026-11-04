namespace Cs2Raid;
// The final transport gate covers every current/future bridge route, not only cash-in.
// Dedicated server lifetime policy: fixture OFF never re-enables external money/link.
internal sealed class MoneyOffHandler : DelegatingHandler
{
    public MoneyOffHandler():this(new HttpClientHandler()){}
    internal MoneyOffHandler(HttpMessageHandler inner):base(inner){}
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
    {
        if(Environment.GetEnvironmentVariable("RAID_TEST_SERVER")=="1")
            throw new InvalidOperationException("TEST SERVER: all bridge HTTP is hard OFF.");
        return base.SendAsync(request,cancellationToken);
    }
}
