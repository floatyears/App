using UnityEngine;
using System.Collections;
using bbproto;

public class ResultController : ConcreteComponent {
	TFriendInfo curFriendInfo;
	public ResultController(string uiName) : base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.ShowFriendPointUpdateResult, ShowFriendPointUpdateResult);
	}

	public override void HideUI(){
		base.HideUI();
		ClearData();
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFriendPointUpdateResult, ShowFriendPointUpdateResult);
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

	private void SendFriendApplyRequest(object args){
		Debug.Log("Receive click, to send Friend apply request...");
		if(curFriendInfo == null) return;

		AddFriendApplication(curFriendInfo.UserId);
		Debug.Log("ResultController.SendFriendApplyRequest(), friendApplying's id is : " + curFriendInfo.UserId);
	}

	//main process
	private void ShowFriendPointUpdateResult(object info){
		TFriendInfo friendInfo = info as TFriendInfo;
		if(friendInfo == null){
			Debug.LogError("ShowFriendPointUpdateResult(), friendInfo is null!!!");
			return;
		}
		AddGotFriendPoint(friendInfo);
		curFriendInfo = friendInfo;
		if(!CheckIsFriend(friendInfo)){
			//support to send the apply of making friend
			SupportApplyFriend(true);
		}
		else
			SupportApplyFriend(false);

		ShowFriendBriefInfo(friendInfo);
		ShowFriendPoint(ConfigBattleUseData.Instance.gotFriendPoint);
	}

	private bool CheckIsFriend(TFriendInfo friendInfo){
		bool isFriend = false;
		if(friendInfo.FriendState == EFriendState.FRIENDIN){
			isFriend = true;
		}
		else
			isFriend = false;

//		Debug.Log("ResultController.CheckIsFriend(), isFriend is : " + isFriend);
		return isFriend;
	}

	private void SupportApplyFriend(bool isSupport){
		Debug.Log("ResultController.SupportApplyFriend(), start...");
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("Stylize", isSupport);
		ExcuteCallback(call);
		Debug.Log("ResultController.SupportApplyFriend(), end...");
	}

	private void ShowFriendBriefInfo(TFriendInfo friendInfo){
		Debug.Log("ResultController.ShowFriendBriefInfo(), start...");
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowTopView", friendInfo);
		ExcuteCallback(call);
		Debug.Log("ResultController.ShowFriendBriefInfo(), end...");
	}

	private void ShowFriendPoint(int friPoint){
		Debug.Log("ResultController.ShowFriendPoint(), start...  friPoint="+friPoint);
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowCenterView", friPoint);
		ExcuteCallback(call);
		Debug.Log("ResultController.ShowFriendPoint(), end...");	
	}

	private void AddFriendApplication(uint friendUid){
		LogHelper.Log("AddFriendApplication () start");
		AddFriend.SendRequest(OnAddFriend, friendUid);
	}

	private void OnAddFriend(object data){
		if (data == null) return;
		
		LogHelper.Log("TFriendList.OnRspAddFriend() begin");
		LogHelper.Log(data);
		bbproto.RspAddFriend rsp = data as bbproto.RspAddFriend;
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			if ( (rsp.header.code != (int)ErrorCode.EF_IS_ALREADY_FRIEND) && (rsp.header.code != (int)ErrorCode.EF_IS_ALREADY_FRIENDOUT) ) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			}	
//			UIManager.Instance.ChangeScene(SceneEnum.Home);
			DGTools.ChangeToQuest();
			return;
		}

		bbproto.FriendList inst = rsp.friends;
		LogHelper.Log("OnAddFriend(), rsp.friends {0}", inst);
		LogHelper.Log("OnAddFriend(), friendlist {0}, friendList == null {1}", DataCenter.Instance.FriendList, DataCenter.Instance.FriendList == null);
		DataCenter.Instance.SetFriendList(inst);

//		UIManager.Instance.ChangeScene(SceneEnum.Home);
		DGTools.ChangeToQuest();
	}

	private void ClearData(){
		curFriendInfo = null;
	}

	private void AddGotFriendPoint(TFriendInfo friendInfo){
//		Debug.Log("Before :: ResultController.AddGotFriendPoint(), TotalFriendPoint is : " + DataCenter.Instance.AccountInfo.FriendPoint);
		DataCenter.Instance.AccountInfo.FriendPoint += friendInfo.FriendPoint;
//		Debug.Log("After :: ResultController.AddGotFriendPoint(), TotalFriendPoint is : " + DataCenter.Instance.AccountInfo.FriendPoint);
	}

}
