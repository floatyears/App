using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendHelperController : ConcreteComponent{
	TFriendInfo selectedHelper;
	uint questID;
	uint stageID;

	List<UnitItemViewInfo> supportFriendViewList = new List<UnitItemViewInfo>();
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();
	
	public FriendHelperController(string uiName):base(uiName) {}
	
	public override void CreatUI () { base.CreatUI (); }

	public override void ShowUI () {
		base.ShowUI ();
		GetSupportFriendInfoList();
		CreateFriendHelperViewList();
		AddCommandListener();
	}
	
	public override void HideUI () {
		base.HideUI ();
		DestoryFriendHelperList();
		ClearSelectedHelper();
		RemoveCommandListener();
	}

	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "ClickItem" :
				CallBackDispatcherHelper.DispatchCallBack(ShowHelperInfo, cbdArgs);
				break;
			case "ClickBottomButton" :
				CallBackDispatcherHelper.DispatchCallBack(QuestStart, cbdArgs);
				break;
			default:
				break;
		}
	}

	void QuestStart(object args){
//		Dictionary<string, object> battleReadyInfo = new Dictionary<string, object>();
//		battleReadyInfo.Add("QuestID", questID);
//		battleReadyInfo.Add("StageID", stageID);
//		battleReadyInfo.Add("PartyID", DataCenter.Instance.PartyInfo.CurrentPartyId);
//		battleReadyInfo.Add("Helper", selectedHelper);
		//TODO Change to Battle here

		StartQuest sq = new StartQuest ();

		StartQuestParam sqp = new StartQuestParam ();

		sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
		sqp.helperUserUnit = selectedHelper;
		sqp.questId = questID;
		sqp.stageId = stageID;
		sqp.startNew = 1;
		sq.OnRequest (sqp, RspStartQuest);
	}

	void RspStartQuest(object data) {
		TQuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		Debug.LogError (rspStartQuest.header.code  + "  " + rspStartQuest.header.error);
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);

			DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;
			tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
			
			ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
		}
		
		if (data == null || tqdd == null) {
			Debug.LogError("Request quest info fail : data " + data + "  TQuestDungeonData : " + tqdd);

			return;
		}
		
		UIManager.Instance.EnterBattle();
	} 

	MsgWindowParams GetStartQuestError () {
		MsgWindowParams mwp = new MsgWindowParams ();
		return mwp;
	}


	List<TUserUnit> GetSupportFriendList(){
		if (DataCenter.Instance.SupportFriends == null){
			LogHelper.LogError("GetFriendUnitItemList(), DataCenter.Instance.SupportFriends == null!!!");
			return null;
		}
		
		List<TUserUnit> tuuList = new List<TUserUnit>();

		for (int i = 0; i < DataCenter.Instance.SupportFriends.Count; i++){
			tuuList.Add(DataCenter.Instance.SupportFriends[ i ].UserUnit);
		}
		
		return tuuList;
	}

	void GetSupportFriendInfoList(){
		supportFriendViewList.Clear();
		List<TFriendInfo> helperList = DataCenter.Instance.SupportFriends;
				
		for (int i = 0; i < helperList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(helperList[ i ]);
			supportFriendViewList.Add(viewItem);
		}
	}


	void CreateFriendHelperViewList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", supportFriendViewList);
		ExcuteCallback(cbdArgs);
	}

	void DestoryFriendHelperList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", null);
		ExcuteCallback(cbdArgs);
	}

	void ShowHelperInfo(object args){
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
		MsgCenter.Instance.AddListener(CommandEnum.ChooseHelper, ChooseHelper);
		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedQuest, RecordSelectedQuest);
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ChooseHelper, ChooseHelper);
		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedQuest, RecordSelectedQuest);
	}

	void ChooseHelper(object msg){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableBottomButton", null);
		ExcuteCallback(cbdArgs);
		if(selectedHelper != null){
			MsgCenter.Instance.Invoke(CommandEnum.AddHelperItem, selectedHelper);
		}
	}
	
	void RecordSelectedQuest(object msg){
		Dictionary<string,uint> idArgs = msg as Dictionary<string,uint>;
		questID = idArgs["QuestID"];
		stageID = idArgs["StageID"];
	}

	void ClearBattleReadyData(){
	}

}
