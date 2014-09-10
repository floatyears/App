using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleMapModule : ModuleBase {
	private GameObject rootObject;
	private TQuestGrid currentMapData;

	public static BattleUseData bud;

	public TClearQuestParam questData {
		get {
			List< TClearQuestParam > _questData = ConfigBattleUseData.Instance.storeBattleData.questData;
			if(_questData.Count == 0) {
				ClearQuestParam qp = new ClearQuestParam();
				TClearQuestParam cqp = new TClearQuestParam(qp);
				_questData.Add(cqp);
			}

			return _questData[ _questData.Count - 1 ];
		}
	}

	private Queue<MapItem> chainLikeMapItem = new Queue<MapItem> ();
	public bool ChainLinkBattle = false;
	
	private RoleStateException roleStateException;

	public static int battleData = 0;

	public BattleMapModule (UIConfigItem config) : base(  config) {
		CreateUI<BattleMapView> ();

		battleData = ConfigBattleUseData.Instance.hasBattleData ();
		rootObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel);
		string tempName = "Map";

		bud = new BattleUseData (this);
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data[0].ToString()) {
		case "rolecoor":
			RoleCoordinate((Coordinate)data[1]);
			break;
		case "":
			break;
		default:
				break;
		}
	}

	public override void ShowUI () {
		base.ShowUI ();

		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RecoverHP);
//		MsgCenter.Instance.AddListener (CommandEnum.LeaderSkillEnd, LeaderSkillEnd);
//		Resources.UnloadUnusedAssets ();

//		GameTimer.GetInstance ().AddCountDown (0.5f, ShowScene);


		MsgCenter.Instance.AddListener (CommandEnum.BattleBaseData, BattleBase);
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData, null);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.GridEnd, GridEnd);
		MsgCenter.Instance.AddListener (CommandEnum.PlayerDead, BattleFail);
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillStandReady, ActiveSkillStandReady);
		MsgCenter.Instance.AddListener (CommandEnum.ShowActiveSkill, ShowActiveSkill);
		MsgCenter.Instance.AddListener (CommandEnum.ShowPassiveSkill, ShowPassiveSkill);

		if (battleData > 0) {
			ContineBattle ();
		} else {
			ConfigBattleUseData.Instance.StoreData(ConfigBattleUseData.Instance.questDungeonData.QuestId);
		}
	}

	public override void HideUI () {
		battleEnemy = false;
		if( bud != null ) {
			bud.excuteActiveSkill.ResetSkill();
			bud.RemoveListen ();
			bud = null;
		}
		base.HideUI ();

		MsgCenter.Instance.RemoveListener (CommandEnum.BattleBaseData, BattleBase);
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
//
//	void LeaderSkillEnd(object data) {
//		MsgCenter.Instance.RemoveListener (CommandEnum.LeaderSkillEnd, LeaderSkillEnd);
//	}

	void ReadyMove() {
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		NoviceGuideStepEntityManager.Instance ().StartStep ( NoviceGuideStartType.BATTLE );
	}

	void AttackEnemy (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;
		}
//		attackEffect.RefreshItem ();
		ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule, "refreshitem", ai.UserUnitID, ai.SkillID, ai.AttackValue, false);
	}

	void RecoverHP(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"refreshitem", ai.UserUnitID, ai.SkillID,ai.AttackValue, true);
	}

	void ShowActiveSkill(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"activeskill", ai);
	}

	void ShowPassiveSkill(object data) {
		AttackInfo uu = data as AttackInfo;
		if (uu == null) {
			return;		
		}
		ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"refreshitem", uu.UserUnitID, uu.SkillID);
	}
	
	void Reset () {
		battleEnemy = false;
		bud.RemoveListen ();
		bud.Reset ();
		ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "refresh");
		BattleMapView.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
//		if (questFullScreenTips == null) {
//			CreatBoosAppear();
//		}
	}

	public override void DestoryUI () {
		base.DestoryUI ();
//		questFullScreenTips.DestoryUI ();
//		battle.DestoryUI ();
////		GameObject.Destroy (attackEffect.gameObject);
//		battle = null;
		Resources.UnloadUnusedAssets ();
	}

