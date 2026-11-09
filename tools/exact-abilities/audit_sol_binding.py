"""Derived SOL binding. Raw r2 snapshots are never modified. No row-number skill fallback."""
import gzip,hashlib,json,pathlib
ROOT=pathlib.Path(__file__).resolve().parents[2]
DEST=ROOT/'data/exact-abilities/sol-binding-v1'
FIELDS=('SOLUltimateSkillID','SOLSupportSkillID','SOLActiveSkillID',
        'SOLPassiveSkillID','SOLCommonPassiveSkillID','SOLCommonSkillID')
def skill_index(rows):
    result={}
    for package,label,value in rows:
        key=value.get('SkillID')
        if type(key) is not int:raise ValueError(f'Missing SkillID: {package}/{label}')
        if key in result:raise ValueError(f'Ambiguous duplicate SkillID {key}')
        result[key]=(package,label,value)
    return result
def bind(hero,expert,index):
    if type(hero.get('ExpertID')) is not int or hero['ExpertID']!=expert.get('ExpertID'):
        raise ValueError('Hero ExpertID mismatch')
    bindings=[]
    for field in FIELDS:
        if field not in expert:raise ValueError(f'Missing SOL field {field}; no MP fallback')
        values=expert[field] if field=='SOLCommonSkillID' else [expert[field]]
        if not isinstance(values,list):raise ValueError('Expected SOL common list')
        for value in values:
            if type(value) is not int or value<0:raise ValueError('Invalid skill identifier')
            if not value:continue
            resolved=index.get(value) # SkillID field ONLY, never the row label.
            bindings.append(dict(field=field,SkillID=value,package=resolved[0] if resolved else None,
                resolvedRowLabel=resolved[1] if resolved else None,missing=resolved is None))
    return bindings
def table_rows(report,suffix):
    matches=[]
    for table in report['tables']+report['supplementaryOperatorAndBaseSkillTables']:
        if not table['package'].endswith(suffix):continue
        for decoded in table.get('decoded',[]):
            for label,value in decoded.get('value',{}).get('Rows',{}).items():
                matches.append((table['package'],label,value))
    if not matches:raise ValueError('Table missing '+suffix)
    return matches
def main():
    source=ROOT/'data/exact-abilities/r2'
    broad=gzip.decompress((source/'deltaops-exact-values-vyron-sourceaudit-r2.json.gz').read_bytes())
    narrow=gzip.decompress((source/'five-operators-exact-vyron-sourceaudit-r2.json.gz').read_bytes())
    report=json.loads(broad);operators=json.loads(narrow)['operators']
    # Lookup package is the base SkillDataConfig explicitly referenced by Lua.
    # Combination table is retained as separate evidence, not an invented override.
    config=table_rows(report,'/Skill/SkillDataConfig.uasset');index=skill_index(config)
    heroes={label:(package,value) for package,label,value in table_rows(report,'/Hero/HeroData.uasset')}
    experts={label:(package,value) for package,label,value in table_rows(report,'/Skill/ExpertSkillData.uasset')}
    receipt={'version':1,'mode':'SOL','sourceSHA256':{'fullR2':hashlib.sha256(broad).hexdigest(),
        'fiveOperatorR2':hashlib.sha256(narrow).hexdigest()},
        'lua':{'path':'D:/df-lua-decompile/decompiled/pak-0-0-pakchunk1-WindowsClient.pak/DeltaForce/Content/LuaSource/DFM/StandaloneLua/BusinessTool/SkillHelperTool.lua',
        'sha256':hashlib.sha256((DEST/'SkillHelperTool.lua').read_bytes()).hexdigest(),
        'lines':{'heroExpert':[10,17],'configBySkillID':[179,186],'modeFields':[213,247],
                 'blastingOnly':[249,262],'SOLGetter':[265,269],'worldToSOL':[138,151]}},
        'limitations':['Decompiled source, not proved semantic equivalence.',
          'SOLCommonSkillID arrays are explicit source fields; GetSkillID suffix list only covers five main slots.',
          'No SOLCustomSkillID is serialized. MPCustomSkillID and BombDefusal group are not SOL bindings.',
          'Base SkillDataConfig package chosen per Lua table name; runtime package override precedence not proved.',
          'Lua provides no cooldown or effect magnitudes; raw DataTable numbers remain evidence.'],
        'operators':[]}
    for number,op in enumerate(operators,1):
        heroPackage,hero=heroes[str(op['heroRow'])]
        assert hero['ExpertID']==op['ExpertID']
        expertPackage,expert=experts[str(hero['ExpertID'])]
        assert expert==op['allExpertFields']
        fields=bind(hero,expert,index)
        assert {b['SkillID'] for b in fields if b['missing']}=={4008010}
        actual={b['SkillID'] for b in fields if not b['missing']}
        previous={s['allSerializedFields']['SkillID'] for s in op['skills']}
        receipt['operators'].append(dict(operator=number,name=op['requestedEnglishLabel'],
            heroRow=str(op['heroRow']),heroPackage=heroPackage,ExpertID=hero['ExpertID'],
            expertRow=str(hero['ExpertID']),expertPackage=expertPackage,
            SOLFields={f:expert[f] for f in FIELDS},bindings=fields,
            excludedOldUnionIDs=sorted(previous-actual),newIDs=sorted(actual-previous),
            rowLabelDiffers=[b for b in fields if not b['missing'] and str(b['SkillID'])!=b['resolvedRowLabel']]))
    (DEST/'receipt.json').write_text(json.dumps(receipt,ensure_ascii=False,indent=2)+'\n')
    print(json.dumps([{k:o[k] for k in ('name','ExpertID','excludedOldUnionIDs','newIDs','rowLabelDiffers')} for o in receipt['operators']],ensure_ascii=False,indent=2))
if __name__=='__main__':main()
