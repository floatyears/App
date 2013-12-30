

public enum SceneEnum
{
	Start,

	Quest,

	Friends,

	Scratch,

	Shop,

	Others,

	Units,

	QuestSelect,

	FriendSelect,

	Party,

	LevelUp,

	Evolve,

	Sell,

	UnitList,

	UnitCatalog,

	Fight,
}

public enum CommandEnum
{
	None = 0,
	#region view to model
	DragCardToBattleArea = 1000,
	#endregion

	#region model to view
	Person = 3000,
	#endregion
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