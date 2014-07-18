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

			return _questData[ _questData.Count - 1 ];
		}
	}

	private string backgroundName = "BattleBackground";
	private AttackEffect attackEffect;

	private Queue<MapItem> chainLikeMapItem = new Queue<MapItem> ();
	public bool ChainLinkBattle = false;

	private ConfigBattleUseData configBattleUseData; 
	private RoleStateException roleStateException;

	public BattleQuest (string name) : base(name) {
		configBattleUseData = ConfigBattleUseData.Instance;
		InitData ();
		rootObject = NGUITools.AddChild(viewManager.ParentPanel);
		string tempName = "Map";
		viewManager.GetBattleMap(tempName, o =>{
			battleMap = o as BattleMap;
			battleMap.transform.parent = viewManager.TopPanel.transform;
			battleMap.transform.localPosition = new Vector3 (0f, 0f, 0f);
			battleMap.transform.localScale = Vector3.one;
			battleMap.BQuest = this;
			Init(battleMap,tempName);
			initCount++;
		});

		tempName = "Role";
		viewManager.GetBattleMap(tempName, o =>{
			role = o as Role;
			role.transform.parent = viewManager.TopPanel.transform;
			role.transform.localPosition = Vector3.zero;
			role.transform.localScale = Vector3.one;
			role.BQuest = this;
			Init(role,tempName);
			initCount++;
		});

		viewManager.GetViewObject(backgroundName , o=>{
			background = o as BattleBackground;
			background.transform.parent = viewManager.BottomPanel.transform;
			background.transform.localPosition = new Vector3(0f,20f,0f);
			background.Init (backgroundName);
			background.SetBattleQuest (this);
			questData.questId = questDungeonData.QuestId;
			InitTopUI ();
			AddSelfObject (battleMap);
			AddSelfObject (role);
			AddSelfObject (background);
			
			roleStateException = new RoleStateException ();
			roleStateException.AddListener ();
			initCount++;
		});

		battle = new Battle("Battle");
		battle.CreatUI( BattleInitEnd );
		battle.HideUI ();
		CreatEffect ();
		bud = new BattleUseData (this);
	}

	void CreatEffect () {
		if (attackEffect == null) {
			ResourceManager.Instance.LoadLocalAsset("Effect/AttackEffect", o => {
				GameObject go = NGUITools.AddChild (ViewManager.Instance.ParentPanel, o as GameObject);
				go.transform.localPosition = battle.battleRootGameObject.transform.localPosition;
				attackEffect = go.GetComponent<AttackEffect> ();	
			});
		}
	}
	
	void InitTopUI () {
		ResourceManager.Instance.LoadLocalAsset ("Prefabs/Fight/TopUI", CallbackFunc);
	}

	private void CallbackFunc(object o){
		GameObject go = GameObject.Instantiate ((o as Object) as GameObject) as GameObject;
		go.transform.parent = viewManager.TopPanel.transform;
		go.transform.localScale = Vector3.one;
		topUI = go.GetComponent<TopUI> ();
		topUI.Init ("TopUI");
		topUI.battleQuest = this;
		topUI.RefreshTopUI (questDungeonData, _questData);
		AddSelfObject (topUI);
	}
		
	public Transform GetTopUITarget () {
		return topUI.coinLabel.transform;
	}

	void InitData() {
		if (questFullScreenTips == null) {
			CreatBoosAppear();
		}
		questDungeonData = configBattleUseData.questDungeonData; 	//GetData (ModelEnum.MapConfig,new ErrorMsg()) as TQuestDungeonData;
		_questData = configBattleUseData.storeBattleData.questData;
		questDungeonData.currentFloor = _questData.Count > 0 ? _questData.Count - 1 : 0;
	}

	void Init(UIBaseUnity ui, string name) {
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
		InitData ();
		GameTimer.GetInstance ().AddCountDown (0.5f, ShowScene);
		base.ShowUI ();
		AddListener ();
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData, null);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.GridEnd, GridEnd);
		MsgCenter.Instance.AddListener (CommandEnum.PlayerDead, BattleFail);
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillStandReady, ActiveSkillStandReady);
		MsgCenter.Instance.AddListener (CommandEnum.ShowActiveSkill, ShowActiveSkill);
		MsgCenter.Instance.AddListener (CommandEnum.ShowPassiveSkill, ShowPassiveSkill);

		if (configBattleUseData.hasBattleData() > 0) {
			ContineBattle ();
		} else {
			configBattleUseData.StoreData(questDungeonData.QuestId);
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
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowActiveSkill, ShowActiveSkill);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowPassiveSkill, ShowPassiveSkill);

		roleStateException.RemoveListener ();
	}
	
	void LeaderSkillEnd(object data) {
		MsgCenter.Instance.RemoveListener (CommandEnum.LeaderSkillEnd, LeaderSkillEnd);
	}

	void ReadyMove() {
		battle.ShieldInput (true);

		NoviceGuideStepEntityManager.Instance ().StartStep ( NoviceGuideStartType.BATTLE );
	}

	void AttackEnemy (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;
		}

		attackEffect.RefreshItem (ai.UserUnitID, ai.SkillID, ai.AttackValue, false);
	}

	void RecoverHP(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		attackEffect.RefreshItem (ai.UserUnitID, ai.SkillID,ai.AttackValue, true);
	}

	void ShowActiveSkill(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		attackEffect.PlayActiveSkill (ai);
	}

	void ShowPassiveSkill(object data) {
		AttackInfo uu = data as AttackInfo;
		if (uu == null) {
			return;		
		}
		attackEffect.RefreshItem (uu.UserUnitID, uu.SkillID);
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
		InitData ();
		topUI.Reset ();
		topUI.RefreshTopUI (questDungeonData, _questData);
		BattleMap.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
		if (questFullScreenTips == null) {
			CreatBoosAppear();
		}
		CreatEffect ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		questFullScreenTips.DestoryUI ();
		battle.DestoryUI ();
		GameObject.Destroy (attackEffect.gameObject);
		battle = null;
		Resources.UnloadUnusedAssets ();
	}

	void CreatBoosAppear () {
		ResourceManager.Instance.LoadLocalAsset ("Prefabs/QuestFullScreenTips", o => {
				GameObject obj = o as GameObject;
				Vector3 pos = obj.transform.localPosition;
				GameObject go = NGUITools.AddChild (viewManager.EffectPanel, obj);
				go.transform.localPosition = pos;
				questFullScreenTips = go.GetComponent<QuestFullScreenTips> ();
				questFullScreenTips.Init ("QuestFullScreenTips");

			initCount++;
		});

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
//		Debug.LogError ("questDungeonData.Floors.Count : " + questDungeonData.Floors.Count);
		if( questDungeonData.currentFloor == questDungeonData.Floors.Count - 1 ) {
			QuestStop ();
		} else {
			EnterNextFloor(null);
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
		} else {
			BattleUseData.maxEnergyPoint += 10;
		}
		configBattleUseData.storeBattleData.roleCoordinate = configBattleUseData.roleInitCoordinate;
//		configBattleUseData.StoreQuestDungeonData (questDungeonData);
		configBattleUseData.StoreMapData (_questData);
	}

	void QuestStop () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