//	void CreatBoosAppear () {
////		ResourceManager.Instance.LoadLocalAsset ("Prefabs/QuestFullScreenTips", o => {
////			GameObject obj = o as GameObject;
////			Vector3 pos = obj.transform.localPosition;
////			GameObject go = NGUITools.AddChild (ViewManager.Instance.EffectPanel, obj);
////			go.transform.localPosition = pos;
////			questFullScreenTips = go.GetComponent<BattleFullScreenTipsView> ();
//////			questFullScreenTips.Init ("QuestFullScreenTips");
////			initCount++;
////		});
//
//	}
//	}
	
//	public Vector3 GetPosition(Coordinate coor) {
//		return battleMap.GetPosition(coor.x, coor.y);
//	}

	public void TargetItem(Coordinate coor) {
		(view as BattleMapView).StartMove (coor);
	}
	  
	void Exit() {
//		UIManager.Instance.ExitBattle();
//		UIManager.Instance.baseScene.PrevScene = ModuleEnum.Evolve;
		ModuleManager.Instance.ExitBattle();
	}

	bool battleEnemy = false;

	public void ClickDoor () {
		if( ConfigBattleUseData.Instance.questDungeonData.isLastFloor() ) {
			QuestStop ();
		} else {
//			battleMap.door.isClick = false;
			EnterNextFloor(null);

		}
	}

	void EnterNextFloor (object data) {
		ConfigBattleUseData.Instance.questDungeonData.currentFloor ++;
		ClearQuestParam clear = new ClearQuestParam ();
		TClearQuestParam cqp = new TClearQuestParam (clear);
		ConfigBattleUseData.Instance.storeBattleData.questData.Add (cqp);
		ModuleManager.SendMessage(ModuleEnum.BattleTopModule,"setfloor");
		if (BattleUseData.maxEnergyPoint >= 10) {
			BattleUseData.maxEnergyPoint = DataCenter.maxEnergyPoint;
		} else {
			BattleUseData.maxEnergyPoint += 10;
		}
		ConfigBattleUseData.Instance.storeBattleData.roleCoordinate = ConfigBattleUseData.Instance.roleInitCoordinate;
		ConfigBattleUseData.Instance.StoreMapData ();

		Reset ();
	}

	void QuestStop () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
//		battleMap
//		battle.ShieldInput (false);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "showtex", MeetBoss as Callback);
		(view as BattleMapView).Stop();
		battleEnemy = true;
	}

	public void NoFriendExit() {
		ModuleManager.Instance.ExitBattle ();
//		UIManager.Instance.ExitBattle ();
	}

	public void Retire(bool gameOver) {
		ConfigBattleUseData.Instance.ClearData ();
		RetireQuest.SendRequest (RetireQuestCallback, ConfigBattleUseData.Instance.questDungeonData.QuestId, gameOver);
	}

	void RetireQuestCallback(object data) {
		NoFriendExit ();
		ModuleManager.Instance.ExitBattle ();
	}

	public void HaveFriendExit() {
		ModuleManager.Instance.ExitBattle ();
		ModuleManager.Instance.ShowModule(ModuleEnum.ResultModule);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, ConfigBattleUseData.Instance.BattleFriend);
	}

	public void QuestEnd (TRspClearQuest trcq) {
		if (ConfigBattleUseData.Instance.currentStageInfo != null) {
			if ( ConfigBattleUseData.Instance.currentStageInfo.Type == QuestType.E_QUEST_STORY ) { // story quest
				DataCenter.Instance.QuestClearInfo.UpdateStoryQuestClear (ConfigBattleUseData.Instance.currentStageInfo.ID, ConfigBattleUseData.Instance.currentQuestInfo.ID);
			} else { 
				DataCenter.Instance.QuestClearInfo.UpdateEventQuestClear (ConfigBattleUseData.Instance.currentStageInfo.ID, ConfigBattleUseData.Instance.currentQuestInfo.ID);
			}	
		}

		NoFriendExit();

		ModuleManager.Instance.ShowModule (ModuleEnum.VictoryModule,trcq);
//		MsgCenter.Instance.Invoke (CommandEnum.VictoryData, trcq);
	}

	private EQuestGridType gridType = EQuestGridType.Q_NONE;
	private Coordinate _currentCoor;
	public Coordinate currentCoor {
		set { _currentCoor = value; ConfigBattleUseData.Instance.storeBattleData.roleCoordinate = _currentCoor; }
		get { return _currentCoor;}
	}

	void YieldShowAnim() {
		int count = bud.Els.CheckLeaderSkillCount();
//		battle.ShieldInput (false);

		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "readymove", ReadyMove as Callback, count * AttackController.normalAttackInterv);
		bud.InitBattleUseData(null);
	}

	void InitContinueData() {
		bud.Els.CheckLeaderSkillCount();
		bud.InitBattleUseData (ConfigBattleUseData.Instance.storeBattleData);
	}

	public void ContineBattle () {
//		Debug.LogError ("ContineBattle NoviceGuideStartType.BATTLE");
		NoviceGuideStepEntityManager.Instance ().StartStep ( NoviceGuideStartType.BATTLE );

		Coordinate coor = ConfigBattleUseData.Instance.storeBattleData.roleCoordinate;
//		currentCoor = coor;
		InitContinueData ();
//		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.BATTLE);
		if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
			return;	
		}

		currentMapData = ConfigBattleUseData.Instance.questDungeonData.GetSingleFloor (coor);
		(view as BattleMapView).ChangeStyle (coor);
		(view as BattleMapView).Stop ();

		if (ConfigBattleUseData.Instance.trapPoison != null) {
			ConfigBattleUseData.Instance.trapPoison.ExcuteByDisk();
		}

		if (ConfigBattleUseData.Instance.trapEnvironment != null) {
			ConfigBattleUseData.Instance.trapEnvironment.ExcuteByDisk();
		}

		TStoreBattleData sbd = ConfigBattleUseData.Instance.storeBattleData;

		// 0 is not in fight.
		if (sbd.isBattle == 0) {
			if (sbd.recoveBattleStep == RecoveBattleStep.RB_BossDead) {
				BossDead();
				return;
			}
			bud.CheckPlayerDead();
			return;
		}

		BattleMapView.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < sbd.enemyInfo.Count; i++) {
			TEnemyInfo tei = new TEnemyInfo(sbd.enemyInfo[i]);
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
			DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}

		if (sbd.isBattle == 1) {		// 1 == battle enemy
			currentMapData.Enemy = temp;
			bud.InitEnemyInfo (currentMapData);
			if(sbd.attackRound == 0) {	// 0 == first attack
				GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
			}
		} else if (sbd.isBattle == 2) {	// 2 == battle boss
			battleEnemy = true;
//			battle.ShieldInput (true);
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
			ConfigBattleUseData.Instance.questDungeonData.Boss= temp;
			TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
			bud.InitBoss (ConfigBattleUseData.Instance.questDungeonData.Boss, bossDrop);
			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
		}
