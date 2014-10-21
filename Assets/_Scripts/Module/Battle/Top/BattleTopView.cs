using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;public class BattleTopView : ViewBase {
	[HideInInspector]
	public UILabel coinLabel;
	private UILabel dropLabel;
	private UILabel floorLabel;
	private UIButton menuButton;

	private UIButton retryButton;

//	private BattleMenu battleMenu;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
//	}
//		base.Init (name);

		coinLabel = FindChild<UILabel> ("Top/CoinLabel");
		dropLabel = FindChild<UILabel> ("Top/DropLabel");
		floorLabel = FindChild<UILabel> ("Top/FloorLabel");
		retryButton = FindChild<UIButton>("Top/RetryButton");
		UIEventListenerCustom.Get (retryButton.gameObject).onClick = Retry;

		menuButton = FindChild<UIButton>("Top/MenuButton");
		UIEventListenerCustom.Get (menuButton.gameObject).onClick = ShowMenu;
	}

	public override void CallbackView (params object[] args)
	{
		switch (args[0].ToString()) {
		case "refresh":
			RefreshTopUI();
			break;
		case "clear_quest":
			RequestData(null);
			break;
		default:
			break;
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		RefreshTopUI ();
			
	}

	void SetFloor () {
		int currentFloor = BattleConfigData.Instance.questDungeonData.currentFloor + 1;
		int maxFloor = BattleConfigData.Instance.questDungeonData.Floors.Count;
//		Debug.LogError ("top ui set floor : " + currentFloor);
		if(floorLabel != null)
			floorLabel.text = currentFloor + "/" + maxFloor + "F";
	}

	void RefreshTopUI() {
		int coin = 0;
		int drop = 0;
		foreach (var item in BattleConfigData.Instance.storeBattleData.questData) {
			coin += item.getMoney;
			drop += item.getUnit.Count;
		}
		coinLabel.text = coin.ToString();
		dropLabel.text = drop.ToString();
		SetFloor ();
	}

	void Reset() {
		coinLabel.text = "0";
		dropLabel.text = "0";
	}

	void Retry(GameObject go) {
#if !UNITY_EDITOR
		if (NoviceGuideStepManager.Instance.isInNoviceGuide()) {
			return;	
		}
#endif
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("RedoQuestTitle"),
		                                    TextCenter.GetText ("RedoQuestContent", DataCenter.redoQuestStone, DataCenter.Instance.UserData.AccountInfo.stone),
		                                    TextCenter.GetText ("OK"), TextCenter.GetText ("Cancel"), SureInitiativeRetry, CancelInitiativeRetry);
//		battleQuest.Retry ();

	}

	void ShowMenu (GameObject go) {
#if !UNITY_EDITOR
		if (NoviceGuideStepManager.Instance.isInNoviceGuide()) {
			return;	
		}
#endif
		ModuleManager.Instance.ShowModule (ModuleEnum.MusicModule);
//		if (battleMenu == null) {
//			CreatBattleMenu ();
//			return;
//		}

//		if (!battleMenu.gameObject.activeSelf) {
//			battleMenu.ShowUI ();
//		} else {
//			battleMenu.HideUI ();
//		}
	}

	void SureInitiativeRetry(object data) {
		if (DataCenter.Instance.UserData.AccountInfo.stone < DataCenter.redoQuestStone) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("NotEnoughStone"));
			return;
		}
		
		//		battle.ShieldInput (false);
		//		if (battle.isShowEnemy) {
		//			MsgCenter.Instance.Invoke(CommandEnum.BattleEnd);
		//		}
		QuestController.Instance.RedoQuest (SureRetryNetWork, BattleConfigData.Instance.questDungeonData.questId, BattleConfigData.Instance.questDungeonData.currentFloor);
		
