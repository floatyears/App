using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListLogic : ConcreteComponent{

	List<UnitItemViewInfo> friendUnitItemViewList = new List<UnitItemViewInfo>();
	List<string> friendNickNameList = new List<string>();

	public FriendListLogic( string uiName ) : base( uiName ) {}

	public override void ShowUI() {
		base.ShowUI();
		AddCommandListener();
		GetFriendUnitItemViewList();
		CallViewCreateDragList();
		EnableUpdateFriend();
	}

	public override void HideUI() {
		base.HideUI();
		RemoveCommandListener();
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
			case "UpdateFriendButtonClick" : 
				CallBackDispatcherHelper.DispatchCallBack(NoteFriendUpdate, cbdArgs);
				break;
			default:
				break;
		}
	}

	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.EnsureUpdateFriend, GetNewestFriendList);
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.EnsureUpdateFriend, GetNewestFriendList);
	}

	void NoteFriendUpdate(object args){
		MsgCenter.Instance.Invoke(CommandEnum.NoteFriendUpdate, null);
	}

	void GetNewestFriendList(object args){
		//ReqSever

	}

	void ViewUnitBriefInfo(object args){
		int position = (int)args;
		TFriendInfo tfi = GlobalData.friends[ position ];
		if( tfi == null ){
			Debug.LogError("ViewUnitBriefInfo(), pos : " + position + " TfriendInfo is null, return!!!");
			return;
		}
//		if(friendUnitItemViewList[ position ] == null)
		MsgCenter.Instance.Invoke(CommandEnum.ShowUserUnitBriefInfo, tfi);
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
		if(unitList == null)	{
			LogHelper.LogError("GetFriendUnitItemViewList GetUnitList return null.");
			return;
		}
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
//			LogHelper.LogError("Global.friends:i={0}, friends:{1} fUserId:{2}", i, GlobalData.friends[ i ],GlobalData.friends[ i ].UserId);
//			LogHelper.LogError("Global.friends:i={0}, friends.UserUnit:{1}", i, GlobalData.friends[ i ].UserUnit);
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

	void EnableUpdateFriend(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableUpdateButton", null);
		ExcuteCallback(cbdArgs);
	}

	void RefreshFriendList(object args){
		LogHelper.Log("RspUpdateFriendClick(), receive the click, to refresh FriendList...");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateFriendList", null);
		ExcuteCallback(cbdArgs);
	}

}
