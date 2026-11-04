using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace Cs2Raid;

// No engine objects, upstream bodies, codes or credentials are retained in diagnostics.
internal sealed class PrivateLink : IDisposable
{
    internal sealed class Session(string steam, uint raw, Guid generation)
    {
        internal readonly string Steam=steam;
        internal readonly uint Raw=raw;
        internal readonly Guid Generation=generation;
        internal readonly CancellationTokenSource Stop=new();
        internal Task Work=Task.CompletedTask;
    }
    private readonly object sync=new();
    private readonly Dictionary<string,Session> sessions=[];
    // Retained across reconnects: cancellation must not allow overlapping HTTP for one Steam ID.
    private readonly Dictionary<string,SemaphoreSlim> lanes=[];
    private readonly HttpClient http;
    private readonly Action<Session,string> notify;
    private readonly Func<TimeSpan,CancellationToken,Task> delay;
    private readonly Func<DateTimeOffset> now;
    private bool disposed;
    internal PrivateLink(HttpClient http,Action<Session,string> notify,
        Func<TimeSpan,CancellationToken,Task>? delay=null,Func<DateTimeOffset>? now=null)
    { this.http=http;this.notify=notify;this.delay=delay??((t,c)=>Task.Delay(t,c));this.now=now??(()=>DateTimeOffset.UtcNow); }
    internal bool Current(Session s) { lock(sync)return !disposed&&!s.Stop.IsCancellationRequested&&sessions.GetValueOrDefault(s.Steam)==s; }
    internal bool Active { get {lock(sync)return sessions.Values.Any(s=>!s.Work.IsCompleted);} }
    internal Session? Start(string steam,uint raw,Guid generation,bool join)
    {
        lock(sync){
            if(disposed)return null;
            if(sessions.TryGetValue(steam,out var old)){
                if(old.Raw==raw&&old.Generation==generation&&!old.Work.IsCompleted)return old;
                old.Stop.Cancel();
            }
            foreach(var prior in sessions.Values.Where(s=>s.Raw==raw))prior.Stop.Cancel();
            var s=new Session(steam,raw,generation);sessions[steam]=s;
            if(!lanes.TryGetValue(steam,out var lane))lanes[steam]=lane=new(1,1);
            s.Work=Run(s,lane,join);return s;
        }
    }
    internal void Disconnect(uint raw) {lock(sync)foreach(var s in sessions.Values.Where(s=>s.Raw==raw))s.Stop.Cancel();}
    internal void Reset() {lock(sync){foreach(var s in sessions.Values)s.Stop.Cancel();sessions.Clear();}}
    public void Dispose(){lock(sync){disposed=true;Reset();}}
    private void Say(Session s,string text){if(Current(s))notify(s,text);}
    private async Task Run(Session s,SemaphoreSlim lane,bool join)
    {
        var ct=s.Stop.Token;var held=false;
        try{
            await lane.WaitAsync(ct);held=true;
            bool probe=true, create=false;
            double interval=5,backoff=5;
            var expires=now().AddMinutes(10);
            while(Current(s)){
                if(now()>=expires){Say(s,"SSG link expired. Use !link for a new private code.");return;}
                try{
                    var origin=Environment.GetEnvironmentVariable("RAID_BACKEND_URL");
                    var token=Environment.GetEnvironmentVariable("BRIDGE_TOKEN")??"";
                    if(origin!="http://backend:3000"||token.Length<32){
                        if(!join)Say(s,"SSG linking unavailable. Ask the server owner; no charge was made.");return;
                    }
                    using var req=new HttpRequestMessage(HttpMethod.Post,origin+(probe?"/link/status":create?"/link":"/link/poll"));
                    req.Headers.Authorization=new AuthenticationHeaderValue("Bearer",token);
                    req.Content=JsonContent.Create(new{steamId64=s.Steam});
                    using var timeout=CancellationTokenSource.CreateLinkedTokenSource(ct);
                    timeout.CancelAfter(TimeSpan.FromSeconds(15));
                    using var response=await http.SendAsync(req,timeout.Token);
                    if(!Current(s))return;
                    if(response.StatusCode==HttpStatusCode.TooManyRequests || (int)response.StatusCode>=500){
                        var retry=response.Headers.RetryAfter;
                        var wait=Math.Max(Math.Max(interval,backoff),Math.Max(retry?.Delta?.TotalSeconds??0,
                            retry?.Date is {} date?(date-now()).TotalSeconds:0));
                        backoff=Math.Min(300,backoff*2);
                        await Pause(wait);continue;
                    }
                    if(!response.IsSuccessStatusCode){
                        if(!probe)Say(s,"SSG link unavailable. Use !link or ask the server owner; no charge was made.");
                        return;
                    }
                    using var doc=JsonDocument.Parse(await response.Content.ReadAsStringAsync(timeout.Token));
                    var root=doc.RootElement;backoff=5;
                    if(probe){
                        if(root.GetProperty("linked").GetBoolean())return;
                        probe=false;create=true;continue;
                    }
                    if(create){
                        var words=root.GetProperty("user_code").GetString()??"";
                        var uri=root.GetProperty("verification_uri").GetString()??"";
                        if(words.Length>128||!Regex.IsMatch(words,@"^[a-z]+ [a-z]+ [a-z]+ [a-z]+$")||
                           uri.Length>512||uri.Any(c=>char.IsControl(c)||c==0x2028||c==0x2029)||
                           !Uri.TryCreate(uri,UriKind.Absolute,out var url)||url.Scheme!="https"||
                           !string.IsNullOrEmpty(url.UserInfo))throw new FormatException();
                        expires=root.GetProperty("expires_at").GetDateTimeOffset();
                        if(expires<=now()){Say(s,"SSG link expired. Use !link for a new private code.");return;}
                        interval=root.TryGetProperty("interval_seconds",out var value)?value.GetDouble():5;
                        if(!double.IsFinite(interval)||interval<5||interval>86400)throw new FormatException();
                        Say(s,$"Private SSG code: {words}. Open {uri} and confirm. Approval is checked automatically. Never share this code.");
                        create=false;probe=false;
                    }else{
                        var state=root.GetProperty("state").GetString();
                        if(state=="approved"){if(!probe)Say(s,"SSG account linked.");return;}
                        if(state=="expired"||state=="denied"){
                            if(!probe)Say(s,"SSG link ended. Use !link for a new private code.");return;
                        }
                        if(state!="authorization_pending")throw new FormatException();

                    }
                    await Pause(interval);
                }catch(OperationCanceledException) when(ct.IsCancellationRequested){return;}
                catch(HttpRequestException){await Retry();}
                catch(OperationCanceledException){await Retry();}
                catch{if(!probe)Say(s,"SSG linking unavailable. Use !link or ask the server owner.");return;}
            }
            async Task Pause(double seconds){
                var left=(expires-now()).TotalSeconds;
                if(left>0)await delay(TimeSpan.FromSeconds(Math.Min(seconds,left)),ct);
            }
            async Task Retry(){await Pause(Math.Max(interval,backoff));backoff=Math.Min(300,backoff*2);}
        }catch(OperationCanceledException){}
        catch{ /* Fail closed; never log exceptions containing link data. */ }
        finally{if(held)lane.Release();}
    }
}
