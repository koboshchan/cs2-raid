import gzip,json,pathlib
root=pathlib.Path(__file__).resolve().parents[2]
x=json.load(gzip.open(root/'data/exact-abilities/r2/five-operators-exact-vyron-sourceaudit-r2.json.gz'))
rows=[]; lines=['using System.Collections.Frozen;','namespace DeltaOps;','public sealed record InterpretedSkill(int Operator, int Id, string Name, int CdId, int Initial, int Capacity, double RechargeSeconds, bool Passive);','public static class InterpretedR2 {','public static readonly FrozenDictionary<(int Operator,int Skill),InterpretedSkill> Skills = new InterpretedSkill[] {']
doc=['# Authorized r2 interpretation, offline only','', 'Raw r2 is unchanged. Raid-mode slot rows are chosen, not MP/BombDefusal. This layer is interpreted, not a claim of DF-equivalent effects. Zero Max with weapon-count ammo is modeled as one ability token; no free unlimited casting. Init remains literal (0 starts charging). Seconds are inferred from Time and TimeRecoverSec field families. Non-CD skills receive a one-second anti-repeat guard, not an archive cooldown. Recharge is sequential, one token per Time; score/perk recovery is not implemented.','', '| Source skill / field | Raw | Chosen representation | Evidence / confidence / assumptions |','|---|---|---|---|']
for n,o in enumerate(x['operators'],1):
 passive={o['allExpertFields']['SOLPassiveSkillID'],o['allGroupFields']['PassiveSkillID'],o['allExpertFields'].get('SOLCommonPassiveSkillID',0)}
 for s in o['skills']:
  f=s['allSerializedFields'];id=int(s['row']);name=f['SkillDisplayName'].get('SourceString') or str(id)
  charge=[c for m in s['cooldownMatches'] for c in m['value']['SlotChargingData'] if any(z.endswith('_Raid') for z in c['LimitPlayMode'])]
  if len(charge)>1: assert all(c==charge[0] for c in charge)
  c=charge[0] if charge else {};init=c.get('Init',1);mx=c.get('Max',1);tm=c.get('Time',max(1,f['UseCD']));cap=max(1,mx)
  lines.append('new(%d,%d,%s,%d,%d,%d,%s,%s),'%(n,id,json.dumps(name,ensure_ascii=False),f['CDID'],init,cap,repr(float(tm)),str(id in passive).lower()))
  for key,raw,chosen,ev in [('CDID',f['CDID'],f['CDID'],'integer join, high'),('Init',c.get('Init','absent'),init,'Raid slot count, high if present; absent=one adapter token'),('Max',c.get('Max','absent'),cap,'literal count except 0=weapon-backed one token, medium'),('Time',c.get('Time','absent'),str(tm)+' seconds','SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard')]:doc.append(f'| {id}/{key} | {raw} | {chosen} | {ev} |')
lines+=['}.ToFrozenDictionary(s=>(s.Operator,s.Id));','}']
(root/'plugins/DeltaOps/InterpretedR2.Generated.cs').write_text('\n'.join(lines)+'\n')
(root/'docs/ABILITY_INTERPRETATIONS.md').write_text('\n'.join(doc)+'\n')
