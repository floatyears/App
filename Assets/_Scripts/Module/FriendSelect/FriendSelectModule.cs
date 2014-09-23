using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class FriendSelectModule : ModuleBase{
	FriendInfo selectedHelper;
	uint questID;
	uint stageID;
	List<UnitItemViewInfo> supportFriendViewList = new List<UnitItemViewInfo>();
	Dictionary<int,UserUnit> userUnit = new Dictionary<int, UserUnit> ();
	private UnitDataModel evolveStart = null;

	public FriendSelectModule( UIConfigItem config, params object[] data):base( config, data) {
		CreateUI<FriendSelectView> ();

	}
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

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (data[0].ToString()){
			case "ClickItem" :
				ShowHelperInfo(data[1]);
				break;
			case "ClickBottomButton" :
				QuestStart(data[1]);
				break;
			default:
				break;
		}
	}

	void QuestStart(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		if (DataCenter.gameState == GameState.Evolve) {
//			evolveStart.EvolveStart.restartNew = 1;
//			evolveStart.EvolveStart.OnRequest(null, RspEvolveStartQuest);
		} 
		else {
//			StartQuest sq = new StartQuest ();
			StartQuestParam sqp = new StartQuestParam ();
			sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
			sqp.helperUserUnit = selectedHelper;
			sqp.questId = questID;
			sqp.stageId = stageID;
			sqp.startNew = 1;
//			DataCenter.StartQuestInfo = sqp;
			QuestController.Instance.StartQuest (sqp, RspStartQuest);
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

//		tqdd.assignData ();
		DataCenter.Instance.SetData(ModelEnum.MapConfig, questDungeonData);

		EnterBattle ();
	}

	void RspStartQuest(object data) {
		QuestDungeonData tqdd = null;
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
			tqdd = rspStartQuest.dungeonData;
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
		BattleConfigData.Instance.BattleFriend = selectedHelper;
		HideUI ();
		ModuleManager.Instance.EnterBattle();
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
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", null);
		view.CallbackView("CreateDragView");
	}

	void DestoryFriendHelperList(){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", null);
		view.CallbackView("DestoryDragView");
	}

	void ShowHelperInfo(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		FriendInfo helper = DataCenter.Instance.SupportFriends[ (int)args ];
		RecordSelectedHelper(helper);
		MsgCenter.Instance.Invoke(CommandEnum.FriendBriefInfoShow, helper);
	}

	void RecordSelectedHelper(FriendInfo tfi){
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
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateViewAfterChooseHelper", null);
		view.CallbackView("UpdateViewAfterChooseHelper");
	}
	
	void RefreshFriendHelper(object data) {
		CanEnterBattle ();
	}

	void CanEnterBattle () {
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableBottomButton", null);
		view.CallbackView("EnableBottomButton");
	}
	
	void EvolveSelectQuest(object data) {
		evolveStart = data as UnitDataModel;
	}
	
	void RecordSelectedQuest(object msg){
		Dictionary<string,uint> idArgs = msg as Dictionary<string,uint>;
		questID = idArgs["QuestID"];
		stageID = idArgs["StageID"];
	}

	void ClearBattleReadyData(){}
}
