using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleQuest : UIBase {
	private GameObject rootObject;
	private TQuestGrid currentMapData;
	public TQuestDungeonData questDungeonData;
	public BattleMap battleMap;
	public Role role;
	public Battle battle;
	private BattleBackground background;
	public QuestFullScreenTips questFullScreenTips;
	public TopUI topUI;
	public static BattleUseData bud;

	private List< TClearQuestParam > _questData = new List<TClearQuestParam>();
	public TClearQuestParam questData {
		get { 
			if(_questData.Count == 0) {
				ClearQuestParam qp = new ClearQuestParam();
				TClearQuestParam cqp = new TClearQuestParam(qp);
				_questData.Add(cqp);
			}
			return _questData[_questData.Count - 1];
		}
	}
	private TUserUnit evolveUser;
	private string backgroundName = "BattleBackground";
	private AttackEffect attackEffect;

	private Queue<MapItem> chainLikeMapItem = new Queue<MapItem> ();
	public static bool ChainLinkBattle = false;

	private ConfigBattleUseData configBattleUseData; 

	public BattleQuest (string name) : base(name) {
		configBattleUseData = ConfigBattleUseData.Instance;
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
		questData.questId = questDungeonData.QuestId;
		InitTopUI ();
		battle = new Battle("Battle");
		battle.CreatUI();
		battle.HideUI ();
		CreatEffect ();
		bud = new BattleUseData (this);

		AddSelfObject (battleMap);
		AddSelfObject (role);
		AddSelfObject (background);
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
		topUI.battleQuest = this;
		topUI.RefreshTopUI (questDungeonData, _questData);
		AddSelfObject (topUI);
	}
	
	void InitData() {
		if (questFullScreenTips == null) {
			CreatBoosAppear();
		}
		questDungeonData = configBattleUseData.questDungeonData; //GetData (ModelEnum.MapConfig,new ErrorMsg()) as TQuestDungeonData;
		_questData = configBattleUseData.storeBattleData.questData;
	}

	void Init(UIBaseUnity ui,string name) {
		ui.Init(name);
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RecoverHP);
		MsgCenter.Instance.AddListener (CommandEnum.LeaderSkillEnd, LeaderSkillEnd);
		Resources.UnloadUnusedAssets ();
		GameTimer.GetInstance ().AddCountDown (0.5f, ShowScene);
		InitData ();
		base.ShowUI ();
		AddListener ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData, null);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.GridEnd, GridEnd);
		MsgCenter.Instance.AddListener (CommandEnum.PlayerDead, BattleFail);
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillStandReady, ActiveSkillStandReady);

		if (configBattleUseData.hasBattleData ()) {
			ContineBattle ();
		} else {
			configBattleUseData.StoreData();
		}
	}

	public override void HideUI () {
		battleEnemy = false;
		if( bud != null ) {
			bud.RemoveListen ();
			bud = null;
		}
		RemoveListener ();
		base.HideUI ();
		
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.GridEnd, GridEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.PlayerDead, BattleFail);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RecoverHP);
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillStandReady, ActiveSkillStandReady);
	}
	
	void LeaderSkillEnd(object data) {
		MsgCenter.Instance.RemoveListener (CommandEnum.LeaderSkillEnd, LeaderSkillEnd);
	}

	void ReadyMove() {
		battle.ShieldInput (true);
	}

	void AttackEnemy (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		attackEffect.RefreshItem (ai);
	}

	void RecoverHP(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		attackEffect.RefreshItem (ai);
	}

	void Reset () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud.Reset ();
		battleMap.HideUI ();
		role.HideUI ();
		background.HideUI ();
		battleMap.ShowUI ();
		role.ShowUI ();
		background.ShowUI ();
