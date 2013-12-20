using UnityEngine;
using System.Collections.Generic;

public class BattleQuest : UIBase
{
	public int MapWidth
	{
		get{ return mapConfig.mapXLength; }
	}

	public int MapHeight
	{
		get{ return mapConfig.mapYLength; }
	}

	private Coordinate roleInitPosition = new Coordinate();

	public Coordinate RoleInitPosition
	{
		get
		{ 
			if(roleInitPosition.x != mapConfig.characterInitCoorX)
			{
				roleInitPosition.x = mapConfig.characterInitCoorX;
				roleInitPosition.y = mapConfig.characterInitCoorY;
			}

			return  roleInitPosition;
		}
	}

	private GameObject rootObject;

	public static MapConfig mapConfig;

	private SingleMapData currentMapData;

	private BattleMap battleMap;
	
	private Role role;

	private Battle battle;

	private BattleBackground background;

	string backgroundName = "BattleBackground";

	public BattleQuest (string name) : base(name)
	{
		InitData ();

		rootObject = NGUITools.AddChild(viewManager.ParentPanel);
		string tempName = "Map";

		battleMap = viewManager.GetViewObject(tempName) as BattleMap;
		battleMap.BQuest = this;
		battleMap.transform.parent = rootObject.transform;
		battleMap.transform.localPosition = Vector3.zero;
		Init(battleMap,tempName);
		tempName = "Role";

		role = viewManager.GetViewObject(tempName) as Role;
		role.BQuest = this;
		role.transform.parent = rootObject.transform;
		Init(role,tempName);

		background = viewManager.GetViewObject(backgroundName) as BattleBackground;
		background.transform.parent = viewManager.ParentPanel.transform.parent;
		background.transform.localPosition = Vector3.zero;
		background.Init (backgroundName);

		AddSelfObject (battleMap);
		AddSelfObject (role);
		AddSelfObject (background);

		CreatUI ();
	}

	void InitData()
	{
		mapConfig = new MapConfig();
	}

	void Init(UIBaseUnity ui,string name)
	{
		ui.Init(name);
	}

	public override void CreatUI ()
	{
		base.CreatUI ();
	}

	public override void ShowUI ()
	{

		InitData ();

		base.ShowUI ();
	}

	public override void HideUI ()
	{


		base.HideUI ();
	}
	  
	public Vector3 GetPosition(Coordinate coor)
	{
		return battleMap.GetPosition(coor.x, coor.y);
	}

	public void TargetItem(Coordinate coor)
	{
		role.StartMove(coor);
	}
	  
	public void RoleCoordinate(Coordinate coor)
	{
		if(!battleMap.ReachMapItem (coor))
		{
			currentMapData = mapConfig.mapData[coor.x,coor.y];

			if(currentMapData.MonsterID.Contains(100))
				controllerManger.ChangeScene(SceneEnum.Quest);
			else if(currentMapData.MonsterID.Count > 0)
			{
				ShowBattle();

				role.Stop();

				battle.ShowEnemy(currentMapData.MonsterID.Count);
			}
		}
	} 

	void ShowBattle()
	{
		if(battle == null)
		{	
			battle = new Battle("Battle");

			battle.CreatUI();
		}

		if(battle.GetState == UIState.UIShow)
			return;

		battle.ShowUI();

	}

	void BattleEnd()
	{
		if(battle == null || battle.GetState == UIState.UIHide)

			return;
		battle.HideUI();
	}
	
}
