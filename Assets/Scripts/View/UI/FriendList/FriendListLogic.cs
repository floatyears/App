using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListLogic : ConcreteComponent{
	TFriendInfo currentFriendPicked;
	List<UnitItemViewInfo> friendUnitItemViewList = new List<UnitItemViewInfo>();
	List<string> friendNickNameList = new List<string>();

	public FriendListLogic(string uiName) : base( uiName ){}

	public override void ShowUI(){
		base.ShowUI();

		AddCommandListener();

		GetFriendUnitItemViewList();
		CallViewCreateDragList();
		EnableExtraFunction();
	}

	public override void HideUI()
	{
		base.HideUI();
		RemoveCommandListener();
		DestoryUnitList();

	}

	public override void Callback(object data)
	{
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (cbdArgs.funcName)
		{
			case "PressItem": 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitDetailInfo, cbdArgs);
				break;
			case "ClickItem": 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitBriefInfo, cbdArgs);
				break;
			case "UpdateFriendButtonClick": 
				CallBackDispatcherHelper.DispatchCallBack(NoteFriendUpdate, cbdArgs);
				break;
			case "RefuseApplyButtonClick": 
				CallBackDispatcherHelper.DispatchCallBack(NoteRefuseAll, cbdArgs);
				break;
			default:
				break;
		}
	}

    MsgWindowParams GetRefuseAllFriendInMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("RefuseAll");
        msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("ConfirmRefuseAll");
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackRefuseAll;
        return msgWindowParam;
    }

    void CallbackRefuseAll(object args){
        MsgCenter.Instance.Invoke(CommandEnum.EnsureRefuseAll);
    }

	void NoteRefuseAll(object args)
	{
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetRefuseAllFriendInMsgWindowParams());
	}

	void RefuseAllApplyFromOthers(object msg)
	{
		RefuseFriendAll();
	}

	void AddCommandListener()
	{
		MsgCenter.Instance.AddListener(CommandEnum.EnsureUpdateFriend, UpdateFriendList);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);          
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteApply, DeleteMyApply);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureRefuseAll, RefuseAllApplyFromOthers);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureAcceptApply, AcceptApplyFromOther);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureRefuseSingleApply, DeleteApplyFromOther);     
                
        }

	void DeleteMyApply(object msg)
	{
		CancelFriendRequest(currentFriendPicked.UserId);
	}
		
	void RemoveCommandListener()
	{
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureUpdateFriend, UpdateFriendList);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteApply, DeleteMyApply);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureRefuseAll, RefuseAllApplyFromOthers);        
        MsgCenter.Instance.RemoveListener(CommandEnum.EnsureAcceptApply, AcceptApplyFromOther);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureRefuseSingleApply, DeleteApplyFromOther);     
	}

	void AcceptApplyFromOther(object msg){
		Debug.LogError("FriendListLogic.AcceptApplyFromOther(), receive the message, to accept apply from other player...");
//		AcceptFriend.SendRequest(OnAcceptFriend, currentFriendPicked.UserId);
		AcceptFriendRequest(currentFriendPicked.UserId);
	}

	void DeleteApplyFromOther(object msg){
		Debug.LogError("FriendListLogic.DeleteApplyFromOther(), receive the message, to delete apply from other player...");
		RefuseFriend(currentFriendPicked.UserId);
	}

    MsgWindowParams GetRefreshFriendListMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("RefreshFriend");
        msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("ConfirmRefreshFriend");
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackRefreshFriend;
        return msgWindowParam;
    }

    void CallbackRefreshFriend(object args){
        MsgCenter.Instance.Invoke(CommandEnum.EnsureUpdateFriend);
    }

	void NoteFriendUpdate(object args)
	{
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetRefreshFriendListMsgWindowParams());
	}
	
	List<TFriendInfo> CurrentFriendListData()
	{
		switch (UIManager.Instance.baseScene.CurrentScene)
		{
			case SceneEnum.FriendList: 
				Debug.Log("CurrentFriendListData() friend Count {0} " + DataCenter.Instance.FriendList.Friend.Count); 
				return DataCenter.Instance.FriendList.Friend;
				break;
			case SceneEnum.Apply: 
				Debug.Log("CurrentFriendListData() friendIn Count {1}" + DataCenter.Instance.FriendList.FriendOut.Count); 
				return DataCenter.Instance.FriendList.FriendOut;
				break;
			case SceneEnum.Reception: 
				Debug.Log("CurrentFriendListData() friendOut Count {2}" + DataCenter.Instance.FriendList.FriendIn.Count); 
				return DataCenter.Instance.FriendList.FriendIn;
				break;
			
			default:
				Debug.Log("CurrentFriendListData(), no return, UIManager.Instance.baseScene.CurrentScene");
				return new List<TFriendInfo>();
				break;
		}

	}

	void DeleteFriendPicked(object msg)
	{
		Debug.LogError("FriendListLogic.DeleteFriendCurrentPicked(), Start...");
		DelFriend.SendRequest(OnDelFriend, currentFriendPicked.UserId);
	}

	void UpdateFriendList(object args)
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

		DataCenter.Instance.SetFriendList(inst);
		// test
		LogHelper.LogError("OnGetFriendList, response friendCount {0}", rsp.friends.friend.Count);
		for (int i = 0; i < rsp.friends.friend.Count; i++)
		{
			LogHelper.LogError("OnGetFriendList, test first friend. nick name", rsp.friends.friend [i].nickName);

		}

		HideUI();
		ShowUI();
	}

	void RefuseFriend(uint friendUid)
	{
		DelFriend.SendRequest(OnDelFriend, friendUid);
	}

	void CancelFriendRequest(uint friendUid)
	{
		DelFriend.SendRequest(OnDelFriend, friendUid);
	}
        
	void RefuseFriendAll()
	{
		List <uint> refuseList = new List<uint>();
		for (int i = 0; i < DataCenter.Instance.FriendList.FriendIn.Count; i++)
		{
			refuseList.Add(DataCenter.Instance.FriendList.FriendIn [i].UserId);
		}
		for (int i = 0; i < refuseList.Count; i++)
		{
			LogHelper.LogError("refuseList, {0}, friendId {1}", i, refuseList [i]);

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
        
		LogHelper.LogError("TFriendList.OnRspAddFriend() begin");
		LogHelper.Log(data);
		bbproto.RspAcceptFriend rsp = data as bbproto.RspAcceptFriend;
        
		if (rsp.header.code != (int)ErrorCode.SUCCESS)
		{
			LogHelper.Log("RspAddFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}

		bbproto.FriendList inst = rsp.friends;

        DataCenter.Instance.SetFriendList(inst);

		// test
//		LogHelper.Log("OnAcceptFriend, test first friend. nick name" + CurrentFriendListData() [1].NickName);
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
			LogHelper.LogError("OnRspDelFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}

		bbproto.FriendList inst = rsp.friends;
		LogHelper.LogError("OnRspDelFriend friends {0}", rsp.friends);
        

        DataCenter.Instance.SetFriendList(inst);
		// test
//		LogHelper.Log("OnAcceptFriend, test first friend. nick name" + CurrentFriendListData() [1].NickName);
		HideUI();
		ShowUI();
	}

	void ViewUnitBriefInfo(object args)
	{
		int position = (int)args;
//		TFriendInfo tfi = CurrentFriendListData() [position];
		currentFriendPicked = CurrentFriendListData() [position];
		if (currentFriendPicked == null)
		{
			Debug.LogError("ViewUnitBriefInfo(), pos : " + position + " TfriendInfo is null, return!!!");
			return;
		}
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.FriendList)
			MsgCenter.Instance.Invoke(CommandEnum.FriendBriefInfoShow, currentFriendPicked);
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.Apply 
		    ||UIManager.Instance.baseScene.CurrentScene == SceneEnum.Reception)
			MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, currentFriendPicked);
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
		if (unitList == null){
			LogHelper.LogError("GetFriendUnitItemViewList GetUnitList return null.");
			return;
		}
		LogHelper.Log("FriendListLogic.GetFriendUnitItemViewList(), unitList Count is : " + unitList.Count);

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
		LogHelper.LogError("GetFriendUnitItemViewList() CurrentFriendListData().Count {0}", CurrentFriendListData().Count);

		for (int i = 0; i < CurrentFriendListData().Count; i++)
			tuuList.Add(CurrentFriendListData() [ i ].UserUnit);
	
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
		for (int i = 0; i < CurrentFriendListData().Count; i++){
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

	void EnableExtraFunction()
	{
		switch (UIManager.Instance.baseScene.CurrentScene)
		{
			case SceneEnum.FriendList:
				CallBackDispatcherArgs updateCbdArgs = new CallBackDispatcherArgs("EnableUpdateButton", null);
				ExcuteCallback(updateCbdArgs);
				break;
			case SceneEnum.Reception:
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