//		GameTimer.GetInstance ().AddCountDown (1f, ShowScene);
		InitData ();
		topUI.Reset ();
		topUI.RefreshTopUI (questDungeonData, _questData);
		BattleMap.waitMove = false;
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
		role.StartMove (coor);
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
			MsgWindowParams mwp = new MsgWindowParams ();
			mwp.btnParams = new BtnParam[2];
			mwp.titleText = "Retry";
			mwp.contentText = "Use two stone to retry this floor of quest ?";
			
			BtnParam sure = new BtnParam ();
			sure.callback = SureRetry;
			sure.text = "OK";
			mwp.btnParams[0] = sure;
			
			sure = new BtnParam ();
			sure.callback = EnterNextFloor;
			sure.text = "Cancel";
			mwp.btnParams[1] = sure;
		}
	}

	void EnterNextFloor (object data) {
		questDungeonData.currentFloor ++;
		ClearQuestParam clear = new ClearQuestParam ();
		TClearQuestParam cqp = new TClearQuestParam (clear);
		_questData.Add (cqp);
		topUI.SetFloor (questDungeonData.currentFloor + 1, questDungeonData.Floors.Count);
		Reset ();
		if (BattleUseData.maxEnergyPoint >= 10) {
			BattleUseData.maxEnergyPoint = DataCenter.maxEnergyPoint;
		} 
		else {
			BattleUseData.maxEnergyPoint += 10;
		}
	}

	void QuestStop () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
		battle.ShieldInput (false);
		questFullScreenTips.ShowTexture (QuestFullScreenTips.BossAppears, MeetBoss);
		role.Stop();
		battleEnemy = true;
	}
	
	public void QuestEnd () {
		if ( configBattleUseData.currentStageInfo.Type == QuestType.E_QUEST_STORY ) { // story quest
			DataCenter.Instance.QuestClearInfo.UpdateStoryQuestClear (configBattleUseData.currentStageInfo.ID, configBattleUseData.currentQuestInfo.ID);
		}else { // event quest
			DataCenter.Instance.QuestClearInfo.UpdateEventQuestClear (configBattleUseData.currentStageInfo.ID, configBattleUseData.currentQuestInfo.ID);
		}

		if (configBattleUseData.BattleFriend != null && configBattleUseData.BattleFriend.FriendPoint > 0) {
			HaveFriendExit ();
		} else {
			NoFriendExit();
		}
	}

	public void NoFriendExit() {
//		ExitFight (true);
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ExitBattle ();
	}

	public void HaveFriendExit() {
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ChangeScene(SceneEnum.Result);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, configBattleUseData.BattleFriend);
	}

	void EvolveEnd () {
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, evolveUser);
	}

	private EQuestGridType gridType = EQuestGridType.Q_NONE;
	private Coordinate _currentCoor;
	public Coordinate currentCoor {
		set { _currentCoor = value; configBattleUseData.storeBattleData.roleCoordinate = _currentCoor; }
		get { return _currentCoor;}
	}

	void YieldShowAnim() {
		int count = bud.Els.CheckLeaderSkillCount();
		battle.ShieldInput (false);
		questFullScreenTips.ShowTexture (QuestFullScreenTips.ReadyMove, ReadyMove, count * AttackController.normalAttackInterv);
		bud.InitBattleUseData(null);
	}

	void InitContinueData() {
		bud.Els.CheckLeaderSkillCount();
		bud.InitBattleUseData (configBattleUseData.storeBattleData);
	}

	public void ContineBattle () {
		Coordinate coor = configBattleUseData.storeBattleData.roleCoordinate;
		InitContinueData ();
		if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
			return;	
		}
		currentMapData = questDungeonData.GetSingleFloor (coor);
		battleMap.ChangeStyle (coor);
		role.Stop ();

		TStoreBattleData sbd = configBattleUseData.storeBattleData;

		// 0 is not in fight.
		if (sbd.isBattle == 0) { 
			if (sbd.recoveBattleStep == RecoveBattleStep.RB_BossDead) {
				BossDead();
				return;
			}

			return;	
		}

		BattleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < sbd.enemyInfo.Count; i++) {
			TEnemyInfo tei = new TEnemyInfo(sbd.enemyInfo[i]);
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
			DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}

		if (sbd.isBattle == 1) { // 1 == battle enemy
			currentMapData.Enemy = temp;
			bud.InitEnemyInfo (currentMapData);
			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_enemy_battle);
			if(sbd.attackRound == 0) { // 0 == first attack
				GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
			}
		}
		else if (sbd.isBattle == 2) {	// 2 == battle boss
			battleEnemy = true;
			battle.ShieldInput (true);
			questDungeonData.Boss= temp;
			bud.InitBoss (questDungeonData.Boss);
			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
		}
		battle.ShowEnemy(temp);
		ExitFight (false);
		GameTimer.GetInstance ().AddCountDown (0.1f, RecoverBuff);
	}

	void RecoverBuff() {
		ExcuteDiskActiveSkill(configBattleUseData.posionAttack);
		ExcuteDiskActiveSkill(configBattleUseData.reduceHurtAttack);
		ExcuteDiskActiveSkill(configBattleUseData.reduceDefenseAttack);
		ExcuteDiskActiveSkill(configBattleUseData.strengthenAttack);
	}

	void ExcuteDiskActiveSkill (AttackInfo ai) {
		if (ai != null) {
			IActiveSkillExcute iase = bud.excuteActiveSkill.GetActiveSkill(ai.UserUnitID);
			Debug.LogError(" ExcuteDiskActiveSkill : " + iase);
			if(iase != null) {
				iase.ExcuteByDisk(ai);
			}
		}
	}

	public void RoleCoordinate(Coordinate coor) {
		if (!battleMap.ReachMapItem (coor)) {
			if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				battleMap.prevMapItem.HideGridNoAnim ();
				GameTimer.GetInstance ().AddCountDown (0.2f, YieldShowAnim);
				return;
			}

			int index = questDungeonData.GetGridIndex (coor);
			if (index != -1) {
				questData.hitGrid.Add ((uint)index);
			}

			currentMapData = questDungeonData.GetSingleFloor (coor);
			role.Stop ();
			if (currentMapData.Star == EGridStar.GS_KEY) {
				BattleMap.waitMove = true;
				configBattleUseData.storeBattleData.HitKey = true;
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
					QuestCoorEnd ();
					break;
			}
		}
		else {
			configBattleUseData.StoreMapData(null);
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
		AudioManager.Instance.PlayAudio (AudioEnum.sound_trigger_trap);
		BattleMap.waitMove = false;
		TrapBase tb = currentMapData.TrapInfo;
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);

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

	public void QuestCoorEnd() {
		if ( DGTools.EqualCoordinate (currentCoor, MapConfig.endCoor)) {
			MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, true);
		} else {
			MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, false);
		}
	}

	public void MeetBoss () {
		battle.ShieldInput (true);

		BattleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < questDungeonData.Boss.Count; i++) {
			TEnemyInfo tei = questDungeonData.Boss [i];
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
		}
		bud.InitBoss (questDungeonData.Boss);
		configBattleUseData.storeBattleData.isBattle = 2; // 2 == battle boss. 
		battle.ShowEnemy(temp);

		ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	public void MapItemEnemy() {
		BattleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < currentMapData.Enemy.Count; i++) {
			TEnemyInfo tei = currentMapData.Enemy[i];
			tei.EnemySymbol = (uint)i;
			temp.Add(tei);
			Debug.LogError(" MapItemEnemy : id : " + tei.EnemyID + " blood : " + tei.initBlood);

			DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}
		bud.InitEnemyInfo (currentMapData);
		configBattleUseData.storeBattleData.isBattle = 1;	// 1 == battle enemy
		battle.ShowEnemy (temp);
		ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_enemy_battle);
		GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
	}

	void ExitFight(bool exit) {
		if (exit) {
			configBattleUseData.storeBattleData.isBattle = 0;
		}
		BattleBottom bb = background.battleBottomScript;
		if (bb != null) {
			bb.Close();	
		}

		if(battleMap.gameObject.activeSelf != exit)
			battleMap.gameObject.SetActive (exit);
		if(role.gameObject.activeSelf != exit)
			role.gameObject.SetActive (exit);
	}

	public void StartBattleEnemyAttack() {
		EnemyAttackEnum eae = battleMap.FirstOrBackAttack ();
		switch (eae) {
		case EnemyAttackEnum.BackAttack:
			questFullScreenTips.ShowTexture (QuestFullScreenTips.BackAttack, BackAttack);
			battle.ShieldInput (false);
			break;
		case EnemyAttackEnum.FirstAttack:
			questFullScreenTips.ShowTexture (QuestFullScreenTips.FirstAttack, FirstAttack);
			break;
		default:
			break;
		}
	}

	void BackAttack() {
		bud.ac.AttackPlayer ();
		battle.BattleCardIns.StartBattle (false);
	}

	void FirstAttack() {
		bud.ac.FirstAttack ();
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

	void BossDead() {
		battle.ShieldInput (false);
		BattleBottom.notClick = true;
		questFullScreenTips.ShowTexture (QuestFullScreenTips.QuestClear, QuestClear);
	}

	void BattleEnd(object data) {
		QuestCoorEnd ();

		ExitFight (true);
		bool b = false;
		if (data != null) {
			b = (bool)data;	
		}

		if (battleEnemy && !b) {
			BossDead();
			configBattleUseData.storeBattleData.recoveBattleStep = RecoveBattleStep.RB_BossDead;
			configBattleUseData.StoreMapData (null);
			return;
		}

		configBattleUseData.storeBattleData.recoveBattleStep = RecoveBattleStep.RB_None;
		configBattleUseData.StoreMapData (_questData);

		int index = questDungeonData.GetGridIndex (currentCoor);
		if (index == -1) {
			return;	
		}
		uint uIndex = (uint)index;
		if (questData.hitGrid.Contains (uIndex)) {
			index = questData.hitGrid.FindIndex(a=>a == uIndex);
			if(index != questData.hitGrid.Count - 1)
				return;		
		}

		TQuestGrid tqg = questDungeonData.GetSingleFloor (currentCoor);
	
		if (tqg == null || tqg.Type != EQuestGridType.Q_ENEMY) {
			return;	
		}

		battleMap.AddMapSecuritylevel (currentCoor);
		chainLikeMapItem = battleMap.AttakAround (currentCoor);	
		if (chainLikeMapItem.Count == 0) {
			if (chainLikeMapItem.Count > 0) {
				ChainLinkBattle = true;
				role.SyncRoleCoordinate (chainLikeMapItem.Dequeue ().Coor);
			}
			else{
				ChainLinkBattle = false;
			}
		} else {
			role.SyncRoleCoordinate (chainLikeMapItem.Dequeue ().Coor);
		}
	}

	void QuestClear() {
		battle.ShieldInput(true);
		BattleMap.waitMove = true;
		topUI.SheildInput (false);
		battleMap.BattleEndRotate(battleMap.door.ShowTapToCheckOut);
	}
	
	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleBaseData, BattleBase);
	}

	void BattleBase (object data) {
		BattleBaseData bbd = data as BattleBaseData;
		background.InitData (bbd.Blood, bbd.maxBlood, bbd.EnergyPoint);
	}

	public void CheckOut () {
		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParams = new BtnParam[2];
		mwp.titleText = "Retry";
		mwp.contentText = "Use one stone to retry this floor of quest ?";
		
		BtnParam sure = new BtnParam ();
		sure.callback = SureRetry;
		sure.text = "OK";
		mwp.btnParams[0] = sure;
		
		sure = new BtnParam ();
		sure.callback = QuestClearShow;
		sure.text = "Cancel";
		mwp.btnParams[1] = sure;

		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
	}

	public void Retry () {
		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParams = new BtnParam[2];
		mwp.titleText = "Retry";
		mwp.contentText = "Use one stone to retry this floor of quest ?";

		BtnParam sure = new BtnParam ();
		sure.callback = SureInitiativeRetry;
		sure.text = "OK";
		mwp.btnParams[0] = sure;

		sure = new BtnParam ();
		sure.callback = CancelInitiativeRetry;
		sure.text = "Cancel";
		mwp.btnParams[1] = sure;

		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
	}

	void SureRetry(object data) {
		battle.ShieldInput (false);
		RedoQuest.SendRequest (SureRetryNetWork, questDungeonData.QuestId, questDungeonData.currentFloor);
	}

	void SureInitiativeRetry(object data) {
		battle.ShieldInput (false);
		if (battle.isShowEnemy) {
			MsgCenter.Instance.Invoke(CommandEnum.BattleEnd);
		}
		RedoQuest.SendRequest (SureRetryNetWork, questDungeonData.QuestId, questDungeonData.currentFloor);
	}

	void CancelInitiativeRetry(object data) {
//		NoFriendExit ();
	}

