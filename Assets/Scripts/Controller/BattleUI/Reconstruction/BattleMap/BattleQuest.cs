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
	public Battle battle;
	private BattleBackground background;
	public static BattleUseData bud;
	private Camera mainCamera;
	private BossAppear bossAppear;
	private ClearQuestParam questData;
	private TUserUnit evolveUser;
	private TopUI topUI;
	private string backgroundName = "BattleBackground";

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
		background.SetBattleQuest (this);
		AddSelfObject (battleMap);
		AddSelfObject (role);
		AddSelfObject (background);
		questData = new ClearQuestParam ();
		questData.questId = questDungeonData.QuestId;
		InitTopUI ();

		battle = new Battle("Battle");
		battle.CreatUI();
		battle.HideUI ();
	}

	void InitTopUI () {
		GameObject go = Resources.Load ("Prefabs/Fight/TopUI") as GameObject;
		go = GameObject.Instantiate (go) as GameObject;
		go.transform.parent = viewManager.TopPanel.transform;
		go.transform.localScale = Vector3.one;
		topUI = go.GetComponent<TopUI> ();
		topUI.Init ("TopUI");
		topUI.RefreshTopUI (questDungeonData, questData);
		AddSelfObject (topUI);
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
		MsgCenter.Instance.AddListener (CommandEnum.GridEnd, GridEnd);
		MsgCenter.Instance.AddListener (CommandEnum.PlayerDead, BattleFail);
	}

	public override void HideUI () {
		battleEnemy = false;
		if( bud != null ) {
			bud.RemoveListen ();
			bud = null;
		}
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		RemoveListener ();
		base.HideUI ();
		
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.GridEnd, GridEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.PlayerDead, BattleFail);
	}

	void Reset () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud = new BattleUseData ();
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
		Resources.UnloadUnusedAssets ();
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
		UIManager.Instance.baseScene.PrevScene = SceneEnum.Evolve;
		UIManager.Instance.ExitBattle();
	}

	bool battleEnemy = false;
	public void ClickDoor () {
		if(questDungeonData.currentFloor == questDungeonData.Floors.Count - 1){
			QuestStop ();
		} else {
			EnterNextFloor();
		}
	}

	void EnterNextFloor () {
		questDungeonData.currentFloor ++;
		topUI.SetFloor (questDungeonData.currentFloor + 1, questDungeonData.Floors.Count);
		Reset ();
	}

	void QuestStop () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);

		bossAppear.PlayBossAppera (MeetBoss);
		role.Stop();
		MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
		battleEnemy = true;
	}

	void QuestEnd () { 
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ExitBattle ();
	}

	void EvolveEnd () {
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, evolveUser);
	}

	public void RoleCoordinate(Coordinate coor) {
		if(!battleMap.ReachMapItem (coor)) {
			if(coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				battleMap.RotateAnim(null);
				return;
			}
			int index = questDungeonData.GetGridIndex(coor);
			if(index != -1) {
				questData.hitGrid.Add((uint)index);
			}
			currentMapData =  questDungeonData.GetSingleFloor(coor);
			role.Stop();
			MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);

			if(currentMapData.Star == EGridStar.GS_KEY) {
				BattleMap.waitMove = true;
				battleMap.RotateAnim(MapItemKey);
				return;
			}
			AudioManager.Instance.PlayAudio(AudioEnum.sound_grid_turn);
			switch (currentMapData.Type) {
			case EQuestGridType.Q_NONE:
				BattleMap.waitMove = true;
				battleMap.RotateAnim(MapItemNone);
				break;
			case EQuestGridType.Q_ENEMY:
				BattleMap.waitMove = true;
				battleMap.RotateAnim(MapItemEnemy);
				break;
			case EQuestGridType.Q_KEY:
				break;
			case EQuestGridType.Q_TREATURE:				
				BattleMap.waitMove = true;
				battleMap.ShowBox();
				battleMap.RotateAnim(MapItemCoin);
				break;
			case EQuestGridType.Q_TRAP:
				BattleMap.waitMove = true;
				battleMap.RotateAnim(MapItemTrap);
				break;
			case EQuestGridType.Q_QUESTION:
				BattleMap.waitMove = true;
				battleMap.RotateAnim(MeetQuestion);
				break;
			case EQuestGridType.Q_EXCLAMATION : 
				BattleMap.waitMove = true;
				battleMap.RotateAnim(MapItemExclamation);
				break;
			default:
				BattleMap.waitMove = false;
				MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
				break;
			}
		}
	}

	void GridEnd(object data) {
		if (currentMapData.Drop != null && currentMapData.Drop.DropId != 0) {
			questData.getUnit.Add (currentMapData.Drop.DropId);	
			topUI.Drop = questData.getUnit.Count;
//			topUI.RefreshTopUI (questData, questDungeonData);
		}
	}

	void MeetQuestion () {
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MeetBoss () {
		BattleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < questDungeonData.Boss.Count; i++) {
			TEnemyInfo tei = questDungeonData.Boss [i];
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
		}

		bud.InitBoss (questDungeonData.Boss);
		battle.ShowEnemy(temp);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	void MapItemExclamation() {
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}
	
	void MapItemTrap() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_trigger_trap);
		BattleMap.waitMove = false;
		TrapBase tb = currentMapData.TrapInfo;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
	}

	void MapItemCoin() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_treasure);
		BattleMap.waitMove = false;
		questData.getMoney += currentMapData.Coins;
		topUI.Coin = questData.getMoney;
