EXACT_ABILITIES
Only Stinger, Luna, Vyron, Hackclaw and Shepherd. Standalone report, no worktree edits.

Vyron = 威龙 is caller-confirmed from official pages. This is NOT archive English-localization proof. Other four English mappings remain archive-candidate-only/unverified. 红狼 / D-Wolf is excluded as an operator. Shared serialized skill references are preserved truthfully, not reassigned.
All included skill fields are serialized config, not an implementation of runtime behavior. Mode override precedence unresolved; CDID uses CdID matches, not UseCD.
Only direct expert/group skill-ID joins included. Referenced effects, projectiles, buffs, nested extra skills and Blueprint logic are NOT decoded transitively. No claim these tables alone implement all ability values.
RushData GPAudioOnAddMap remains typed-unresolved. Raw 28 bytes preserved in full report. No proven reference join from these five candidates to that Rush row established, so it is NOT an actionable parameter here.

result = PASS_ALL_FIVE
method = Compared actual serialized HeroData, ExpertSkillData, ExpertSkillGroupData, every included SkillDataConfig row/all fields, and every CdID match against original full decode exports. Not labels alone.
changedIDs
  (empty array)
changedSkillCounts = False
operators
  [0]
    label = Stinger
    heroRow = 88000000027
    archiveName = 蜂医
    expertRow = 20003
    groupRow = 20003
    skillIDs
      [0]
        2008002
      [1]
        2008005
      [2]
        4008007
      [3]
        4008010
      [4]
        20010101
      [5]
        20010201
      [6]
        20010215
      [7]
        30010203
    skillConfigCount = 7
    missingSkillIDs
      [0]
        4008010
    allSerializedFieldsMatchOriginalExports = True
    allCooldownJoinsMatchOriginalExports = True
  [1]
    label = Luna
    heroRow = 88000000028
    archiveName = 露娜
    expertRow = 40005
    groupRow = 40005
    skillIDs
      [0]
        2002017
      [1]
        3008002
      [2]
        4008002
      [3]
        4008007
      [4]
        4008010
      [5]
        30010203
      [6]
        40010201
    skillConfigCount = 6
    missingSkillIDs
      [0]
        4008010
    allSerializedFieldsMatchOriginalExports = True
    allCooldownJoinsMatchOriginalExports = True
  [2]
    label = Vyron
    heroRow = 88000000025
    archiveName = 威龙
    expertRow = 10010
    groupRow = 10010
    skillIDs
      [0]
        1009001
      [1]
        1009002
      [2]
        1009003
      [3]
        1009004
      [4]
        1009005
      [5]
        4008007
      [6]
        4008010
      [7]
        10010301
      [8]
        30010203
    skillConfigCount = 8
    missingSkillIDs
      [0]
        4008010
    allSerializedFieldsMatchOriginalExports = True
    allCooldownJoinsMatchOriginalExports = True
  [3]
    label = Hackclaw
    heroRow = 88000000026
    archiveName = 骇爪
    expertRow = 40010
    groupRow = 40010
    skillIDs
      [0]
        1002016
      [1]
        4008007
      [2]
        4008010
      [3]
        4009002
      [4]
        4009003
      [5]
        30010203
      [6]
        40010202
    skillConfigCount = 6
    missingSkillIDs
      [0]
        4008010
    allSerializedFieldsMatchOriginalExports = True
    allCooldownJoinsMatchOriginalExports = True
  [4]
    label = Shepherd
    heroRow = 88000000029
    archiveName = 牧羊人
    expertRow = 30008
    groupRow = 30008
    skillIDs
      [0]
        3008001
      [1]
        3008008
      [2]
        4008007
      [3]
        4008010
      [4]
        30010101
      [5]
        30010201
      [6]
        30010202
    skillConfigCount = 6
    missingSkillIDs
      [0]
        4008010
    allSerializedFieldsMatchOriginalExports = True
    allCooldownJoinsMatchOriginalExports = True

Stinger
requestedEnglishLabel = Stinger
EnglishMappingStatus = UNVERIFIED_CHINESE_ARCHIVE_CANDIDATE_ONLY
heroPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Hero/HeroData.uasset
heroRow = 88000000027
heroName
  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_HeroData.LOC_HeroData
  Key = 88000000027_Name
  SourceString = 蜂医
  LocalizedString = 蜂医
ExpertID = 20003
expertPackage = DeltaForce/Content/R13N/Global/Base/DataTables/Skill/ExpertSkillData.uasset
expertRow = 20003
allExpertFields
  ExpertID = 20003
  ArmedForceID = 2
  MPUltimateSkillID = 20010101
  MPSupportSkillID = 2008002
  MPPassiveSkillID = 20010201
  MPActiveSkillID = 2008005
  MPCommonPassiveSkillID = 0
  MPCustomSkillID = 30010203
  MPCommonSkillID
    [0]
      4008007
  SOLUltimateSkillID = 20010101
  SOLSupportSkillID = 2008002
  SOLPassiveSkillID = 20010201
  SOLActiveSkillID = 2008005
  SOLCommonPassiveSkillID = 0
  SOLCommonSkillID
    [0]
      4008007
    [1]
      4008010
  BombDefusal_SkillGroupID = 20003
  RowDescription = None
groupPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Skill/ExpertSkillGroupData.uasset
groupRow = 20003
allGroupFields
  SkillGroupID = 20003
  UltimateSkillID = 20010101
  SupportSkillID = 2008002
  PassiveSkillID = 20010215
  ActiveSkillID = 2008005
  CommonPassiveSkillID = 0
  CustomSkillID = 0
  CommonSkillID
    [0]
      4008007
  RowDescription = None
skillIDs
  [0]
    2008002
  [1]
    2008005
  [2]
    4008007
  [3]
    4008010
  [4]
    20010101
  [5]
    20010201
  [6]
    20010215
  [7]
    30010203
missingSkillIDs
  [0]
    4008010
