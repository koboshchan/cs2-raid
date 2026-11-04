namespace Cs2Debug;
// Each optional disable and essential removal has its own exception boundary.
public static class WorldCleanup
{
    public static bool Attempt(Func<bool> valid,Action disable,Action remove,Action fault){
        try{if(!valid())return true;}catch{fault();return false;}
        try{disable();}catch{fault();}
        try{remove();return true;}catch{fault();return false;}
    }
    public static void FinalDrain(Action drain,Func<int> pending,Action<int> residual){
        // Synchronous only: disposal removes tick listeners. No deferred native callbacks.
        for(var pass=0;pass<3&&pending()>0;pass++)drain();
        if(pending()>0)residual(pending());
    }
    public static void Guard(Action evaluate,Action cancel){try{evaluate();}catch{cancel();}}
}
