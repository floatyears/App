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
			default:
				break;
		}
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
		if(GlobalData.friends == null){
			LogHelper.LogError("FriendListLogic.GetFriendUnitItemList(), GlobalData.friends == null!!!");
			return null;
		}

		List<TUserUnit> tuuList = new List<TUserUnit>();
		for (int i = 0; i < GlobalData.friends.Count; i++){
			tuuList.Add(GlobalData.friends[ i ].UserUnit);
		}

		return tuuList;
	}

	List<string> GetFriendNickNameList(){
		if(GlobalData.friends == null){
			LogHelper.LogError("FriendListLogic.GetFriendNickNameList(), GlobalData.friends == null!!!");
			return null;
		}

		List<string> nameList = new List<string>();
		for (int i = 0; i < GlobalData.friends.Count; i++){
			nameList.Add(GlobalData.friends[ i ].NickName);
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
}
