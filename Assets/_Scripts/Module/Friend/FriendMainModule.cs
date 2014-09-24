using UnityEngine;
using System.Collections;

public class FriendMainModule : ModuleBase{
	public FriendMainModule(UIConfigItem config):base(  config){
		CreateUI<FriendMainView> ();
	}

	public override void OnReceiveMessages(params object[] data){    
		switch (data[0].ToString()){
			case "SyncFriendList": 
				SyncFriendList(data[1]);
				break;
			default:
				break;
		}
	}

	public void SyncFriendList(object args){
		if (DataCenter.Instance.FriendData != null){
//			Debug.Log("SyncFriendList().FriendList not null, not need to sync friend list from server");
			CallTurnToNextScene();
			return;
		}
		FriendController.Instance.GetFriendList(OnSyncFriendList,true,false);
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

		DataCenter.Instance.FriendData.RefreshFriendList(inst);
		CallTurnToNextScene();
	}

	void CallTurnToNextScene(){
		view.CallbackView("TurnRequiredFriendListScene");
	}
}
