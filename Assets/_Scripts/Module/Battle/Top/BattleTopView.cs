using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class BattleTopView : ViewBase {
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
		UIEventListener.Get (retryButton.gameObject).onClick = Retry;

		menuButton = FindChild<UIButton>("Top/MenuButton");
		UIEventListener.Get (menuButton.gameObject).onClick = ShowMenu;
	}

	public override void HideUI () {
		if(gameObject.activeSelf)
			gameObject.SetActive (false);
	}

	public override void ShowUI () {
		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);	
			Coin= 0;
			Drop = 0;
		}
			
	}
	
	int GetCoin() {
		int coin = 0;
		foreach (var item in BattleConfigData.Instance.storeBattleData.questData) {
			coin += item.getMoney;
		}
		return coin;
	}


	public int Coin {
		set { if(coinLabel != null) coinLabel.text = value.ToString(); }
	}

	public int Drop {
		set { if(dropLabel != null) dropLabel.text = value.ToString(); }
	}

	public void SetFloor () {
		int currentFloor = BattleConfigData.Instance.questDungeonData.currentFloor + 1;
		int maxFloor = BattleConfigData.Instance.questDungeonData.Floors.Count;
//		Debug.LogError ("top ui set floor : " + currentFloor);
		if(floorLabel != null)
			floorLabel.text = currentFloor + "/" + maxFloor + "F";
	}

	public void RefreshTopUI() {
		int coin = 0;
		int drop = 0;
		foreach (var item in BattleConfigData.Instance.storeBattleData.questData) {
			coin += item.getMoney;
			drop += item.getUnit.Count;
		}
		Coin = coin;
		Drop = drop;
		SetFloor ();
	}

	public void Reset() {
		coinLabel.text = "";
		dropLabel.text = "";
		SheildInput (true);
	}

	public void SheildInput(bool b) {
		retryButton.isEnabled = b;
		menuButton.isEnabled = b;
	}

	void Retry(GameObject go) {
#if !UNITY_EDITOR
		if (NoviceGuideStepEntityManager.isInNoviceGuide()) {
			return;	
		}
#endif

//		battleQuest.Retry ();

	}

	void ShowMenu (GameObject go) {
#if !UNITY_EDITOR
		if (NoviceGuideStepEntityManager.isInNoviceGuide()) {
			return;	
		}
#endif

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

	void CreatBattleMenu () {
		 ResourceManager.Instance.LoadLocalAsset ("Prefabs/BattleMenu", o => {
			GameObject go = o as GameObject;
			go = NGUITools.AddChild (ViewManager.Instance.CenterPanel, go);
			go.transform.localPosition = new Vector3 (0f, 0f, 0f);
			go.layer = GameLayer.BottomInfo;
//			battleMenu = go.GetComponent<BattleMenu> ();
//			battleMenu.battleQuest = battleQuest;
////			battleMenu.Init ("BattleMenu");
//			battleMenu.ShowUI();
		});

	}

	
	public void Retry () {
//		Main.Instance.GInput.IsCheckInput = false;
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
		RedoQuest.SendRequest (SureRetryNetWork, BattleConfigData.Instance.questDungeonData.QuestId, BattleConfigData.Instance.questDungeonData.currentFloor);
		
//		Main.Instance.GInput.IsCheckInput = true;
		BattleBottomView.notClick = false;
	}
	
	void CancelInitiativeRetry(object data) {
//		Main.Instance.GInput.IsCheckInput = true;
		BattleBottomView.notClick = false;
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
		
		DataCenter.Instance.AccountInfo.Stone = rrq.stone;
		BattleConfigData.Instance.RefreshCurrentFloor(rrq);;
		BattleConfigData.Instance.InitRoleCoordinate (new Coordinate (MapConfig.characterInitCoorX, MapConfig.characterInitCoorY));
		BattleConfigData.Instance.StoreMapData ();
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
		BattleAttackManager.Instance.ResetBlood ();
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

	TClearQuestParam GetQuestData () {
		ClearQuestParam cq = new ClearQuestParam ();
		TClearQuestParam cqp = new TClearQuestParam (cq);
		cqp.questId = BattleConfigData.Instance.questDungeonData.QuestId;
		foreach (var item in BattleConfigData.Instance.storeBattleData.questData) {
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

			Umeng.GA.FinishLevel ("Quest" + BattleConfigData.Instance.currentQuestInfo.ID.ToString());
		} else {
			TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("RetryClearQuestTitle"),TextCenter.GetText("RetryClearQuestNet",DataCenter.redoQuestStone, 
			                                                                                                  DataCenter.Instance.AccountInfo.Stone),TextCenter.GetText("Retry"),RequestData);
			
		}
	}

	
	void QuestEnd (TRspClearQuest trcq) {
		if (BattleConfigData.Instance.currentStageInfo != null) {
			if ( BattleConfigData.Instance.currentStageInfo.Type == QuestType.E_QUEST_STORY ) { // story quest
				DataCenter.Instance.QuestClearInfo.UpdateStoryQuestClear (BattleConfigData.Instance.currentStageInfo.ID, BattleConfigData.Instance.currentQuestInfo.ID);
			} else { 
				DataCenter.Instance.QuestClearInfo.UpdateEventQuestClear (BattleConfigData.Instance.currentStageInfo.ID, BattleConfigData.Instance.currentQuestInfo.ID);
			}	
		}
		
		ModuleManager.Instance.ExitBattle ();
		
		ModuleManager.Instance.ShowModule (ModuleEnum.VictoryModule,trcq);
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
		
		BattleConfigData.Instance.gameState = (byte)GameState.Normal;
		//		DataCenter.Instance.RefreshUserInfo(rsp)
		DataCenter.Instance.UserInfo.Rank = rsp.rank;
		DataCenter.Instance.UserInfo.Exp = rsp.exp;
		DataCenter.Instance.AccountInfo.Money = rsp.money;
		DataCenter.Instance.AccountInfo.FriendPoint = rsp.friendPoint;
		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaMax = rsp.staminaMax;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;	
		//
		TUnitParty tup = BattleConfigData.Instance.party;
		foreach (var item in tup.UserUnit.Values) {
			if(item == null ) {
				continue;
			}
			if ( item.ID != rsp.evolvedUnit.uniqueId ) { //only delete Evo Materials, not delete BaseUnit
				DataCenter.Instance.UserUnitList.DelMyUnit(item.ID);
			}
		}
		BattleConfigData.Instance.party = null;
		
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

	
	void End() {
//		BattleManipulationView.colorIndex = 0;
//		BattleManipulationView.isShow = false;
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_quest_clear);
	}
}