//	object tempData = null;
	void SureRetryNetWork(object data) {
		BattleMap.waitMove = false;
		battleMap.BattleEndRotate(null);
		RefreshRetryData (data);
		main.GInput.IsCheckInput = true;
		GameInput.OnPressEvent += SureRetryPress;
	}

	void RefreshRetryData(object data) {
		RspRedoQuest rrq = data as RspRedoQuest;
		if (rrq == null) {
			return;	
		}
		DataCenter.Instance.AccountInfo.Stone = rrq.stone;
		_questData.RemoveAt (_questData.Count - 1);
		ClearQuestParam cq = new ClearQuestParam ();
		TClearQuestParam cqp = new TClearQuestParam (cq);
		_questData.Add (cqp);
		TQuestDungeonData tqdd = new TQuestDungeonData (rrq.dungeonData);
		int floor = questDungeonData.currentFloor;
		List<TQuestGrid> reQuestGrid = tqdd.Floors[floor];
		questDungeonData.Floors [floor] = reQuestGrid;
		questDungeonData.Boss = tqdd.Boss;
		configBattleUseData.roleInitCoordinate =  new Coordinate (MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);
		configBattleUseData.storeBattleData.roleCoordinate = configBattleUseData.roleInitCoordinate ;
		configBattleUseData.storeBattleData.questData = _questData;
		configBattleUseData.StoreMapData (_questData);
	}

	void SureRetryPress() {
		GameInput.OnPressEvent -= SureRetryPress;
		RetryRefreshUI ();
	}

	void RetryRefreshUI() {
		battle.ShieldInput (true);
		if (battle.isShowEnemy) {
			ExitFight (true);
			configBattleUseData.storeBattleData.attackRound = 0;
			configBattleUseData.StoreMapData(null);
			battle.ExitFight();
		}
		Reset ();
		bud.ResetBlood ();
	}

	void CancelRetry(object data) {
		RequestData ();
	}

	void QuestClearShow(object data) {
		configBattleUseData.ClearData ();
		RequestData ();
	}

	public TClearQuestParam GetQuestData () {
		ClearQuestParam cq = new ClearQuestParam ();
		TClearQuestParam cqp = new TClearQuestParam (cq);
		cqp.questId = questDungeonData.QuestId;
		foreach (var item in _questData) {
			cqp.getMoney += item.getMoney;
			cqp.getUnit.AddRange(item.getUnit);
			cqp.hitGrid.AddRange(item.hitGrid);
		}
		return cqp;
	}

	void RequestData () {
		if (DataCenter.gameStage == GameState.Evolve) {
			EvolveDone evolveDone = new EvolveDone ();
			TClearQuestParam cqp = GetQuestData();
			evolveDone.QuestId = cqp.questId;
			evolveDone.GetMoney = cqp.getMoney;
			evolveDone.GetUnit = cqp.getUnit;
			evolveDone.HitGrid = cqp.hitGrid;
			evolveDone.OnRequest (null, ResponseEvolveQuest);
		} else {
			INetBase netBase = new ClearQuest ();
			netBase.OnRequest (GetQuestData (), ResponseClearQuest);
		}
	}

	void ResponseEvolveQuest (object data) {
		if (data == null)
			return;
		bbproto.RspEvolveDone rsp = data as bbproto.RspEvolveDone;

		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);

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
//		DataCenter.Instance.MyUnitList.DelMyUnit(tes.EvolveStart.BaseUnitId);
		DataCenter.Instance.UserUnitList.DelMyUnit(tes.EvolveStart.BaseUnitId);
		for (int i = 0; i < tes.EvolveStart.PartUnitId.Count; i++) {
//			DataCenter.Instance.MyUnitList.DelMyUnit(tes.EvolveStart.PartUnitId[i]);
			DataCenter.Instance.UserUnitList.DelMyUnit(tes.EvolveStart.PartUnitId[i]);
		}
		for (int i = 0; i < rsp.gotUnit.Count; i++) {
//			DataCenter.Instance.MyUnitList.AddMyUnit(rsp.gotUnit[i]);
			DataCenter.Instance.UserUnitList.AddMyUnit(rsp.gotUnit[i]);
		}
