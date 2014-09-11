using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleMapModule : ModuleBase {
	private GameObject rootObject;
	private TQuestGrid currentMapData;

//	public static BattleUseData bud;

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
//		rootObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel);
		string tempName = "Map";

//		bud = new BattleUseData (this);
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data[0].ToString()) {
//			case "rolecoor":
//				RoleCoordinate((Coordinate)data[1]);
//				break;
			case "playerdead":
				BattleFail();
				break;
			default:
					break;
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		BattleUseData.Instance.GetBaseData ();

		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RecoverHP);

//		GameTimer.GetInstance ().AddCountDown (0.5f, ShowScene);


//		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData, null);

		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);

		if (battleData > 0) {
			ContineBattle ();
		} else {
			ConfigBattleUseData.Instance.StoreData(ConfigBattleUseData.Instance.questDungeonData.QuestId);
		}
	}

	public override void HideUI () {
		battleEnemy = false;
		BattleUseData.Instance.excuteActiveSkill.ResetSkill();
		BattleUseData.Instance.RemoveListen ();
		base.HideUI ();

//		MsgCenter.Instance.RemoveListener (CommandEnum.BattleBaseData, BattleBase);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
//		MsgCenter.Instance.RemoveListener (CommandEnum.GridEnd, GridEnd);
//		MsgCenter.Instance.RemoveListener (CommandEnum.PlayerDead, BattleFail);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RecoverHP);


		roleStateException.RemoveListener ();
	}

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

	void Reset () {
		battleEnemy = false;
		BattleUseData.Instance.RemoveListen ();
		BattleUseData.Instance.Reset ();
		ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "refresh");
		BattleMapView.waitMove = false;
//		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
		BattleUseData.Instance.GetBaseData ();
//		if (questFullScreenTips == null) {
//			CreatBoosAppear();
//		}
	}


//	public void TargetItem(Coordinate coor) {
//		(view as BattleMapView).StartMove (coor);
//	}

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
	

	public void Retire(bool gameOver) {
		ConfigBattleUseData.Instance.ClearData ();
		RetireQuest.SendRequest (o=>{
			ModuleManager.Instance.ExitBattle ();
			ModuleManager.Instance.ExitBattle ();
		}, ConfigBattleUseData.Instance.questDungeonData.QuestId, gameOver);
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

		ModuleManager.Instance.ExitBattle ();

		ModuleManager.Instance.ShowModule (ModuleEnum.VictoryModule,trcq);
	}

	private EQuestGridType gridType = EQuestGridType.Q_NONE;
	private Coordinate _currentCoor;
	public Coordinate currentCoor {
		set { _currentCoor = value; ConfigBattleUseData.Instance.storeBattleData.roleCoordinate = _currentCoor; }
		get { return _currentCoor;}
	}

	void YieldShowAnim() {
		int count = BattleUseData.Instance.Els.CheckLeaderSkillCount();
//		battle.ShieldInput (false);

		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "readymove", ReadyMove as Callback, count * AttackController.normalAttackInterv);
		BattleUseData.Instance.InitBattleUseData(null);
	}

	void ContineBattle () {
		NoviceGuideStepEntityManager.Instance ().StartStep ( NoviceGuideStartType.BATTLE );

		Coordinate coor = ConfigBattleUseData.Instance.storeBattleData.roleCoordinate;
//		currentCoor = coor;
		BattleUseData.Instance.Els.CheckLeaderSkillCount();
		BattleUseData.Instance.InitBattleUseData (ConfigBattleUseData.Instance.storeBattleData);
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
			BattleUseData.Instance.CheckPlayerDead();
			return;
		}

		BattleMapView.waitMove = false;
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < sbd.enemyInfo.Count; i++) {
			TEnemyInfo tei = new TEnemyInfo(sbd.enemyInfo[i]);
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
			DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}

		if (sbd.isBattle == 1) {		// 1 == battle enemy
			currentMapData.Enemy = temp;
			BattleUseData.Instance.InitEnemyInfo (currentMapData);
			if(sbd.attackRound == 0) {	// 0 == first attack
				GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
			}
		} else if (sbd.isBattle == 2) {	// 2 == battle boss
			battleEnemy = true;
//			battle.ShieldInput (true);
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
			ConfigBattleUseData.Instance.questDungeonData.Boss= temp;
			TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
			BattleUseData.Instance.InitBoss (ConfigBattleUseData.Instance.questDungeonData.Boss, bossDrop);
			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
		}
//		battle.ShowEnemy(temp);

		ExitFight (false);
		GameTimer.GetInstance ().AddCountDown (0.1f, RecoverBuff);

		BattleUseData.Instance.CheckPlayerDead();
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
			IActiveSkillExcute iase = BattleUseData.Instance.excuteActiveSkill.GetActiveSkill (ai.UserUnitID);
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
//		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < ConfigBattleUseData.Instance.questDungeonData.Boss.Count; i++) {
			TEnemyInfo tei = ConfigBattleUseData.Instance.questDungeonData.Boss [i];
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
		}
		TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
		BattleUseData.Instance.InitBoss (ConfigBattleUseData.Instance.questDungeonData.Boss, bossDrop);

		ConfigBattleUseData.Instance.storeBattleData.isBattle = 2; // 2 == battle boss. 
//		battle.ShowEnemy (temp, true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);

		ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	public void MapItemEnemy() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_battle);

		BattleMapView.waitMove = false;
//		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < currentMapData.Enemy.Count; i++) {
			TEnemyInfo tei = currentMapData.Enemy[i];
			tei.EnemySymbol = (uint)i;
			temp.Add(tei);
			DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}
		BattleUseData.Instance.InitEnemyInfo (currentMapData);
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
		BattleUseData.Instance.ac.AttackPlayer ();
//		battle.BattleCardIns.StartBattle (false);
	}

	void FirstAttack() {
		BattleUseData.Instance.ac.FirstAttack ();
	}
	
	void AttackEnd () {
//		battle.ShieldInput(true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
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

	void BattleEnd(object data = null) {
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);

		QuestCoorEnd ();
		ShieldAllInput (false);
		ExitFight (true);
		bool b = data != null ? (bool)data : false;
		if (battleEnemy && b) {
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

	public void CheckOut () {
		QuestClearShow(null);
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

	void BattleFail() {
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
			BattleUseData.Instance.AddBlood (BattleUseData.Instance.maxBlood);
			BattleUseData.Instance.RecoverEnergePoint (DataCenter.maxEnergyPoint);
			ConfigBattleUseData.Instance.StoreMapData ();
			
			Main.Instance.GInput.IsCheckInput = true;
			BattleBottomView.notClick = false;
		}, ConfigBattleUseData.Instance.questDungeonData.QuestId);
		BattleUseData.Instance.ClearData ();
	}


	void BattleFailExit(object data) {
		RetireQuest.SendRequest (o=>{
			AudioManager.Instance.PlayAudio (AudioEnum.sound_game_over);
			
			ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "over", (Callback)(()=>{
				BattleManipulationModule.colorIndex = 0;
				BattleManipulationModule.isShow = false;
				BattleEnd ();
				ModuleManager.Instance.ExitBattle ();
			}));
			ConfigBattleUseData.Instance.ClearData ();
		}, ConfigBattleUseData.Instance.questDungeonData.QuestId, true);
	}
}
