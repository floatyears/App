using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendHelperController : ConcreteComponent{
	TFriendInfo selectedHelper;
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
		List<TUserUnit> unitList = GetSupportFriendList();
		if (unitList == null){
			LogHelper.LogError("GetFriendUnitItemViewList GetUnitList return null.");
			return;
		}

		LogHelper.Log("FriendListLogic.GetFriendUnitItemViewList(), unitList Count is : " + unitList.Count);
		
		for (int i = 0; i < unitList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(unitList [i]);
			supportFriendViewList.Add(viewItem);
		}
		
		LogHelper.Log("FriendListLogic.GetFriendUnitItemViewList(), ViewItem Count is : " + supportFriendViewList.Count);
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
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ChooseHelper, ChooseHelper);
	}

	void ChooseHelper(object msg){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableBottomButton", null);
		ExcuteCallback(cbdArgs);
		if(selectedHelper != null)
			MsgCenter.Instance.Invoke(CommandEnum.AddHelperItem, selectedHelper);
	}

}
