namespace DeltaOps;

// CS2 interpretation of 10010301: scoped state controls a cooperative movement
// lease. DF buff303054 magnitude and non-scope ADS weapons remain unresolved.
// Failure keeps the old lease until successful cleanup; never acquire over it.
public sealed class NativeScopedMobility(Func<bool> admitted,Func<bool> scoped,
    Func<IDisposable?> acquire):IDisposable
{
    private IDisposable? current;
    private bool closed;
    public void Tick(){
        if(closed)return;
        if(!admitted()||!scoped()){Release();return;}
        current??=acquire();
    }
    private void Release(){current?.Dispose();current=null;}
    public void Dispose(){if(closed)return;Release();closed=true;}
}
