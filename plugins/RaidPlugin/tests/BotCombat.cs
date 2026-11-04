using Cs2Raid;
static class BotCombatTests {
 public static void Run(){
  int checks=0;
  for(int mask=0;mask<128;mask++){
   bool B(int n)=>(mask&(1<<n))!=0;
   var expected=B(0)&&B(1)&&B(2)&&(B(3)||B(4))&&B(5)&&B(6);
   if(TestFlow.LobbyBotCombat(B(0),B(1),B(2),B(3),B(4),B(5),B(6))!=expected)throw new Exception("bot Lobby policy "+mask);
   checks++;
  }

  for(int mask=0;mask<32;mask++){
   bool B(int n)=>(mask&(1<<n))!=0;
   if(TestFlow.LobbyRecovery(B(0),B(1),B(2),B(3),B(4))!=(B(0)&&B(1)&&B(2)&&B(3)&&!B(4)))throw new Exception("recovery fence");
   checks++;
  }
  var resources=new Dictionary<int,int>{{1,1},{2,2},{3,3}};int failures=0;
  TestModeState.RestoreEntries(resources,(key,value)=>{if(key==2)throw new Exception("injected");resources.Remove(key);},ex=>failures++);
  if(resources.Count!=1||!resources.ContainsKey(2)||failures!=1)throw new Exception("per-entry cleanup recovery");
  checks++;

  var dir=Path.Combine(Path.GetTempPath(),"raid-isolation-"+Guid.NewGuid());Directory.CreateDirectory(dir);
  try{
   TestModeState.RequireIsolatedFixture(dir,false);checks++;
   bool Denied(bool declared){try{TestModeState.RequireIsolatedFixture(dir,declared);return false;}catch(InvalidOperationException){return true;}}
   if(!Denied(true))throw new Exception("declared shared world accepted");checks++;
   var file=Path.Combine(dir,"settlement-recovery.jsonl");File.WriteAllText(file,"historical-corrupt");
   if(!Denied(false)||File.ReadAllText(file)!="historical-corrupt")throw new Exception("historical journal mutated or accepted");checks++;
   File.WriteAllText(file,"");if(!Denied(false))throw new Exception("empty journal accepted");checks++;
   File.Delete(file);Directory.CreateDirectory(file);if(!Denied(false))throw new Exception("recovery directory accepted");checks++;
  }finally{Directory.Delete(dir,true);}
  Console.WriteLine($"{checks} bot combat Lobby policy checks passed");
 }
}
