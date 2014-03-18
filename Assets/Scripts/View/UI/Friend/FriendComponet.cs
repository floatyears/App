using UnityEngine;
using System.Collections;

public class FriendComponent : ConcreteComponent
{
	public FriendComponent(string uiName):base(uiName)
	{
	}
	
	public override void CreatUI()
	{
		base.CreatUI();
	}
	
	public override void ShowUI()
	{
		base.ShowUI();
	}
	
	public override void HideUI()
	{
		base.HideUI();
	}
	
	public override void DestoryUI()
	{
		base.DestoryUI();
	}

	public override void Callback(object data)
	{
		base.Callback(data);

        
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
		switch (cbdArgs.funcName)
		{
			case "SyncFriendList": 
				CallBackDispatcherHelper.DispatchCallBack(SyncFriendList, cbdArgs);
				break;
			default:
				break;
		}
	}

	public void SyncFriendList(object args)
	{
		if (DataCenter.Instance.FriendList != null)
		{
			Debug.Log("SyncFriendList().FriendList not null, not need to sync friend list from server");
			callTurnToNextScene();
			return;
		}
		GetFriendList.SendRequest(OnSyncFriendList);
	}

	public void OnSyncFriendList(object data)
	{
		Debug.Log("OnSyncFriendList, data = " + data);
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

		if (rsp.friends == null)
		{
			LogHelper.Log("RspGetFriend getFriend null");
			return;
		}

        DataCenter.Instance.SetFriendList(inst);
//			Debug.Log("OnGetFriendList, test first friend. nick name" + DataCenter.Instance.FriendList.Friend [0].NickName);
		callTurnToNextScene();
	}

	private void callTurnToNextScene()
	{
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("TurnRequiredFriendListScene", null);
		ExcuteCallback(cbdArgs);
	}
}
