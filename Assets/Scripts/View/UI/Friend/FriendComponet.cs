using UnityEngine;
using System.Collections;

public class FriendComponent : ConcreteComponent{
	public FriendComponent(string uiName):base(uiName){}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
		switch (cbdArgs.funcName){
			case "SyncFriendList": 
				CallBackDispatcherHelper.DispatchCallBack(SyncFriendList, cbdArgs);
				break;
			default:
				break;
		}
	}

	public void SyncFriendList(object args){
		if (DataCenter.Instance.FriendList != null){
//			Debug.Log("SyncFriendList().FriendList not null, not need to sync friend list from server");
			CallTurnToNextScene();
			return;
		}
		GetFriendList.SendRequest(OnSyncFriendList);
	}

	public void OnSyncFriendList(object data){
		Debug.Log("OnSyncFriendList, data = " + data);
		if (data == null)
			return;
        
		LogHelper.Log("TFriendList.Refresh() begin");
		LogHelper.Log(data);
		bbproto.RspGetFriend rsp = data as bbproto.RspGetFriend;
        
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
        
		bbproto.FriendList inst = rsp.friends;

		if (rsp.friends == null){
			LogHelper.Log("RspGetFriend getFriend null");
			return;
		}

        DataCenter.Instance.SetFriendList(inst);
		CallTurnToNextScene();
	}

	void CallTurnToNextScene(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("TurnRequiredFriendListScene", null);
		ExcuteCallback(cbdArgs);
	}
}