//		battleMap
		battle.ShieldInput (false);
		questFullScreenTips.ShowTexture (QuestFullScreenTips.BossAppears, MeetBoss);
		role.Stop();
		battleEnemy = true;
	}

	public void NoFriendExit() {
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ExitBattle ();
	}

	public void Retire(bool gameOver) {
		configBattleUseData.ClearData ();
		RetireQuest.SendRequest (RetireQuestCallback, questDungeonData.QuestId, gameOver);
	}

	void RetireQuestCallback(object data) {
		NoFriendExit ();
	}

	public void HaveFriendExit() {
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ChangeScene(SceneEnum.Result);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, configBattleUseData.BattleFriend);
	}

	public void QuestEnd (TRspClearQuest trcq) {
		if (configBattleUseData.currentStageInfo != null) {
			if ( configBattleUseData.currentStageInfo.Type == QuestType.E_QUEST_STORY ) { // story quest
				DataCenter.Instance.QuestClearInfo.UpdateStoryQuestClear (configBattleUseData.currentStageInfo.ID, configBattleUseData.currentQuestInfo.ID);
			}else { 
				DataCenter.Instance.QuestClearInfo.UpdateEventQuestClear (configBattleUseData.currentStageInfo.ID, configBattleUseData.currentQuestInfo.ID);
			}	
		}

		NoFriendExit();

		UIManager.Instance.ChangeScene (SceneEnum.Victory);
		MsgCenter.Instance.Invoke (CommandEnum.VictoryData, trcq);
	}

	void EvolveEnd (TRspClearQuest trcq) {
		ControllerManager.Instance.ExitBattle ();
		DataCenter.Instance.PartyInfo.CurrentPartyId = 0;

		UIManager.Instance.ChangeScene (SceneEnum.Victory);
		MsgCenter.Instance.Invoke (CommandEnum.VictoryData, trcq);
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
//		Debug.LogError ("coor : " + coor.x + " y : " + coor.y);
		InitContinueData ();
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.BATTLE);
		if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
			return;	
		}

		currentMapData = questDungeonData.GetSingleFloor (coor);
		battleMap.ChangeStyle (coor);
		role.Stop ();

		if (configBattleUseData.trapPoison != null) {
			configBattleUseData.trapPoison.ExcuteByDisk();
		}

		if (configBattleUseData.trapEnvironment != null) {
			configBattleUseData.trapEnvironment.ExcuteByDisk();
		}

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
			if(sbd.attackRound == 0) { // 0 == first attack
				GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
			}
		}
		else if (sbd.isBattle == 2) {	// 2 == battle boss
			battleEnemy = true;
			battle.ShieldInput (true);
			questDungeonData.Boss= temp;
			TDropUnit bossDrop = questDungeonData.DropUnit.Find (a => a.DropId == 0);
			bud.InitBoss (questDungeonData.Boss, bossDrop);
			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
		}
		battle.ShowEnemy(temp);
		ExitFight (false);
		GameTimer.GetInstance ().AddCountDown (0.1f, RecoverBuff);
	}

	public static bool recoverPosion = false;
	public static bool reduceHurt = false;
	public static bool reduceDefense = false;
	public static bool strengthenAttack = false;

	void RecoverBuff() {
		ExcuteDiskActiveSkill(configBattleUseData.posionAttack, ref recoverPosion);
		ExcuteDiskActiveSkill(configBattleUseData.reduceHurtAttack, ref reduceHurt);
		ExcuteDiskActiveSkill(configBattleUseData.reduceDefenseAttack, ref reduceDefense);
		ExcuteDiskActiveSkill(configBattleUseData.strengthenAttack, ref strengthenAttack);
	}

	void ExcuteDiskActiveSkill (AttackInfo ai, ref bool excute) {
		if (ai != null) {
			IActiveSkillExcute iase = bud.excuteActiveSkill.GetActiveSkill (ai.UserUnitID);
			if (iase != null) {
				excute = true;
				iase.ExcuteByDisk (ai);
			}else{
				excute = false;
			}
		} else {
			excute = false;
		}
	}

	public void RoleCoordinate(Coordinate coor) {
		if (!battleMap.ReachMapItem (coor)) {
			if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				battleMap.prevMapItem.HideGridNoAnim ();
				GameTimer.GetInstance ().AddCountDown (0.2f, YieldShowAnim);
				configBattleUseData.StoreMapData(null);
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
					MsgCenter.Instance.Invoke(CommandEnum.ShowCoin, currentMapData.Coins);
					GameTimer.GetInstance().AddCountDown(ShowBottomInfo.showTime + ShowBottomInfo.scaleTime, MapItemCoin);
					break;
				case EQuestGridType.Q_TRAP:
					BattleMap.waitMove = true;
					MsgCenter.Instance.Invoke(CommandEnum.ShowTrap, currentMapData.TrapInfo);
					GameTimer.GetInstance().AddCountDown(ShowBottomInfo.showTime + ShowBottomInfo.scaleTime, MapItemTrap);
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
		} else {
			QuestCoorEnd ();
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
		battleMap.RotateAnim (RotateEndTrap);
	}

	void RotateEndTrap() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_trigger_trap);
		BattleMap.waitMove = false;
		TrapBase tb = currentMapData.TrapInfo;
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemCoin() {
		battleMap.RotateAnim (RotateEndCoin);
	}

	void RotateEndCoin() {
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
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);

		battle.ShieldInput ( true );
		BattleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < questDungeonData.Boss.Count; i++) {
			TEnemyInfo tei = questDungeonData.Boss [i];
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
		}
		TDropUnit bossDrop = questDungeonData.DropUnit.Find (a => a.DropId == 0);
		bud.InitBoss (questDungeonData.Boss, bossDrop);
		configBattleUseData.storeBattleData.isBattle = 2; // 2 == battle boss. 
		battle.ShowEnemy(temp);

		ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	public void MapItemEnemy() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_battle);

		BattleMap.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < currentMapData.Enemy.Count; i++) {
			TEnemyInfo tei = currentMapData.Enemy[i];
			tei.EnemySymbol = (uint)i;
			temp.Add(tei);
			DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}
		bud.InitEnemyInfo (currentMapData);
		configBattleUseData.storeBattleData.isBattle = 1;	// 1 == battle enemy
		battle.ShowEnemy (temp);
		ExitFight (false);
		GameTimer.GetInstance ().AddCountDown ( 0.3f, StartBattleEnemyAttack );
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
				AudioManager.Instance.PlayAudio(AudioEnum.sound_back_attack);
				battle.ShieldInput (false);
				break;
			case EnemyAttackEnum.FirstAttack:
				questFullScreenTips.ShowTexture (QuestFullScreenTips.FirstAttack, FirstAttack);
				AudioManager.Instance.PlayAudio(AudioEnum.sound_first_attack);
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
		AudioManager.Instance.PlayAudio (AudioEnum.sound_active_skill);
	}

	void BossDead() {
		battle.ShieldInput (false);
		BattleBottom.notClick = true;

		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_die);

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
			if(index != questData.hitGrid.Count - 1) {
				if(ChainLinkBattle ){
					ChainLinkBattle = false;
				}
				return;		
			}
		}

		TQuestGrid tqg = questDungeonData.GetSingleFloor (currentCoor);
		if (tqg == null || tqg.Type != EQuestGridType.Q_ENEMY) {
			return;	
		}

		battleMap.AddMapSecuritylevel (currentCoor);
		chainLikeMapItem = battleMap.AttakAround (currentCoor);	
		if (chainLikeMapItem.Count == 0) {
			ChainLinkBattle = false;
		} else {
			ChainLinkBattle = true;
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
		QuestClearShow(null);

//		MsgWindowParams mwp = new MsgWindowParams ();
//		mwp.btnParams = new BtnParam[2];
//		mwp.titleText = TextCenter.GetText("RedoQuestTitle");
//		mwp.contentText = TextCenter.GetText("RedoQuestContent",DataCenter.redoQuestStone, 
//		                                     DataCenter.Instance.AccountInfo.Stone);
//		
//		BtnParam sure = new BtnParam ();
//		sure.callback = QuestClearShow;
//		sure.text = TextCenter.GetText("GoOnQuest");
//		mwp.btnParams[0] = sure;
//
//		sure = new BtnParam ();
//		sure.callback = SureRetry;
//		sure.text = TextCenter.GetText("RedoQuest");
//		mwp.btnParams[1] = sure;
//
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
	}

	void SureRetry(object data) {
		battle.ShieldInput (false);
		RedoQuest.SendRequest (SureRetryNetWork, questDungeonData.QuestId, questDungeonData.currentFloor);
	}

	public void Retry () {
		main.GInput.IsCheckInput = false;
		BattleBottom.notClick = true;

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParams = new BtnParam[2];
		mwp.titleText = TextCenter.GetText("RedoQuestTitle");
		mwp.contentText = TextCenter.GetText("RedoQuestContent", DataCenter.redoQuestStone, 
		                                     DataCenter.Instance.AccountInfo.Stone);
		BtnParam sure = new BtnParam ();
		sure.callback = SureInitiativeRetry;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParams[0] = sure;

		sure = new BtnParam ();
		sure.callback = CancelInitiativeRetry;
		sure.text = TextCenter.GetText("Cancel");
		mwp.btnParams[1] = sure;

		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
	}
	
	void SureInitiativeRetry(object data) {
		if (DataCenter.Instance.AccountInfo.Stone < DataCenter.redoQuestStone) {
			viewManager.ShowTipsLabel(TextCenter.GetText("NotEnoughStone"));
			return;
		}

		battle.ShieldInput (false);
		if (battle.isShowEnemy) {
			MsgCenter.Instance.Invoke(CommandEnum.BattleEnd);
		}
		RedoQuest.SendRequest (SureRetryNetWork, questDungeonData.QuestId, questDungeonData.currentFloor);

		main.GInput.IsCheckInput = true;
		BattleBottom.notClick = false;
	}

	void CancelInitiativeRetry(object data) {
		main.GInput.IsCheckInput = true;
		BattleBottom.notClick = false;
	}

