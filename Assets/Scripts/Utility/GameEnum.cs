public enum SceneEnum
{
	None,

    Loading,

	Start,

	Quest,

	Friends,

	Scratch,

    FriendScratch,

    RareScratch,

    EventScratch,

	Shop,

	Others,

	Units,

	QuestSelect,

	FriendSelect,

	FriendList,

	Information,

	SearchFriend,

	Apply,

	Reception,

	YourID,

	Party,

	LevelUp,

	Evolve,

	Sell,

	UnitList,

	UnitCatalog,

	UnitDetail,

	Fight,

	SelectRole,

	Result,

	StandBy
}

public enum CommandEnum{
	None 						= 0,
	ChangeScene 				= 1,
    QuestSelectSaveState,
//	EvolveSaveState,
    LevelUpSaveState,
    SellUnitSaveState,
    EvolveSaveState,
    PartySaveState,
    BackSceneEnable,
	InquiryBattleBaseData 		= 1001,
	MoveToMapItem 				= 1002,
	StartAttack					= 1003,
	EnterUnitInfo				= 1004,
	Person 						= 3000,
	BattleBaseData 				= 3001,
	UnitBlood 					= 3002,
	EnergyPoint					= 3003,
	ShowEnemy 					= 3004,
	AttackEnemy					= 3005,
	AttackPlayer				= 3006,
	AttackRecoverRange			= 3007,
	BattleEnd					= 3008,
	EnemyAttack					= 3010,
	EnemyAttackEnd				= 3009,
	EnemyRefresh				= 3011,
	EnemyDead					= 3012,
	RecoverHP					= 3013,
	LeaderSkillBoost			= 3014,
	LeaderSkillDelayTime		= 3015,
	ActiveSkillAttack			= 3016,
	LaunchActiveSkill			= 3017,
	ActiveSkillRecoverHP		= 3018,
	ActiveSkillDrawHP			= 3019,
	SkillSucide					= 3020,
	SkillGravity				= 3021,
	SkillRecoverSP				= 3022,
	SkillPosion					= 3023,
	AttackEnemyEnd				= 3024,
	/// <summary>
	/// zhong du 
	/// </summary>
	BePosion					= 3025,
	/// <summary>
	/// The color of the change card.
	/// </summary>
	ChangeCardColor				= 3026,

	ReduceDefense				= 3027,

	DelayTime					= 3028,

	ActiveReduceHurt			= 3029,

	DeferAttackRound			= 3030,

	AttackTargetType			= 3031,

	StrengthenTargetType		= 3032,

	ActiveSkillCooling			= 3033,

	TrapMove					= 3034,

	NoSPMove					= 3035,

	TrapInjuredDead				= 3036,

	ConsumeSP					= 3037,

	ConsumeCoin					= 3038,

	ShieldMap					= 3039,

	PlayerPosion				= 3040,
	InjuredNotDead				= 3041,
	MeetEnemy					= 3042,
	MeetTrap					= 3043,
	TrapTargetPoint				= 3044,
	OpenDoor					= 3045,
	RotateDown					= 3046,
	MeetCoin					= 3047,
	StateInfo					= 3048,
	StopInput					= 3049,
	GridEnd						= 3050,
	DropItem					= 3051,
	SelectUnitBase				= 3052,
	selectUnitMaterial			= 3053,
	UnitDisplayState			= 3054,
	UnitDisplayBaseData			= 3055,
	UnitDisplayMaterialData		= 3056,
	EvolveFriend				= 3057,
	UnitMaterialList 			= 3058,
	EvolveStart,
	ReturnPreScene,
	RefreshFriendHelper,
	PlayerDead,
	EnterBattle,
	LeftBattle,
	QuestEnd,
	TargetEnemy,
	ReduceActiveSkillRound,
	//-----------------View Cmd-----------------------//
	//Add By Lynn
	PanelFocus					= 4000,
	TransmitStageInfo			= 4001,
	PickBaseUnitInfo				= 4002,
	PickFriendUnitInfo			= 4003,
	PickMaterialUnitInfo			= 4004,
	CheckLevelUpInfo			= 4005,
	LevelUp					= 4006,
	ShowUnitDetail				= 4007,
	SendLevelUpInfo			= 4008,
	TryEnableLevelUp			= 4009,
	CrossFade					= 4010,
	UpdateNickName			= 4011,
	NoteFriendUpdate			= 4012,
	ShowUnitBriefInfo			= 4013,
	UpdatePartyInfoPanel			= 4014,
	ShowFocusUnitDetail			= 4015,
	NoticeFuncParty				= 4016,
	OnPartySelectUnit			= 4017,
	OnSubmitChangePartyItem		= 4018,
	GetSubmitChangeState		= 4019,
	ShowMyUnitListBriefInfo		= 4020,
	ActivateMyUnitDragPanelState	= 4021,
	EnsureFocusOnPartyItem		= 4022,
	EnsureSubmitUnitToParty		= 4023,
	ReplacePartyFocusItem		= 4024,
	BaseAlreadySelect			= 4025,
	MaterialSelect				= 4026,
	ShieldMaterial				= 4027,
	RejectPartyPageFocusItem		= 4028,
	RefreshPartyUnitList			= 4029,
	RefreshPartyPanelInfo			= 4030,
	FriendBriefInfoShow		= 4031,
	EnsureUpdateFriend			= 4032,
	CancelSelectunitbrief		= 4033,
	EnsureDeleteFriend					= 4034,
	ViewApplyInfo					= 4035,
	ShowApplyInfo				= 4036,
	EnsureDeleteApply			= 4037,
	NoteRefuseAll				= 4038,
	EnsureRefuseAll				= 4039,
	ErrorIDInputEmpty			= 4040,
	SubmitFriendApply			= 4041,
	OpenMsgWindow				= 4042,
	EnsureRefuseSingleApply			= 4043,
	EnsureAcceptApply				= 4044,
    FriendExpansion                 = 4045,
    StaminaRecover                  = 4046,
    UnitExpansion                   = 4047,
    SyncStamina                     = 4048,
	AddHelperItem				= 4051,
	ChooseHelper					= 4052,
	GetSelectedStage				= 4053,
	GetSelectedQuest				= 4054,
	
