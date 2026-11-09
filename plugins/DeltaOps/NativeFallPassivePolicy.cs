namespace DeltaOps;
public static class NativeFallPassivePolicy
{
 public static float Apply(float damage,uint damageType,int selected,bool admitted,bool alive,
     bool quarantined,float multiplier){
  // Only a pure fall event, not a combined bullet/blast flag, can acquire this interpretation.
  if(selected!=4||!admitted||!alive||quarantined||damageType!=32||
     !float.IsFinite(damage)||damage<=0||!float.IsFinite(multiplier)||multiplier<0||multiplier>1)return damage;
  return damage*multiplier;
 }
}