//		topUI.RefreshTopUI (questData, questDungeonData);

		MsgCenter.Instance.Invoke (CommandEnum.MeetCoin, currentMapData);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemKey() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_key);
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		MsgCenter.Instance.Invoke (CommandEnum.OpenDoor, null);
	}

	void MapItemNone () {
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemEnemy() {
		BattleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < currentMapData.Enemy.Count; i++) {
			TEnemyInfo tei = currentMapData.Enemy[i];
			tei.EnemySymbol = (uint)i;
			temp.Add(tei);
		}
		bud.InitEnemyInfo (currentMapData);
		battle.ShowEnemy (temp);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_enemy_battle);
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
		bool b = false;
		if (data != null) {
			b = (bool)data;	
		}

		if (battleEnemy && !b) {
			battle.SwitchInput(true);
			RequestData();
			battleMap.BattleEndRotate();
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

	void RequestData () {
		if (DataCenter.gameStage == GameState.Evolve) {
			EvolveDone evolveDone = new EvolveDone ();
			evolveDone.QuestId = questData.questId;
			evolveDone.GetMoney = questData.getMoney;
			evolveDone.GetUnit = questData.getUnit;
			evolveDone.HitGrid = questData.hitGrid;
			evolveDone.OnRequest (null, ResponseEvolveQuest);
		} else {
			INetBase netBase = new ClearQuest ();
			netBase.OnRequest (questData, ResponseClearQuest);
		}
	}

	void ResponseEvolveQuest (object data) {
		if (data == null)
			return;
		bbproto.RspEvolveDone rsp = data as bbproto.RspEvolveDone;

		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			LogHelper.Log("ReqEvolveDone code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}

		DataCenter.Instance.UserInfo.Rank = rsp.rank;
		DataCenter.Instance.UserInfo.Exp = rsp.exp;
		DataCenter.Instance.AccountInfo.Money = rsp.money;
		DataCenter.Instance.AccountInfo.FriendPoint = rsp.friendPoint;
		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaMax = rsp.staminaMax;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;	
		TEvolveStart tes = DataCenter.evolveInfo;
		DataCenter.Instance.MyUnitList.DelMyUnit(tes.EvolveStart.BaseUnitId);
		DataCenter.Instance.UserUnitList.DelMyUnit(tes.EvolveStart.BaseUnitId);
		for (int i = 0; i < tes.EvolveStart.PartUnitId.Count; i++) {
			DataCenter.Instance.MyUnitList.DelMyUnit(tes.EvolveStart.PartUnitId[i]);
			DataCenter.Instance.UserUnitList.DelMyUnit(tes.EvolveStart.PartUnitId[i]);
		}
		for (int i = 0; i < rsp.gotUnit.Count; i++) {
			DataCenter.Instance.MyUnitList.AddMyUnit(rsp.gotUnit[i]);
			DataCenter.Instance.UserUnitList.AddMyUnit(rsp.gotUnit[i]);
		}
		DataCenter.Instance.MyUnitList.AddMyUnit(rsp.evolvedUnit);
		DataCenter.Instance.UserUnitList.AddMyUnit(rsp.evolvedUnit);
		evolveUser = TUserUnit.GetUserUnit (DataCenter.Instance.UserInfo.UserId, rsp.evolvedUnit);

		TRspClearQuest trcq = new TRspClearQuest ();
		trcq.exp = rsp.exp;
		trcq.gotExp = rsp.gotExp;
		trcq.money = rsp.money;
		trcq.gotMoney = rsp.gotMoney;
		trcq.gotStone = rsp.gotStone;
		List<TUserUnit> temp = new List<TUserUnit> ();
		for (int i = 0; i <  rsp.gotUnit.Count; i++) {
			TUserUnit tuu = TUserUnit.GetUserUnit(DataCenter.Instance.UserInfo.UserId,rsp.gotUnit[i]);
			temp.Add(tuu);
		}
		trcq.gotUnit = temp;
		trcq.rank = rsp.rank;
		DataCenter.Instance.oldAccountInfo = DataCenter.Instance.UserInfo;
		End (trcq, EvolveEnd);
	}

	void ResponseClearQuest (object data) {
		if ( data != null ) {
			DataCenter.Instance.oldAccountInfo = DataCenter.Instance.UserInfo;
			TRspClearQuest clearQuest = data as TRspClearQuest;
			DataCenter.Instance.RefreshUserInfo (clearQuest);
			End (clearQuest,QuestEnd);
		}
	}

	void End(TRspClearQuest clearQuest,Callback questEnd) {
		battle.SwitchInput (true);
		Battle.colorIndex = 0;
		Battle.isShow = false;
		GameObject obj = Resources.Load("Prefabs/Victory") as GameObject;
		Vector3 tempScale = obj.transform.localScale;
		obj = NGUITools.AddChild(viewManager.CenterPanel,obj);
		obj.transform.localScale = tempScale;
		VictoryEffect ve = obj.GetComponent<VictoryEffect>();
		ve.Init("Victory");
		ve.ShowData (clearQuest);
		ve.PlayAnimation(questEnd);

		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_victory);
	}

	void BattleFail(object data) {
		battle.SwitchInput (true);
		Battle.colorIndex = 0;
		Battle.isShow = false;
		QuestEnd ();
	}
}