//		battle.ShowEnemy(temp);

		ExitFight (false);
		GameTimer.GetInstance ().AddCountDown (0.1f, RecoverBuff);

		bud.CheckPlayerDead();
	}

	public static bool recoverPosion = false;
	public static bool reduceHurt = false;
	public static bool reduceDefense = false;
	public static bool strengthenAttack = false;

	void RecoverBuff() {
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.posionAttack, ref recoverPosion);
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.reduceHurtAttack, ref reduceHurt);
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.reduceDefenseAttack, ref reduceDefense);
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.strengthenAttack, ref strengthenAttack);
	}

	void ExcuteDiskActiveSkill (AttackInfo ai, ref bool excute) {
		if (ai != null) {
			IActiveSkillExcute iase = bud.excuteActiveSkill.GetActiveSkill (ai.UserUnitID);
			if (iase != null) {
				excute = true;
				iase.ExcuteByDisk (ai);
			} else { 
				excute = false;
			}
		} else {
			excute = false;
		}
	}

	public void RoleCoordinate(Coordinate coor) {

		BattleMapView v = view as BattleMapView;
//		Debug.LogError ("coor : " + coor.x + " coor : " + coor.y);
		if (!v.ReachMapItem (coor)) {
			if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				v.prevMapItem.HideGridNoAnim ();
				GameTimer.GetInstance ().AddCountDown (0.2f, YieldShowAnim);
				ConfigBattleUseData.Instance.StoreMapData();
				return;
			}

			int index = ConfigBattleUseData.Instance.questDungeonData.GetGridIndex (coor);

			if (index != -1) {
				questData.hitGrid.Add ((uint)index);
			}

			currentMapData = ConfigBattleUseData.Instance.questDungeonData.GetSingleFloor (coor);

			(view as BattleMapView).Stop ();
			if (currentMapData.Star == EGridStar.GS_KEY) {
				BattleMapView.waitMove = true;
				ConfigBattleUseData.Instance.storeBattleData.HitKey = true;
				v.RotateAnim (MapItemKey);
				return;
			}

			AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);

			switch (currentMapData.Type) {
				case EQuestGridType.Q_NONE:
					BattleMapView.waitMove = true;
					v.RotateAnim (MapItemNone);
					break;
				case EQuestGridType.Q_ENEMY:
					BattleMapView.waitMove = true;
					v.RotateAnim (MapItemEnemy);
					break;
				case EQuestGridType.Q_KEY:
					break;
				case EQuestGridType.Q_TREATURE:
					BattleMapView.waitMove = true;
//					MsgCenter.Instance.Invoke(CommandEnum.ShowCoin, currentMapData.Coins);
					(view as BattleMapView).ShowCoin(currentMapData.Coins);
					MapItemCoin();
//					GameTimer.GetInstance().AddCountDown(ShowBottomInfo.showTime + ShowBottomInfo.scaleTime, MapItemCoin);
					break;
				case EQuestGridType.Q_TRAP:
					BattleMapView.waitMove = true;
//					MsgCenter.Instance.Invoke(CommandEnum.ShowTrap, currentMapData.TrapInfo);
					v.ShowTrap(currentMapData.TrapInfo);
					GameTimer.GetInstance().AddCountDown(BattleMapView.showTime + BattleMapView.scaleTime, ()=>{
						(view as BattleMapView).RotateAnim (RotateEndTrap);
					});
					break;
				case EQuestGridType.Q_QUESTION:
					BattleMapView.waitMove = true;
					v.RotateAnim (MeetQuestion);
					break;
				case EQuestGridType.Q_EXCLAMATION:
					BattleMapView.waitMove = true;
					v.RotateAnim (MapItemExclamation);
					break;
				default:
					BattleMapView.waitMove = false;
					MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
					QuestCoorEnd ();
					break;
			}
		} else {
			QuestCoorEnd ();
			ConfigBattleUseData.Instance.StoreMapData();
		}
	}
	
	void GridEnd(object data) {
		if (currentMapData.Drop != null && currentMapData.Drop.DropId != 0) {
			questData.getUnit.Add (currentMapData.Drop.DropId);	

//			topUI.Drop = GetDrop(); //questData.getUnit.Count;
		}
	}

	void MeetQuestion () {
		BattleMapView.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);

	}
	
	void MapItemExclamation() {
		BattleMapView.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);

	}

	void RotateEndTrap() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_trigger_trap);
		BattleMapView.waitMove = false;
		TrapBase tb = currentMapData.TrapInfo;
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemCoin() {
		(view as BattleMapView).RotateAnim (RotateEndCoin);
	}

	void RotateEndCoin() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_treasure);
		BattleMapView.waitMove = false;
		questData.getMoney += currentMapData.Coins;