skills
  [0]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 2008002
    allSerializedFields
      SkillID = 2008002
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 2008002_SkillDisplayName
        SourceString = 烟幕
        LocalizedString = 烟幕
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 2008002_ShortName
        SourceString = 烟幕
        LocalizedString = 烟幕
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 14
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::RightHandSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_SupportSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support/BakedSprite/SupportSoldier_Skill_Icon_04.SupportSoldier_Skill_Icon_04
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support_Mobile/BakedSprite/SupportSoldier_Skill_Icon_13_m.SupportSoldier_Skill_Icon_13_m
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support_Mobile/BakedSprite/SupportSoldier_Skill_Icon_04_m.SupportSoldier_Skill_Icon_04_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support_Mobile/BakedSprite/SupportSoldier_Skill_Icon_13_m.SupportSoldier_Skill_Icon_13_m
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 2002
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        [0]
          ECharacterAudioType::Ability_VO7
        [1]
          ECharacterAudioType::Ability_VO8
        [2]
          ECharacterAudioType::Ability_VO9
      VosList
        (empty array)
      ActivatedWeaponSlot = 131
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Selected
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanAimOn
      UnLockStateAbilitys
        [0]
          ECharacterStateMask::CanProneMove
        [1]
          ECharacterStateMask::CanProneStandUp
        [2]
          ECharacterStateMask::CanStandUp
        [3]
          ECharacterStateMask::CanProne
        [4]
          ECharacterStateMask::CanCrouch
        [5]
          ECharacterStateMask::CanClimbLadder
        [6]
          ECharacterStateMask::CanSprint
        [7]
          ECharacterStateMask::CanAimOff
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_UseItem
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = True
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21020300007
      WeaponAttachPos = 1136
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::PutWeaponQuiet
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = True
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginKeepPreFire
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginEquip
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanUseLegSkill
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            LockAbility
              (empty array)
            UnLockAbility
              [0]
                ECharacterStateMask::CanUseLegSkill
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060008
          Value = Scalpel_0q_HiveTechDrone_Inspect
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 2002
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 2002
        field = CdID
        value
          CdID = 2002
          Slot = ECharingSlot::ArmdedForceItem1
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 1
              Max = 1
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 1
              Max = 1
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 1
              Max = 1
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 1
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [4]
              Init = 1
              Max = 1
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [1]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 2008005
    allSerializedFields
      SkillID = 2008005
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 2008003_SkillDisplayName
        SourceString = 蜂巢科技烟雾弹
        LocalizedString = 蜂巢科技烟雾弹
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 2008003_ShortName
        SourceString = 烟雾
        LocalizedString = 烟雾
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 0
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::LeftHandSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_ActiveSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support/BakedSprite/SupportSoldier_Skill_Icon_05.SupportSoldier_Skill_Icon_05
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support_Mobile/BakedSprite/SupportSoldier_Skill_Icon_05_m.SupportSoldier_Skill_Icon_05_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 2001
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.5
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        [0]
          ECharacterAudioType::Ability_VO7
        [1]
          ECharacterAudioType::Ability_VO8
        [2]
          ECharacterAudioType::Ability_VO9
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = True
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanDetectRescue
        [1]
          ECharacterStateMask::CanReload
        [2]
          ECharacterStateMask::CanAutoChangeClip
        [3]
          ECharacterStateMask::CanAimOn
        [4]
          ECharacterStateMask::CanLooting
        [5]
          ECharacterStateMask::DisableWhitelistOperate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanPlayEmote
      UnLockStateAbilitys
        [0]
          ECharacterStateMask::CanMove
        [1]
          ECharacterStateMask::CanRotate
        [2]
          ECharacterStateMask::CanCharacterRotate
        [3]
          ECharacterStateMask::CanCrouch
        [4]
          ECharacterStateMask::CanProne
        [5]
          ECharacterStateMask::CanJump
        [6]
          ECharacterStateMask::CanStandUp
        [7]
          ECharacterStateMask::CanFire
        [8]
          ECharacterStateMask::CanSprint
        [9]
          ECharacterStateMask::CanProneMove
        [10]
          ECharacterStateMask::CanProneStandUp
        [11]
          ECharacterStateMask::CanClimbLadder
        [12]
          ECharacterStateMask::CanAimOff
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_Rescue
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [4]
          EFSMCharacterState::FSMPlayerState_WeaponBipodDeploy
        [5]
          EFSMCharacterState::FSMPlayerState_GameOperate
        [6]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        [0]
          EGPWeaponStateType::EWeapState_Equipping
        [1]
          EGPWeaponStateType::EWeapState_Unequipping
        [2]
          EGPWeaponStateType::EWeapState_ChangingClip
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21010000015
      WeaponAttachPos = 1135
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 2
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = True
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = True
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_PreFire
          Value
            TipsDataList
              (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = True
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 2001
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 2001
        field = CdID
        value
          CdID = 2001
          Slot = ECharingSlot::ArmdedForceItem2
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 1
              Max = 1
              Time = 40.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 1
              Max = 2
              Time = 0.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 1
              AmmoPackChargeCnt = 1
              SceneAmmoBoxChargeCnt = 1
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 1
              Max = 2
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 1
              Max = 1
              Time = 40.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [2]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4008007
    allSerializedFields
      SkillID = 4008007
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_SkillDisplayName
        SourceString = 拔箭
        LocalizedString = 拔箭
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_ShortName
        SourceString = 拔箭
        LocalizedString = 拔箭
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          3012006
        [1]
          4008009
      AddSkills_Sol
        [0]
          4008008
        [1]
          6010100
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 23
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityIns_Scout_PullOutArrow.BP_AbilityIns_Scout_PullOutArrow_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityTemp_Scout_PullOutArrow.BP_AbilityTemp_Scout_PullOutArrow_C
        SubPathString = 
      SkillStateType = ESkillStateType::Custom2Skill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = False
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = False
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanHighValueItemInspect
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanPlayTacticalAction
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanUseZipline
        [8]
          ECharacterStateMask::CanDetectInteractor
        [9]
          ECharacterStateMask::CanLooting
        [10]
          ECharacterStateMask::CanShowInteractUI
        [11]
          ECharacterStateMask::CanSkillSwtichWeapon
        [12]
          ECharacterStateMask::CanDetectRescue
        [13]
          ECharacterStateMask::CanUseMachineGun
        [14]
          ECharacterStateMask::CanLean
        [15]
          ECharacterStateMask::CanReload
        [16]
          ECharacterStateMask::CanWeaponBipodDeploy
        [17]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [18]
          ECharacterStateMask::CanOperate
        [19]
          ECharacterStateMask::CanSwitchHelmetMask
        [20]
          ECharacterStateMask::CanUseTelescope
        [21]
          ECharacterStateMask::CanChangeWeaponMode
        [22]
          ECharacterStateMask::CanParachute
        [23]
          ECharacterStateMask::CanAimOn
        [24]
          ECharacterStateMask::CanFire
        [25]
          ECharacterStateMask::CanSprint
        [26]
          ECharacterStateMask::CanPlayMarkingAnim
        [27]
          ECharacterStateMask::CanApplySpray
        [28]
          ECharacterStateMask::CanFirefighting
        [29]
          ECharacterStateMask::CanPlayOneHandedGesture
        [30]
          ECharacterStateMask::CanPlayTwoHandedGesture
        [31]
          ECharacterStateMask::CanBeBreakByOtherSkill
        [32]
          ECharacterStateMask::CanVault
        [33]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_CommonLeftArmAnim
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = 0
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.0
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [3]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 20010101
    allSerializedFields
      SkillID = 20010101
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 20010101_SkillDisplayName
        SourceString = 激素枪
        LocalizedString = 激素枪
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 20010101_ShortName
        SourceString = 治疗
        LocalizedString = 治疗
      LogicType = ESkillLogicType::Support
      TeamPlayerIcon
        AssetPathName = /Game/UI/UIAtlas/Common/BakedSprite/Common_Arms_Icon_0003.Common_Arms_Icon_0003
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 36
      FlowDataType = EFlowDataType::ActiveSKill
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/MedicalSoldier/101/BP_AbilityIns_CreateGun.BP_AbilityIns_CreateGun_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          [0]
            Rescue_20003
        GlobalPreloadAttachMesh
          [0]
            MedicGun
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_FireButtonLeft
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support/BakedSprite/SupportSoldier_Skill_Icon_01.SupportSoldier_Skill_Icon_01
        SubPathString = 
      Cancel
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Btn_46.HUD_Controller_Btn_46
        SubPathString = 
      Disable
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support/BakedSprite/SupportSoldier_Skill_Icon_02.SupportSoldier_Skill_Icon_02
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0001.HUD_Controller_Icon_0001
        SubPathString = 
      NormalSlot1
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Btn_46.HUD_Controller_Btn_46
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support_Mobile/BakedSprite/SupportSoldier_Skill_Icon_01_m.SupportSoldier_Skill_Icon_01_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Btn_46.HUD_Controller_Btn_46
        SubPathString = 
      Disable_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Support/BakedSprite/SupportSoldier_Skill_Icon_02.SupportSoldier_Skill_Icon_02
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0001.HUD_Controller_Icon_0001
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Btn_46.HUD_Controller_Btn_46
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = True
      CDID = 2
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        [0]
          ECharacterAudioType::Ability_VO4
        [1]
          ECharacterAudioType::Ability_VO5
        [2]
          ECharacterAudioType::Ability_VO6
        [3]
          ECharacterAudioType::Ability_VO18
      VosList
        [0]
          UltimateUse
      ActivatedWeaponSlot = 130
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        [0]
          ECharacterStateMask::CanMove
        [1]
          ECharacterStateMask::CanCrouch
        [2]
          ECharacterStateMask::CanVault
        [3]
          ECharacterStateMask::CanSprint
        [4]
          ECharacterStateMask::CanRotate
        [5]
          ECharacterStateMask::CanCharacterRotate
        [6]
          ECharacterStateMask::CanJump
        [7]
          ECharacterStateMask::CanStandUp
        [8]
          ECharacterStateMask::CanSkillSwtichWeapon
        [9]
          ECharacterStateMask::CanSwitchWeapon
        [10]
          ECharacterStateMask::CanProneMove
        [11]
          ECharacterStateMask::CanProneStandUp
        [12]
          ECharacterStateMask::CanStandUp
        [13]
          ECharacterStateMask::CanAimOff
        [14]
          ECharacterStateMask::CanProne
        [15]
          ECharacterStateMask::CanSlidingTackle
        [16]
          ECharacterStateMask::CanLean
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = True
      ShortPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 20010101_ShortPressTipsDetailText
        SourceString = 切出激素枪
        LocalizedString = 切出激素枪
      LongPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 20010101_LongPressTipsDetailText
        SourceString = 使用激素枪自我治疗
        LocalizedString = 使用激素枪自我治疗
      LongPressCallSkill = 2001010
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = TipsDesc_3
        SourceString = （长按）自我治疗
        LocalizedString = （长按）自我治疗
      LongPressTipsActionName = AbilitySkill
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 10070000246
      WeaponAttachPos = 1134
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 1134
      bAddSkillWeapon = True
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
              [1]
                Key = ToggleAiming
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_2
                  SourceString = 开镜
                  LocalizedString = 开镜
                bOverrideDesc = True
              [2]
                Key = AbilitySkill
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_3
                  SourceString = （长按）自我治疗
                  LocalizedString = （长按）自我治疗
                bOverrideDesc = True
        [1]
          Key = EGPWeaponInnerEvent::EInnerEvent_DoBeginZoom
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
              [1]
                Key = AbilitySkill
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_3
                  SourceString = （长按）自我治疗
                  LocalizedString = （长按）自我治疗
                bOverrideDesc = True
        [2]
          Key = EGPWeaponInnerEvent::EInnerEvent_DoEndZoom
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
              [1]
                Key = ToggleAiming
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_2
                  SourceString = 开镜
                  LocalizedString = 开镜
                bOverrideDesc = True
              [2]
                Key = AbilitySkill
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_3
                  SourceString = （长按）自我治疗
                  LocalizedString = （长按）自我治疗
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060008
          Value = Scalpel_0q_MedicalGun_Inspect
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 2
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 2
        field = CdID
        value
          CdID = 2
          Slot = ECharingSlot::Ability
          WeaponSlot = 1134
          SlotChargingData
            [0]
              Init = 6
              Max = 6
              Time = 6.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 900.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 6
              MaxClipAmmoNum = 6
              SkillNumType = ESkillNumType::UT_Normal
              DoneVoiceString = UltimateFullyCharged
              NoCDType = True
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 3
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 3
              MaxClipAmmoNum = 3
              SkillNumType = ESkillNumType::UT_Normal
              DoneVoiceString = UltimateFullyCharged
              NoCDType = True
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 6
              Max = 6
              Time = 6.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 900.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 6
              MaxClipAmmoNum = 6
              SkillNumType = ESkillNumType::UT_Normal
              DoneVoiceString = UltimateFullyCharged
              NoCDType = True
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 3
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_Arena
              AddToCurrentAmmo = True
              CurrentAmmoNum = 3
              MaxClipAmmoNum = 3
              SkillNumType = ESkillNumType::UT_Normal
              DoneVoiceString = UltimateFullyCharged
              NoCDType = True
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 1
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [4]
              Init = 6
              Max = 6
              Time = 6.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 900.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_TacticalConquest
                [1]
                  EDFMGamePlaySubMode::GamePlaySubMode_CaptureFlag
              AddToCurrentAmmo = True
              CurrentAmmoNum = 6
              MaxClipAmmoNum = 6
              SkillNumType = ESkillNumType::UT_Normal
              DoneVoiceString = UltimateFullyCharged
              NoCDType = True
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [5]
              Init = 0
              Max = 0
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 3
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = UltimateFullyCharged
              NoCDType = True
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 1
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                [0]
                  EventType = ESlotChargingEventType::RoundStart
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [1]
                  EventType = ESlotChargingEventType::RoudnOTStart
                  PercentRecover = 3.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [2]
                  EventType = ESlotChargingEventType::PlantBomb
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [3]
                  EventType = ESlotChargingEventType::PlantBombPersonal
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [4]
                  EventType = ESlotChargingEventType::BombDefusal
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [5]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [6]
                  EventType = ESlotChargingEventType::Dead
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
            [6]
              Init = 0
              Max = 3
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 3
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = UltimateFullyCharged
              NoCDType = True
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = True
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [4]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 20010201
    allSerializedFields
      SkillID = 20010201
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 20010201_SkillDisplayName
        SourceString = 高效救援
        LocalizedString = 高效救援
      ShortName
        CultureInvariantString = None
      LogicType = ESkillLogicType::Support
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 1
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          2001010
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        [0]
          8009
      ConflictID = 0
      FlowDataType = EFlowDataType::Passive
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [5]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 20010215
    allSerializedFields
      SkillID = 20010215
      SkillDisplayName
        CultureInvariantString = None
      ShortName
        CultureInvariantString = None
      LogicType = ESkillLogicType::Support
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 1
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          2001010
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        [0]
          8009
      ConflictID = 0
      FlowDataType = EFlowDataType::Passive
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [6]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 30010203
    allSerializedFields
      SkillID = 30010203
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_SkillDisplayName
        SourceString = 工具箱
        LocalizedString = 工具箱
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_ShortName
        SourceString = 工具箱
        LocalizedString = 工具箱
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 31
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = False
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_Custom
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon/BakedSprite/ArmsCommon_Skill_Icon_12.ArmsCommon_Skill_Icon_12
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_23_m.Engineer_Skill_Icon_23_m
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon_Mobile/BakedSprite/ArmsCommon_Skill_Icon_12_m.ArmsCommon_Skill_Icon_12_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 1031
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = True
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanVault
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21020300011
      WeaponAttachPos = 1133
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 1031
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 工具箱
        field = CdID
        value
          CdID = 1031
          Slot = ECharingSlot::ArmdedForceItem1Ready
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 99
              Max = 99
              Time = 0.1
              UseTime = 0.0
              ChargeAmmoReplenish = 99
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
                [1]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 99
              MaxClipAmmoNum = 99
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED

Luna
requestedEnglishLabel = Luna
EnglishMappingStatus = UNVERIFIED_CHINESE_ARCHIVE_CANDIDATE_ONLY
heroPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Hero/HeroData.uasset
heroRow = 88000000028
heroName
  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_HeroData.LOC_HeroData
  Key = 88000000028_Name
  SourceString = 露娜
  LocalizedString = 露娜
ExpertID = 40005
expertPackage = DeltaForce/Content/R13N/Global/Base/DataTables/Skill/ExpertSkillData.uasset
expertRow = 40005
allExpertFields
  ExpertID = 40005
  ArmedForceID = 4
  MPUltimateSkillID = 2002017
  MPSupportSkillID = 4008002
  MPPassiveSkillID = 40010201
  MPActiveSkillID = 3008002
  MPCommonPassiveSkillID = 0
  MPCustomSkillID = 30010203
  MPCommonSkillID
    [0]
      4008007
  SOLUltimateSkillID = 2002017
  SOLSupportSkillID = 4008002
  SOLPassiveSkillID = 40010201
  SOLActiveSkillID = 3008002
  SOLCommonPassiveSkillID = 0
  SOLCommonSkillID
    [0]
      4008007
    [1]
      4008010
  BombDefusal_SkillGroupID = 40005
  RowDescription = None
groupPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Skill/ExpertSkillGroupData.uasset
groupRow = 40005
allGroupFields
  SkillGroupID = 40005
  UltimateSkillID = 2002017
  SupportSkillID = 4008002
  PassiveSkillID = 40010201
  ActiveSkillID = 3008002
  CommonPassiveSkillID = 0
  CustomSkillID = 0
  CommonSkillID
    [0]
      4008007
  RowDescription = None
skillIDs
  [0]
    2002017
  [1]
    3008002
  [2]
    4008002
  [3]
    4008007
  [4]
    4008010
  [5]
    30010203
  [6]
    40010201
missingSkillIDs
  [0]
    4008010
skills
  [0]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 2002017
    allSerializedFields
      SkillID = 2002017
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 40010101_SkillDisplayName
        SourceString = 侦察箭矢
        LocalizedString = 侦察箭矢
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 2002017_ShortName
        SourceString = 侦察
        LocalizedString = 侦察
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = /Game/UI/UIAtlas/Common/BakedSprite/Common_Arms_Icon_0001.Common_Arms_Icon_0001
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = True
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 14
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_FireButtonLeft
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_01.Scout_Skill_Icon_01
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_02.Scout_Skill_Icon_02
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_SKill_Icon_05.HUD_SKill_Icon_05
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts_Mobile/BakedSprite/Scout_Skill_Icon_01_m.Scout_Skill_Icon_01_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0001.HUD_Controller_Icon_0001
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = True
      CDID = 14
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        [0]
          ECharacterAudioType::Ability_VO10
        [1]
          ECharacterAudioType::Ability_VO11
        [2]
          ECharacterAudioType::Ability_VO17
      VosList
        (empty array)
      ActivatedWeaponSlot = 131
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Selected
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = True
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanCarrybody
        [1]
          ECharacterStateMask::CanSlidingTackle
        [2]
          ECharacterStateMask::CanClimbLadder
        [3]
          ECharacterStateMask::CanWeaponInspect
        [4]
          ECharacterStateMask::CanPlayTacticalAction
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanHighValueItemInspect
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_UseItem
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 10130000876
      WeaponAttachPos = 1134
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::Left
        ModelConflict = EGPWeaponAttachBackConflict::LunaDoubleBow
        AttachBackRelativeTransform
          Rotation
            X = 0.707107
            Y = 0.0
            Z = 0.0
            W = 0.707107
            IsNormalized = True
            Size = 1.0000002
            SizeSquared = 1.0000006
          Translation
            X = 0.0
            Y = 15.0
            Z = 20.0
          Scale3D
            X = 0.8
            Y = 0.8
            Z = 0.8
        AttachBagRelativeTransform
          Rotation
            X = -0.707107
            Y = -0.0
            Z = 0.0
            W = 0.707107
            IsNormalized = True
            Size = 1.0000002
            SizeSquared = 1.0000006
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 0.8
            Y = 0.8
            Z = 0.8
        DisabledAttachBackDeviceLevels
          [0]
            0
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
              [1]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_7
                  SourceString = （长按）蓄力
                  LocalizedString = （长按）蓄力
                bOverrideDesc = True
        [1]
          Key = EGPWeaponInnerEvent::EInnerEvent_Charging
          Value
            TipsDataList
              [0]
                Key = Reload
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_9
                  SourceString = 取消蓄力
                  LocalizedString = 取消蓄力
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EInnerEvent_Charging
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanParachute
              [1]
                ECharacterStateMask::CanDetectRescue
              [2]
                ECharacterStateMask::CanHighValueItemInspect
              [3]
                ECharacterStateMask::CanOperate
              [4]
                ECharacterStateMask::CanUseActionSkill
              [5]
                ECharacterStateMask::CanUseZipline
              [6]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_EndFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanUseItem
            UnLockAbility
              [0]
                ECharacterStateMask::CanParachute
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [2]
          Key = EGPWeaponInnerEvent::EInnerEvent_CancelCharge
          Value
            LockAbility
              (empty array)
            UnLockAbility
              [0]
                ECharacterStateMask::CanParachute
              [1]
                ECharacterStateMask::CanDetectRescue
              [2]
                ECharacterStateMask::CanHighValueItemInspect
              [3]
                ECharacterStateMask::CanOperate
              [4]
                ECharacterStateMask::CanUseActionSkill
              [5]
                ECharacterStateMask::CanUseZipline
              [6]
                ECharacterStateMask::CanDive
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [3]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 14
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 14
        field = CdID
        value
          CdID = 14
          Slot = ECharingSlot::Ability
          WeaponSlot = 1134
          SlotChargingData
            [0]
              Init = 0
              Max = 0
              Time = 75.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Ultimate_Fully_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 0
              Time = 70.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 10500.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Ultimate_Fully_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 0
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 0
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_Arena
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Ultimate_Fully_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [4]
              Init = 0
              Max = 0
              Time = 75.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Ultimate_Fully_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 6
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                [0]
                  EventType = ESlotChargingEventType::RoundStart
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [1]
                  EventType = ESlotChargingEventType::RoudnOTStart
                  PercentRecover = 0.835
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [2]
                  EventType = ESlotChargingEventType::PlantBomb
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [3]
                  EventType = ESlotChargingEventType::PlantBombPersonal
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [4]
                  EventType = ESlotChargingEventType::BombDefusal
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [5]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [6]
                  EventType = ESlotChargingEventType::Dead
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
            [5]
              Init = 0
              Max = 0
              Time = 75.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Ultimate_Fully_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                [0]
                  EventType = ESlotChargingEventType::RoundStart
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [1]
                  EventType = ESlotChargingEventType::RoudnOTStart
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [2]
                  EventType = ESlotChargingEventType::PlantBomb
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [3]
                  EventType = ESlotChargingEventType::PlantBombPersonal
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [4]
                  EventType = ESlotChargingEventType::BombDefusal
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [5]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [6]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
          CheckSubMode = True
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [1]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 3008002
    allSerializedFields
      SkillID = 3008002
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008002_SkillDisplayName
        SourceString = 破片手雷
        LocalizedString = 破片手雷
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1008001_ShortName
        SourceString = 伤害
        LocalizedString = 伤害
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 14
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::RightHandSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_ActiveSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_05.Scout_Skill_Icon_05
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0005.HUD_Controller_Icon_0005
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon_Mobile/BakedSprite/ArmsCommon_Skill_Icon_05_m.ArmsCommon_Skill_Icon_05_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0005.HUD_Controller_Icon_0005
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 92
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanReload
        [1]
          ECharacterStateMask::CanAutoChangeClip
        [2]
          ECharacterStateMask::CanAimOn
        [3]
          ECharacterStateMask::CanWeaponInspect
        [4]
          ECharacterStateMask::CanHighValueItemInspect
        [5]
          ECharacterStateMask::CanWeaponBipodDeploy
        [6]
          ECharacterStateMask::CanSwitchHelmetMask
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = True
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = True
      ShortPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008002_ShortPressTipsDetailText
        SourceString = 点按开火按键快速投掷手雷
        LocalizedString = 点按开火按键快速投掷手雷
      LongPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008002_LongPressTipsDetailText
        SourceString = 长按开火按键可提前开始引信计时，松开按键投掷
        LocalizedString = 长按开火按键可提前开始引信计时，松开按键投掷
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21010000001
      WeaponAttachPos = 1135
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_10
                  SourceString = 投掷
                  LocalizedString = 投掷
                bOverrideDesc = True
              [1]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_11
                  SourceString = （长按）消耗引信
                  LocalizedString = （长按）消耗引信
                bOverrideDesc = True
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginKeepPreFire
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_11
                  SourceString = （长按）消耗引信
                  LocalizedString = （长按）消耗引信
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginKeepPreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanAssassinate
              [1]
                ECharacterStateMask::CanUseZipline
              [2]
                ECharacterStateMask::CanRaiseWatch
              [3]
                ECharacterStateMask::CanShowInteractUI
              [4]
                ECharacterStateMask::CanParachute
              [5]
                ECharacterStateMask::CanVault
              [6]
                ECharacterStateMask::CanClimbLadder
              [7]
                ECharacterStateMask::CanLooting
              [8]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [2]
          Key = EGPWeaponInnerEvent::EStateEvent_PreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [3]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            LockAbility
              (empty array)
            UnLockAbility
              [0]
                ECharacterStateMask::CanAssassinate
              [1]
                ECharacterStateMask::CanUseZipline
              [2]
                ECharacterStateMask::CanRaiseWatch
              [3]
                ECharacterStateMask::CanShowInteractUI
              [4]
                ECharacterStateMask::CanParachute
              [5]
                ECharacterStateMask::CanVault
              [6]
                ECharacterStateMask::CanClimbLadder
              [7]
                ECharacterStateMask::CanLooting
              [8]
                ECharacterStateMask::CanDive
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 92
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 破片手雷露娜
        field = CdID
        value
          CdID = 92
          Slot = ECharingSlot::ArmdedForceItem2
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 2
              Max = 2
              Time = 55.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 1
              Max = 1
              Time = 0.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 1
              AmmoPackChargeCnt = 1
              SceneAmmoBoxChargeCnt = 1
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 1
              Max = 1
              Time = 0.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 1
              AmmoPackChargeCnt = 1
              SceneAmmoBoxChargeCnt = 1
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 2
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [2]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4008002
    allSerializedFields
      SkillID = 4008002
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008002_SkillDisplayName
        SourceString = 电击箭矢
        LocalizedString = 电击箭矢
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008002_ShortName
        SourceString = 电击
        LocalizedString = 电击
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = /Game/UI/UIAtlas/Common/BakedSprite/Common_Arms_Icon_0001.Common_Arms_Icon_0001
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = True
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 14
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_SupportSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_04.Scout_Skill_Icon_04
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_04.Scout_Skill_Icon_04
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0001.HUD_Controller_Icon_0001
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts_Mobile/BakedSprite/Scout_Skill_Icon_04_m.Scout_Skill_Icon_04_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0001.HUD_Controller_Icon_0001
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = True
      CDID = 13
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        [0]
          ECharacterAudioType::Ability_VO10
        [1]
          ECharacterAudioType::Ability_VO11
        [2]
          ECharacterAudioType::Ability_VO17
      VosList
        (empty array)
      ActivatedWeaponSlot = 1136
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Selected
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanCarrybody
        [1]
          ECharacterStateMask::CanSlidingTackle
        [2]
          ECharacterStateMask::CanClimbLadder
        [3]
          ECharacterStateMask::CanWeaponInspect
        [4]
          ECharacterStateMask::CanPlayTacticalAction
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanHighValueItemInspect
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_UseItem
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 10130000875
      WeaponAttachPos = 1136
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::Left
        ModelConflict = EGPWeaponAttachBackConflict::LunaDoubleBow
        AttachBackRelativeTransform
          Rotation
            X = 0.707107
            Y = 0.0
            Z = 0.0
            W = 0.707107
            IsNormalized = True
            Size = 1.0000002
            SizeSquared = 1.0000006
          Translation
            X = 0.0
            Y = 15.0
            Z = 20.0
          Scale3D
            X = 0.8
            Y = 0.8
            Z = 0.8
        AttachBagRelativeTransform
          Rotation
            X = -0.707107
            Y = -0.0
            Z = 0.0
            W = 0.707107
            IsNormalized = True
            Size = 1.0000002
            SizeSquared = 1.0000006
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 0.8
            Y = 0.8
            Z = 0.8
        DisabledAttachBackDeviceLevels
          [0]
            0
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
              [1]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_7
                  SourceString = （长按）蓄力
                  LocalizedString = （长按）蓄力
                bOverrideDesc = True
              [2]
                Key = ToggleAiming
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_8
                  SourceString = 切换反弹
                  LocalizedString = 切换反弹
                bOverrideDesc = True
        [1]
          Key = EGPWeaponInnerEvent::EInnerEvent_Charging
          Value
            TipsDataList
              [0]
                Key = Reload
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_9
                  SourceString = 取消蓄力
                  LocalizedString = 取消蓄力
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EInnerEvent_Charging
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanParachute
              [1]
                ECharacterStateMask::CanDetectRescue
              [2]
                ECharacterStateMask::CanHighValueItemInspect
              [3]
                ECharacterStateMask::CanOperate
              [4]
                ECharacterStateMask::CanUseActionSkill
              [5]
                ECharacterStateMask::CanUseZipline
              [6]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_EndFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanUseItem
            UnLockAbility
              [0]
                ECharacterStateMask::CanParachute
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [2]
          Key = EGPWeaponInnerEvent::EInnerEvent_CancelCharge
          Value
            LockAbility
              (empty array)
            UnLockAbility
              [0]
                ECharacterStateMask::CanParachute
              [1]
                ECharacterStateMask::CanDetectRescue
              [2]
                ECharacterStateMask::CanHighValueItemInspect
              [3]
                ECharacterStateMask::CanOperate
              [4]
                ECharacterStateMask::CanUseActionSkill
              [5]
                ECharacterStateMask::CanUseZipline
              [6]
                ECharacterStateMask::CanDive
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [3]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
              [1]
                ECharacterStateMask::CanDetectRescue
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 13
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 13
        field = CdID
        value
          CdID = 13
          Slot = ECharingSlot::ArmdedForceItem1
          WeaponSlot = 1136
          SlotChargingData
            [0]
              Init = 1
              Max = 1
              Time = 50.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 1
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 0
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 1
              Max = 1
              Time = 50.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [3]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4008007
    allSerializedFields
      SkillID = 4008007
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_SkillDisplayName
        SourceString = 拔箭
        LocalizedString = 拔箭
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_ShortName
        SourceString = 拔箭
        LocalizedString = 拔箭
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          3012006
        [1]
          4008009
      AddSkills_Sol
        [0]
          4008008
        [1]
          6010100
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 23
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityIns_Scout_PullOutArrow.BP_AbilityIns_Scout_PullOutArrow_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityTemp_Scout_PullOutArrow.BP_AbilityTemp_Scout_PullOutArrow_C
        SubPathString = 
      SkillStateType = ESkillStateType::Custom2Skill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = False
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = False
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanHighValueItemInspect
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanPlayTacticalAction
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanUseZipline
        [8]
          ECharacterStateMask::CanDetectInteractor
        [9]
          ECharacterStateMask::CanLooting
        [10]
          ECharacterStateMask::CanShowInteractUI
        [11]
          ECharacterStateMask::CanSkillSwtichWeapon
        [12]
          ECharacterStateMask::CanDetectRescue
        [13]
          ECharacterStateMask::CanUseMachineGun
        [14]
          ECharacterStateMask::CanLean
        [15]
          ECharacterStateMask::CanReload
        [16]
          ECharacterStateMask::CanWeaponBipodDeploy
        [17]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [18]
          ECharacterStateMask::CanOperate
        [19]
          ECharacterStateMask::CanSwitchHelmetMask
        [20]
          ECharacterStateMask::CanUseTelescope
        [21]
          ECharacterStateMask::CanChangeWeaponMode
        [22]
          ECharacterStateMask::CanParachute
        [23]
          ECharacterStateMask::CanAimOn
        [24]
          ECharacterStateMask::CanFire
        [25]
          ECharacterStateMask::CanSprint
        [26]
          ECharacterStateMask::CanPlayMarkingAnim
        [27]
          ECharacterStateMask::CanApplySpray
        [28]
          ECharacterStateMask::CanFirefighting
        [29]
          ECharacterStateMask::CanPlayOneHandedGesture
        [30]
          ECharacterStateMask::CanPlayTwoHandedGesture
        [31]
          ECharacterStateMask::CanBeBreakByOtherSkill
        [32]
          ECharacterStateMask::CanVault
        [33]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_CommonLeftArmAnim
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = 0
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.0
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [4]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 30010203
    allSerializedFields
      SkillID = 30010203
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_SkillDisplayName
        SourceString = 工具箱
        LocalizedString = 工具箱
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_ShortName
        SourceString = 工具箱
        LocalizedString = 工具箱
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 31
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = False
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_Custom
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon/BakedSprite/ArmsCommon_Skill_Icon_12.ArmsCommon_Skill_Icon_12
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_23_m.Engineer_Skill_Icon_23_m
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon_Mobile/BakedSprite/ArmsCommon_Skill_Icon_12_m.ArmsCommon_Skill_Icon_12_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 1031
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = True
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanVault
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21020300011
      WeaponAttachPos = 1133
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 1031
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 工具箱
        field = CdID
        value
          CdID = 1031
          Slot = ECharingSlot::ArmdedForceItem1Ready
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 99
              Max = 99
              Time = 0.1
              UseTime = 0.0
              ChargeAmmoReplenish = 99
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
                [1]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 99
              MaxClipAmmoNum = 99
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [5]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 40010201
    allSerializedFields
      SkillID = 40010201
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 40010201_SkillDisplayName
        SourceString = 敌情追踪
        LocalizedString = 敌情追踪
      ShortName
        CultureInvariantString = None
      LogicType = ESkillLogicType::Scout
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        [0]
          8011
        [1]
          100008
      ConflictID = 0
      FlowDataType = EFlowDataType::Passive
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = True
      bEnableSniperMark = True
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED

Vyron
requestedEnglishLabel = Vyron
EnglishMappingStatus = CALLER_CONFIRMED_OFFICIAL_MAPPING_VYRON_EQUALS_威龙_NOT_ARCHIVE_ENGLISH_LOCALIZATION
heroPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Hero/HeroData.uasset
heroRow = 88000000025
heroName
  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_HeroData.LOC_HeroData
  Key = 88000000025_Name
  SourceString = 威龙
  LocalizedString = 威龙
ExpertID = 10010
expertPackage = DeltaForce/Content/R13N/Global/Base/DataTables/Skill/ExpertSkillData.uasset
expertRow = 10010
allExpertFields
  ExpertID = 10010
  ArmedForceID = 1
  MPUltimateSkillID = 1009003
  MPSupportSkillID = 1009001
  MPPassiveSkillID = 1009004
  MPActiveSkillID = 1009002
  MPCommonPassiveSkillID = 10010301
  MPCustomSkillID = 30010203
  MPCommonSkillID
    [0]
      4008007
  SOLUltimateSkillID = 1009003
  SOLSupportSkillID = 1009001
  SOLPassiveSkillID = 1009004
  SOLActiveSkillID = 1009005
  SOLCommonPassiveSkillID = 10010301
  SOLCommonSkillID
    [0]
      4008007
    [1]
      4008010
  BombDefusal_SkillGroupID = 10010
  RowDescription = None
groupPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Skill/ExpertSkillGroupData.uasset
groupRow = 10010
allGroupFields
  SkillGroupID = 10010
  UltimateSkillID = 1009003
  SupportSkillID = 1009001
  PassiveSkillID = 1009004
  ActiveSkillID = 1009005
  CommonPassiveSkillID = 0
  CustomSkillID = 0
  CommonSkillID
    [0]
      4008007
  RowDescription = None
skillIDs
  [0]
    1009001
  [1]
    1009002
  [2]
    1009003
  [3]
    1009004
  [4]
    1009005
  [5]
    4008007
  [6]
    4008010
  [7]
    10010301
  [8]
    30010203
missingSkillIDs
  [0]
    4008010
skills
  [0]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 1009001
    allSerializedFields
      SkillID = 1009001
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1009001_SkillDisplayName
        SourceString = 动力推进
        LocalizedString = 动力推进
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1009001_ShortName
        SourceString = 推进
        LocalizedString = 推进
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = True
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 0
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/AbilityAdvanced/BP_AbilityInstance_Rush.BP_AbilityInstance_Rush_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/AbilityAdvanced/BP_AbilityTempLate_Rush.BP_AbilityTemplate_Rush_C
        SubPathString = 
      SkillStateType = ESkillStateType::LegsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          [0]
            RushActionF
          [1]
            RushActionB
          [2]
            RushActionL
          [3]
            RushActionR
          [4]
            RushActionFL
          [5]
            RushActionFR
          [6]
            RushActionBL
          [7]
            RushActionBR
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          [0]
            AssetPathName = /Game/WwiseAudio/GPEvents/Classes/Classes_Ability/Classes_Ability_C302/Classes_C302_Skill/C302_Skill_JetSystem_Char_Boost.C302_Skill_JetSystem_Char_Boost
            SubPathString = 
          [1]
            AssetPathName = /Game/WwiseAudio/GPEvents/Classes/Classes_Ability/Classes_Ability_C302_Outers_1u/Classes_C302_Outers_1u_Skill/C302_Outers_1u_Skill_JetSystem_Char_Boost.C302_Outers_1u_Skill_JetSystem_Char_Boost
            SubPathString = 
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = False
      bCanCastOnWaterSurface = False
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = True
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_SupportSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando/BakedSprite/Commando_Skill_Icon_09.Commando_Skill_Icon_09
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando_Mobile/BakedSprite/Commando_Skill_Icon_09_m.Commando_Skill_Icon_09_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 18
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.5
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        [0]
          ECharacterAudioType::Ability_VO7
        [1]
          ECharacterAudioType::Ability_VO8
        [2]
          ECharacterAudioType::Ability_VO9
      VosList
        (empty array)
      ActivatedWeaponSlot = 1136
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanParachute
        [1]
          ECharacterStateMask::CanMove
        [2]
          ECharacterStateMask::CanCrouch
        [3]
          ECharacterStateMask::CanProne
        [4]
          ECharacterStateMask::CanJump
        [5]
          ECharacterStateMask::CanHighValueItemInspect
        [6]
          ECharacterStateMask::CanWeaponInspect
        [7]
          ECharacterStateMask::CanWeaponBolt
        [8]
          ECharacterStateMask::CanWeaponBipodDeploy
        [9]
          ECharacterStateMask::CanSkillSwtichWeapon
        [10]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [11]
          ECharacterStateMask::CanUseMachineGun
        [12]
          ECharacterStateMask::CanUseItem
        [13]
          ECharacterStateMask::CanOperate
        [14]
          ECharacterStateMask::CanLooting
        [15]
          ECharacterStateMask::CanDetectInteractor
        [16]
          ECharacterStateMask::CanDetectRescue
        [17]
          ECharacterStateMask::CanUserChangeXPP
        [18]
          ECharacterStateMask::CanSwitchHelmetMask
        [19]
          ECharacterStateMask::CanDrive
        [20]
          ECharacterStateMask::CanClimbLadder
        [21]
          ECharacterStateMask::CanUseTelescope
        [22]
          ECharacterStateMask::CanLean
        [23]
          ECharacterStateMask::CanShowInteractUI
        [24]
          ECharacterStateMask::CanUseZipline
        [25]
          ECharacterStateMask::CanProneStandUp
        [26]
          ECharacterStateMask::CanAssassinate
        [27]
          ECharacterStateMask::CanVault
        [28]
          ECharacterStateMask::CanDive
        [29]
          ECharacterStateMask::CanFire
        [30]
          ECharacterStateMask::CanAimOn
        [31]
          ECharacterStateMask::CanAimOff
        [32]
          ECharacterStateMask::CanAutoChangeClip
        [33]
          ECharacterStateMask::CanReload
        [34]
          ECharacterStateMask::CanPlayEmote
        [35]
          ECharacterStateMask::CanPlayMarkingAnim
        [36]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        [0]
          ECharacterStateMask::CanCharacterRotate
        [1]
          ECharacterStateMask::CanMove
        [2]
          ECharacterStateMask::CanRotate
        [3]
          ECharacterStateMask::CanAimOff
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_CarryBody
        [2]
          EFSMCharacterState::FSMPlayerState_Rescue
        [3]
          EFSMCharacterState::FSMPlayerState_BeingRescue
        [4]
          EFSMCharacterState::FSMPlayerState_Interact
        [5]
          EFSMCharacterState::FSMPlayerState_Vehicle
        [6]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [7]
          EFSMCharacterState::FSMPlayerState_ImpedingDeath
        [8]
          EFSMCharacterState::FSMPlayerState_Death
        [9]
          EFSMCharacterState::FSMPlayerState_CutScene
        [10]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [11]
          EFSMCharacterState::FSMPlayerState_PreciseInteract
        [12]
          EFSMCharacterState::FSMPlayerState_DelayInteract
        [13]
          EFSMCharacterState::FSMPlayerState_GameOperate
        [14]
          EFSMCharacterState::FSMPlayerState_Dive
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_Prone
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 1136
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = True
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.4
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 18
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 动力推进
        field = CdID
        value
          CdID = 18
          Slot = ECharingSlot::ArmdedForceItem1
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 0
              Max = 1
              Time = 20.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Intro
                [2]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 1
              Max = 1
              Time = 15.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 1
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 1
              Time = 20.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [1]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 1009002
    allSerializedFields
      SkillID = 1009002
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1009002_SkillDisplayName
        SourceString = 磁吸炸弹
        LocalizedString = 磁吸炸弹
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1008001_ShortName
        SourceString = 伤害
        LocalizedString = 伤害
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 11
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          [0]
            Outers_1u_C4_Inspect
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = True
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_ActiveSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando/BakedSprite/Commando_Skill_Icon_11.Commando_Skill_Icon_11
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando_Mobile/BakedSprite/Commando_Skill_Icon_11_m.Commando_Skill_Icon_11_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando_Mobile/BakedSprite/Commando_Skill_Icon_25_m.Commando_Skill_Icon_25_m
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 16
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 1.0
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = True
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanDetectRescue
        [1]
          ECharacterStateMask::CanReload
        [2]
          ECharacterStateMask::CanAutoChangeClip
        [3]
          ECharacterStateMask::CanAimOn
        [4]
          ECharacterStateMask::CanLooting
        [5]
          ECharacterStateMask::DisableWhitelistOperate
        [6]
          ECharacterStateMask::CanCarrybody
        [7]
          ECharacterStateMask::CanWeaponInspect
        [8]
          ECharacterStateMask::CanHighValueItemInspect
        [9]
          ECharacterStateMask::CanWeaponBipodDeploy
        [10]
          ECharacterStateMask::CanSwitchHelmetMask
        [11]
          ECharacterStateMask::CanAssassinate
        [12]
          ECharacterStateMask::CanParachute
        [13]
          ECharacterStateMask::CanOperate
        [14]
          ECharacterStateMask::CanShowInteractUI
        [15]
          ECharacterStateMask::CanRaiseWatch
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        [0]
          ECharacterStateMask::CanDive
        [1]
          ECharacterStateMask::CanLooting
        [2]
          ECharacterStateMask::CanOperate
        [3]
          ECharacterStateMask::CanUseItem
        [4]
          ECharacterStateMask::CanDetectInteractor
        [5]
          ECharacterStateMask::CanPlayTacticalAction
        [6]
          ECharacterStateMask::CanShowInteractUI
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        [0]
          ECharacterStateMask::CanRaiseWatch
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_Interact
        [1]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [2]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [3]
          EFSMCharacterState::FSMPlayerState_Dive
        [4]
          EFSMCharacterState::FSMPlayerState_SpecialMove
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = True
      DefaultThrowMode = EThrowMode::DirectThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21010000009
      WeaponAttachPos = 1135
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::PutQuietAndGetWeaponFast
      bHoldToFire = True
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanUseItem
              [1]
                ECharacterStateMask::CanLooting
              [2]
                ECharacterStateMask::CanVault
              [3]
                ECharacterStateMask::CanWeaponInspect
              [4]
                ECharacterStateMask::CanCarrybody
              [5]
                ECharacterStateMask::CanHighValueItemInspect
              [6]
                ECharacterStateMask::CanUseZipline
              [7]
                ECharacterStateMask::CanClimbLadder
              [8]
                ECharacterStateMask::CanPlayTacticalAction
              [9]
                ECharacterStateMask::CanUseSpecialSkill
              [10]
                ECharacterStateMask::CanDive
              [11]
                ECharacterStateMask::CanSwitchWeapon
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_PreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanUseItem
              [1]
                ECharacterStateMask::CanLooting
              [2]
                ECharacterStateMask::CanVault
              [3]
                ECharacterStateMask::CanWeaponInspect
              [4]
                ECharacterStateMask::CanCarrybody
              [5]
                ECharacterStateMask::CanHighValueItemInspect
              [6]
                ECharacterStateMask::CanUseZipline
              [7]
                ECharacterStateMask::CanClimbLadder
              [8]
                ECharacterStateMask::CanPlayTacticalAction
              [9]
                ECharacterStateMask::CanUseSpecialSkill
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060007
          Value = Outers_1u_C4_Inspect
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = True
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 16
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 磁吸炸弹
        field = CdID
        value
          CdID = 16
          Slot = ECharingSlot::ArmdedForceItem2
          WeaponSlot = 1135
          SlotChargingData
            [0]
              Init = 0
              Max = 0
              Time = 55.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 0
              Time = 0.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 1
              AmmoPackChargeCnt = 1
              SceneAmmoBoxChargeCnt = 1
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 0
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 0
              Time = 55.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [2]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 1009003
    allSerializedFields
      SkillID = 1009003
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1009003_SkillDisplayName
        SourceString = 虎蹲炮
        LocalizedString = 虎蹲炮
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1009003_ShortName
        SourceString = 击倒
        LocalizedString = 击倒
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          5000001
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 14
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/AbilityAdvanced/BP_AbilityTemp_1009003.BP_AbilityTemp_1009003_C
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          [0]
            Outers_1u_AirCannon_Inspect
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = True
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_FireButtonLeft
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando/BakedSprite/Commando_Skill_Icon_07.Commando_Skill_Icon_07
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando/BakedSprite/Commando_Skill_Icon_08.Commando_Skill_Icon_08
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0001.HUD_Controller_Icon_0001
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando_Mobile/BakedSprite/Commando_Skill_Icon_07_m.Commando_Skill_Icon_07_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0001.HUD_Controller_Icon_0001
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = True
      CDID = 17
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        [0]
          ECharacterAudioType::Ability_VO7
        [1]
          ECharacterAudioType::Ability_VO8
        [2]
          ECharacterAudioType::Ability_VO9
      VosList
        (empty array)
      ActivatedWeaponSlot = 130
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = False
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanSlidingTackle
        [1]
          ECharacterStateMask::CanDetectInteractor
        [2]
          ECharacterStateMask::CanShowInteractUI
        [3]
          ECharacterStateMask::CanWeaponInspect
        [4]
          ECharacterStateMask::CanPlayTacticalAction
        [5]
          ECharacterStateMask::CanHighValueItemInspect
        [6]
          ECharacterStateMask::None
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_UseItem
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 10080000855
      WeaponAttachPos = 1134
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::Right
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = -0.106339
            Y = 0.249515
            Z = 0.106339
            W = 0.956622
            IsNormalized = True
            Size = 0.99999964
            SizeSquared = 0.99999934
          Translation
            X = -3.0
            Y = 0.0
            Z = -3.0
          Scale3D
            X = 0.9
            Y = 0.9
            Z = 0.9
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = -0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 0.9
            Y = 0.9
            Z = 0.9
        DisabledAttachBackDeviceLevels
          [0]
            0
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::PutWeaponQuiet
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060007
          Value = Outers_1u_AirCannon_Inspect
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 17
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 虎蹲炮
        field = CdID
        value
          CdID = 17
          Slot = ECharingSlot::Ability
          WeaponSlot = 1134
          SlotChargingData
            [0]
              Init = 0
              Max = 0
              Time = 70.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_Intro
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 0
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 13500.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 0
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_Arena
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 0
              Time = 70.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 3
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                [0]
                  EventType = ESlotChargingEventType::RoundStart
                  PercentRecover = 0.34
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [1]
                  EventType = ESlotChargingEventType::RoudnOTStart
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [2]
                  EventType = ESlotChargingEventType::PlantBomb
                  PercentRecover = 0.34
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [3]
                  EventType = ESlotChargingEventType::PlantBombPersonal
                  PercentRecover = 0.34
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [4]
                  EventType = ESlotChargingEventType::BombDefusal
                  PercentRecover = 0.34
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [5]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.34
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [6]
                  EventType = ESlotChargingEventType::Dead
                  PercentRecover = 0.34
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
            [4]
              Init = 0
              Max = 0
              Time = 70.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 1
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = True
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [3]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 1009004
    allSerializedFields
      SkillID = 1009004
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1009004_SkillDisplayName
        SourceString = 动能辅助系统
        LocalizedString = 动能辅助系统
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 10010101_ShortName
        SourceString = 加速
        LocalizedString = 加速
      LogicType = ESkillLogicType::Assault
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          1009001
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        [0]
          303016
        [1]
          8013
      ConflictID = 0
      FlowDataType = EFlowDataType::Passive
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 1.0
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = True
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [4]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 1009005
    allSerializedFields
      SkillID = 1009005
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1009002_SkillDisplayName
        SourceString = 磁吸炸弹
        LocalizedString = 磁吸炸弹
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1008001_ShortName
        SourceString = 伤害
        LocalizedString = 伤害
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 11
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          [0]
            Outers_1u_C4_Inspect
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = True
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_ActiveSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando/BakedSprite/Commando_Skill_Icon_11.Commando_Skill_Icon_11
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Commando_Mobile/BakedSprite/Commando_Skill_Icon_11_m.Commando_Skill_Icon_11_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 16
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 1.0
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = True
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanDetectRescue
        [1]
          ECharacterStateMask::CanReload
        [2]
          ECharacterStateMask::CanAutoChangeClip
        [3]
          ECharacterStateMask::CanAimOn
        [4]
          ECharacterStateMask::CanLooting
        [5]
          ECharacterStateMask::DisableWhitelistOperate
        [6]
          ECharacterStateMask::CanCarrybody
        [7]
          ECharacterStateMask::CanWeaponInspect
        [8]
          ECharacterStateMask::CanHighValueItemInspect
        [9]
          ECharacterStateMask::CanWeaponBipodDeploy
        [10]
          ECharacterStateMask::CanSwitchHelmetMask
        [11]
          ECharacterStateMask::CanAssassinate
        [12]
          ECharacterStateMask::CanOperate
        [13]
          ECharacterStateMask::CanShowInteractUI
        [14]
          ECharacterStateMask::CanRaiseWatch
      UnLockStateAbilitys
        [0]
          ECharacterStateMask::CanAimOff
      LockStateAbilitys_DirectThrow
        [0]
          ECharacterStateMask::CanDive
        [1]
          ECharacterStateMask::CanLooting
        [2]
          ECharacterStateMask::CanOperate
        [3]
          ECharacterStateMask::CanUseItem
        [4]
          ECharacterStateMask::CanDetectInteractor
        [5]
          ECharacterStateMask::CanPlayTacticalAction
        [6]
          ECharacterStateMask::CanShowInteractUI
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        [0]
          ECharacterStateMask::CanRaiseWatch
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [4]
          EFSMCharacterState::FSMPlayerState_Dive
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = True
      DefaultThrowMode = EThrowMode::DirectThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21010000012
      WeaponAttachPos = 1135
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::PutQuietAndGetWeaponFast
      bHoldToFire = True
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanUseItem
              [1]
                ECharacterStateMask::CanLooting
              [2]
                ECharacterStateMask::CanVault
              [3]
                ECharacterStateMask::CanWeaponInspect
              [4]
                ECharacterStateMask::CanCarrybody
              [5]
                ECharacterStateMask::CanHighValueItemInspect
              [6]
                ECharacterStateMask::CanUseZipline
              [7]
                ECharacterStateMask::CanClimbLadder
              [8]
                ECharacterStateMask::CanPlayTacticalAction
              [9]
                ECharacterStateMask::CanUseSpecialSkill
              [10]
                ECharacterStateMask::CanDive
              [11]
                ECharacterStateMask::CanSwitchWeapon
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_PreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanUseItem
              [1]
                ECharacterStateMask::CanLooting
              [2]
                ECharacterStateMask::CanVault
              [3]
                ECharacterStateMask::CanWeaponInspect
              [4]
                ECharacterStateMask::CanCarrybody
              [5]
                ECharacterStateMask::CanHighValueItemInspect
              [6]
                ECharacterStateMask::CanUseZipline
              [7]
                ECharacterStateMask::CanClimbLadder
              [8]
                ECharacterStateMask::CanPlayTacticalAction
              [9]
                ECharacterStateMask::CanUseSpecialSkill
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060007
          Value = Outers_1u_C4_Inspect
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = True
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 16
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 磁吸炸弹
        field = CdID
        value
          CdID = 16
          Slot = ECharingSlot::ArmdedForceItem2
          WeaponSlot = 1135
          SlotChargingData
            [0]
              Init = 0
              Max = 0
              Time = 55.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 0
              Time = 0.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 1
              AmmoPackChargeCnt = 1
              SceneAmmoBoxChargeCnt = 1
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 0
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 0
              Time = 55.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [5]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4008007
    allSerializedFields
      SkillID = 4008007
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_SkillDisplayName
        SourceString = 拔箭
        LocalizedString = 拔箭
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_ShortName
        SourceString = 拔箭
        LocalizedString = 拔箭
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          3012006
        [1]
          4008009
      AddSkills_Sol
        [0]
          4008008
        [1]
          6010100
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 23
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityIns_Scout_PullOutArrow.BP_AbilityIns_Scout_PullOutArrow_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityTemp_Scout_PullOutArrow.BP_AbilityTemp_Scout_PullOutArrow_C
        SubPathString = 
      SkillStateType = ESkillStateType::Custom2Skill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = False
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = False
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanHighValueItemInspect
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanPlayTacticalAction
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanUseZipline
        [8]
          ECharacterStateMask::CanDetectInteractor
        [9]
          ECharacterStateMask::CanLooting
        [10]
          ECharacterStateMask::CanShowInteractUI
        [11]
          ECharacterStateMask::CanSkillSwtichWeapon
        [12]
          ECharacterStateMask::CanDetectRescue
        [13]
          ECharacterStateMask::CanUseMachineGun
        [14]
          ECharacterStateMask::CanLean
        [15]
          ECharacterStateMask::CanReload
        [16]
          ECharacterStateMask::CanWeaponBipodDeploy
        [17]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [18]
          ECharacterStateMask::CanOperate
        [19]
          ECharacterStateMask::CanSwitchHelmetMask
        [20]
          ECharacterStateMask::CanUseTelescope
        [21]
          ECharacterStateMask::CanChangeWeaponMode
        [22]
          ECharacterStateMask::CanParachute
        [23]
          ECharacterStateMask::CanAimOn
        [24]
          ECharacterStateMask::CanFire
        [25]
          ECharacterStateMask::CanSprint
        [26]
          ECharacterStateMask::CanPlayMarkingAnim
        [27]
          ECharacterStateMask::CanApplySpray
        [28]
          ECharacterStateMask::CanFirefighting
        [29]
          ECharacterStateMask::CanPlayOneHandedGesture
        [30]
          ECharacterStateMask::CanPlayTwoHandedGesture
        [31]
          ECharacterStateMask::CanBeBreakByOtherSkill
        [32]
          ECharacterStateMask::CanVault
        [33]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_CommonLeftArmAnim
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = 0
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.0
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [6]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 10010301
    allSerializedFields
      SkillID = 10010301
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 10010301_SkillDisplayName
        SourceString = 敏捷射手
        LocalizedString = 敏捷射手
      ShortName
        CultureInvariantString = None
      LogicType = ESkillLogicType::Assault
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 1
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        [0]
          303054
      ConflictID = 0
      FlowDataType = EFlowDataType::Passive
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [7]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 30010203
    allSerializedFields
      SkillID = 30010203
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_SkillDisplayName
        SourceString = 工具箱
        LocalizedString = 工具箱
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_ShortName
        SourceString = 工具箱
        LocalizedString = 工具箱
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 31
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = False
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_Custom
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon/BakedSprite/ArmsCommon_Skill_Icon_12.ArmsCommon_Skill_Icon_12
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_23_m.Engineer_Skill_Icon_23_m
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon_Mobile/BakedSprite/ArmsCommon_Skill_Icon_12_m.ArmsCommon_Skill_Icon_12_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 1031
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = True
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanVault
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21020300011
      WeaponAttachPos = 1133
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 1031
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 工具箱
        field = CdID
        value
          CdID = 1031
          Slot = ECharingSlot::ArmdedForceItem1Ready
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 99
              Max = 99
              Time = 0.1
              UseTime = 0.0
              ChargeAmmoReplenish = 99
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
                [1]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 99
              MaxClipAmmoNum = 99
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED

Hackclaw
requestedEnglishLabel = Hackclaw
EnglishMappingStatus = UNVERIFIED_CHINESE_ARCHIVE_CANDIDATE_ONLY
heroPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Hero/HeroData.uasset
heroRow = 88000000026
heroName
  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_HeroData.LOC_HeroData
  Key = 88000000026_Name
  SourceString = 骇爪
  LocalizedString = 骇爪
ExpertID = 40010
expertPackage = DeltaForce/Content/R13N/Global/Base/DataTables/Skill/ExpertSkillData.uasset
expertRow = 40010
allExpertFields
  ExpertID = 40010
  ArmedForceID = 4
  MPUltimateSkillID = 1002016
  MPSupportSkillID = 4009003
  MPPassiveSkillID = 40010202
  MPActiveSkillID = 4009002
  MPCommonPassiveSkillID = 0
  MPCustomSkillID = 30010203
  MPCommonSkillID
    [0]
      4008007
  SOLUltimateSkillID = 1002016
  SOLSupportSkillID = 4009003
  SOLPassiveSkillID = 40010202
  SOLActiveSkillID = 4009002
  SOLCommonPassiveSkillID = 0
  SOLCommonSkillID
    [0]
      4008007
    [1]
      4008010
  BombDefusal_SkillGroupID = 40010
  RowDescription = None
groupPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Skill/ExpertSkillGroupData.uasset
groupRow = 40010
allGroupFields
  SkillGroupID = 40010
  UltimateSkillID = 1002016
  SupportSkillID = 4009003
  PassiveSkillID = 40010202
  ActiveSkillID = 4009002
  CommonPassiveSkillID = 0
  CustomSkillID = 0
  CommonSkillID
    [0]
      4008007
  RowDescription = None
skillIDs
  [0]
    1002016
  [1]
    4008007
  [2]
    4008010
  [3]
    4009002
  [4]
    4009003
  [5]
    30010203
  [6]
    40010202
missingSkillIDs
  [0]
    4008010
skills
  [0]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 1002016
    allSerializedFields
      SkillID = 1002016
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1002016_SkillDisplayName
        SourceString = 信号破译器
        LocalizedString = 信号破译器
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1002016_ShortName
        SourceString = 追踪
        LocalizedString = 追踪
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          5000001
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 0
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C202/BP_AbilityIns_Scout_C202_PathTracing.BP_AbilityIns_Scout_C202_PathTracing_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C202/BP_AbilityTemp_Scout_C202_PathTracing.BP_AbilityTemp_Scout_C202_PathTracing_C
        SubPathString = 
      SkillStateType = ESkillStateType::LegsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = True
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_FireButtonLeft
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_09.Scout_Skill_Icon_09
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts_Mobile/BakedSprite/Scout_Skill_Icon_09_m.Scout_Skill_Icon_09_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 22
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        [0]
          Skill_Ultimate_Use
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        [0]
          100018
        [1]
          100030
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanHighValueItemInspect
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanPlayTacticalAction
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanUseZipline
        [8]
          ECharacterStateMask::CanDetectInteractor
        [9]
          ECharacterStateMask::CanLooting
        [10]
          ECharacterStateMask::CanShowInteractUI
        [11]
          ECharacterStateMask::CanDetectRescue
        [12]
          ECharacterStateMask::CanUseMachineGun
        [13]
          ECharacterStateMask::CanLean
        [14]
          ECharacterStateMask::CanReload
        [15]
          ECharacterStateMask::CanWeaponBipodDeploy
        [16]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [17]
          ECharacterStateMask::CanOperate
        [18]
          ECharacterStateMask::CanSwitchHelmetMask
        [19]
          ECharacterStateMask::CanUseTelescope
        [20]
          ECharacterStateMask::CanChangeWeaponMode
        [21]
          ECharacterStateMask::CanParachute
        [22]
          ECharacterStateMask::CanAimOn
        [23]
          ECharacterStateMask::CanVault
        [24]
          ECharacterStateMask::CanCarrybody
        [25]
          ECharacterStateMask::CanSwitchWeapon
        [26]
          ECharacterStateMask::CanAutoChangeClip
        [27]
          ECharacterStateMask::CanWeaponInspect
        [28]
          ECharacterStateMask::CanWeaponBolt
        [29]
          ECharacterStateMask::CanFire
        [30]
          ECharacterStateMask::CanFirefighting
        [31]
          ECharacterStateMask::CanPlayEmote
        [32]
          ECharacterStateMask::CanPlayMarkingAnim
        [33]
          ECharacterStateMask::CanUseHelmet
        [34]
          ECharacterStateMask::CanPlayOneHandedGesture
        [35]
          ECharacterStateMask::CanPlayTwoHandedGesture
        [36]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_Vehicle
        [3]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [4]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = DeciphererInspect_S
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = True
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 22
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 信号破译器
        field = CdID
        value
          CdID = 22
          Slot = ECharingSlot::Ability
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 0
              Max = 1
              Time = 90.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 1
              Time = 70.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 13500.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 1
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_Arena
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 1
              Time = 120.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 6
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                [0]
                  EventType = ESlotChargingEventType::RoundStart
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [1]
                  EventType = ESlotChargingEventType::RoudnOTStart
                  PercentRecover = 0.835
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [2]
                  EventType = ESlotChargingEventType::PlantBomb
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [3]
                  EventType = ESlotChargingEventType::PlantBombPersonal
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [4]
                  EventType = ESlotChargingEventType::BombDefusal
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [5]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [6]
                  EventType = ESlotChargingEventType::Dead
                  PercentRecover = 0.167
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
            [4]
              Init = 0
              Max = 1
              Time = 120.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = Skill_Ultimate_Charged
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                [0]
                  EventType = ESlotChargingEventType::RoundStart
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [1]
                  EventType = ESlotChargingEventType::RoudnOTStart
                  PercentRecover = 1.0
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [2]
                  EventType = ESlotChargingEventType::PlantBomb
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [3]
                  EventType = ESlotChargingEventType::PlantBombPersonal
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [4]
                  EventType = ESlotChargingEventType::BombDefusal
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [5]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [6]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.2
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
          CheckSubMode = True
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [1]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4008007
    allSerializedFields
      SkillID = 4008007
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_SkillDisplayName
        SourceString = 拔箭
        LocalizedString = 拔箭
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_ShortName
        SourceString = 拔箭
        LocalizedString = 拔箭
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          3012006
        [1]
          4008009
      AddSkills_Sol
        [0]
          4008008
        [1]
          6010100
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 23
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityIns_Scout_PullOutArrow.BP_AbilityIns_Scout_PullOutArrow_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityTemp_Scout_PullOutArrow.BP_AbilityTemp_Scout_PullOutArrow_C
        SubPathString = 
      SkillStateType = ESkillStateType::Custom2Skill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = False
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = False
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanHighValueItemInspect
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanPlayTacticalAction
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanUseZipline
        [8]
          ECharacterStateMask::CanDetectInteractor
        [9]
          ECharacterStateMask::CanLooting
        [10]
          ECharacterStateMask::CanShowInteractUI
        [11]
          ECharacterStateMask::CanSkillSwtichWeapon
        [12]
          ECharacterStateMask::CanDetectRescue
        [13]
          ECharacterStateMask::CanUseMachineGun
        [14]
          ECharacterStateMask::CanLean
        [15]
          ECharacterStateMask::CanReload
        [16]
          ECharacterStateMask::CanWeaponBipodDeploy
        [17]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [18]
          ECharacterStateMask::CanOperate
        [19]
          ECharacterStateMask::CanSwitchHelmetMask
        [20]
          ECharacterStateMask::CanUseTelescope
        [21]
          ECharacterStateMask::CanChangeWeaponMode
        [22]
          ECharacterStateMask::CanParachute
        [23]
          ECharacterStateMask::CanAimOn
        [24]
          ECharacterStateMask::CanFire
        [25]
          ECharacterStateMask::CanSprint
        [26]
          ECharacterStateMask::CanPlayMarkingAnim
        [27]
          ECharacterStateMask::CanApplySpray
        [28]
          ECharacterStateMask::CanFirefighting
        [29]
          ECharacterStateMask::CanPlayOneHandedGesture
        [30]
          ECharacterStateMask::CanPlayTwoHandedGesture
        [31]
          ECharacterStateMask::CanBeBreakByOtherSkill
        [32]
          ECharacterStateMask::CanVault
        [33]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_CommonLeftArmAnim
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = 0
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.0
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [2]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4009002
    allSerializedFields
      SkillID = 4009002
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4009002_SkillDisplayName
        SourceString = 数据飞刀
        LocalizedString = 数据飞刀
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1008001_ShortName
        SourceString = 伤害
        LocalizedString = 伤害
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 6
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::RightHandSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = True
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_ActiveSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_06.Scout_Skill_Icon_06
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_De_Bullet.HUD_De_Bullet
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts_Mobile/BakedSprite/Scout_Skill_Icon_06_m.Scout_Skill_Icon_06_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_De_Bullet.HUD_De_Bullet
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 20
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.5
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanReload
        [1]
          ECharacterStateMask::CanAutoChangeClip
        [2]
          ECharacterStateMask::CanAimOn
        [3]
          ECharacterStateMask::CanLooting
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanHighValueItemInspect
        [6]
          ECharacterStateMask::CanWeaponBipodDeploy
        [7]
          ECharacterStateMask::CanSwitchHelmetMask
        [8]
          ECharacterStateMask::DisableWhitelistOperate
        [9]
          ECharacterStateMask::CanParachute
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [4]
          EFSMCharacterState::FSMPlayerState_Dive
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = True
      DefaultThrowMode = EThrowMode::DirectThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21010000003
      WeaponAttachPos = 1136
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::PutWeaponQuiet
      bHoldToFire = True
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_PreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [2]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginKeepPreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = DataThrowingKnife_S02
      SkillInspectText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4009002_SkillDisplayName
        SourceString = 数据飞刀
        LocalizedString = 数据飞刀
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060002
          Value = DataThrowingKnife_S02
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = True
      bWeaponUseSkillConflict = True
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 20
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 数据飞刀
        field = CdID
        value
          CdID = 20
          Slot = ECharingSlot::ArmdedForceItem2
          WeaponSlot = 1136
          SlotChargingData
            [0]
              Init = 0
              Max = 0
              Time = 40.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 0
              Time = 0.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 2000.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
                [1]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 1
              AmmoPackChargeCnt = 1
              SceneAmmoBoxChargeCnt = 1
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 0
              Time = 40.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_Arena
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 0
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 1
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [4]
              Init = 0
              Max = 0
              Time = 40.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = True
              CurrentAmmoNum = 2
              MaxClipAmmoNum = 2
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = True
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = True
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [3]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4009003
    allSerializedFields
      SkillID = 4009003
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4009003_SkillDisplayName
        SourceString = 闪光巡飞器
        LocalizedString = 闪光巡飞器
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4009003_ShortName
        SourceString = 闪光
        LocalizedString = 闪光
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 12
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::LeftHandSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = True
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_SupportSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_08.Scout_Skill_Icon_08
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts_Mobile/BakedSprite/Scout_Skill_Icon_14_m.Scout_Skill_Icon_14_m
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts_Mobile/BakedSprite/Scout_Skill_Icon_08_m.Scout_Skill_Icon_08_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 21
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.5
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = True
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanDetectRescue
        [1]
          ECharacterStateMask::CanReload
        [2]
          ECharacterStateMask::CanAutoChangeClip
        [3]
          ECharacterStateMask::CanAimOn
        [4]
          ECharacterStateMask::CanLooting
        [5]
          ECharacterStateMask::DisableWhitelistOperate
        [6]
          ECharacterStateMask::CanCarrybody
        [7]
          ECharacterStateMask::CanWeaponInspect
        [8]
          ECharacterStateMask::CanHighValueItemInspect
        [9]
          ECharacterStateMask::CanWeaponBipodDeploy
        [10]
          ECharacterStateMask::CanSwitchHelmetMask
        [11]
          ECharacterStateMask::CanParachute
        [12]
          ECharacterStateMask::CanPlayEmote
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_Rescue
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [4]
          EFSMCharacterState::FSMPlayerState_WeaponBipodDeploy
        [5]
          EFSMCharacterState::FSMPlayerState_GameOperate
        [6]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        [0]
          EGPWeaponStateType::EWeapState_Equipping
        [1]
          EGPWeaponStateType::EWeapState_Unequipping
        [2]
          EGPWeaponStateType::EWeapState_ChangingClip
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21010000004
      WeaponAttachPos = 1135
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 2
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = True
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = True
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = FlashLoiteringDrone
      SkillInspectText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4009003_SkillDisplayName
        SourceString = 闪光巡飞器
        LocalizedString = 闪光巡飞器
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060002
          Value = FlashLoiteringDrone
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = True
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 21
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 闪光巡飞器
        field = CdID
        value
          CdID = 21
          Slot = ECharingSlot::ArmdedForceItem1
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 2
              Max = 2
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 1
              Max = 1
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 2000.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 2
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 2
              Max = 2
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [4]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 30010203
    allSerializedFields
      SkillID = 30010203
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_SkillDisplayName
        SourceString = 工具箱
        LocalizedString = 工具箱
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010203_ShortName
        SourceString = 工具箱
        LocalizedString = 工具箱
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 31
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = False
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_Custom
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon/BakedSprite/ArmsCommon_Skill_Icon_12.ArmsCommon_Skill_Icon_12
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_23_m.Engineer_Skill_Icon_23_m
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon_Mobile/BakedSprite/ArmsCommon_Skill_Icon_12_m.ArmsCommon_Skill_Icon_12_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 1031
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = True
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanVault
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21020300011
      WeaponAttachPos = 1133
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 1031
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 工具箱
        field = CdID
        value
          CdID = 1031
          Slot = ECharingSlot::ArmdedForceItem1Ready
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 99
              Max = 99
              Time = 0.1
              UseTime = 0.0
              ChargeAmmoReplenish = 99
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
                [1]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 99
              MaxClipAmmoNum = 99
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [5]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 40010202
    allSerializedFields
      SkillID = 40010202
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 40010202_SkillDisplayName
        SourceString = 202被动-静匿消声
        LocalizedString = 202被动-静匿消声
      ShortName
        CultureInvariantString = None
      LogicType = ESkillLogicType::Assault
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 1
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        [0]
          100020
        [1]
          202001
      ConflictID = 0
      FlowDataType = EFlowDataType::Passive
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_10.Scout_Skill_Icon_10
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts_Mobile/BakedSprite/Scout_Skill_Icon_10_m.Scout_Skill_Icon_10_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED

Shepherd
requestedEnglishLabel = Shepherd
EnglishMappingStatus = UNVERIFIED_CHINESE_ARCHIVE_CANDIDATE_ONLY
heroPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Hero/HeroData.uasset
heroRow = 88000000029
heroName
  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_HeroData.LOC_HeroData
  Key = 88000000029_Name
  SourceString = 牧羊人
  LocalizedString = 牧羊人
ExpertID = 30008
expertPackage = DeltaForce/Content/R13N/Global/Base/DataTables/Skill/ExpertSkillData.uasset
expertRow = 30008
allExpertFields
  ExpertID = 30008
  ArmedForceID = 3
  MPUltimateSkillID = 30010101
  MPSupportSkillID = 3008001
  MPPassiveSkillID = 30010201
  MPActiveSkillID = 3008008
  MPCommonPassiveSkillID = 0
  MPCustomSkillID = 30010202
  MPCommonSkillID
    [0]
      4008007
  SOLUltimateSkillID = 30010101
  SOLSupportSkillID = 3008001
  SOLPassiveSkillID = 30010201
  SOLActiveSkillID = 3008008
  SOLCommonPassiveSkillID = 0
  SOLCommonSkillID
    [0]
      4008007
    [1]
      4008010
  BombDefusal_SkillGroupID = 30008
  RowDescription = None
groupPackage = DeltaForce/Content/R13N/Common/Base/DataTables/Skill/ExpertSkillGroupData.uasset
groupRow = 30008
allGroupFields
  SkillGroupID = 30008
  UltimateSkillID = 30010101
  SupportSkillID = 3008001
  PassiveSkillID = 30010201
  ActiveSkillID = 3008008
  CommonPassiveSkillID = 0
  CustomSkillID = 0
  CommonSkillID
    [0]
      4008007
  RowDescription = None
skillIDs
  [0]
    3008001
  [1]
    3008008
  [2]
    4008007
  [3]
    4008010
  [4]
    30010101
  [5]
    30010201
  [6]
    30010202
missingSkillIDs
  [0]
    4008010
skills
  [0]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 3008001
    allSerializedFields
      SkillID = 3008001
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008001_SkillDisplayName
        SourceString = 声波陷阱
        LocalizedString = 声波陷阱
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008001_ShortName
        SourceString = 陷阱
        LocalizedString = 陷阱
      LogicType = ESkillLogicType::Engineer
      TeamPlayerIcon
        AssetPathName = /Game/UI/UIAtlas/Common/BakedSprite/Common_Arms_Icon_0004.Common_Arms_Icon_0004
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 28
      FlowDataType = EFlowDataType::Props
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Engineers/C401/BP_AbilityIns_Engineers_Bunker.BP_AbilityIns_Engineers_Bunker_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_DefaultFalse
      Slot = ESkillUISlot::UIState_SupportSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = True
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer/BakedSprite/Engineer_Skill_Icon_06.Engineer_Skill_Icon_06
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0011.HUD_Controller_Icon_0011
        SubPathString = 
      NormalSlot1
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Btn_46.HUD_Controller_Btn_46
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_06_m.Engineer_Skill_Icon_06_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0011.HUD_Controller_Icon_0011
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Btn_46.HUD_Controller_Btn_46
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 11
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanReload
        [1]
          ECharacterStateMask::CanAutoChangeClip
        [2]
          ECharacterStateMask::CanAimOn
        [3]
          ECharacterStateMask::CanWeaponInspect
        [4]
          ECharacterStateMask::CanHighValueItemInspect
        [5]
          ECharacterStateMask::CanWeaponBipodDeploy
        [6]
          ECharacterStateMask::CanSwitchHelmetMask
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [4]
          EFSMCharacterState::FSMPlayerState_Vehicle
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = True
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21020300006
      WeaponAttachPos = 1136
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::PutWeaponQuiet
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_6
                  SourceString = 部署
                  LocalizedString = 部署
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 11
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 11
        field = CdID
        value
          CdID = 11
          Slot = ECharingSlot::ArmdedForceItem1
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 1
              Max = 2
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 2
              Max = 2
              Time = 40.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_SafeHouse
                [4]
                  EDFMGamePlayMode::GamePlayMode_Intro
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 1
              Max = 2
              Time = 30.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 1
              Max = 2
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [4]
              Init = 2
              Max = 2
              Time = 40.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [1]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 3008008
    allSerializedFields
      SkillID = 3008008
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008002_SkillDisplayName
        SourceString = 破片手雷
        LocalizedString = 破片手雷
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 1008001_ShortName
        SourceString = 伤害
        LocalizedString = 伤害
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 7
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          5000001
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 14
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::RightHandSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          [0]
            Inspect_Engineering_C401_M67_Cowboy_m0
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = True
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_ActiveSkill
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Scouts/BakedSprite/Scout_Skill_Icon_05.Scout_Skill_Icon_05
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0005.HUD_Controller_Icon_0005
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/ArmsCommon_Mobile/BakedSprite/ArmsCommon_Skill_Icon_05_m.ArmsCommon_Skill_Icon_05_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/HUD/BakedSprite/HUD_Controller_Icon_0005.HUD_Controller_Icon_0005
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 15
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = True
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanReload
        [1]
          ECharacterStateMask::CanAutoChangeClip
        [2]
          ECharacterStateMask::CanAimOn
        [3]
          ECharacterStateMask::CanWeaponInspect
        [4]
          ECharacterStateMask::CanHighValueItemInspect
        [5]
          ECharacterStateMask::CanWeaponBipodDeploy
        [6]
          ECharacterStateMask::CanSwitchHelmetMask
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = True
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = True
      ShortPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008002_ShortPressTipsDetailText
        SourceString = 点按开火按键快速投掷手雷
        LocalizedString = 点按开火按键快速投掷手雷
      LongPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 3008002_LongPressTipsDetailText
        SourceString = 长按开火按键可提前开始引信计时，松开按键投掷
        LocalizedString = 长按开火按键可提前开始引信计时，松开按键投掷
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 21010000022
      WeaponAttachPos = 1135
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_10
                  SourceString = 投掷
                  LocalizedString = 投掷
                bOverrideDesc = True
              [1]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_11
                  SourceString = （长按）消耗引信
                  LocalizedString = （长按）消耗引信
                bOverrideDesc = True
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginKeepPreFire
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_10
                  SourceString = 投掷
                  LocalizedString = 投掷
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginKeepPreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanAssassinate
              [1]
                ECharacterStateMask::CanUseZipline
              [2]
                ECharacterStateMask::CanRaiseWatch
              [3]
                ECharacterStateMask::CanShowInteractUI
              [4]
                ECharacterStateMask::CanParachute
              [5]
                ECharacterStateMask::CanVault
              [6]
                ECharacterStateMask::CanClimbLadder
              [7]
                ECharacterStateMask::CanLooting
              [8]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [1]
          Key = EGPWeaponInnerEvent::EStateEvent_BeginFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [2]
          Key = EGPWeaponInnerEvent::EStateEvent_PreFire
          Value
            LockAbility
              [0]
                ECharacterStateMask::CanDive
            UnLockAbility
              (empty array)
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
        [3]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            LockAbility
              (empty array)
            UnLockAbility
              [0]
                ECharacterStateMask::CanAssassinate
              [1]
                ECharacterStateMask::CanUseZipline
              [2]
                ECharacterStateMask::CanRaiseWatch
              [3]
                ECharacterStateMask::CanShowInteractUI
              [4]
                ECharacterStateMask::CanParachute
              [5]
                ECharacterStateMask::CanVault
              [6]
                ECharacterStateMask::CanClimbLadder
              [7]
                ECharacterStateMask::CanLooting
              [8]
                ECharacterStateMask::CanDive
            DSLockAbility
              (empty array)
            DSUnLockAbility
              (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060010
          Value = Inspect_Engineering_C401_M67_Cowboy_m0
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 15
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 破片手雷
        field = CdID
        value
          CdID = 15
          Slot = ECharingSlot::ArmdedForceItem2
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 2
              Max = 2
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Raid
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 1
              Max = 1
              Time = 0.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
                [1]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 1
              AmmoPackChargeCnt = 1
              SceneAmmoBoxChargeCnt = 1
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 2
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 2
              Max = 2
              Time = 45.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_Item
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [2]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 4008007
    allSerializedFields
      SkillID = 4008007
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_SkillDisplayName
        SourceString = 拔箭
        LocalizedString = 拔箭
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 4008007_ShortName
        SourceString = 拔箭
        LocalizedString = 拔箭
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          3012006
        [1]
          4008009
      AddSkills_Sol
        [0]
          4008008
        [1]
          6010100
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 23
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityIns_Scout_PullOutArrow.BP_AbilityIns_Scout_PullOutArrow_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/Scout/C201/BP_AbilityTemp_Scout_PullOutArrow.BP_AbilityTemp_Scout_PullOutArrow_C
        SubPathString = 
      SkillStateType = ESkillStateType::Custom2Skill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = False
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = False
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = True
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanHighValueItemInspect
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanPlayTacticalAction
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanUseZipline
        [8]
          ECharacterStateMask::CanDetectInteractor
        [9]
          ECharacterStateMask::CanLooting
        [10]
          ECharacterStateMask::CanShowInteractUI
        [11]
          ECharacterStateMask::CanSkillSwtichWeapon
        [12]
          ECharacterStateMask::CanDetectRescue
        [13]
          ECharacterStateMask::CanUseMachineGun
        [14]
          ECharacterStateMask::CanLean
        [15]
          ECharacterStateMask::CanReload
        [16]
          ECharacterStateMask::CanWeaponBipodDeploy
        [17]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [18]
          ECharacterStateMask::CanOperate
        [19]
          ECharacterStateMask::CanSwitchHelmetMask
        [20]
          ECharacterStateMask::CanUseTelescope
        [21]
          ECharacterStateMask::CanChangeWeaponMode
        [22]
          ECharacterStateMask::CanParachute
        [23]
          ECharacterStateMask::CanAimOn
        [24]
          ECharacterStateMask::CanFire
        [25]
          ECharacterStateMask::CanSprint
        [26]
          ECharacterStateMask::CanPlayMarkingAnim
        [27]
          ECharacterStateMask::CanApplySpray
        [28]
          ECharacterStateMask::CanFirefighting
        [29]
          ECharacterStateMask::CanPlayOneHandedGesture
        [30]
          ECharacterStateMask::CanPlayTwoHandedGesture
        [31]
          ECharacterStateMask::CanBeBreakByOtherSkill
        [32]
          ECharacterStateMask::CanVault
        [33]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        [0]
          EFSMCharacterState::FSMPlayerState_CommonLeftArmAnim
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = 0
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.0
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [3]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 30010101
    allSerializedFields
      SkillID = 30010101
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010101_SkillDisplayName
        SourceString = 声波震慑
        LocalizedString = 声波震慑
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010101_ShortName
        SourceString = 压制
        LocalizedString = 压制
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = /Game/UI/UIAtlas/Common/BakedSprite/Common_Arms_Icon_0004.Common_Arms_Icon_0004
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 7
      AbilitySpawnType = 7
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          30010102
        [1]
          5000001
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 1
      FlowDataType = EFlowDataType::ActiveSKill
      SkillEntityComponentClass
        AssetPathName = /Game/BluePrints/Ability/Engineers/C401/BP_AbilityIns_401.BP_AbilityIns_401_C
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = /Game/BluePrints/Ability/Engineers/C401/Ability_Engineer_Fly.Ability_Engineer_Fly_C
        SubPathString = 
      SkillStateType = ESkillStateType::ActionSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = True
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_FireButtonLeft
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer/BakedSprite/Engineer_Skill_Icon_02.Engineer_Skill_Icon_02
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer/BakedSprite/Engineer_Skill_Icon_04.Engineer_Skill_Icon_04
        SubPathString = 
      Activate
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer/BakedSprite/Engineer_Skill_Icon_01.Engineer_Skill_Icon_01
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_02_m.Engineer_Skill_Icon_02_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_04_m.Engineer_Skill_Icon_04_m
        SubPathString = 
      Activate_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_01_m.Engineer_Skill_Icon_01_m
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 12
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = True
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanHighValueItemInspect
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanPlayTacticalAction
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanAssassinate
        [6]
          ECharacterStateMask::CanUseItem
        [7]
          ECharacterStateMask::CanUseZipline
        [8]
          ECharacterStateMask::CanDetectInteractor
        [9]
          ECharacterStateMask::CanLooting
        [10]
          ECharacterStateMask::CanShowInteractUI
        [11]
          ECharacterStateMask::CanDetectRescue
        [12]
          ECharacterStateMask::CanUseMachineGun
        [13]
          ECharacterStateMask::CanLean
        [14]
          ECharacterStateMask::CanReload
        [15]
          ECharacterStateMask::CanWeaponBipodDeploy
        [16]
          ECharacterStateMask::CanDiscardOrEquipWeapon
        [17]
          ECharacterStateMask::CanOperate
        [18]
          ECharacterStateMask::CanSwitchHelmetMask
        [19]
          ECharacterStateMask::CanUseTelescope
        [20]
          ECharacterStateMask::CanChangeWeaponMode
        [21]
          ECharacterStateMask::CanParachute
        [22]
          ECharacterStateMask::CanAimOn
        [23]
          ECharacterStateMask::CanVault
        [24]
          ECharacterStateMask::CanCarrybody
        [25]
          ECharacterStateMask::CanSwitchWeapon
        [26]
          ECharacterStateMask::CanAutoChangeClip
        [27]
          ECharacterStateMask::CanWeaponInspect
        [28]
          ECharacterStateMask::CanWeaponBolt
        [29]
          ECharacterStateMask::CanFire
        [30]
          ECharacterStateMask::CanDive
        [31]
          ECharacterStateMask::CanPlayEmote
        [32]
          ECharacterStateMask::CanPlayMarkingAnim
        [33]
          ECharacterStateMask::CanSwimSprint
      UnLockStateAbilitys
        [0]
          ECharacterStateMask::CanMove
        [1]
          ECharacterStateMask::CanCrouch
        [2]
          ECharacterStateMask::CanVault
        [3]
          ECharacterStateMask::CanSprint
        [4]
          ECharacterStateMask::CanRotate
        [5]
          ECharacterStateMask::CanCharacterRotate
        [6]
          ECharacterStateMask::CanJump
        [7]
          ECharacterStateMask::CanStandUp
        [8]
          ECharacterStateMask::CanSkillSwtichWeapon
        [9]
          ECharacterStateMask::CanProneMove
        [10]
          ECharacterStateMask::CanProneStandUp
        [11]
          ECharacterStateMask::CanStandUp
        [12]
          ECharacterStateMask::CanAimOff
        [13]
          ECharacterStateMask::CanProne
        [14]
          ECharacterStateMask::CanSlidingTackle
        [15]
          ECharacterStateMask::CanLean
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        [0]
          EFSMCharacterState::FSMPlayerState_SpecialMove
        [1]
          EFSMCharacterState::FSMPlayerState_Interact
        [2]
          EFSMCharacterState::FSMPlayerState_SceneWeapon
        [3]
          EFSMCharacterState::FSMPlayerState_SearchItem
        [4]
          EFSMCharacterState::FSMPlayerState_Dive
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = True
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = True
      ShortPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010101_ShortPressTipsDetailText
        SourceString = 向前方部署无人机，发动1次强力震慑
        LocalizedString = 向前方部署无人机，发动1次强力震慑
      LongPressTipsDetailText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010101_LongPressTipsDetailText
        SourceString = 原地部署无人机，发动4次震慑
        LocalizedString = 原地部署无人机，发动4次震慑
      LongPressCallSkill = 30010102
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010101_LongPressTipsActionText
        SourceString = 长按原地部署无人机
        LocalizedString = 长按原地部署无人机
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = 0.4
      CacheInputPriority = 10
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        [0]
          EFSMForbidReason::EFR_Sprint
        [1]
          EFSMForbidReason::EFR_Vault
        [2]
          EFSMForbidReason::EFR_Reload
        [3]
          EFSMForbidReason::EFR_Jump
        [4]
          EFSMForbidReason::EFR_ProneFire
        [5]
          EFSMForbidReason::EFR_FreeLook
        [6]
          EFSMForbidReason::EFR_Aim
        [7]
          EFSMForbidReason::EFR_QuickThrow
        [8]
          EFSMForbidReason::EFR_Fire
        [9]
          EFSMForbidReason::EFR_ClimbLadder
        [10]
          EFSMForbidReason::EFR_WeaponState
        [11]
          EFSMForbidReason::EFR_RelaxIde
        [12]
          EFSMForbidReason::EFR_BeHit
        [13]
          EFSMForbidReason::EFR_StaminaLow
        [14]
          EFSMForbidReason::EFR_UsingSkill
        [15]
          EFSMForbidReason::EFR_FallHigh
        [16]
          EFSMForbidReason::EFR_SlidingTackle
        [17]
          EFSMForbidReason::EFR_TutorialRotate
        [18]
          EFSMForbidReason::EFR_PreViewActor
        [19]
          EFSMForbidReason::EFR_FireModePlacement
        [20]
          EFSMForbidReason::EFR_WeaponInspect
        [21]
          EFSMForbidReason::EFR_WeaponBipodDeploy
        [22]
          EFSMForbidReason::EFR_BowArrow
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        [0]
          Key = AbilitySkill
          Desc
            TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
            Key = TipsDesc_4
            SourceString = 向前震慑
            LocalizedString = 向前震慑
          bOverrideDesc = True
        [1]
          Key = AbilitySkill
          Desc
            TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
            Key = TipsDesc_5
            SourceString = （长按）原地震慑
            LocalizedString = （长按）原地震慑
          bOverrideDesc = True
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        [0]
          Key = 30000060010
          Value = Inspect_Engineering_C401_UAV_Cowboy_m0
      bCanInspectDuringEquip = False
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = True
      bTryAddAmmo = True
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = True
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 12
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 12
        field = CdID
        value
          CdID = 12
          Slot = ECharingSlot::Ability
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 0
              Max = 1
              Time = 90.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_SOL
                [1]
                  EDFMGamePlayMode::GamePlayMode_Intro
                [2]
                  EDFMGamePlayMode::GamePlayMode_IrisDiscovery
                [3]
                  EDFMGamePlayMode::GamePlayMode_Raid
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = UltimateFullyCharged401
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [1]
              Init = 0
              Max = 1
              Time = 70.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 13500.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = UltimateFullyCharged401
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [2]
              Init = 0
              Max = 1
              Time = 70.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 13500.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = UltimateFullyCharged401
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [3]
              Init = 0
              Max = 1
              Time = 60.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_Arena
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = UltimateFullyCharged401
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [4]
              Init = 0
              Max = 1
              Time = 90.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 13500.0
              LimitPlayMode
                (empty array)
              LimitPlaySubMode
                [0]
                  EDFMGamePlaySubMode::GamePlaySubMode_CaptureFlag
                [1]
                  EDFMGamePlaySubMode::GamePlaySubMode_TacticalConquest
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = UltimateFullyCharged401
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = False
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
            [5]
              Init = 0
              Max = 1
              Time = 90.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_BombDefusal
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = UltimateFullyCharged401
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 8
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                [0]
                  EventType = ESlotChargingEventType::RoundStart
                  PercentRecover = 0.125
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [1]
                  EventType = ESlotChargingEventType::RoudnOTStart
                  PercentRecover = 0.625
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [2]
                  EventType = ESlotChargingEventType::PlantBomb
                  PercentRecover = 0.125
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [3]
                  EventType = ESlotChargingEventType::PlantBombPersonal
                  PercentRecover = 0.125
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [4]
                  EventType = ESlotChargingEventType::BombDefusal
                  PercentRecover = 0.125
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [5]
                  EventType = ESlotChargingEventType::Kill
                  PercentRecover = 0.125
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
                [6]
                  EventType = ESlotChargingEventType::Dead
                  PercentRecover = 0.125
                  TimeRecoverSec = 0.0
                  bAllowWhenPause = True
            [6]
              Init = 0
              Max = 1
              Time = 90.0
              UseTime = 0.0
              ChargeAmmoReplenish = 1
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_GoldenGunGun
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 0
              MaxClipAmmoNum = 0
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = UltimateFullyCharged401
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = True
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
  [4]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 30010201
    allSerializedFields
      SkillID = 30010201
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010201_SkillDisplayName
        SourceString = 共振防御
        LocalizedString = 共振防御
      ShortName
        CultureInvariantString = None
      LogicType = ESkillLogicType::Engineer
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 1
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        [0]
          4010002
        [1]
          4010003
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        [0]
          401001
        [1]
          8010
      ConflictID = 0
      FlowDataType = EFlowDataType::Passive
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_NoSlot
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = False
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = None
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = None
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = False
      Normal_Mobile
        AssetPathName = None
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 0
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        (empty array)
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        (empty array)
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = True
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = False
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 0
      WeaponAttachPos = 0
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::Default
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        (empty array)
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        [0]
          fModifyRateExplodeDamage = 0.7
          fModifyRateAbilitAssault = 1.0
          LimitPlayMode
            (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 0
    cooldownMatches
      (empty array)
    cooldownStatus = ZERO_OR_UNRESOLVED
  [5]
    package = DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset
    row = 30010202
    allSerializedFields
      SkillID = 30010202
      SkillDisplayName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010202_SkillDisplayName
        SourceString = 工程兵喷枪
        LocalizedString = 工程兵喷枪
      ShortName
        TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
        Key = 30010202_ShortName
        SourceString = 喷枪
        LocalizedString = 喷枪
      LogicType = ESkillLogicType::None
      TeamPlayerIcon
        AssetPathName = None
        SubPathString = 
      ClientPredictCountFSMEnter = True
      bRollbackCheck = False
      bIsClientPredcit = False
      bIsClientPredcitAck = True
      CheckConsumeCommond = True
      bResetWhenReconnect = False
      bResetWhenNetReconnect = True
      AbilityDataSpawnType = 3
      AbilitySpawnType = 3
      ClientRTTTime = 10.0
      bCanRPCEnd = True
      bRPCEndCastCD = False
      ServerUnEquipSkillWeapon = True
      ClientUnEquipSkillWeapon = False
      fEnterState3pCheckTime = 0.0
      bRecord3P = True
      ClientForceLeaveTime = -1.0
      DS1pAbandomTime = -1.0
      DS3pAbandomTime = -1.0
      SkillBagWeapons
        (empty array)
      AddSkills
        (empty array)
      AddSkills_Sol
        (empty array)
      AddSkills_Mp
        (empty array)
      AddSkills_Avatar
        (empty array)
      AddBuffs
        (empty array)
      ConflictID = 20
      FlowDataType = EFlowDataType::None
      SkillEntityComponentClass
        AssetPathName = None
        SubPathString = 
      SkillEntityTemplateClass
        AssetPathName = None
        SubPathString = 
      SkillStateType = ESkillStateType::DoubleHandsSkill
      MPPreLoad = False
      SkillPreLoadData
        GlobalPreLoadCharacterAnimations
          (empty array)
        GlobalPreloadAttachMesh
          (empty array)
        AutonomousPreloadAudio
          (empty array)
        SimulatedPreloadAudio
          (empty array)
        LoadSkillResource1P = True
        LoadSkillResource3P = False
        GlobalAllAvatarAnim = False
        GlobalAllAvatarMesh = False
        ActorClassSoftPathPreLoad
          (empty array)
        ActorObjectSoftPathPreLoad
          (empty array)
      CanUseWhenChangeWeapon = False
      CanUseWhenFalling = True
      bCanCastUnderWater = True
      bCanCastOnWaterSurface = True
      bWaterConditionUseInClient = True
      CanUseInAerialWorkPlatform = True
      bCanUseWhenSlidingTackle = False
      CannotUseWhenActiavte = False
      UseVehicleAbilityType = EVehicleAbilityType::EVAT_USESkillCommon
      Slot = ESkillUISlot::UIState_Custom
      LoadHudName = None
      bIsShowHud = False
      InVisibleIconWhenUsing = False
      InitVisible = True
      bShowCancle = False
      bReplaceFire = False
      bReplaceFireImage = True
      TipsText
        CultureInvariantString = None
      TipsVisible = True
      ButtonState = ESkillButtonState::ESB_Normal
      ResetUI2Normal = True
      ShowKeyWhenActivated = False
      Normal
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer/BakedSprite/Engineer_Skill_Icon_08.Engineer_Skill_Icon_08
        SubPathString = 
      Cancel
        AssetPathName = None
        SubPathString = 
      Disable
        AssetPathName = None
        SubPathString = 
      Activate
        AssetPathName = None
        SubPathString = 
      Fire
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer/BakedSprite/Engineer_Skill_Icon_08.Engineer_Skill_Icon_08
        SubPathString = 
      NormalSlot1
        AssetPathName = None
        SubPathString = 
      NormalSlot2
        AssetPathName = None
        SubPathString = 
      MapIconPath
        AssetPathName = None
        SubPathString = 
      UseMobile = True
      Normal_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_24_m.Engineer_Skill_Icon_24_m
        SubPathString = 
      Cancel_Mobile
        AssetPathName = None
        SubPathString = 
      Disable_Mobile
        AssetPathName = None
        SubPathString = 
      Activate_Mobile
        AssetPathName = None
        SubPathString = 
      Fire_Mobile
        AssetPathName = /Game/UI/UIAtlas/System/Arms/Engineer_Mobile/BakedSprite/Engineer_Skill_Icon_24_m.Engineer_Skill_Icon_24_m
        SubPathString = 
      NormalSlot1_Mobile
        AssetPathName = None
        SubPathString = 
      NormalSlot2_Mobile
        AssetPathName = None
        SubPathString = 
      bForceCheckSlot = False
      CDID = 1030
      UseCDCheckCount = 5
      UseCD = 0.0
      ClientUseCD = 0.0
      ClientEndUseCD = 0.0
      LockRTT = 0.5
      CDAtStart = False
      MoifyCDAudioName = None
      ScoreCost = -1
      ScoreDiscount
        AssetPathName = None
        SubPathString = 
      bDieWhenUsingCastCD = False
      VoList
        (empty array)
      VosList
        (empty array)
      ActivatedWeaponSlot = 0
      ActivatedBuffid
        (empty array)
      CountDownBuff = -1
      AbilityUseType = ESkillItemUsingType::UT_Normal
      bCanBreakByClimbingLadder = True
      bCanBreakByFalling = False
      bCanBreakByLongFalling = False
      bCanBreakByFire = False
      bCanBreakBySwitchWeapon = False
      bCanBreakByBotControlled = True
      bCanBreakByZipline = True
      bCanBeBreakByKnockBackWhenRopeBack = False
      bCanBeBreakByPassiveMoveWhenRopeBack = False
      bBreakSkillInKnockBack = False
      bBreakSkillInPassiveMove = False
      bBreakSkillInSwim = False
      bBreakSkillUnderWater = False
      bBreakSkillInBeingAssassinate = False
      bBreakSkillInCrowdControl = False
      bBreakByDie = True
      bBreakByImpendingDeath = True
      bBreakByEnterZipline = False
      bHideSkillSlotUI = False
      bHideWhenRescue = False
      bHideWhenParachute = False
      LockStateAbilitys
        [0]
          ECharacterStateMask::CanLooting
        [1]
          ECharacterStateMask::CanCarrybody
        [2]
          ECharacterStateMask::CanSlidingTackle
        [3]
          ECharacterStateMask::CanClimbLadder
        [4]
          ECharacterStateMask::CanWeaponInspect
        [5]
          ECharacterStateMask::CanPlayTacticalAction
        [6]
          ECharacterStateMask::CanAssassinate
        [7]
          ECharacterStateMask::CanHighValueItemInspect
      UnLockStateAbilitys
        (empty array)
      LockStateAbilitys_DirectThrow
        (empty array)
      UnLockStateAbilitys_DirectThrow
        (empty array)
      DSLockStateAbilitys
        (empty array)
      DSUnLockStateAbilitys
        (empty array)
      CharacterBreakStates
        (empty array)
      CharacterBlockStatus
        (empty array)
      WeaponBlockStatus
        (empty array)
      MainWeaponBlockStates
        (empty array)
      BreakWeaponActionCharacterStates
        (empty array)
      BlockWeaponFireCharacterStates
        [0]
          EFSMCharacterState::FSMPlayerState_Dive
      CharacterPreBlockStatus
        (empty array)
      ClientCharacterBlockStatus
        (empty array)
      bBreakLeanState = False
      DeathCanNotActivate = True
      ImpendingDeathCanNotActivate = True
      CanShowInThrowText = False
      CanChangeFastThrow = False
      DefaultThrowMode = EThrowMode::IdleToThrow
      CanLongPress = False
      ShortPressTipsDetailText
        CultureInvariantString = None
      LongPressTipsDetailText
        CultureInvariantString = None
      LongPressCallSkill = -1
      LongPressTimeConfig = 0.2
      LongPressUIOffsetTime = 0.1
      LongPressEnable = True
      LongPressTipsActionText
        CultureInvariantString = None
      LongPressTipsActionName = None
      bSupportHoldToActive = True
      bTriggerFromWeapon = True
      bCantSwitchWeaponSkillNoUsing = False
      SkillWeaponId = 18100000003
      WeaponAttachPos = 1133
      SkillWeaponAttachInfo
        AvatarID = 0
        SkillID = 0
        Socket = EGPWeaponAttachBackSocket::None
        ModelConflict = EGPWeaponAttachBackConflict::None
        AttachBackRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        AttachBagRelativeTransform
          Rotation
            X = 0.0
            Y = 0.0
            Z = 0.0
            W = 1.0
            IsNormalized = True
            Size = 1.0
            SizeSquared = 1.0
          Translation
            X = 0.0
            Y = 0.0
            Z = 0.0
          Scale3D
            X = 1.0
            Y = 1.0
            Z = 1.0
        DisabledAttachBackDeviceLevels
          (empty array)
        RowDescription = None
      WeaponManagerSlotIndex = 0
      WeaponSwitchType = ESkillWeaponSwitchType::GetAndPutWeaponFast
      bHoldToFire = False
      ClickSkillWeaponPos = 0
      bAddSkillWeapon = False
      UnormalBreakUnequipSkillWeapon = False
      bSkillWeaponBreakPredictSkill = True
      bSkillWeaponBreakPredictSkillOnlyClient = False
      UnequipSkillWeaponSwitchType = EWeaponSwitchType::Instant
      EMPUnEquipSkillWeaponSwitchType = EWeaponSwitchType::GetAndPutWeaponFast
      NotUnEquipSameSlotWeapon = False
      bFireUnEquipWeapon = False
      bSwitchAwaySkillWeaponOnFrozenEnd = False
      CacheInputTime = -0.0
      CacheInputPriority = 0
      JumpCacheInputBuffs
        (empty array)
      NoCacheCharacterStatus
        (empty array)
      CacheInputReasons
        (empty array)
      CacheInputInWalkingPoseTransition = False
      CacheInputInWalkingMainWeaponChange = False
      CancelWeaponOperations = True
      bCancelWeaponOperationInPressed = False
      bBreakWeaponInspect = True
      bSwitchActiveLeftWeapon = True
      WeaponOperateTips
        [0]
          Key = EGPWeaponInnerEvent::EStateEvent_SwitchToIdle
          Value
            TipsDataList
              [0]
                Key = Fire
                Desc
                  TableId = /Game/StringTables/STForDataTable/Common/Base/LOC_SkillDataConfig.LOC_SkillDataConfig
                  Key = TipsDesc_1
                  SourceString = 发射
                  LocalizedString = 发射
                bOverrideDesc = True
      SkillOperateTips
        (empty array)
      WeaponLockData
        (empty array)
      WeaponLockStateData
        (empty array)
      SkillUsingOperateTips
        (empty array)
      bUseWeaponInspect = False
      SkillInspectID = None
      SkillInspectText
        CultureInvariantString = None
      SkillInspectItemIDMap
        (empty array)
      SkillInspectAvatarMap
        (empty array)
      bCanInspectDuringEquip = True
      EndSkillBehavior
        (empty array)
      LocalSkillBehavior
        (empty array)
      LocalUnEquipSkillBehavior
        (empty array)
      LocalUnEquipStatus
        (empty array)
      CheckWeaponIndexs
        (empty array)
      bCheckLeaveEvent = True
      DSForceLeaveTime = -1.0
      bBlockPronedMove = False
      bStopMontageWhenTakeDamge = False
      bEndAbilityWhenTakeDamge = False
      bEndAbilityWhenTakeEMP = False
      bTryAddAmmo = False
      bWeaponUseSkillConflict = False
      bCanServerDoTrigger = False
      bOverrideAimingCheck = False
      bEnableSniperMark = False
      FootStepAudioSeqDataID = 0
      DamgeCoifficient
        (empty array)
      UseCustomHeight = False
      bFirePreLock = True
      BuffDurationRateMap
        (empty array)
      NeedCoverKillFXAvatars
        (empty array)
      OnEndVaultAndClimbDynamicObForbitTime = 0.0
      bUseInVehicleShouldNotLeanout = False
      BombDefusalPrice = 0
      BreakByVehiclePush = False
      RowDescription = None
    CDID = 1030
    cooldownMatches
      [0]
        package = DeltaForce/Content/DataTables/Inventory/SlotCharing.uasset
        row = 喷枪
        field = CdID
        value
          CdID = 1030
          Slot = ECharingSlot::ArmdedForceItem1Ready
          WeaponSlot = 0
          SlotChargingData
            [0]
              Init = 99
              Max = 99
              Time = 0.1
              UseTime = 0.0
              ChargeAmmoReplenish = 99
              PerkScaleMap
                (empty array)
              MaxScoreReduceCDPercent = 0.25
              MaxScore = 0.0
              LimitPlayMode
                [0]
                  EDFMGamePlayMode::GamePlayMode_Conquest
                [1]
                  EDFMGamePlayMode::GamePlayMode_Breakthrough
              LimitPlaySubMode
                (empty array)
              AddToCurrentAmmo = False
              CurrentAmmoNum = 99
              MaxClipAmmoNum = 99
              SkillNumType = ESkillNumType::UT_NoShow
              DoneVoiceString = 
              NoCDType = False
              bWeaponCheckAmmo = False
              bCheckWeaponAmmoWhenActivate = True
              bUseLocalAmmo = False
              AmmoBoxChargeCnt = 0
              AmmoPackChargeCnt = 0
              SceneAmmoBoxChargeCnt = 0
              bOnlyWeaponCountAmmo = True
              bRebornReset = True
              bPauseCanAddScore = True
              InitialChargingPercent = 0.0
              bChargingOnlyWhenEmpty = False
              BombDefusalChargeGranularity = 0
              bChargingWhenAmmoBelowThreshold = False
              ChargingAmmoThreshold = 0
              EventRules
                (empty array)
          CheckSubMode = False
          RowDescription = None
    cooldownStatus = CONFIG_MATCH_RUNTIME_PRECEDENCE_UNRESOLVED
