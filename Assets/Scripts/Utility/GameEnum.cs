

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

	Fight,
}

public enum DataEnum
{
	#region to model
	DragCardToBattleArea,
	#endregion

	#region to view
	Person,
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