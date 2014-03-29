using UnityEngine;
using System.Collections;

public class ResultController : ConcreteComponent {
	TFriendInfo curFriendInfo;
	public ResultController(string name) : base(name){}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.ShowFriendPointUpdateResult, ShowFightResult);
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFriendPointUpdateResult, ShowFightResult);
	}

	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;
		switch (call.funcName){
			case "ClickOk" : 
				CallBackDispatcherHelper.DispatchCallBack(SendFriendApplyRequest, call);
				break;
			default:
				break;
		}
	}

	void SendFriendApplyRequest(object args){
		Debug.Log("Receive click, to send Friend apply request...");
		if(curFriendInfo == null) return;
	}

	//main process
	void ShowFightResult(object info){
		TFriendInfo friendInfo = info as TFriendInfo;
		if(friendInfo == null) return;
		if(!CheckIsFriend(friendInfo)){
			//support to send the apply of making friend
			SupportApplyFriend(true);
		}
		else
			SupportApplyFriend(false);

		ShowFriendBriefInfo(friendInfo);
		ShowFriendPoint(friendInfo.FriendPoint);
	}

	bool CheckIsFriend(TFriendInfo friendInfo){
		bool isFriend = false;
		if(friendInfo.FriendState == bbproto.EFriendState.FRIENDIN)
			isFriend = true;
		else
			isFriend = false;

		Debug.Log("ResultController.CheckIsFriend(), isFriend is : " + isFriend);
		return isFriend;
	}

	void SupportApplyFriend(bool isSupport){
		Debug.Log("ResultController.SupportApplyFriend(), start...");
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("Stylize", isSupport);
		ExcuteCallback(call);
		Debug.Log("ResultController.SupportApplyFriend(), end...");
	}

	void ShowFriendBriefInfo(TFriendInfo friendInfo){
		Debug.Log("ResultController.ShowFriendBriefInfo(), start...");
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowTopView", friendInfo);
		ExcuteCallback(call);
		Debug.Log("ResultController.ShowFriendBriefInfo(), end...");
	}

	void ShowFriendPoint(int friPoint){
		Debug.Log("ResultController.ShowFriendPoint(), start...");
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowCenterView", friPoint);
		ExcuteCallback(call);
		Debug.Log("ResultController.ShowFriendPoint(), end...");	
	}

}