//		DataCenter.Instance.MyUnitList.AddMyUnit(rsp.evolvedUnit);
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
//		battle.SwitchInput (true);
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
		battle.ShieldInput (true);

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParams = new BtnParam[2];
		mwp.titleText = "Retry";
		mwp.contentText = "Use one stone to recover sp and recover hp ?";
		
		BtnParam sure = new BtnParam ();
		sure.callback = BattleFailRecover;
		sure.text = "OK";
		mwp.btnParams[0] = sure;
		
		sure = new BtnParam ();
		sure.callback = BattleFailExit;
		sure.text = "Cancel";
		mwp.btnParams[1] = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
	}

	void BattleFailRecover(object data) {
		ResumeQuest.SendRequest (ResumeQuestNet, questDungeonData.QuestId);
		bud.ClearData ();
	}
	
	void ResumeQuestNet(object data) {
		bud.Blood = bud.maxBlood;
		bud.RecoverEnergePoint (DataCenter.maxEnergyPoint);
		configBattleUseData.StoreMapData (null);
	}

	void BattleFailExit(object data) {
		questFullScreenTips.ShowTexture (QuestFullScreenTips.GameOver, BattleFail);
		configBattleUseData.ClearData ();
	}

	void BattleFail () {
		Battle.colorIndex = 0;
		Battle.isShow = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd);
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ExitBattle ();
	}
}
