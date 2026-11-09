import unittest
from audit_sol_binding import bind,skill_index,FIELDS
class SolBindingTests(unittest.TestCase):
    def expert(self):
        x={f:0 for f in FIELDS};x.update(ExpertID=42,SOLActiveSkillID=500,SOLCommonSkillID=[4008010],
            MPActiveSkillID=900,MPCustomSkillID=777,BombDefusal_SkillGroupID=3);return x
    def test_field_not_row(self):
        idx=skill_index([('base','900',{'SkillID':500}),('base','500',{'SkillID':900})])
        result=bind({'ExpertID':42},self.expert(),idx)
        self.assertEqual(result[0]['SkillID'],500);self.assertEqual(result[0]['resolvedRowLabel'],'900')
        self.assertEqual(result[1]['SkillID'],4008010);self.assertTrue(result[1]['missing'])
        self.assertNotIn(900,[r['SkillID'] for r in result]);self.assertNotIn(777,[r['SkillID'] for r in result])
    def test_no_row_fallback(self):
        idx=skill_index([('base','500',{'SkillID':900})])
        self.assertTrue(bind({'ExpertID':42},self.expert(),idx)[0]['missing'])
    def test_no_mp_fallback(self):
        e=self.expert();del e['SOLActiveSkillID']
        with self.assertRaises(ValueError):bind({'ExpertID':42},e,{})
    def test_duplicate_same_id_rejected(self):
        for second in [('base','b',{'SkillID':500}),('override','a',{'SkillID':500,'CDID':3})]:
            with self.assertRaises(ValueError):skill_index([('base','a',{'SkillID':500}),second])
    def test_hero_join_mismatch(self):
        with self.assertRaises(ValueError):bind({'ExpertID':43},self.expert(),{})
    def test_missing_skillid_no_label_inference(self):
        with self.assertRaises(ValueError):skill_index([('base','500',{})])
    def test_zero_not_imported(self):
        self.assertEqual(len(bind({'ExpertID':42},self.expert(),{})),2)
if __name__=='__main__':unittest.main()