//	object tempData = null;
	void SureRetryNetWork(object data) {

		BattleMap.waitMove = false;
		battleMap.BattleEndRotate(null);
		RefreshRetryData (data);
		main.GInput.IsCheckInput = true;
		GameInput.OnPressEvent += SureRetryPress;
//		Debug.LogError ("SureRetryNetWork " + main.GInput.IsCheckInput);
	}

	void RefreshRetryData(object data) {
		RspRedoQuest rrq = data as RspRedoQuest;
		if (rrq == null) {
			return;	
		}
	
//		Debug.LogError ("rrq : " + rrq.dungeonData);
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
		configBattleUseData.storeBattleData.roleCoordinate = configBattleUseData.roleInitCoordinate;
		configBattleUseData.storeBattleData.questData = _questData;
		configBattleUseData.StoreMapData (_questData);
	}

	void SureRetryPress() {
//		Debug.LogError ("SureRetryPress ");
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
		RequestData (null);
	}

	void QuestClearShow(object data) {
		configBattleUseData.ClearData ();
		RequestData (null);
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

	void RequestData (object data) {
		if (DataCenter.gameState == GameState.Evolve) {
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
		configBattleUseData.gameState = (byte)GameState.Normal;
//		DataCenter.Instance.RefreshUserInfo(rsp)
		DataCenter.Instance.UserInfo.Rank = rsp.rank;
		DataCenter.Instance.UserInfo.Exp = rsp.exp;
		DataCenter.Instance.AccountInfo.Money = rsp.money;
		DataCenter.Instance.AccountInfo.FriendPoint = rsp.friendPoint;
		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaMax = rsp.staminaMax;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;	
//
		TUnitParty tup = configBattleUseData.party;
		foreach (var item in tup.UserUnit.Values) {
			if(item == null) {
				continue;
			}
			DataCenter.Instance.UserUnitList.DelMyUnit(item.ID);
		}
		configBattleUseData.party = null;

		for (int i = 0; i < rsp.gotUnit.Count; i++) {
			DataCenter.Instance.UserUnitList.AddMyUnit(rsp.gotUnit[i]);
		}
		DataCenter.Instance.UserUnitList.AddMyUnit(rsp.evolvedUnit);
//		evolveUser = 

		TRspClearQuest trcq = new TRspClearQuest ();
		trcq.exp = rsp.exp;
		trcq.gotExp = rsp.gotExp;
		trcq.money = rsp.money;
		trcq.gotMoney = rsp.gotMoney;
		trcq.gotStone = rsp.gotStone;
		trcq.evolveUser = TUserUnit.GetUserUnit (DataCenter.Instance.UserInfo.UserId, rsp.evolvedUnit);
		List<TUserUnit> temp = new List<TUserUnit> ();
		for (int i = 0; i <  rsp.gotUnit.Count; i++) {
			TUserUnit tuu = TUserUnit.GetUserUnit(DataCenter.Instance.UserInfo.UserId,rsp.gotUnit[i]);
			temp.Add(tuu);
		}
		trcq.gotUnit = temp;
		trcq.rank = rsp.rank;
		DataCenter.Instance.oldAccountInfo = DataCenter.Instance.UserInfo;
//		End (trcq, EvolveEnd);
		End();
		EvolveEnd (trcq);
	}

	void ResponseClearQuest (object data) {
		if (data != null) {
			DataCenter.Instance.oldAccountInfo = DataCenter.Instance.UserInfo;
			TRspClearQuest clearQuest = data as TRspClearQuest;
//			Debug.LogError("exp : " + DataCenter.Instance.UserInfo.Exp);

			DataCenter.Instance.RefreshUserInfo (clearQuest);

//			Debug.LogError("exp : " + DataCenter.Instance.UserInfo.Exp + " clearQuest : " + clearQuest.exp);
//			End (clearQuest, QuestEnd);
			End();
			QuestEnd(clearQuest);
		} else {
			RetryClearQuestRequest();
		}
	}

	void RetryClearQuestRequest () {
		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.GetText("RetryClearQuestTitle");
		mwp.contentText = TextCenter.GetText("RetryClearQuestNet",DataCenter.redoQuestStone, 
		                                     DataCenter.Instance.AccountInfo.Stone);
		BtnParam sure = new BtnParam ();
		sure.callback = RequestData;
		sure.text = TextCenter.GetText("Retry");
		mwp.btnParam = sure;
		
//		sure = new BtnParam ();
//		sure.callback = CancelInitiativeRetry;
//		sure.text = TextCenter.GetText("Cancel");
//		mwp.btnParams[1] = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,mwp);
	}

	void End() {
		Battle.colorIndex = 0;
		Battle.isShow = false;

//		UIManager.Instance.ChangeScene (SceneEnum.Victory);
//		MsgCenter.Instance.Invoke (CommandEnum.VictoryData, clearQuest);

//		ResourceManager.Instance.LoadLocalAsset ("Prefabs/Victory", o => {
//			GameObject obj = o as GameObject;
//			Vector3 tempScale = obj.transform.localScale;
//			obj = NGUITools.AddChild(viewManager.EffectPanel, obj);
//			obj.transform.localScale = tempScale;
//			obj.transform.localPosition = new Vector3(0f, 475f, 0f);
////			VictoryEffect ve = obj.GetComponent<VictoryEffect>();
////			ve.Init("Victory");
////			ve.battleQuest = this;
////			ve.ShowData (clearQuest);
////			ve.PlayAnimation(questEnd);
		AudioManager.Instance.PlayAudio (AudioEnum.sound_quest_clear);
//		});
	}

	void BattleFail(object data) {
		battle.ShieldInput (true);

//		AudioManager.Instance.PlayAudio (AudioEnum.sound_game_over);

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParams = new BtnParam[2];
		mwp.titleText = TextCenter.GetText("ResumeQuestTitle");
		mwp.contentText = TextCenter.GetText("ResumeQuestContent", DataCenter.resumeQuestStone);
		
		BtnParam sure = new BtnParam ();
		sure.callback = BattleFailRecover;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParams[0] = sure;
		
		sure = new BtnParam ();
		sure.callback = BattleFailExit;
		sure.text = TextCenter.GetText("Cancel");
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
		RetireQuest.SendRequest (RetireQusetBattleFail, questDungeonData.QuestId, true);
	}

	void RetireQusetBattleFail(object data) {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_game_over);

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