//		Main.Instance.GInput.IsCheckInput = true;
//		BattleBottomView.notClick = false;
	}
	
	void CancelInitiativeRetry(object data) {
//		Main.Instance.GInput.IsCheckInput = true;
//		BattleBottomView.notClick = false;
	}

	
	void SureRetryNetWork(object data) {
		Umeng.GA.Buy ("RedoQuest", 1, DataCenter.redoQuestStone);
//		BattleMapView.waitMove = false;
//		(view as BattleMapView).BattleEndRotate(null);
		RefreshRetryData (data);
//		Main.Instance.GInput.IsCheckInput = true;
		GameInput.OnPressEvent += SureRetryPress;
	}

	
	void RefreshRetryData(object data) {
		RspRedoQuest rrq = data as RspRedoQuest;
		if (rrq == null) {
			return;	
		}
		
		DataCenter.Instance.UserData.AccountInfo.stone = rrq.stone;
		BattleConfigData.Instance.RefreshCurrentFloor(rrq);
	}
	
	void SureRetryPress() {
		//		Debug.LogError ("SureRetryPress ");
		GameInput.OnPressEvent -= SureRetryPress;
		RetryRefreshUI ();
	}
	
	void RetryRefreshUI() {
		//		battle.ShieldInput (true);
		//		if (battle.isShowEnemy) {
		//			ExitFight (true);
		//			configBattleUseData.storeBattleData.attackRound = 0;
		//			configBattleUseData.StoreMapData(null);
		//			battle.ExitFight();
		//		}
		Reset ();
		BattleAttackManager.Instance.ResetBlood ();
	}

	void RequestData (object data) {
//		if (DataCenter.gameState == GameState.Evolve) {
////			EvolveDone evolveDone = new EvolveDone ();
//			ClearQuestParam cqp = GetQuestData();
//			UnitController.Instance.EvolveDone(ResponseEvolveQuest,cqp.questID,0,cqp.getMoney,cqp.getUnit,cqp.hitGrid);
//		} else {
		QuestController.Instance.ClearQuest(GetQuestData (), ResponseClearQuest);
//		}
	}

	ClearQuestParam GetQuestData () {
		ClearQuestParam cqp = new ClearQuestParam ();
		cqp.questID = BattleConfigData.Instance.questDungeonData.questId;
		foreach (var item in BattleConfigData.Instance.storeBattleData.questData) {
			cqp.getMoney += item.getMoney;
			cqp.getUnit.AddRange(item.getUnit);
			cqp.hitGrid.AddRange(item.hitGrid);
		}
		
		return cqp;
	}

	void ResponseClearQuest (object data) {
		if (data != null) {
			DataCenter.Instance.oldAccountInfo = DataCenter.Instance.UserData.UserInfo;
			TRspClearQuest clearQuest = data as TRspClearQuest;
			DataCenter.Instance.UserData.RefreshUserInfo (clearQuest);
			End();
			QuestEnd(clearQuest);

			Umeng.GA.FinishLevel ("Quest" + BattleConfigData.Instance.currentQuestInfo.id.ToString());
		} else {
			TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("RetryClearQuestTitle"),TextCenter.GetText("RetryClearQuestNet",DataCenter.redoQuestStone, 
			                                                                                                  DataCenter.Instance.UserData.AccountInfo.stone),TextCenter.GetText("Retry"),RequestData);
			
		}
	}

	
	void QuestEnd (TRspClearQuest trcq) {
		if (BattleConfigData.Instance.currentStageInfo != null) {
			if ( BattleConfigData.Instance.currentStageInfo.type == QuestType.E_QUEST_STORY ) { // story quest
				DataCenter.Instance.QuestData.QuestClearInfo.UpdateStoryQuestClear (BattleConfigData.Instance.currentStageInfo.id, BattleConfigData.Instance.currentQuestInfo.id);
			} else { 
				DataCenter.Instance.QuestData.QuestClearInfo.UpdateEventQuestClear (BattleConfigData.Instance.currentStageInfo.id, BattleConfigData.Instance.currentQuestInfo.id);
			}	
		}
		
		ModuleManager.Instance.ExitBattle ();
		
		ModuleManager.Instance.ShowModule (ModuleEnum.BattleResultModule,"data", trcq);
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
		
//		BattleConfigData.Instance.gameState = (byte)GameState.Normal;
		//		DataCenter.Instance.RefreshUserInfo(rsp)
		DataCenter.Instance.UserData.UserInfo.rank = rsp.rank;
		DataCenter.Instance.UserData.UserInfo.exp = rsp.exp;
		DataCenter.Instance.UserData.AccountInfo.money = rsp.money;
		DataCenter.Instance.UserData.AccountInfo.friendPoint = rsp.friendPoint;
		DataCenter.Instance.UserData.UserInfo.staminaNow = rsp.staminaNow;
		DataCenter.Instance.UserData.UserInfo.staminaMax = rsp.staminaMax;
		DataCenter.Instance.UserData.UserInfo.staminaRecover = rsp.staminaRecover;	
		//
		UnitParty tup = BattleConfigData.Instance.party;
		foreach (var item in tup.UserUnit) {
			if(item == null ) {
				continue;
			}
			if ( item.isFavorite != rsp.evolvedUnit.uniqueId ) { //only delete Evo Materials, not delete BaseUnit
				DataCenter.Instance.UnitData.UserUnitList.DelMyUnit(item.uniqueId);
			}
		}
		BattleConfigData.Instance.party = null;
		
		for (int i = 0; i < rsp.gotUnit.Count; i++) {
			DataCenter.Instance.UnitData.UserUnitList.AddMyUnit(rsp.gotUnit[i]);
		}
		
		//update the evolved unit
		DataCenter.Instance.UnitData.UserUnitList.AddMyUnit(rsp.evolvedUnit);
		
		TRspClearQuest trcq = new TRspClearQuest ();
		trcq.exp = rsp.exp;
		trcq.gotExp = rsp.gotExp;
		trcq.money = rsp.money;
		trcq.gotMoney = rsp.gotMoney;
		trcq.gotStone = rsp.gotStone;
		rsp.evolvedUnit.userID = DataCenter.Instance.UserData.UserInfo.userId;
		trcq.evolveUser = rsp.evolvedUnit;
		List<UserUnit> temp = new List<UserUnit> ();
		for (int i = 0; i <  rsp.gotUnit.Count; i++) {
			rsp.gotUnit[i].userID = DataCenter.Instance.UserData.UserInfo.userId;
			UserUnit tuu = rsp.gotUnit[i];
			temp.Add(tuu);
		}
		trcq.gotUnit = temp;
		trcq.rank = rsp.rank;
		DataCenter.Instance.oldAccountInfo = DataCenter.Instance.UserData.UserInfo;
		End();
		ModuleManager.Instance.ExitBattle ();
		DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId = 0;
		
		ModuleManager.Instance.ShowModule (ModuleEnum.BattleResultModule,trcq);
	}

	
	void End() {
//		BattleManipulationView.colorIndex = 0;
//		BattleManipulationView.isShow = false;
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_quest_clear);
	}
}
