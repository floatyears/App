using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleMapModule : ModuleBase {
	private GameObject rootObject;

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
			(view as BattleMapView).ContineBattle ();
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

	public static bool recoverPosion = false;
	public static bool reduceHurt = false;
	public static bool reduceDefense = false;
	public static bool strengthenAttack = false;



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

		(view as BattleMapView).ExitFight (false);
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	void ShieldAllInput (bool shiled) {
		Main.Instance.NguiCamera.enabled = shiled;
		Main.Instance.GInput.IsCheckInput = shiled;
		BattleBottomView.notClick = !shiled;
	}


	void BattleEnd(object data = null) {

		QuestCoorEnd ();
		ShieldAllInput (false);
		(view as BattleMapView).ExitFight (true);
		bool b = data != null ? (bool)data : false;
		if (battleEnemy && b) {
			(view as BattleMapView).BossDead();
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
//				BattleEnd ();

				ModuleManager.Instance.ExitBattle ();
			}));
			ConfigBattleUseData.Instance.ClearData ();
		}, ConfigBattleUseData.Instance.questDungeonData.QuestId, true);
	}
}