//		topUI.Coin = GetCoin ();//questData.getMoney;
		ModuleManager.SendMessage(ModuleEnum.BattleTopModule,"coin",GetCoin ());

		MsgCenter.Instance.Invoke (CommandEnum.MeetCoin, currentMapData);
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
	}

	void MapItemKey() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_key);
//		battle.ShieldInput (false);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "gate", OpenGate as Callback);
		BattleMapView.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		MsgCenter.Instance.Invoke (CommandEnum.OpenDoor, null);
	}

	int GetCoin() {
		int coin = 0;
		foreach (var item in ConfigBattleUseData.Instance.storeBattleData.questData) {
			coin += item.getMoney;
		}
		return coin;
	}

	void OpenGate() {
//		battle.ShieldInput (true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
	}

	void MapItemNone() {
		BattleMapView.waitMove = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);

	}

	private void QuestCoorEnd() {
//		Debug.LogError ("current coor x : " + currentCoor.x + " y : " + currentCoor.y);
//		Debug.LogError ("MapConfig.endCoor x : " + MapConfig.endCoor.x + " y : " + MapConfig.endCoor.y);

		if ( DGTools.EqualCoordinate (currentCoor, MapConfig.endCoor)) {
			MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, true);
		} else {
			MsgCenter.Instance.Invoke (CommandEnum.QuestEnd, false);
		}
	}

	public void MeetBoss () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
//		AudioManager.Instance.StopBackgroundMusic (true);
//		battle.ShieldInput ( true );
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		BattleMapView.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < ConfigBattleUseData.Instance.questDungeonData.Boss.Count; i++) {
			TEnemyInfo tei = ConfigBattleUseData.Instance.questDungeonData.Boss [i];
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
		}
		TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
		bud.InitBoss (ConfigBattleUseData.Instance.questDungeonData.Boss, bossDrop);

		ConfigBattleUseData.Instance.storeBattleData.isBattle = 2; // 2 == battle boss. 