    EnterGachaWindow                  = 4056,
    SyncChips                       = 4057,
    SyncCoins                       = 4058,
	RefreshPlayerCoin				= 4059,

    SeletEvolveInfo					= 4060,
    EnableMenuBtns                  = 4061,
	EvolveSelectQuest 				= 4062,

	RefreshItemCount				= 4063,

	ShowFriendPointUpdateResult					= 4064,
	PickOnSaleUnit				= 4065,
	SortByRule							= 4066,
	OpenSortRuleWindow				= 4067,

	//new add
	TransPickedCity					= 4068,
	OnPickQuest,
	OnPickHelper,

    WaitResponse,
    SetBlocker,
    LevelUpSucceed,
    StartFirstLogin,
    FocusLevelUpPanel,
	LeaderSkillEnd,
	ShowHands,
	ActiveSkillStandReady,
	CloseMsgWindow,
	ExcuteActiveSkill,
	//-----------------Server Protocol-----------------------//
	// user - 5000
//	ReqAuthUser					= 5000,
//	RspAuthUser					= 5001,
	ReqRenameNick				= 5002,
//	RspRenameNick				= 5003,

	// quest - 5100
//	ReqStartQuest				= 5101,
//	RspStartQuest				= 5102,
	ReqClearQuest				= 5103,
//	RspClearQuest				= 5104,
	
	//unit - 5200
	ReqLevelUp					= 5201,
	RspLevelUp					= 5202,

	//party - 5300 
	ReqChangeParty				= 5301,
	RspChangeParty				= 5302,

	//-----------------Server Protocol-----------------------//
}

public enum MapItemEnum
{
	None					= 0,
	Enemy					= 1,
	Trap					= 2,
	Coin					= 3,
	key						= 4,
	Start					= 5,
	Exclamation 			= 6,
}

public enum UIParentEnum : byte
{
	/// <summary>
	/// screen center anchor
	/// </summary>
	Center = 0,

	/// <summary>
	/// screen top anchor
	/// </summary>
	Top = 1,

	/// <summary>
	/// screen bottom anchor
	/// </summary>
	Bottom = 2,

	/// <summary>
	/// no UIPanel component
	/// </summary>
	BottomNoPanel = 3,

	/// <summary>
	/// The none.
	/// </summary>
	None = 254,
}

public enum UIState
{
	UIInit,
	UICreat,
	UIShow,
	UIHide,
	UIDestory,
}

public enum ResourceEuum : byte
{
	Prefab = 0,
	Image = 1,
}


public enum CardPoolEnum
{
	ActorCard,

	FightCard,
}

public enum CardColorType : byte
{

	fire = 0,

	water = 1,
	
	wind = 2,

	light = 3,

	dark = 4,

	nothing = 5,

	heart = 6,
}


public enum AudioEnum{
	music_home 					= 0,
	music_dungeon				= 1,
	music_enemy_battle		= 2,
	music_boss_battle			= 3,
	music_victory					= 4,

	sound_click					= 100,
	sound_ui_back				= 101,
	sound_dungeon_ready 	= 102,
	sound_quest_ready			= 103,
	sound_quest_go				= 104,
	sound_walk					= 105,
	sound_walk_hurt				= 106,
	sound_grid_turn				= 107,
	sound_get_treasure			= 108,
	sound_trigger_trap			= 109,
	sound_get_key				= 110,
	sound_door_open			= 111,
	sound_enemy_battle		= 112,
	sound_first_attack			= 113,
	sound_back_attack			= 114,
	sound_active_skill			= 115,
	sound_water_attack			= 116,
	sound_fire_attack			= 117,
	sound_wind_attack			= 118,
	sound_light_attack			= 119,
	sound_dark_attack			= 120,
	sound_zero_attack			= 121,
	sound_count_down			= 122,
	sound_drag_tile				= 123,
	sound_combo					= 124,
	sound_hp_recover			= 125,
	sound_enemy_attack		= 126,
	sound_enemy_die			= 127,
	sound_get_chess			= 128,
	sound_boss_battle			= 129,
	sound_devour_unit			= 130,
	sound_get_exp				= 131,
	sound_level_up				= 132,
	sound_check_role			= 133,
	sound_sold_out				= 134,
	sound_use_chip				= 135,
	
}

public enum SkillType {
	ActiveSkill = 1,
	PassiveSkill = 2,
	LeaderSkill	= 3,
	NormalSkill = 4,

}

public enum UnitAssetType{
	Avatar 	= 0,
	Profile	= 1,
}

public enum EnemyAttackEnum{
	None,
	FirstAttack,
	BackAttack,
}
