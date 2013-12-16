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

	private MapConfig mapConfig;

	private SingleMapData currentMapData;

	private BattleMap battleMap;
	
	private Role role;

	private Battle battle;

	public BattleQuest (string name) : base(name)
	{
		rootObject = NGUITools.AddChild(viewManager.ParentPanel);

		mapConfig = new MapConfig();

		string tempName = "Map";
		battleMap = viewManager.GetViewObject(tempName) as BattleMap;
		battleMap.BQuest = this;
		Init(battleMap,tempName);


		tempName = "Role";
		role = viewManager.GetViewObject(tempName) as Role;
		role.BQuest = this;
		Init(role,tempName);
	}

	void Init(UIBaseUnity ui,string name)
	{
		ui.transform.parent = rootObject.transform;
		ui.Init(name);
	}

	public override void CreatUI ()
	{
		base.CreatUI ();

		battleMap.CreatUI();

		role.CreatUI();
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
		battleMap.ReachMapItem(coor);

		currentMapData = mapConfig.mapData[coor.x,coor.y];

		if(currentMapData.MonsterID.Count > 0)
		{
			ShowBattle();

			role.Stop();

			battle.ShowEnemy(currentMapData.MonsterID.Count);
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
