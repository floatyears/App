using UnityEngine;
using System.Collections.Generic;

public class BattleQuest : UIBase {
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
	private Camera mainCamera;
	private BossAppear bossAppear;

	string backgroundName = "BattleBackground";

	public BattleQuest (string name) : base(name) {

		InitData ();
		rootObject = NGUITools.AddChild(viewManager.ParentPanel);
		string tempName = "Map";
		battleMap = viewManager.GetBattleMap(tempName) as BattleMap;
		battleMap.transform.localPosition = new Vector3 (-1100f, 0f, 0f);
		battleMap.BQuest = this;
		Init(battleMap,tempName);
		tempName = "Role";
		role = viewManager.GetBattleMap(tempName) as Role;
		role.BQuest = this;
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

	void Init(UIBaseUnity ui,string name) {
		ui.Init(name);
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		Resources.UnloadUnusedAssets ();
		bud = new BattleUseData ();
		mainCamera = Camera.main;
		mainCamera.clearFlags = CameraClearFlags.Depth;
		mainCamera.enabled = false;
		GameTimer.GetInstance ().AddCountDown (0.5f, ShowScene);
		InitData ();
		base.ShowUI ();
		AddListener ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
		if (bossAppear == null) {
			CreatBoosAppear();
		}

		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
	}

	public override void HideUI () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud = null;
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		RemoveListener ();
		base.HideUI ();
		
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		bossAppear.DestoryUI ();
	}

	void CreatBoosAppear () {
		GameObject obj = Resources.Load("Prefabs/BossAppear") as GameObject;
		Vector3 pos = obj.transform.localPosition;
		GameObject go = NGUITools.AddChild (viewManager.BottomPanel, obj);
		go.transform.localPosition = pos;
		bossAppear = go.GetComponent<BossAppear> ();
		bossAppear.Init("BossAppear");
	}

	void ShowScene () {
		mainCamera.enabled = true;
	}


	  
	public Vector3 GetPosition(Coordinate coor) {
		return battleMap.GetPosition(coor.x, coor.y);
	}

	public void TargetItem(Coordinate coor) {
		role.StartMove(coor);
	}
	  
	void Exit() {
		controllerManger.ExitBattle();
		UIManager.Instance.ExitBattle();
	}

	bool battleEnemy = false;
	public void ClickDoor () {
//		Debug.LogError (bossAppear);
		bossAppear.PlayBossAppera (MeetBoss);
		role.Stop();
		MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
		battleEnemy = true;
	}

	void QuestEnd () { }

	public void RoleCoordinate(Coordinate coor) {
		if(!battleMap.ReachMapItem (coor)) {
			currentMapData = mapConfig.mapData[coor.x,coor.y];
			role.Stop();
			if(currentMapData.ContentType == MapItemEnum.Start) {
				return;
			}
//			Debug.LogError("ContentType : " + currentMapData.ContentType);
			MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
			switch (currentMapData.ContentType) {
			case MapItemEnum.None:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemNone);
				break;
			case MapItemEnum.Enemy:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemEnemy);
				break;
			case MapItemEnum.key:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemKey);
				break;
			case MapItemEnum.Coin:
				battleMap.waitMove = true;
				battleMap.ShowBox();
				battleMap.RotateAnim(MapItemCoin);
				break;
			case MapItemEnum.Trap:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemTrap);
				break;
			case MapItemEnum.Exclamation : 
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemExclamation);
				break;
			default:
					break;
			}
		}
	}

	void MeetBoss () {
		battleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = bud.GetEnemyInfo(mapConfig.BossID);
		battle.ShowEnemy(temp);
	}

	void MapItemExclamation() {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}
	
	void MapItemTrap() {
		battleMap.waitMove = false;
		TrapBase tb = GlobalData.trapInfo[currentMapData.TypeValue];
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemCoin() {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.MeetCoin, currentMapData);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemKey() {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.OpenDoor, null);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemNone () {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemEnemy() {
		battleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = bud.GetEnemyInfo(currentMapData.MonsterID);
		battle.ShowEnemy(temp);
	}

	void ShowBattle() {
		if(battle == null) {	
			battle = new Battle("Battle"); 
			battle.CreatUI();
		}

		if(battle.GetState == UIState.UIShow)
			return;

		battle.ShowUI();
	}

	void BattleEnd(object data) {
		if (battleEnemy) {
			GameObject obj = Resources.Load("Prefabs/Victory") as GameObject;
			Vector3 tempScale = obj.transform.localScale;
			obj = NGUITools.AddChild(viewManager.CenterPanel,obj);
			obj.transform.localScale = tempScale;
			VictoryEffect ve = obj.GetComponent<VictoryEffect>();
			ve.Init("Victory");
			ve.PlayAnimation(QuestEnd,new VictoryInfo(100,0,0,100));
		}
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