//		battle.ShowEnemy (temp, true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);

		ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	public void MapItemEnemy() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_battle);

		BattleMapView.waitMove = false;
		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < currentMapData.Enemy.Count; i++) {
			TEnemyInfo tei = currentMapData.Enemy[i];
			tei.EnemySymbol = (uint)i;
			temp.Add(tei);
			DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}
		bud.InitEnemyInfo (currentMapData);
		ConfigBattleUseData.Instance.storeBattleData.isBattle = 1;	// 1 == battle enemy
//		battle.ShowEnemy (temp);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		ExitFight (false);
		GameTimer.GetInstance ().AddCountDown ( 0.3f, StartBattleEnemyAttack );
	}

	void ExitFight(bool exit) {
		if (exit) {
			ConfigBattleUseData.Instance.storeBattleData.isBattle = 0;
		}
	}

	public void StartBattleEnemyAttack() {
		EnemyAttackEnum eae = (view as BattleMapView).FirstOrBackAttack ();
		switch (eae) {
			case EnemyAttackEnum.BackAttack:
				ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "back", BackAttack as Callback);
				AudioManager.Instance.PlayAudio(AudioEnum.sound_back_attack);
//				battle.ShieldInput (false);
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
				break;
			case EnemyAttackEnum.FirstAttack:
				ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "first", FirstAttack as Callback);
				AudioManager.Instance.PlayAudio(AudioEnum.sound_first_attack);
				break;
			default:
				break;
		}
	}

	void BackAttack() {
		bud.ac.AttackPlayer ();
//		battle.BattleCardIns.StartBattle (false);
	}

	void FirstAttack() {
		bud.ac.FirstAttack ();
	}
	
	void AttackEnd () {
//		battle.ShieldInput(true);
	}

	void ShowBattle() {
//		if(battle == null) {	
//			battle = new Battle("Battle");
//			battle.InitUI();
//		}
//		if(battle.GetState == UIState.UIShow)
//			return;
//		battle.ShowUI();
	}

	void ActiveSkillStandReady(object data) {
		TUserUnit tuu = data as TUserUnit;
		ModuleManager.SendMessage (ModuleEnum.BattleFullScreenTipsModule, "ready");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_active_skill);
	}

	void BossDead() {

		TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
		if (bossDrop != null) {
			questData.getUnit.Add(bossDrop.DropId);
		}

//		battle.ShieldInput (false);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
		BattleBottomView.notClick = true;

		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_die);

		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "clear", QuestClear as Callback);
	}

	void ShieldAllInput (bool shiled) {
		Main.Instance.NguiCamera.enabled = shiled;
		Main.Instance.GInput.IsCheckInput = shiled;
		BattleBottomView.notClick = !shiled;
	}

	void BattleEnd(object data) {
		QuestCoorEnd ();
		ShieldAllInput (false);
		ExitFight (true);

		bool b = data != null ? (bool)data : false;

		if (battleEnemy && !b) {
			BossDead();
			ConfigBattleUseData.Instance.storeBattleData.recoveBattleStep = RecoveBattleStep.RB_BossDead;
			ConfigBattleUseData.Instance.StoreMapData ();

			ShieldAllInput (true);
			return;
		}

		ConfigBattleUseData.Instance.storeBattleData.recoveBattleStep = RecoveBattleStep.RB_None;
		ConfigBattleUseData.Instance.StoreMapData ();

		int index = ConfigBattleUseData.Instance.questDungeonData.GetGridIndex (currentCoor);

		if (index == -1) {
			ShieldAllInput (true);
			return;	
		}

		uint uIndex = (uint)index;
		if (questData.hitGrid.Contains (uIndex)) {
			index = questData.hitGrid.FindIndex(a=>a == uIndex);
			if(index != questData.hitGrid.Count - 1) {
				if(ChainLinkBattle ){
					ChainLinkBattle = false;
				}
				ShieldAllInput (true);
				return;
			}
		}

		TQuestGrid tqg = ConfigBattleUseData.Instance.questDungeonData.GetSingleFloor (currentCoor);

		if (tqg == null || tqg.Type != EQuestGridType.Q_ENEMY) {
			ShieldAllInput (true);
			return;
		}

		(view as BattleMapView).AddMapSecuritylevel (currentCoor);
		chainLikeMapItem = (view as BattleMapView).AttakAround (currentCoor);

		if (chainLikeMapItem.Count == 0) {
			ChainLinkBattle = false;
		} else {
			ChainLinkBattle = true;
			(view as BattleMapView).SyncRoleCoordinate (chainLikeMapItem.Dequeue ().Coor);
		}
		ShieldAllInput (true);
	}

	void QuestClear() {
		BattleMapView.waitMove = true;
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick",true);
		ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "banclick",false);
		(view as BattleMapView).BattleEndRotate((view as BattleMapView).ShowTapToCheckOut);
	}

	void BattleBase (object data) {
		BattleBaseData bbd = data as BattleBaseData;
		(view as BattleMapView).InitData (bbd.Blood, bbd.maxBlood, bbd.EnergyPoint);
	}

	public void CheckOut () {
		QuestClearShow(null);
	}

	void SureRetry(object data) {
//		battle.ShieldInput (false);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
		RedoQuest.SendRequest (SureRetryNetWork, ConfigBattleUseData.Instance.questDungeonData.QuestId, ConfigBattleUseData.Instance.questDungeonData.currentFloor);
	}

	public void Retry () {
		Main.Instance.GInput.IsCheckInput = false;
		BattleBottomView.notClick = true;

		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("RedoQuestTitle"),
		                                   TextCenter.GetText ("RedoQuestContent", DataCenter.redoQuestStone, DataCenter.Instance.AccountInfo.Stone),
		                                   TextCenter.GetText ("OK"), TextCenter.GetText ("Cancel"), SureInitiativeRetry, CancelInitiativeRetry);
	}
	
	void SureInitiativeRetry(object data) {
		if (DataCenter.Instance.AccountInfo.Stone < DataCenter.redoQuestStone) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("NotEnoughStone"));
			return;
		}

