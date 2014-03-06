using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleQuest : UIBase {
	private Coordinate roleInitPosition = new Coordinate();
	public Coordinate RoleInitPosition {
		get { 
			if(roleInitPosition.x != MapConfig.characterInitCoorX) {
				roleInitPosition.x = MapConfig.characterInitCoorX;
				roleInitPosition.y = MapConfig.characterInitCoorY;
			}
			return  roleInitPosition;
		}
	}

	private GameObject rootObject;
	private TQuestGrid currentMapData;
	public static TQuestDungeonData questDungeonData;
	private BattleMap battleMap;
	private Role role;
	private Battle battle;
	private BattleBackground background;
	public static BattleUseData bud;
	private Camera mainCamera;
	private BossAppear bossAppear;
	private int questFloor = 0;

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
		questDungeonData = ModelManager.Instance.GetData (ModelEnum.MapConfig,new ErrorMsg()) as TQuestDungeonData;
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

	void Reset () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud = new BattleUseData ();
//		mainCamera = Camera.main;
//		mainCamera.clearFlags = CameraClearFlags.Depth;
		battleMap.HideUI ();
		role.HideUI ();
		background.HideUI ();
		mainCamera.enabled = false;
		battleMap.ShowUI ();
		role.ShowUI ();
		background.ShowUI ();
		GameTimer.GetInstance ().AddCountDown (1f, ShowScene);
		InitData ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
		if (bossAppear == null) {
			CreatBoosAppear();
		}
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
//		Debug.LogError ("ClickDoor : " + questFloor + " mapConfig.floor : " + mapConfig.floor);
//		if (questFloor == mapConfig.floor) {
		if(questDungeonData.currentFloor == questDungeonData.Floors.Count - 1){
			QuestStop ();
		} else {
			EnterNextFloor();
		}
	}

	void EnterNextFloor () {
		questDungeonData.currentFloor ++;
		Reset ();
	}

	void QuestStop () {
		bossAppear.PlayBossAppera (MeetBoss);
		role.Stop();
		MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
		battleEnemy = true;
	}

	void QuestEnd () { }

	public void RoleCoordinate(Coordinate coor) {
		if(!battleMap.ReachMapItem (coor)) {
			if(coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				battleMap.RotateAnim(null);
				return;
			}

			currentMapData =  questDungeonData.GetSingleFloor(coor);  //mapConfig.mapData[coor.x,coor.y];
			role.Stop();
//			Debug.LogError("currentMapData.Type : " + currentMapData.Type);
			MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
			switch (currentMapData.Type) {
			case EQuestGridType.Q_NONE:
				battleMap.waitMove = true;
//				Debug.LogError(Time.realtimeSinceStartup + " Q_NONE : " + battleMap.waitMove);
				battleMap.RotateAnim(MapItemNone);
				break;
			case EQuestGridType.Q_ENEMY:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemEnemy);
				break;
			case EQuestGridType.Q_KEY:
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemKey);
				break;
			case EQuestGridType.Q_TREATURE:				
				battleMap.waitMove = true;
				battleMap.ShowBox();
				battleMap.RotateAnim(MapItemCoin);
				break;
			case EQuestGridType.Q_TRAP:
//				Debug.LogError("coor : " + coor.x + "    " + coor.y + "     " + Time.realtimeSinceStartup);
				battleMap.waitMove = true;
//				Debug.LogError(Time.realtimeSinceStartup + " Q_TRAP : " + battleMap.waitMove);
				battleMap.RotateAnim(MapItemTrap);
				break;
			case EQuestGridType.Q_QUESTION:
				battleMap.waitMove = true;
//				Debug.LogError(Time.realtimeSinceStartup + " Q_TRAP : " + battleMap.waitMove);
				battleMap.RotateAnim(MeetQuestion);
				break;
			case EQuestGridType.Q_EXCLAMATION : 
				battleMap.waitMove = true;
				battleMap.RotateAnim(MapItemExclamation);
				break;
			default:
//				Debug.LogError("RoleCoordinate default : " + currentMapData.Type);
				battleMap.waitMove = false;
				MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
					break;
			}
		}
	}

	void MeetQuestion () {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MeetBoss () {
		battleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < questDungeonData.Boss.Count; i++) {
			TEnemyInfo tei = questDungeonData.Boss[i];
			tei.EnemySymbol = (uint)i;
			temp.Add(tei);
		}
//		 = questDungeonData.Boss; //bud.GetEnemyInfo(mapConfig.BossID);
		bud.InitEnemyInfo (temp);
		battle.ShowEnemy(temp);
	}

	void MapItemExclamation() {
		battleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}
	
	void MapItemTrap() {
		battleMap.waitMove = false;
		TrapBase tb = currentMapData.TrapInfo;
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
//		Debug.LogError(Time.realtimeSinceStartup + " Q_NONE : " + battleMap.waitMove);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemEnemy() {
		battleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < currentMapData.Enemy.Count; i++) {
			TEnemyInfo tei = currentMapData.Enemy[i];
			tei.EnemySymbol = (uint)i;
			temp.Add(tei);
		}
		bud.InitEnemyInfo (temp);
		battle.ShowEnemy (temp);
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
			battleMap.BattleEndRotate();
			GameTimer.GetInstance().AddCountDown(2f,End);
		}
	}

	void End() {
		GameObject obj = Resources.Load("Prefabs/Victory") as GameObject;
		Vector3 tempScale = obj.transform.localScale;
		obj = NGUITools.AddChild(viewManager.CenterPanel,obj);
		obj.transform.localScale = tempScale;
		VictoryEffect ve = obj.GetComponent<VictoryEffect>();
		ve.Init("Victory");
		ve.PlayAnimation(QuestEnd,new VictoryInfo(100,0,0,100));
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
