using System.Net;
using System.Text;
using Cs2Raid;
internal static class PrivateLinkTests
{
    private sealed class Fake(Func<HttpRequestMessage,CancellationToken,Task<HttpResponseMessage>> send):HttpMessageHandler
    {
        internal int Calls,InFlight,Peak;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage r,CancellationToken c){
            Calls++;InFlight++;Peak=Math.Max(Peak,InFlight);try{return await send(r,c);}finally{InFlight--;}
        }
    }
    private static HttpResponseMessage Json(string text,HttpStatusCode status=HttpStatusCode.OK)=>new(status){Content=new StringContent(text,Encoding.UTF8,"application/json")};
    private static string SourcePath([System.Runtime.CompilerServices.CallerFilePath]string path="")=>path;
    internal static void Run()=>RunAsync().GetAwaiter().GetResult();
    private static async Task RunAsync()
    {
        int checks=0;
        void Check(bool ok){checks++;if(!ok)throw new Exception("Private link check "+checks);}
        Environment.SetEnvironmentVariable("RAID_BACKEND_URL","http://backend:3000");
        Environment.SetEnvironmentVariable("BRIDGE_TOKEN",new string('t',40));
        var generation=Guid.NewGuid();var now=DateTimeOffset.UtcNow;
        string Code(int? interval=5)=>System.Text.Json.JsonSerializer.Serialize(new{user_code="four test words here",verification_uri="https://example.test/link",expires_at=now.AddMinutes(2),interval_seconds=interval});
        foreach(var linked in new[]{true,false}){
            var messages=new List<string>();var waits=new List<double>();int polls=0;
            using var fake=new Fake((r,c)=>Task.FromResult(r.RequestUri!.AbsolutePath switch{
                "/link/status"=>Json(linked?"{\"linked\":true}":"{\"linked\":false}"),
                "/link"=>Json(Code(7)),
                _=>Json(++polls==1?"{\"state\":\"authorization_pending\"}":"{\"state\":\"approved\"}")
            }));
            using var client=new HttpClient(new LinkOnlyHandler(fake));
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m),(t,c)=>{waits.Add(t.TotalSeconds);now+=t;return Task.CompletedTask;},()=>now);
            var session=service.Start("76561198000000001",11,generation,true)!;await session.Work;
            Check(linked?fake.Calls==1:fake.Calls==4);Check(linked?messages.Count==0:messages.Count==2);
            Check(fake.Peak==1);if(!linked){Check(waits.SequenceEqual(new[]{7d,7d}));Check(messages.Last()=="SSG account linked.");}
        }
        // Missing interval defaults to 5; 429 Retry-After and network error backoff respected.
        {
            var waits=new List<double>();int poll=0;var messages=new List<string>();
            using var fake=new Fake((r,c)=>{
                if(r.RequestUri!.AbsolutePath=="/link/status")return Task.FromResult(Json("{\"linked\":false}"));
                if(r.RequestUri.AbsolutePath=="/link")return Task.FromResult(Json(Code().Replace(",\"interval_seconds\":5","")));
                poll++;
                if(poll==1){var response=Json("{}",HttpStatusCode.TooManyRequests);response.Headers.RetryAfter=new(TimeSpan.FromSeconds(19));return Task.FromResult(response);}
                if(poll==2)throw new HttpRequestException("secret must never be forwarded");
                return Task.FromResult(Json("{\"state\":\"expired\"}"));
            });
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m),(t,c)=>{waits.Add(t.TotalSeconds);now+=t;return Task.CompletedTask;},()=>now);
            await service.Start("76561198000000001",12,generation,false)!.Work;
            Check(waits.SequenceEqual(new[]{5d,19d,10d}));Check(messages.Count==2);
            Check(!messages.Any(m=>m.Contains("secret must")));Check(fake.Peak==1);
        }
        // A transport ignoring cancellation cannot overlap after same-Steam reconnect.
        {
            var entered=new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var release=new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var messages=new List<(PrivateLink.Session,string)>();
            using var fake=new Fake(async(r,c)=>{entered.TrySetResult();await release.Task;return Json("{\"linked\":true}");});
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>messages.Add((s,m)));
            var first=service.Start("76561198000000001",77,generation,true)!;await entered.Task;
            Check(ReferenceEquals(first,service.Start(first.Steam,77,generation,true)));
            service.Disconnect(77);
            var second=service.Start(first.Steam,78,generation,true)!;
            Check(!service.Current(first)&&service.Current(second));Check(fake.Calls==1);
            release.SetResult();await Task.WhenAll(first.Work,second.Work);
            Check(fake.Peak==1&&fake.Calls==2);Check(messages.Count==0);
            var reused=service.Start("76561198000000002",77,Guid.NewGuid(),true)!;await reused.Work;
            Check(!service.Current(first));service.Reset();Check(!service.Current(reused));
            service.Dispose();Check(service.Start(first.Steam,77,generation,true)==null);
        }
        // Expiry stops without a poll, cancellation on unload and strict financial isolation.
        {
            int calls=0;var messages=new List<string>();
            using var fake=new Fake((r,c)=>{calls++;return Task.FromResult(r.RequestUri!.AbsolutePath=="/link/status"?Json("{\"linked\":false}"):Json(Code()));});
            using var client=new HttpClient(new LinkOnlyHandler(fake));
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m),(t,c)=>{now+=TimeSpan.FromMinutes(3);return Task.CompletedTask;},()=>now);
            await service.Start("76561198000000001",90,generation,false)!.Work;
            Check(calls==2&&messages.Count==2&&messages.Last().Contains("expired"));
            foreach(var path in new[]{"/cashin","/extract","/link/anything","/link/status?x=y"}){
                try{await client.PostAsync("http://backend:3000"+path,new StringContent("{}"));throw new Exception("Financial route allowed");}
                catch(InvalidOperationException){checks++;}
            }
        }
        {
            var sleeping=new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            using var fake=new Fake((r,c)=>Task.FromResult(r.RequestUri!.AbsolutePath=="/link/status"?Json("{\"linked\":false}"):Json(Code())));
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>{},async(t,c)=>{sleeping.SetResult();await Task.Delay(Timeout.Infinite,c);},()=>now);
            var s=service.Start("76561198000000001",92,generation,true)!;await sleeping.Task;service.Dispose();await s.Work;
            Check(!service.Current(s));Check(fake.Calls==2);
        }

        // Unknown status must never be interpreted as unlinked; malformed data stays private.
        foreach(var status in new[]{HttpStatusCode.BadRequest,HttpStatusCode.Unauthorized}){
            var messages=new List<string>();
            using var fake=new Fake((r,c)=>Task.FromResult(Json("upstream-secret",status)));
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m));
            await service.Start("76561198000000001",31,generation,true)!.Work;
            Check(fake.Calls==1&&messages.Count==0);
        }
        {
            int statusCalls=0;var waits=new List<double>();var messages=new List<string>();
            using var fake=new Fake((r,c)=>{
                statusCalls++;
                if(statusCalls==1)throw new TaskCanceledException("upstream secret timeout");
                return Task.FromResult(Json("{\"linked\":true}"));
            });
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m),(t,c)=>{waits.Add(t.TotalSeconds);return Task.CompletedTask;});
            await service.Start("76561198000000001",32,generation,true)!.Work;
            Check(fake.Calls==2&&waits.SequenceEqual(new[]{5d})&&messages.Count==0);
        }
        {
            var pending=new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            using var fake=new Fake((r,c)=>Task.FromResult(Json("{\"linked\":false}")));
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>pending.TrySetResult());
            var invalid=service.Start("76561198000000001",33,generation,true)!;
            await invalid.Work;
            Check(fake.Calls==2&&pending.Task.IsCompleted); // Missing code is rejected, no upstream text.
            var old=service.Start("76561198000000001",34,generation,true)!;await old.Work;
            var replacement=service.Start("76561198000000002",34,generation,true)!;await replacement.Work;
            Check(!service.Current(old)&&service.Current(replacement));
        }

        var pluginSource=File.ReadAllText(Path.Combine(Path.GetDirectoryName(SourcePath())!,"../RaidPlugin.cs"));
        Check(pluginSource.Contains("RegisterListener<Listeners.OnMapEnd>(()=>privateLinks?.EndMap(()=>mapGeneration=Guid.NewGuid()))"));
        Check(pluginSource.Contains("privateLinks?.Schedule(session,action=>Server.NextFrame(action)"));
        // Review regressions: strict words/URL, explicit failures, map-end gap, native exceptions.
        foreach(var bad in new[]{"four test words here\n","four test words here\r","four test\x01 words here","four test words here\u2028","four test words here\u200b","four  test words here"}){
            var messages=new List<string>();
            using var fake=new Fake((r,c)=>Task.FromResult(Json(r.RequestUri!.AbsolutePath=="/link/status"?"{\"linked\":false}":Code().Replace("four test words here",System.Text.Json.JsonEncodedText.Encode(bad).ToString()))));
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m));
            await service.Start("76561198000000001",101,generation,false)!.Work;
            Check(messages.Count==1&&!messages[0].Contains("Private SSG code")&&fake.Calls==2);
        }
        foreach(var badUrl in new[]{"https://example.test/link%0a","https://example.test/link\u202e","https://example.test/link%01","https://example.test/link%7f"}){
            var messages=new List<string>();
            using var fake=new Fake((r,c)=>Task.FromResult(Json(r.RequestUri!.AbsolutePath=="/link/status"?"{\"linked\":false}":Code().Replace("https://example.test/link",System.Text.Json.JsonEncodedText.Encode(badUrl).ToString()))));
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m));
            await service.Start("76561198000000001",101,generation,false)!.Work;
            Check(messages.Count==1&&!messages[0].Contains("Private SSG code")&&fake.Calls==2);
        }
        foreach(var malformed in new[]{false,true}){
            var messages=new List<string>();
            using var fake=new Fake((r,c)=>Task.FromResult(malformed?Json("{\"bad\":true}"):Json("{}",HttpStatusCode.Unauthorized)));
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>messages.Add(m));
            await service.Start("76561198000000001",102,generation,false)!.Work;
            Check(messages.Count==1&&messages[0].Contains("unavailable"));
        }
        {
            var entered=new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var finish=new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var notifications=0;var delivered=0;Action? queued=null;bool advanced=false;
            using var fake=new Fake(async(r,c)=>{entered.SetResult();await finish.Task;return Json("{\"linked\":false}");});
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>notifications++);
            var session=service.Start("76561198000000001",103,generation,true)!;await entered.Task;
            service.Schedule(session,a=>queued=a,()=>delivered++);
            service.EndMap(()=>advanced=true);finish.SetResult();await session.Work;queued!();
            Check(advanced&&fake.Calls==1&&notifications==0&&delivered==0&&!service.Current(session));
        }
        {
            using var fake=new Fake((r,c)=>Task.FromResult(Json("{\"linked\":true}")));
            using var client=new HttpClient(fake);
            using var service=new PrivateLink(client,(s,m)=>{});
            var session=service.Start("76561198000000001",104,generation,true)!;await session.Work;
            foreach(var stage in new[]{"reacquire","property","chat"}){
                Action? queued=null;int after=0;
                service.Schedule(session,a=>queued=a,()=>{throw new Exception(stage+" secret");});
                queued!();Check(after==0); // Exceptions from native provider/property/chat cannot escape.
            }
            service.Schedule(session,a=>throw new Exception("scheduler secret"),()=>throw new Exception());
            PrivateLink.NativeSafe(()=>throw new Exception("disconnect provider secret"));Check(true);
            Action? stale=null;int delivered=0;
            service.Schedule(session,a=>stale=a,()=>delivered++);
            service.Disconnect(104);stale!();Check(delivered==0);
        }
        Environment.SetEnvironmentVariable("RAID_BACKEND_URL",null);
        Environment.SetEnvironmentVariable("BRIDGE_TOKEN",null);
        Console.WriteLine(checks+" private link checks passed");
    }
}
