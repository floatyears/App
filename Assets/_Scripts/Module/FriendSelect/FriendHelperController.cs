using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendHelperController : ModuleBase{
	TFriendInfo selectedHelper;
	uint questID;
	uint stageID;
	List<UnitItemViewInfo> supportFriendViewList = new List<UnitItemViewInfo>();
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();
	private TEvolveStart evolveStart = null;

	public FriendHelperController( UIConfigItem config):base( config) {}
	public override void InitUI () { base.InitUI (); }
	public override void ShowUI () {
		base.ShowUI ();
//		CreateFriendHelperViewList();
//		AddCommandListener();
	}
	
	public override void HideUI () {
		base.HideUI ();
//		DestoryFriendHelperList();
//		ClearSelectedHelper();
//		RemoveCommandListener();
	}

	public override void OnReceiveMessages(object data){
		base.OnReceiveMessages(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
//			case "ClickItem" :
//				CallBackDispatcherHelper.DispatchCallBack(ShowHelperInfo, cbdArgs);
//				break;
//			case "ClickBottomButton" :
//				CallBackDispatcherHelper.DispatchCallBack(QuestStart, cbdArgs);
//				break;
			default:
				break;
		}
	}

	void QuestStart(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		if (DataCenter.gameState == GameState.Evolve) {
			evolveStart.EvolveStart.restartNew = 1;
			evolveStart.EvolveStart.OnRequest(null, RspEvolveStartQuest);
		} 
		else {
			StartQuest sq = new StartQuest ();
			StartQuestParam sqp = new StartQuestParam ();
			sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
			sqp.helperUserUnit = selectedHelper;
			sqp.questId = questID;
			sqp.stageId = stageID;
			sqp.startNew = 1;
//			DataCenter.StartQuestInfo = sqp;
			sq.OnRequest (sqp, RspStartQuest);
		}
	}

	void RspEvolveStartQuest (object data) {
		if (data == null){
//			Debug.Log("OnRspEvolveStart(), response null");
			return;
		}
//		evolveStart.StoreData ();

		bbproto.RspEvolveStart rsp = data as bbproto.RspEvolveStart;
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}

		// TODO do evolve start over;
		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;
		bbproto.QuestDungeonData questDungeonData = rsp.dungeonData;
		TQuestDungeonData tqdd = new TQuestDungeonData (questDungeonData);
//		tqdd.assignData ();
		DataCenter.Instance.SetData(ModelEnum.MapConfig, tqdd);

		EnterBattle ();
	}

	void RspStartQuest(object data) {
		TQuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		if (rspStartQuest.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rspStartQuest.header.code+", error:"+rspStartQuest.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspStartQuest.header.code);
			return;
		}

//		Debug.LogError (rspStartQuest.header.code  + "  " + rspStartQuest.header.error);
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			LogHelper.Log ("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;
			tqdd = new TQuestDungeonData (rspStartQuest.dungeonData);
//			tqdd.assignData();
			DataCenter.Instance.SetData (ModelEnum.MapConfig, tqdd);
		} 
		
		if (data == null || tqdd == null) {
//			Debug.LogError("Request quest info fail : data " + data + "  TQuestDungeonData : " + tqdd);
//			DataCenter.StartQuestInfo = null;
			return;
		}
//		HideUI ();
		EnterBattle ();
	} 
		
	void EnterBattle () {
		ConfigBattleUseData.Instance.BattleFriend = selectedHelper;
		HideUI ();
		UIManager.Instance.EnterBattle();
	} 

	MsgWindowParams GetStartQuestError () {
		MsgWindowParams mwp = new MsgWindowParams ();
		return mwp;
	}


//	List<TUserUnit> GetSupportFriendList(){
//		if (DataCenter.Instance.SupportFriends == null){
//			LogHelper.LogError("GetFriendUnitItemList(), DataCenter.Instance.SupportFriends == null!!!");
//			return null;
//		}
//		
//		List<TUserUnit> tuuList = new List<TUserUnit>();
//
//		for (int i = 0; i < DataCenter.Instance.SupportFriends.Count; i++){
//			tuuList.Add(DataCenter.Instance.SupportFriends[ i ].UserUnit);
//		}
//		
//		return tuuList;
//	}

//	void GetSupportFriendInfoList(){
//		supportFriendViewList.Clear();
//		List<TFriendInfo> helperList = DataCenter.Instance.SupportFriends;
//				
//		for (int i = 0; i < helperList.Count; i++){
//			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(helperList[ i ]);
//			supportFriendViewList.Add(viewItem);
//		}
//	}

	void CreateFriendHelperViewList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", null);
		view.CallbackView(cbdArgs);
	}

	void DestoryFriendHelperList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", null);
		view.CallbackView(cbdArgs);
	}

	void ShowHelperInfo(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TFriendInfo helper = DataCenter.Instance.SupportFriends[ (int)args ];
		RecordSelectedHelper(helper);
		MsgCenter.Instance.Invoke(CommandEnum.FriendBriefInfoShow, helper);
	}

	void RecordSelectedHelper(TFriendInfo tfi){
		selectedHelper = tfi;
	}

	void ClearSelectedHelper(){
		selectedHelper = null;
	}

	void AddCommandListener(){
//		MsgCenter.Instance.AddListener(CommandEnum.ChooseHelper, ChooseHelper);
		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedQuest, RecordSelectedQuest);
		MsgCenter.Instance.AddListener(CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
		MsgCenter.Instance.AddListener (CommandEnum.RefreshFriendHelper, RefreshFriendHelper);
	}

	void RemoveCommandListener(){
//		MsgCenter.Instance.RemoveListener(CommandEnum.ChooseHelper, ChooseHelper);
		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedQuest, RecordSelectedQuest);
		MsgCenter.Instance.RemoveListener(CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
		MsgCenter.Instance.RemoveListener (CommandEnum.RefreshFriendHelper, RefreshFriendHelper);
	}

	void ChooseHelper(object msg){
//		selectedHelper = msg as TFriendInfo;
		if(selectedHelper == null) { 
			Debug.Log("selectedHelper is NULL, return...");
			return;
		}
	
		MsgCenter.Instance.Invoke(CommandEnum.AddHelperItem, selectedHelper);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateViewAfterChooseHelper", null);
		view.CallbackView(cbdArgs);
	}
	
	void RefreshFriendHelper(object data) {
		CanEnterBattle ();
	}

	void CanEnterBattle () {
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableBottomButton", null);
		view.CallbackView(cbdArgs);
	}
	
	void EvolveSelectQuest(object data) {
		evolveStart = data as TEvolveStart;
	}
	
	void RecordSelectedQuest(object msg){
		Dictionary<string,uint> idArgs = msg as Dictionary<string,uint>;
		questID = idArgs["QuestID"];
		stageID = idArgs["StageID"];
	}

	void ClearBattleReadyData(){}
}
