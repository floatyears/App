using UnityEngine;
using System.Collections;

public class FriendListUserBriefInfo : UserBriefInfoModule{
	
	public FriendListUserBriefInfo(UIConfigItem config):base(  config){}

	public override void ShowUI(){
		base.ShowUI();
		EnableDeleteFriend();
	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (data[0].ToString()){
			case "ClickDelete": 
				DeleteFriend(data[1]);
				break;
			default:
				break;
		}
	}
	
	void DeleteFriend(object args){
//		int position = (int)args;
		Debug.LogError("Delete Friend.... ");
		MsgCenter.Instance.Invoke(CommandEnum.EnsureDeleteFriend, null);
	}

	void EnableDeleteFriend(){
//		Debug.LogError("Call View to Enable Delete Friend...");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableDeleteFriend", null);
		view.CallbackView("EnableDeleteFriend");
	}

}

