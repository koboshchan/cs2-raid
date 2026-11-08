namespace Cs2Raid;
// Separate, explicit link-only transport. The frozen MoneyOffHandler is unchanged.
internal sealed class LinkOnlyHandler : DelegatingHandler
{
    internal LinkOnlyHandler():this(new HttpClientHandler {AllowAutoRedirect=false}){}
    internal LinkOnlyHandler(HttpMessageHandler inner):base(inner){}
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
    {
        if(MoneyOffHandler.PrivateRehearsalHardOff)throw new InvalidOperationException("PRIVATE REHEARSAL: all HTTP hard OFF.");
        var uri=request.RequestUri;
        if(request.Method!=HttpMethod.Post||uri==null||uri.Scheme!="http"||uri.Host!="backend"||uri.Port!=3000||
            uri.UserInfo!=""||uri.Query!=""||uri.Fragment!=""||
            uri.AbsolutePath is not ("/link" or "/link/poll" or "/link/status"))
            throw new InvalidOperationException("Only local account linking is allowed.");
        return base.SendAsync(request,cancellationToken);
    }
}
