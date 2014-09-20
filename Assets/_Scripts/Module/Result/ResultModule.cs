using UnityEngine;
using System.Collections;
using bbproto;

public class ResultModule : ModuleBase {
	TFriendInfo curFriendInfo;
	public ResultModule(UIConfigItem config) : base(  config){
		CreateUI<ResultView> ();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.ShowFriendPointUpdateResult, ShowFriendPointUpdateResult);
	}

	public override void HideUI(){
		base.HideUI();
		ClearData();
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFriendPointUpdateResult, ShowFriendPointUpdateResult);
	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//
//		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;
		switch (data[0].ToString()){
			case "ClickOk" : 
				SendFriendApplyRequest();
				break;
			default:
				break;
		}
	}

	private void SendFriendApplyRequest(){
		if(curFriendInfo == null) return;
		AddFriendApplication(curFriendInfo.UserId);
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
			SupportApplyFriend(true);
		}
		else
			SupportApplyFriend(false);

		ShowFriendBriefInfo(friendInfo);
		ShowFriendPoint(BattleConfigData.Instance.gotFriendPoint);
	}

	private bool CheckIsFriend(TFriendInfo friendInfo){
		bool isFriend = false;
		if(friendInfo.FriendState == EFriendState.ISFRIEND){
			isFriend = true;
		}
		else
			isFriend = false;

		return isFriend;
	}

	private void SupportApplyFriend(bool isSupport){
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("Stylize", isSupport);
		view.CallbackView("Stylize", isSupport);
	}

	private void ShowFriendBriefInfo(TFriendInfo friendInfo){
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowTopView", friendInfo);
		view.CallbackView("ShowTopView", friendInfo);
	}

	private void ShowFriendPoint(int friPoint){
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowCenterView", friPoint);
		view.CallbackView("ShowCenterView", friPoint);
	}

	private void AddFriendApplication(uint friendUid){
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
			DGTools.ChangeToQuest();
			return;
		}

		bbproto.FriendList inst = rsp.friends;
		LogHelper.Log("OnAddFriend(), rsp.friends {0}", inst);
		LogHelper.Log("OnAddFriend(), friendlist {0}, friendList == null {1}", DataCenter.Instance.FriendList, DataCenter.Instance.FriendList == null);
		DataCenter.Instance.SetFriendList(inst);

		DGTools.ChangeToQuest();
	}

	private void ClearData(){
		curFriendInfo = null;
	}

	private void AddGotFriendPoint(TFriendInfo friendInfo){
		DataCenter.Instance.AccountInfo.FriendPoint += friendInfo.FriendPoint;
	}

}
