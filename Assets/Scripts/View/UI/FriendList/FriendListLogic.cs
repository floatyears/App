using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListLogic : ConcreteComponent{

	List<UnitItemViewInfo> friendUnitItemViewList = new List<UnitItemViewInfo>();
	List<string> friendNickNameList = new List<string>();

	public FriendListLogic( string uiName ) : base( uiName ) {}

	public override void ShowUI() {
		base.ShowUI();

		GetFriendUnitItemViewList();
		CallViewCreateDragList();
	}

	public override void HideUI() {
		base.HideUI();
		DestoryUnitList();
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (cbdArgs.funcName){
			case "PressItem" : 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitDetailInfo, cbdArgs);
				break;
			case "ClickItem" : 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitBriefInfo, cbdArgs);
				break;
			default:
				break;
		}
	}
	void ViewUnitBriefInfo(object args){
		int position = (int)args;
	}

	void ViewUnitDetailInfo(object args){
		int position = (int)args;
		TUserUnit tuu = friendUnitItemViewList[ position ].DataItem;
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, tuu);
	}

	void GetFriendUnitItemViewList(){
		//First, clear the current if exist
		friendUnitItemViewList.Clear();
		//Then, get the newest from DataCenter
		List<TUserUnit> unitList = GetFriendUnitItemList();
		if(unitList == null)	return;
		for(int i = 0; i < unitList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(unitList[ i ]);
			friendUnitItemViewList.Add(viewItem);
		}

		friendNickNameList = GetFriendNickNameList();
		LogHelper.Log("FriendListLogic.GetFriendUnitItemViewList(), ViewItem Count is : " + friendUnitItemViewList.Count);
	}

	List<TUserUnit> GetFriendUnitItemList(){
		if(DataCenter.Instance.SupportFriends == null){
			LogHelper.LogError("FriendListLogic.GetFriendUnitItemList(), DataCenter.Instance.SupportFriends == null!!!");
			return null;
		}

		List<TUserUnit> tuuList = new List<TUserUnit>();
		for (int i = 0; i < DataCenter.Instance.SupportFriends.Count; i++){
			tuuList.Add(DataCenter.Instance.SupportFriends[ i ].UserUnit);
		}

		return tuuList;
	}

	List<string> GetFriendNickNameList(){
		if(DataCenter.Instance.SupportFriends == null){
			LogHelper.LogError("FriendListLogic.GetFriendNickNameList(), DataCenter.Instance.SupportFriends == null!!!");
			return null;
		}

		List<string> nameList = new List<string>();
		for (int i = 0; i < DataCenter.Instance.SupportFriends.Count; i++){
			nameList.Add(DataCenter.Instance.SupportFriends[ i ].NickName);
		}

		return nameList;
	}

	void CallViewCreateDragList(){
		CallBackDispatcherArgs mainCbdArgs = new CallBackDispatcherArgs("CreateDragView", friendUnitItemViewList);
		ExcuteCallback(mainCbdArgs);

		CallBackDispatcherArgs nameCbdArgs = new CallBackDispatcherArgs("ShowFriendName", friendNickNameList);
		ExcuteCallback(nameCbdArgs);
	}

	void DestoryUnitList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", null);
		ExcuteCallback(cbdArgs);
	}

	void ActivateUpdateFriendList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ActivateUpdateButton", null);
		ExcuteCallback(cbdArgs);
	}

	void RefreshFriendList(object args){
		LogHelper.Log("RspUpdateFriendClick(), receive the click, to refresh FriendList...");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateFriendList", null);
		ExcuteCallback(cbdArgs);
	}

}
