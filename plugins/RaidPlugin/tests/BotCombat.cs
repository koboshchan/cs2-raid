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
  Console.WriteLine($"{checks} bot combat Lobby policy checks passed");
 }
}