//		battle.ShieldInput (false);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
//		if (battle.isShowEnemy) {
//			MsgCenter.Instance.Invoke(CommandEnum.BattleEnd);
//		}
		RedoQuest.SendRequest (SureRetryNetWork, ConfigBattleUseData.Instance.questDungeonData.QuestId, ConfigBattleUseData.Instance.questDungeonData.currentFloor);

		Main.Instance.GInput.IsCheckInput = true;
		BattleBottomView.notClick = false;
	}

	void CancelInitiativeRetry(object data) {
		Main.Instance.GInput.IsCheckInput = true;
		BattleBottomView.notClick = false;
	}
	
	void SureRetryNetWork(object data) {
		Umeng.GA.Buy ("RedoQuest", 1, DataCenter.redoQuestStone);
		BattleMapView.waitMove = false;
		(view as BattleMapView).BattleEndRotate(null);
		RefreshRetryData (data);
		Main.Instance.GInput.IsCheckInput = true;
		GameInput.OnPressEvent += SureRetryPress;
	}

	void RefreshRetryData(object data) {
		RspRedoQuest rrq = data as RspRedoQuest;
		if (rrq == null) {
			return;	
		}
	
		DataCenter.Instance.AccountInfo.Stone = rrq.stone;
		ConfigBattleUseData.Instance.RefreshCurrentFloor(rrq);;
		ConfigBattleUseData.Instance.InitRoleCoordinate (new Coordinate (MapConfig.characterInitCoorX, MapConfig.characterInitCoorY));
		ConfigBattleUseData.Instance.StoreMapData ();
	}

	void SureRetryPress() {
//		Debug.LogError ("SureRetryPress ");
		GameInput.OnPressEvent -= SureRetryPress;
		RetryRefreshUI ();
	}

	void RetryRefreshUI() {
//		battle.ShieldInput (true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
//		if (battle.isShowEnemy) {
//			ExitFight (true);
//			configBattleUseData.storeBattleData.attackRound = 0;
//			configBattleUseData.StoreMapData(null);
//			battle.ExitFight();
//		}
		Reset ();
		bud.ResetBlood ();
	}

	void CancelRetry(object data) {
		RequestData (null);
	}

	void QuestClearShow(object data) {
		ConfigBattleUseData.Instance.ClearData ();
		RequestData (null);
	}

	public TClearQuestParam GetQuestData () {
		ClearQuestParam cq = new ClearQuestParam ();
		TClearQuestParam cqp = new TClearQuestParam (cq);
		cqp.questId = ConfigBattleUseData.Instance.questDungeonData.QuestId;
		foreach (var item in ConfigBattleUseData.Instance.storeBattleData.questData) {
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

		Umeng.GA.Event ("Evolve");

		ConfigBattleUseData.Instance.gameState = (byte)GameState.Normal;
//		DataCenter.Instance.RefreshUserInfo(rsp)
		DataCenter.Instance.UserInfo.Rank = rsp.rank;
		DataCenter.Instance.UserInfo.Exp = rsp.exp;
		DataCenter.Instance.AccountInfo.Money = rsp.money;
		DataCenter.Instance.AccountInfo.FriendPoint = rsp.friendPoint;
		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaMax = rsp.staminaMax;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;	
//
		TUnitParty tup = ConfigBattleUseData.Instance.party;
		foreach (var item in tup.UserUnit.Values) {
			if(item == null ) {
				continue;
			}
			if ( item.ID != rsp.evolvedUnit.uniqueId ) { //only delete Evo Materials, not delete BaseUnit
				DataCenter.Instance.UserUnitList.DelMyUnit(item.ID);
			}
		}
		ConfigBattleUseData.Instance.party = null;

		for (int i = 0; i < rsp.gotUnit.Count; i++) {
			DataCenter.Instance.UserUnitList.AddMyUnit(rsp.gotUnit[i]);
		}

		//update the evolved unit
		DataCenter.Instance.UserUnitList.AddMyUnit(rsp.evolvedUnit);

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
		End();
		ModuleManager.Instance.ExitBattle ();
		DataCenter.Instance.PartyInfo.CurrentPartyId = 0;
		
		ModuleManager.Instance.ShowModule (ModuleEnum.VictoryModule,trcq);
	}

	void ResponseClearQuest (object data) {
		if (data != null) {
			DataCenter.Instance.oldAccountInfo = DataCenter.Instance.UserInfo;
			TRspClearQuest clearQuest = data as TRspClearQuest;
			DataCenter.Instance.RefreshUserInfo (clearQuest);
			End();
			QuestEnd(clearQuest);
		} else {
			TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("RetryClearQuestTitle"),TextCenter.GetText("RetryClearQuestNet",DataCenter.redoQuestStone, 
			                                                                                                  DataCenter.Instance.AccountInfo.Stone),TextCenter.GetText("Retry"),RequestData);

		}
	}
	
	void End() {
		BattleManipulationModule.colorIndex = 0;
		BattleManipulationModule.isShow = false;

		AudioManager.Instance.PlayAudio (AudioEnum.sound_quest_clear);
	}

	void BattleFail(object data) {
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);

		Main.Instance.GInput.IsCheckInput = false;
		BattleBottomView.notClick = true;

		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("ResumeQuestTitle"), TextCenter.GetText ("ResumeQuestContent", DataCenter.resumeQuestStone), TextCenter.GetText ("OK"), TextCenter.GetText ("Cancel"), BattleFailRecover, BattleFailExit);
	}

	void BattleFailRecover(object data) {
		if (DataCenter.Instance.AccountInfo.Stone < DataCenter.redoQuestStone) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("NotEnoughStone"));
			return;
		}

		ResumeQuest.SendRequest (o=>{
			Umeng.GA.Buy ("ResumeQuest" , 1, DataCenter.resumeQuestStone);
			bud.AddBlood (bud.maxBlood);
			bud.RecoverEnergePoint (DataCenter.maxEnergyPoint);
			ConfigBattleUseData.Instance.StoreMapData ();
			
			Main.Instance.GInput.IsCheckInput = true;
			BattleBottomView.notClick = false;
		}, ConfigBattleUseData.Instance.questDungeonData.QuestId);
		bud.ClearData ();
	}


	void BattleFailExit(object data) {
		RetireQuest.SendRequest (o=>{
			AudioManager.Instance.PlayAudio (AudioEnum.sound_game_over);
			
			ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "over", BattleFail as DataListener);
			ConfigBattleUseData.Instance.ClearData ();
		}, ConfigBattleUseData.Instance.questDungeonData.QuestId, true);
	}

	void BattleFail () {
		BattleManipulationModule.colorIndex = 0;
		BattleManipulationModule.isShow = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd);
		ModuleManager.Instance.ExitBattle ();
//		UIManager.Instance.ExitBattle ();
	}
}
