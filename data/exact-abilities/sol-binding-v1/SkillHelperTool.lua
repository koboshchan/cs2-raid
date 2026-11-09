local SkillHelperTool = {}
local HeroData = Facade.TableManager:GetTable("Hero/HeroData")
local ExpertSkillData = Facade.TableManager:GetTable("ExpertSkillData")
local SkillDataTable = Facade.TableManager:GetTable("SkillInfoData")
local SkillDataConfig = Facade.TableManager:GetTable("SkillDataConfig")
local ExpertSkillGroupData = Facade.TableManager:GetTable("ExpertSkillGroupData")
local MapModeSkillId2Info = {}
local LobbySkillId2Data = {}

function SkillHelperTool.GetExpertID(heroId)
  local heroDataRow = heroId and HeroData[heroId]
  return heroDataRow and heroDataRow.ExpertID or 1
end

function SkillHelperTool.GetExpertSkillDataRow(expertId)
  return expertId and ExpertSkillData[expertId]
end

function SkillHelperTool.GetExpertSkillGroupDataRow(expertGroupId)
  return expertGroupId and ExpertSkillGroupData[expertGroupId]
end

function SkillHelperTool.CheckIsSkillDataValid(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  return expertSkillRow ~= nil
end

function SkillHelperTool.GetArmedForceID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.ArmedForceID or 1
end

function SkillHelperTool.GetMPUltimateSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.MPUltimateSkillID or 1
end

function SkillHelperTool.GetMPSupportSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.MPSupportSkillID or 1
end

function SkillHelperTool.GetMPActiveSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.MPActiveSkillID or 1
end

function SkillHelperTool.GetMPPassiveSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.MPPassiveSkillID or 1
end

function SkillHelperTool.GetMPCommonPassiveSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.MPCommonPassiveSkillID or 1
end

function SkillHelperTool.GetSOLActiveSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.SOLActiveSkillID or 1
end

function SkillHelperTool.GetSOLUltimateSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.SOLUltimateSkillID or 1
end

function SkillHelperTool.GetSOLSupportSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.SOLSupportSkillID or 1
end

function SkillHelperTool.GetSOLPassiveSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.SOLPassiveSkillID or 1
end

function SkillHelperTool.GetSOLCommonPassiveSkillID(heroId)
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if not expertSkillRow then
    return 1
  end
  return expertSkillRow.SOLCommonPassiveSkillID or 1
end

function SkillHelperTool.GetSkillInfoById(skillId)
  local armedForceMode = Server.ArmedForceServer:GetCurArmedForceMode()
  if armedForceMode == EArmedForceMode.BHD or armedForceMode == EArmedForceMode.Invalid then
    logerror("armedForceMode not have skill id", armedForceMode)
    return nil
  end
  if MapModeSkillId2Info[armedForceMode] == nil then
    declare_if_nil(MapModeSkillId2Info, armedForceMode, {})
    for k, skillInfo in pairs(SkillDataTable) do
      local skillArmedForceMode = SkillHelperTool.GetArmedForceModeBySkillGameModeType(skillInfo.GameMode)
      declare_if_nil(MapModeSkillId2Info, skillArmedForceMode, {})
      MapModeSkillId2Info[skillArmedForceMode][skillInfo.SkillID] = skillInfo
    end
  end
  if MapModeSkillId2Info[armedForceMode][skillId] then
    return MapModeSkillId2Info[armedForceMode][skillId]
  else
    logerror("\230\137\128\233\133\141\231\189\174\231\154\132SkillID", skillId, "\229\176\154\230\156\170\229\156\168Skill\232\161\168\228\184\173\233\133\141\231\189\174\230\138\128\232\131\189\228\191\161\230\129\175")
    return nil
  end
end

function SkillHelperTool.GetArmedForceModeBySkillGameModeType(inGameMode)
  if inGameMode == MatchGameMode.WorldGameMode then
    return EArmedForceMode.SOL
  end
  if inGameMode == MatchGameMode.TDMGameMode then
    return EArmedForceMode.MP
  end
  if inGameMode == MatchGameMode.BlastGameMode then
    return EArmedForceMode.Blasting
  end
  if inGameMode == MatchGameMode.BlackHawkDown then
    return EArmedForceMode.BHD
  end
  return EArmedForceMode.Invalid
end

function SkillHelperTool.GetSkillNameById(skillId)
  local skillInfo = SkillHelperTool.GetSkillInfoById(skillId)
  return skillInfo and skillInfo.Name or ""
end

function SkillHelperTool.GetSkillDescById(skillId)
  local skillInfo = SkillHelperTool.GetSkillInfoById(skillId)
  return skillInfo and skillInfo.Desc or ""
end

function SkillHelperTool.GetSkillIconById(skillId)
  local skillInfo = SkillHelperTool.GetSkillInfoById(skillId)
  return skillInfo and skillInfo.Icon or ""
end

function SkillHelperTool.GetSkillTypeById(skillId)
  local skillInfo = SkillHelperTool.GetSkillInfoById(skillId)
  return skillInfo and skillInfo.SkillType or 1
end

function SkillHelperTool.GetSkillBuffNameById(skillId)
  local skillInfo = SkillHelperTool.GetSkillInfoById(skillId)
  return skillInfo and skillInfo.BuffName or 1
end

function SkillHelperTool.GetSkillDataById(skillId)
  for k, skillData in pairs(SkillDataConfig) do
    if skillData.SkillID == skillId then
      return skillData
    end
  end
  logerror("\230\137\128\233\133\141\231\189\174\231\154\132SkillID", skillId, "\229\176\154\230\156\170\229\156\168SkillDataConfig\232\161\168\228\184\173\233\133\141\231\189\174\230\138\128\232\131\189\228\191\161\230\129\175")
  return nil
end

function SkillHelperTool.GetSkillLongPressDetailById(skillId)
  SkillData = SkillHelperTool.GetSkillDataById(skillId)
  if SkillData then
    return SkillData.LongPressTipsDetailText
  end
  return nil
end

function SkillHelperTool.GetSkillShortPressDetailById(skillId)
  SkillData = SkillHelperTool.GetSkillDataById(skillId)
  if SkillData then
    return SkillData.ShortPressTipsDetailText
  end
  return nil
end

function SkillHelperTool.GetSkillCanLongPress(skillId)
  SkillData = SkillHelperTool.GetSkillDataById(skillId)
  if SkillData then
    return SkillData.CanLongPress
  end
  return false
end

SkillHelperTool.ESkillTypeSuffix = {
  Ultimate = "UltimateSkillID",
  Support = "SupportSkillID",
  Active = "ActiveSkillID",
  Passive = "PassiveSkillID",
  CommonPassive = "CommonPassiveSkillID"
}

function SkillHelperTool.GetSkillID(heroId, skillType, armedForceMode)
  armedForceMode = armedForceMode or Server.ArmedForceServer:GetCurArmedForceMode()
  local expertSkillRow = ExpertSkillData[SkillHelperTool.GetExpertID(heroId)]
  if expertSkillRow == nil then
    logerror("SkillHelperTool.GetSkillID: expertSkillRow is nil", heroId)
    return 1
  end
  local skillGetter = SkillHelperTool.SkillMode2Getter[armedForceMode]
  if skillGetter == nil then
    logerror("SkillHelperTool.GetSkillID: no skill getter for armedForceMode ", armedForceMode)
    return 1
  end
  return skillGetter(expertSkillRow, skillType, heroId)
end

local function MakeExpertSkillDataGetter(prefix)
  return function(expertSkillRow, skillType, heroId)
    local fieldName = prefix .. skillType
    local skillId = expertSkillRow[fieldName]
    if skillId == nil then
      logerror("SkillHelperTool.GetSkillID: skillId is nil", heroId, fieldName)
      return 1
    end
    logframe("SkillHelperTool.GetSkillID: ", heroId, fieldName)
    return skillId
  end
end

local function GetBlastingSkillID(expertSkillRow, skillType, heroId)
  local skillGroupId = expertSkillRow.BombDefusal_SkillGroupID
  local skillGroupRow = SkillHelperTool.GetExpertSkillGroupDataRow(skillGroupId)
  if skillGroupRow == nil then
    logerror("SkillHelperTool.GetSkillID: ExpertSkillGroupData row is nil", heroId, skillGroupId)
    return 1
  end
  local skillId = skillGroupRow[skillType]
  if skillId == nil then
    logerror("SkillHelperTool.GetSkillID: skillId is nil", heroId, skillGroupId, skillType)
    return 1
  end
  logframe("SkillHelperTool.GetSkillID: ", heroId, skillType)
  return skillId
end

SkillHelperTool.SkillMode2Getter = {
  [EArmedForceMode.MP] = MakeExpertSkillDataGetter("MP"),
  [EArmedForceMode.SOL] = MakeExpertSkillDataGetter("SOL"),
  [EArmedForceMode.Blasting] = GetBlastingSkillID
}
return SkillHelperTool
