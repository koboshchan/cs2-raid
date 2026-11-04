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
  Console.WriteLine($"{checks} bot combat Lobby policy checks passed");
 }
}
