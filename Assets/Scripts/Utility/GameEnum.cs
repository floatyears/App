//public enum 

public enum SceneEnum
{
	None,

	Start,

	Quest,

	Friends,

	Scratch,

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
}

public enum CommandEnum
{
	None 						= 0,
	ChangeScene 				= 1,
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

	//Add By Ling yan	
	LevelUpPanelFocus			= 4000,
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

	//-----------------Server Protocol-----------------------//
	// user - 5000
	ReqAuthUser					= 5000,
	RspAuthUser					= 5001,
	ReqRenameNick				= 5002,
	RspRenameNick				= 5003,

	// quest - 5100
	ReqStartQuest				= 5101,
	RspStartQuest				= 5102,

	//unit - 5200
	ReqLevelUp					= 5201,
	RspLevelUp					= 5202,



	//-----------------Server Protocol-----------------------//
}

public enum MapItemEnum {
	None					= 0,
	Enemy					= 1,
	Trap					= 2,
	Coin					= 3,
	key						= 4,
	Start					= 5,
	Exclamation 			= 6,
}

public enum UIParentEnum : byte {
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

public enum CardColorType : byte {

	fire = 0,

	water = 1,
	
	wind = 2,

	light = 3,

	dark = 4,

	nothing = 5,

	heart = 6,
}


public enum AudioEnum {
	music_home 				= 0,
	music_dungeon  			= 1,
	music_fight				= 2,
	sound_click				= 3,
	sound_ui_back			= 4,
	sound_dungeon_ready 	= 5,
	sound_explore_ready		= 6,
	sound_explore_start		= 7,
	sound_explore_done		= 8,
	sound_chess_move_hurt	= 9,
	sound_flip_grid			= 10,
	sound_get_treasure		= 11,
	sound_trigger_trap		= 12,
	sound_get_key			= 13,
	sound_door_open			= 14,
	sound_enemy_battle		= 15,
	sound_first_attack		= 16,
	sound_after_attack		= 17,
	sound_skill_activate	= 18,
	sound_water_attack		= 19,
	sound_fire_attack		= 20,
	sound_wind_attack		= 21,
	sound_light_attack		= 22,
	sound_dark_attack		= 23,
	sound_wu_attack			= 24,
	sound_count_time		= 25,
	sound_drag_tile			= 26,
	sound_combo				= 27,
	sound_hp_recover		= 28,
	sound_enemy_attack		= 29,
	sound_enemy_die			= 30,
	sound_get_chess			= 31,
	sound_boss_battle		= 32,
	sound_quest_over		= 33,
	sound_swallow_card		= 34,
	sound_exp_increase		= 35,
	sound_level_up			= 36,
	sound_check_role		= 37,
	sound_chess_move		= 38
}


public enum showTurn{
	FirstTurn,
	SecondTurn	
}

public enum UnitAssetType {
	Avatar 	= 0,
	Profile	= 1,
}
