"""Guard the literal extracted behavior evidence independently of wiki descriptions."""
import gzip,json,pathlib,re,unittest
ROOT=pathlib.Path(__file__).resolve().parents[2]
class PassiveEvidenceTests(unittest.TestCase):
    def rows(self):
        path=ROOT/'data/exact-abilities/r2/five-operators-exact-vyron-sourceaudit-r2.json.gz'
        with gzip.open(path,'rt',encoding='utf-8') as f: data=json.load(f)
        return {s['allSerializedFields']['SkillID']:s['allSerializedFields']
                for op in data['operators'] for s in op['skills']}
    def test_shepherd_literal_behavior(self):
        row=self.rows()[30010201]
        self.assertEqual(row['DamgeCoifficient'],[{
            'fModifyRateExplodeDamage':.7,'fModifyRateAbilitAssault':1.,
            'LimitPlayMode':[]}])
        src=(ROOT/'plugins/DeltaOps/ExtractedPassiveValues.cs').read_text()
        value=re.search(r'ShepherdExplosionMultiplier=(\d+\.\d+)f',src)
        self.assertEqual(float(value[1]),row['DamgeCoifficient'][0]['fModifyRateExplodeDamage'])
    def test_luna_does_not_invent_buff_decoding(self):
        row=self.rows()[40010201]
        self.assertEqual(row['AddBuffs'],[8011,100008])
        self.assertTrue(row['bEnableSniperMark'])
        self.assertTrue(row['bOverrideAimingCheck'])
        self.assertEqual(row['TeamPlayerIcon']['AssetPathName'],'None')
    def test_fall_gate_operator_matches_executable_catalog(self):
        # Bind the policy identity to the actual generated catalog, not a test's
        # duplicate hardcoded expectation that could repeat the same mistaken ID.
        catalog=(ROOT/'plugins/DeltaOps/InterpretedR2.Generated.cs').read_text()
        owner=int(re.search(r'new\((\d+),1009004,',catalog)[1])
        policy=(ROOT/'plugins/DeltaOps/NativeFallPassivePolicy.cs').read_text()
        gate=int(re.search(r'selected!=(\d+)',policy)[1])
        self.assertEqual(gate,owner)
if __name__=='__main__':unittest.main()
