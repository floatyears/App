using UnityEngine;
using System.Collections;

public class UserBriefInfoLogic : ConcreteComponent{

	TUserUnit currentPickedUserUnit;

	public UserBriefInfoLogic(string uiName):base(uiName) {}

	public override void ShowUI(){
		base.ShowUI();
		AddEventListener();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveEventListener();
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "ViewDetailInfo" : 
				CallBackDispatcherHelper.DispatchCallBack(ViewUserUnitDetailInfo, cbdArgs);
				break;
			default:
				break;
		}
	}

	void ViewUserUnitDetailInfo(object args){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		if(currentPickedUserUnit == null){
			return;
		}
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, currentPickedUserUnit);
	}

	void Exit(){
		ClearInfo();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		ExcuteCallback(cbdArgs);
	}

	void ClearInfo(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClearPanel", null);
		ExcuteCallback(cbdArgs);
	} 

	void AddEventListener(){
		MsgCenter.Instance.AddListener(CommandEnum.FriendBriefInfoShow, ReceiveShowBriefRquest);
	}

	void RemoveEventListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.FriendBriefInfoShow, ReceiveShowBriefRquest);
	}

	void ReceiveShowBriefRquest(object msg){
		TFriendInfo tfi = msg as TFriendInfo;
		Debug.LogError ("ReceiveShowBriefRquest : " + tfi);
		currentPickedUserUnit = tfi.UserUnit;
		RefreshUnitInfo(tfi.UserUnit);
		RefreshRank(tfi.Rank);
		RefreshUserName(tfi.NickName);
		RefreshLastLogin(tfi.LastPlayTime);
	}

//	void SupportExtraFeature(){
//		SceneEnum current = UIManager.Instance.baseScene.CurrentScene;
//		switch (current){
//			case SceneEnum.FriendList : 
//				SupportDeleteFriend(true);
//				break;
//			default:
//				break;
//		}
//	}
//
//	void SupportDeleteFriend(bool support){
//		if(support){
//			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableDeleteFriend", null);
//			ExcuteCallback(cbdArgs);
//		}
//		else{
//
//		}
//	}

	void RefreshUnitInfo(TUserUnit tuu){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshUnitInfoView", tuu);
		ExcuteCallback(cbdArgs);
	}

	void RefreshLastLogin(uint hourCount){
		string text = hourCount.ToString();
		TimeHelper.FormattedTimeNow();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshLastLogin", text);
		ExcuteCallback(cbdArgs);
	}

	void RefreshRank(int rank){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshRank", rank.ToString());
		ExcuteCallback(cbdArgs);
	}

	void RefreshUserName(string userName){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshUserName", userName);
		ExcuteCallback(cbdArgs);
	}

}
