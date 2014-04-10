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
	public QuestFullScreenTips questFullScreenTips;
	private TopUI topUI;
	public static BattleUseData bud;
	private ClearQuestParam questData;
	private TUserUnit evolveUser;
	private string backgroundName = "BattleBackground";
	private AttackEffect attackEffect;

	public BattleQuest (string name) : base(name) {
		Debug.LogError ("new BattleQuest");
		InitData ();
		rootObject = NGUITools.AddChild(viewManager.ParentPanel);
		string tempName = "Map";
		battleMap = viewManager.GetBattleMap(tempName) as BattleMap;
		battleMap.transform.parent = viewManager.BottomPanel.transform.parent;
		battleMap.transform.localPosition = Vector3.zero;
		battleMap.transform.localScale = Vector3.one;
		battleMap.BQuest = this;
		Init(battleMap,tempName);
		tempName = "Role";
		role = viewManager.GetBattleMap(tempName) as Role;
		role.transform.parent = viewManager.BottomPanel.transform.parent;
		role.transform.localPosition = Vector3.zero;
		role.transform.localScale = Vector3.one;
		role.BQuest = this;
		Init(role,tempName);
		background = viewManager.GetViewObject(backgroundName) as BattleBackground;
		background.transform.parent = viewManager.CenterPanel.transform.parent;
		background.transform.localPosition = Vector3.zero;
		background.Init (backgroundName);
		background.SetBattleQuest (this);

		questData = new ClearQuestParam ();
		questData.questId = questDungeonData.QuestId;
		InitTopUI ();
		battle = new Battle("Battle");
		battle.CreatUI();
		battle.HideUI ();
		CreatEffect ();
		MapCamera.IsClick = false;
		bud = new BattleUseData (this);

		AddSelfObject (battleMap);
		AddSelfObject (role);
		AddSelfObject (background);
//		AddSelfObject (battle);
//		AddSelfObject (questFullScreenTips);
	}

	void CreatEffect () {
		GameObject go = Resources.Load("Effect/AttackEffect") as GameObject;
		go = NGUITools.AddChild (ViewManager.Instance.ParentPanel, go);
		go.transform.localPosition = battle.battleRootGameObject.transform.localPosition;
		attackEffect = go.GetComponent<AttackEffect> ();
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
		if (questFullScreenTips == null) {
			CreatBoosAppear();
		}
		questDungeonData = ModelManager.Instance.GetData (ModelEnum.MapConfig,new ErrorMsg()) as TQuestDungeonData;
	}

	void Init(UIBaseUnity ui,string name) {
		ui.Init(name);
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.LeaderSkillEnd, LeaderSkillEnd);
		Resources.UnloadUnusedAssets ();
//		mainCamera = Camera.main;
//		mainCamera.enabled = false;
		GameTimer.GetInstance ().AddCountDown (0.5f, ShowScene);
		InitData ();
		base.ShowUI ();
		AddListener ();
	
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData, null);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.GridEnd, GridEnd);
		MsgCenter.Instance.AddListener (CommandEnum.PlayerDead, BattleFail);
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillStandReady, ActiveSkillStandReady);
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
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillStandReady, ActiveSkillStandReady);
	}



	void LeaderSkillEnd(object data) {
		MsgCenter.Instance.RemoveListener (CommandEnum.LeaderSkillEnd, LeaderSkillEnd);
	}

	void ReadyMove() {
		battle.ShieldInput (true);
		MapCamera.IsClick = true;
	}

	void AttackEnemy (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		attackEffect.RefreshItem (ai);
	}

	void Reset () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud = new BattleUseData (this);
		battleMap.HideUI ();
		role.HideUI ();
		background.HideUI ();
