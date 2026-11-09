import copy
import unittest
from import_evidence import ALIASES, DATA, load, validate, table_rows

class EvidenceTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.r1 = load(DATA / "r1/five-operators-exact.json")
        cls.r2 = load(DATA / "r2/five-operators-exact-vyron-sourceaudit-r2.json.gz")
        cls.full1 = load(DATA / "r1/deltaops-exact-values.json.gz")
        cls.full2 = load(DATA / "r2/deltaops-exact-values-vyron-sourceaudit-r2.json.gz")
    def test_valid(self):
        validate(self.r1)
        validate(self.r2)
    def test_every_raw_operator_value_unchanged(self):
        self.assertEqual(self.r1["operators"], self.r2["operators"])
    def test_full_export_semantics_unchanged(self):
        for key in self.full1:
            self.assertEqual(self.full1[key], self.full2[key], key)
    def test_alias_tamper(self):
        d = copy.deepcopy(self.r2)
        d["operators"][2]["heroName"]["SourceString"] = "红狼"
        with self.assertRaises(AssertionError): validate(d)
    def test_excluded_dwolf(self):
        d = copy.deepcopy(self.r2)
        d["operators"][2]["requestedEnglishLabel"] = "D-Wolf"
        with self.assertRaises(AssertionError): validate(d)
    def test_identity_tamper(self):
        d = copy.deepcopy(self.r2)
        d["operators"][2]["heroRow"] = "88000000024"
        with self.assertRaises(AssertionError): validate(d)
    def test_missing_operator(self):
        d = copy.deepcopy(self.r2)
        d["operators"].pop()
        with self.assertRaises(AssertionError): validate(d)
    def test_missing_skill_not_hidden(self):
        d = copy.deepcopy(self.r2)
        d["operators"][0]["missingSkillIDs"] = []
        with self.assertRaises(AssertionError): validate(d)
    def test_skill_row_tamper(self):
        d = copy.deepcopy(self.r2)
        d["operators"][0]["skills"][0]["row"] = "123"
        with self.assertRaises(AssertionError): validate(d)
    def test_cd_join_tamper(self):
        d = copy.deepcopy(self.r2)
        d["operators"][0]["skills"][0]["cooldownMatches"][0]["value"]["CdID"] = 1
        with self.assertRaises(AssertionError): validate(d)
    def test_group_is_not_skill(self):
        for o in self.r2["operators"]:
            self.assertNotIn(int(o["groupRow"]), o["skillIDs"])
    def test_normalized_fail_closed_and_complete(self):
        d = load(DATA / "normalized-evidence.json")
        self.assertEqual(58, len(d["rows"]))
        self.assertEqual([], d["executableAdapters"])
        self.assertIsNone(d["catalogExport"])
        for row in d["rows"]:
            self.assertIsNone(row["assetSha256"])
            self.assertIsNone(row["numericSemanticValues"])
            self.assertIsNone(row["unitEvidence"])
            self.assertTrue(row["sourcePointer"].startswith("/"))
        for o in d["operators"]:
            self.assertFalse(o["activationAllowed"])
            self.assertTrue(any("precedence" in b for b in o["blockers"]))
            self.assertTrue(any("modifiers" in b for b in o["blockers"]))
    def test_raw_mode_arrays_preserved(self):
        s = self.r2["operators"][0]["skills"][0]
        rows = s["cooldownMatches"][0]["value"]["SlotChargingData"]
        self.assertEqual([60.0,45.0,45.0,60.0,60.0], [r["Time"] for r in rows])
        self.assertEqual(0.25, rows[0]["MaxScoreReduceCDPercent"])
        self.assertEqual(0.0, s["allSerializedFields"]["UseCD"])
    def test_broader_source_rows_exist(self):
        rows = table_rows(self.full2)
        for o in self.r2["operators"]:
            h, _ = rows[(o["heroPackage"], o["heroRow"])]
            self.assertEqual(h["ExpertID"], o["ExpertID"])
            self.assertEqual(h["Name"], o["heroName"])
    def test_rush_not_promoted(self):
        d = load(DATA / "normalized-evidence.json")
        self.assertEqual(28, d["rush"]["evidence"]["size"])
        self.assertIn("unproven", d["rush"]["relevance"])
        self.assertEqual(1, len(d["sourceHashFields"]))
        self.assertEqual("/boundedRushRecovery/rawContainerSHA256",
                         d["sourceHashFields"][0]["pointer"])

if __name__ == "__main__":
    unittest.main()
