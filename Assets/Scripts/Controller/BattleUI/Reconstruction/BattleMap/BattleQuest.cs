using UnityEngine;
using System.Collections.Generic;

public class BattleQuest : UIBase
{
	public int MapWidth {
		get{ return mapConfig.mapXLength; }
	}

	public int MapHeight {
		get{ return mapConfig.mapYLength; }
	}

	private Coordinate roleInitPosition = new Coordinate();

	public Coordinate RoleInitPosition {
		get { 
			if(roleInitPosition.x != mapConfig.characterInitCoorX) {
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

	public static BattleUseData bud;

	string backgroundName = "BattleBackground";

	public BattleQuest (string name) : base(name)
	{
		bud = new BattleUseData ();

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
		background.transform.parent = viewManager.CenterPanel.transform.parent;
		background.transform.localPosition = Vector3.zero;
		background.Init (backgroundName);

		AddSelfObject (battleMap);
		AddSelfObject (role);
		AddSelfObject (background);
	}

	void InitData() {
		mapConfig = ModelManager.Instance.GetData (ModelEnum.MapConfig,new ErrorMsg()) as MapConfig; //new MapConfig (); //
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
		AddListener ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
	}

	public override void HideUI ()
	{
		RemoveListener ();
		base.HideUI ();
//		StartView.mainBg.gameObject.SetActive(true);
//		StartView.menuBtns.gameObject.SetActive(true);
//		StartView.playerInfoBar.gameObject.SetActive(true);
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

			if(currentMapData.MonsterID.Contains(100)) {
				controllerManger.ExitBattle();
				UIManager.Instance.ExitBattle();
			}
				//controllerManger.ChangeScene(SceneEnum.Quest);
			else if(currentMapData.MonsterID.Count > 0)
			{
				ShowBattle();
				role.Stop();
//				List<int> temp = new List<ShowEnemyUtility>();
//				for (int i = 0; i < currentMapData.MonsterID.Count; i++) {
//					temp.Add(GlobalData.tempEnemyInfo[currentMapData.MonsterID[i]]);
//				}
//          		battle.ShowEnemy(temp);
				List<ShowEnemyUtility> temp = bud.GetEnemyInfo(currentMapData.MonsterID);
				battle.ShowEnemy(temp);
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

	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void BattleBase (object data) {
		BattleBaseData bbd = (BattleBaseData)data;
		background.InitData (bbd.Blood, bbd.EnergyPoint);
	}
}
