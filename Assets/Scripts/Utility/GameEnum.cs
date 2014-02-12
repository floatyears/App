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
	None 					= 0,
	ChangeScene 			= 1,
	InquiryBattleBaseData 	= 1001,
	MoveToMapItem 			= 1002,
	StartAttack				= 1003,
	EnterUnitInfo			= 1004,
	Person 					= 3000,
	BattleBaseData 			= 3001,
	UnitBlood 				= 3002,
	EnergyPoint				= 3003,
	ShowEnemy 				= 3004,
	AttackEnemy				= 3005,
	AttackPlayer			= 3006,
	AttackRecoverRange		= 3007,
	BattleEnd				= 3008,
	EnemyAttack				= 3010,
	EnemyAttackEnd			= 3009,
	EnemyRefresh			= 3011,
	EnemyDead				= 3012,
	RecoverHP				= 3013,
	LeaderSkillBoost		= 3014,
	LeaderSkillDelayTime	= 3015,
	ActiveSkillAttack		= 3016,
	LaunchActiveSkill		= 3017,
	ActiveSkillRecoverHP	= 3018,
	ActiveSkillDrawHP		= 3019,
	SkillSucide				= 3020,
	SkillGravity			= 3021,
	SkillRecoverSP			= 3022,
	SkillPosion				= 3023,
	AttackEnemyEnd			= 3024,
	/// <summary>
	/// zhong du 
	/// </summary>
	BePosion				= 3025,
	/// <summary>
	/// The color of the change card.
	/// </summary>
	ChangeCardColor			= 3026,

	ReduceDefense			= 3027,

	DelayTime				= 3028,

	ActiveReduceHurt		= 3029,

	DeferAttackRound		= 3030,

	AttackTargetType		= 3031,

	StrengthenTargetType	= 3032,

	ActiveSkillCooling		= 3033,

	TrapMove				= 3034,

	NoSPMove				= 3035,

	TrapInjuredDead			= 3036,

	ConsumeSP				= 3037,

	ConsumeCoin				= 3038,

	ShieldMap				= 3039,

	InjuredNotDead			= 3040,
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
