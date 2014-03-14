using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListLogic : ConcreteComponent
{

	List<UnitItemViewInfo> friendUnitItemViewList = new List<UnitItemViewInfo>();
	List<string> friendNickNameList = new List<string>();

	public FriendListLogic(string uiName) : base( uiName ){

	}

	public override void ShowUI()
	{
		base.ShowUI();

		AddCommandListener();

		GetFriendUnitItemViewList();
		CallViewCreateDragList();
		EnableExtraFunction();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveCommandListener();
		DestoryUnitList();

	}

	public override void Callback(object data)
	{
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (cbdArgs.funcName){
			case "PressItem": 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitDetailInfo, cbdArgs);
				break;
			case "ClickItem": 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitBriefInfo, cbdArgs);
				break;
			case "UpdateFriendButtonClick": 
				CallBackDispatcherHelper.DispatchCallBack(NoteFriendUpdate, cbdArgs);
				break;
			default:
				break;
		}
	}

	void AddCommandListener()
	{
		MsgCenter.Instance.AddListener(CommandEnum.EnsureUpdateFriend, GetNewestFriendList);
	}
		
	void RemoveCommandListener()
	{
		MsgCenter.Instance.AddListener(CommandEnum.EnsureUpdateFriend, GetNewestFriendList);
	}
	void NoteFriendUpdate(object args)
	{
		MsgCenter.Instance.Invoke(CommandEnum.NoteFriendUpdate, null);
	}
	
	List<TFriendInfo> CurrentFriendListData()
	{
		switch (UIManager.Instance.baseScene.CurrentScene)
		{
			case SceneEnum.FriendList : 
				Debug.Log("CurrentFriendListData()" + DataCenter.Instance.FriendList.Friend); 
				return DataCenter.Instance.FriendList.Friend;
				break;
			case SceneEnum.Apply : 
				Debug.Log("CurrentFriendListData()" + DataCenter.Instance.FriendList.FriendIn); 
				return DataCenter.Instance.FriendList.FriendIn;
				break;
			case SceneEnum.Reception : 
				Debug.Log("CurrentFriendListData()" + DataCenter.Instance.FriendList.FriendOut); 
				return DataCenter.Instance.FriendList.FriendOut;
				break;
			
			default:
				return new List<TFriendInfo>();
				break;
		}

	}

	void GetNewestFriendList(object args)
	{
		//ReqSever
		GetFriendList.SendRequest(OnGetFriendList);
	}


	void OnGetFriendList(object data)
	{
		if (data == null)
			return;
        
		LogHelper.Log("TFriendList.Refresh() begin");
		LogHelper.Log(data);
		bbproto.RspGetFriend rsp = data as bbproto.RspGetFriend;
        
		if (rsp.header.code != (int)ErrorCode.SUCCESS)
		{
			LogHelper.Log("RspGetFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}
        
		bbproto.FriendList inst = rsp.friends;

		DataCenter.Instance.FriendList.RefreshFriendList(inst);
		// test
		LogHelper.Log("OnGetFriendList, test first friend. nick name" + CurrentFriendListData() [1].NickName);
		HideUI();
		ShowUI();
	}

    void RefuseFriend(uint friendUid){
        DelFriend.SendRequest(OnDelFriend, friendUid);
    }

    void RefuseFriendAll(){
        List <uint> refuseList = new List<uint>;
        for(int i = 0; i < DataCenter.Instance.FriendList.FriendIn.Count; i++){
            refuseList.Add(DataCenter.Instance.FriendList.FriendIn[i].UserId);
        }
        DelFriend.SendRequest(OnDelFriend, refuseList);
    }

    void AcceptFriendRequest(uint friendUid){
        AcceptFriend.SendRequest(OnAcceptFriend, friendUid);
    }

	void OnAcceptFriend(object data)
	{
		if (data == null)
			return;
        
		LogHelper.Log("TFriendList.OnRspAddFriend() begin");
		LogHelper.Log(data);
		bbproto.RspAddFriend rsp = data as bbproto.RspAddFriend;
        
		if (rsp.header.code != (int)ErrorCode.SUCCESS)
		{
			LogHelper.Log("RspAddFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}

		bbproto.FriendList inst = rsp.friends;
        
		DataCenter.Instance.FriendList.RefreshFriendList(inst);

		// test
		LogHelper.Log("OnAcceptFriend, test first friend. nick name" + CurrentFriendListData() [1].NickName);
		HideUI();
		ShowUI();
	}

	void OnDelFriend(object data)
	{
		if (data == null)
			return;
        
		LogHelper.Log("TFriendList.OnDelFriend() begin");
		LogHelper.Log(data);
		bbproto.RspDelFriend rsp = data as bbproto.RspDelFriend;
		if (rsp.header.code != (int)ErrorCode.SUCCESS)
		{
			LogHelper.Log("OnRspDelFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}

		bbproto.FriendList inst = rsp.friends;
        
		DataCenter.Instance.FriendList.RefreshFriendList(inst);
        
		// test
		LogHelper.Log("OnAcceptFriend, test first friend. nick name" + CurrentFriendListData() [1].NickName);
		HideUI();
		ShowUI();
	}

	void ViewUnitBriefInfo(object args){
		int position = (int)args;
		TFriendInfo tfi = CurrentFriendListData() [position];
		if (tfi == null){
			Debug.LogError("ViewUnitBriefInfo(), pos : " + position + " TfriendInfo is null, return!!!");
			return;
		}

		MsgCenter.Instance.Invoke(CommandEnum.ShowUserUnitBriefInfo, tfi);
	}

	void ViewUnitDetailInfo(object args){
		int position = (int)args;
		TUserUnit tuu = friendUnitItemViewList [position].DataItem;
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, tuu);
	}

	void GetFriendUnitItemViewList()
	{
		//First, clear the current if exist
		friendUnitItemViewList.Clear();
		//Then, get the newest from DataCenter
		List<TUserUnit> unitList = GetFriendUnitItemList();
		if (unitList == null)
		{
			LogHelper.LogError("GetFriendUnitItemViewList GetUnitList return null.");
			return;
		}
		for (int i = 0; i < unitList.Count; i++)
		{
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(unitList [i]);
			friendUnitItemViewList.Add(viewItem);
		}

		friendNickNameList = GetFriendNickNameList();
		LogHelper.Log("FriendListLogic.GetFriendUnitItemViewList(), ViewItem Count is : " + friendUnitItemViewList.Count);
	}

	List<TUserUnit> GetFriendUnitItemList()
	{
		if (CurrentFriendListData() == null)
		{
			LogHelper.LogError("FriendListLogic.GetFriendUnitItemList(), DataCenter.Instance.FriendList == null!!!");
			return null;
		}

		List<TUserUnit> tuuList = new List<TUserUnit>();
		for (int i = 0; i < CurrentFriendListData().Count; i++)
		{
//			LogHelper.LogError("Global.friends:i={0}, friends:{1} fUserId:{2}", i, DataCenter.Instance.FriendList[ i ],DataCenter.Instance.FriendList[ i ].UserId);
//			LogHelper.LogError("Global.friends:i={0}, friends.UserUnit:{1}", i, DataCenter.Instance.FriendList[ i ].UserUnit);
			tuuList.Add(CurrentFriendListData() [i].UserUnit);
		}

		return tuuList;
	}

	List<string> GetFriendNickNameList()
	{
		if (CurrentFriendListData() == null)
		{
			LogHelper.LogError("FriendListLogic.GetFriendNickNameList(), DCurrentFriendListData() == null!!!");
			return null;
		}

		List<string> nameList = new List<string>();
		for (int i = 0; i < CurrentFriendListData().Count; i++)
		{
			nameList.Add(CurrentFriendListData() [i].NickName);
		}

		return nameList;
	}

	void CallViewCreateDragList()
	{
		CallBackDispatcherArgs mainCbdArgs = new CallBackDispatcherArgs("CreateDragView", friendUnitItemViewList);
		ExcuteCallback(mainCbdArgs);

		CallBackDispatcherArgs nameCbdArgs = new CallBackDispatcherArgs("ShowFriendName", friendNickNameList);
		ExcuteCallback(nameCbdArgs);
	}

	void DestoryUnitList()
	{
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", null);
		ExcuteCallback(cbdArgs);
	}

	void EnableExtraFunction(){
		switch (UIManager.Instance.baseScene.CurrentScene){
			case SceneEnum.FriendList :
				CallBackDispatcherArgs updateCbdArgs = new CallBackDispatcherArgs("EnableUpdateButton", null);
				ExcuteCallback(updateCbdArgs);
				break;
			case SceneEnum.Reception :
				CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableRefuseButton", null);
				ExcuteCallback(cbdArgs);
				break;
			default:
				break;
		}

	}

	void RefreshFriendList(object args)
	{
		LogHelper.Log("RspUpdateFriendClick(), receive the click, to refresh FriendList...");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateFriendList", null);
		ExcuteCallback(cbdArgs);
	}

}