//		mainCamera.enabled = false;
		battleMap.ShowUI ();
		role.ShowUI ();
		background.ShowUI ();
		GameTimer.GetInstance ().AddCountDown (1f, ShowScene);
		InitData ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
		if (questFullScreenTips == null) {
			CreatBoosAppear();
		}
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		questFullScreenTips.DestoryUI ();
		battle.DestoryUI ();
		battle = null;
		Resources.UnloadUnusedAssets ();
	}

	void CreatBoosAppear () {
		GameObject obj = Resources.Load("Prefabs/QuestFullScreenTips") as GameObject;
		Vector3 pos = obj.transform.localPosition;
		GameObject go = NGUITools.AddChild (viewManager.EffectPanel, obj);
		go.transform.localPosition = pos;
		questFullScreenTips = go.GetComponent<QuestFullScreenTips> ();
		questFullScreenTips.Init("QuestFullScreenTips");
	}

	void ShowScene () {
//		mainCamera.enabled = true;
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
		battle.ShieldInput (false);
		questFullScreenTips.ShowTexture (QuestFullScreenTips.BossAppears, MeetBoss);
		role.Stop();
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

	private EQuestGridType gridType = EQuestGridType.Q_NONE;
	private Coordinate currentCoor;

	void YieldShowAnim() {
		int count = bud.Els.CheckLeaderSkillCount();
		battle.ShieldInput (false);
		questFullScreenTips.ShowTexture (QuestFullScreenTips.ReadyMove, ReadyMove, count * AttackController.normalAttackInterv);
		bud.InitBattleUseData();
	}

	public void RoleCoordinate(Coordinate coor) {
		currentCoor = coor;
		if (!battleMap.ReachMapItem (coor)) {
			if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				battleMap.RotateAnim (null);
				GameTimer.GetInstance().AddCountDown(0.2f,YieldShowAnim);
				return;
			}

			int index = questDungeonData.GetGridIndex (coor);
			if (index != -1) {
					questData.hitGrid.Add ((uint)index);
			}
			currentMapData = questDungeonData.GetSingleFloor (coor);
			role.Stop ();
			MsgCenter.Instance.Invoke (CommandEnum.MeetEnemy, true);

			if (currentMapData.Type != EQuestGridType.Q_TRAP && DGTools.EqualCoordinate (coor, MapConfig.endCoor)) {
					MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, true);
			} else {
					MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, false);
			}

			if (currentMapData.Star == EGridStar.GS_KEY) {
					BattleMap.waitMove = true;
					battleMap.RotateAnim (MapItemKey);
					return;
			}
			AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);
			switch (currentMapData.Type) {
			case EQuestGridType.Q_NONE:
					BattleMap.waitMove = true;
					battleMap.RotateAnim (MapItemNone);
					break;
			case EQuestGridType.Q_ENEMY:
					BattleMap.waitMove = true;
					battleMap.RotateAnim (MapItemEnemy);
					break;
			case EQuestGridType.Q_KEY:
					break;
			case EQuestGridType.Q_TREATURE:				
					BattleMap.waitMove = true;
//					battleMap.ShowBox ();
					battleMap.RotateAnim (MapItemCoin);
					break;
			case EQuestGridType.Q_TRAP:
					BattleMap.waitMove = true;
					battleMap.RotateAnim (MapItemTrap);
					break;
			case EQuestGridType.Q_QUESTION:
					BattleMap.waitMove = true;
					battleMap.RotateAnim (MeetQuestion);
					break;
			case EQuestGridType.Q_EXCLAMATION: 
					BattleMap.waitMove = true;
					battleMap.RotateAnim (MapItemExclamation);
					break;
			default:
					BattleMap.waitMove = false;
					MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
					break;
			}
		} else {
			if(DGTools.EqualCoordinate (coor, MapConfig.endCoor)) {
				MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, true);
			} else {
				MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, false);
			}
		}
	}
	
	void GridEnd(object data) {
		if (currentMapData.Drop != null && currentMapData.Drop.DropId != 0) {
			questData.getUnit.Add (currentMapData.Drop.DropId);	
			topUI.Drop = questData.getUnit.Count;
		}
	}

	void MeetQuestion () {
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}
	
	void MapItemExclamation() {
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}
	
	void MapItemTrap() {
		if (gridType == EQuestGridType.Q_TRAP) {
			MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		}

		gridType = EQuestGridType.Q_TRAP;
		AudioManager.Instance.PlayAudio (AudioEnum.sound_trigger_trap);
		BattleMap.waitMove = false;
		TrapBase tb = currentMapData.TrapInfo;
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		gridType = EQuestGridType.Q_NONE;
		if (DGTools.EqualCoordinate (currentCoor, MapConfig.endCoor)) {
			MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, true);
		} else {
			MsgCenter.Instance.Invoke(CommandEnum.QuestEnd, false);
		}
	}

	void MapItemCoin() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_treasure);
		BattleMap.waitMove = false;
		questData.getMoney += currentMapData.Coins;
		topUI.Coin = questData.getMoney;
		MsgCenter.Instance.Invoke (CommandEnum.MeetCoin, currentMapData);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemKey() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_key);
		battle.ShieldInput (false);
		questFullScreenTips.ShowTexture (QuestFullScreenTips.OpenGate, OpenGate);
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		MsgCenter.Instance.Invoke (CommandEnum.OpenDoor, null);
	}

	void OpenGate() {
		battle.ShieldInput (true);
	}

	void MapItemNone() {
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MeetBoss () {
		battle.ShieldInput (true);
		MsgCenter.Instance.Invoke(CommandEnum.MeetEnemy, true);
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
		ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
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
		ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_enemy_battle);
		GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
	}

	void ExitFight(bool exit) {
		battleMap.gameObject.SetActive (exit);
		role.gameObject.SetActive (exit);
	}

	void StartBattleEnemyAttack() {
		EnemyAttackEnum eae = battleMap.FirstOrBackAttack ();
		switch (eae) {
		case EnemyAttackEnum.BackAttack:
//			battle.ShieldInput(false);
			questFullScreenTips.ShowTexture(QuestFullScreenTips.BackAttack,null);
			bud.ac.AttackPlayer();
			break;
		case EnemyAttackEnum.FirstAttack:
//			battle.ShieldInput(false);
			questFullScreenTips.ShowTexture(QuestFullScreenTips.FirstAttack,null);
			bud.ac.FirstAttack();
			break;
		default:
			break;
		}
	}
	void AttackEnd () {
//		battle.ShieldInput(true);
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

	void ActiveSkillStandReady(object data) {
		TUserUnit tuu = data as TUserUnit;
		questFullScreenTips.ShowTexture (QuestFullScreenTips.standReady, null);
	}

	void BattleEnd(object data) {
		ExitFight (true);
		bool b = false;
		if (data != null) {
			b = (bool)data;	
		}
		if (battleEnemy && !b) {
			battle.SwitchInput(true);
			battle.ShieldInput(false);
			questFullScreenTips.ShowTexture (QuestFullScreenTips.QuestClear, QuestClear);
		}
	}

	void QuestClear() {
		battle.ShieldInput(true);
		battleMap.BattleEndRotate();
		RequestData();
	}
	
	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void BattleBase (object data) {
		BattleBaseData bbd = data as BattleBaseData;
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
			End (clearQuest, QuestEnd);
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
		ve.battleQuest = this;
		ve.ShowData (clearQuest);
		ve.PlayAnimation(questEnd);
		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_victory);
	}

	void BattleFail(object data) {
		battle.ShieldInput(true);
		battle.ShieldInput(false);
		questFullScreenTips.ShowTexture (QuestFullScreenTips.GameOver, BattleFail);
	}

	void BattleFail () {
		battle.SwitchInput (true);
		Battle.colorIndex = 0;
		Battle.isShow = false;
		QuestEnd ();
	}
}
