using UnityEngine;
using System.Collections;

public class UserBriefInfoModule : ModuleBase{
	TUserUnit currentPickedUserUnit;

	public UserBriefInfoModule(UIConfigItem config):base(  config){
		CreateUI<UserBriefInfoView> ();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddEventListener();

//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		view.CallbackView("HidePanel");
	}

	public override void HideUI(){
		base.HideUI();
		RemoveEventListener();
	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (data[0].ToString()){
			case "ViewDetailInfo": 
				ViewUserUnitDetailInfo(data[1]);
				break;
			default:
				break;
		}
	}

	void ViewUserUnitDetailInfo(object args){

		if (currentPickedUserUnit == null) return;
		ModuleManager.Instance.ShowModule(ModuleEnum.UnitDetailModule,"unit", currentPickedUserUnit);
	}

	void Exit(){
		ClearInfo();
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		view.CallbackView("HidePanel");
	}

	void ClearInfo(){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClearPanel", null);
		view.CallbackView("HidePanel");
	} 

	void AddEventListener(){
		MsgCenter.Instance.AddListener(CommandEnum.FriendBriefInfoShow, ReceiveShowBriefRquest);
	}

	void RemoveEventListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.FriendBriefInfoShow, ReceiveShowBriefRquest);
	}

	void ReceiveShowBriefRquest(object msg){
		TFriendInfo tfi = msg as TFriendInfo;
		currentPickedUserUnit = tfi.UserUnit;
		RefreshUnitInfo(tfi.UserUnit);
		RefreshRank(tfi.Rank);
		RefreshUserName(tfi.NickName);
		RefreshLastLogin(tfi.LastPlayTime);
	}

	void RefreshUnitInfo(TUserUnit tuu){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshUnitInfoView", tuu);
		view.CallbackView("RefreshUnitInfoView");
	}

	void RefreshLastLogin(uint unixTime){
		string text = Utility.TimeHelper.GetLatestPlayTime(unixTime).ToString();
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshLastLogin", text);
		view.CallbackView("RefreshLastLogin");
	}

	void RefreshRank(int rank){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshRank", rank.ToString());
		view.CallbackView("RefreshRank");
	}

	void RefreshUserName(string userName){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshUserName", userName);
		view.CallbackView("RefreshUserName");
	}

}

