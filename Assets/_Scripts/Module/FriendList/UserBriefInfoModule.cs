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

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		view.CallbackView(cbdArgs);
	}

	public override void HideUI(){
		base.HideUI();
		RemoveEventListener();
	}

	public override void OnReceiveMessages(object data){
		base.OnReceiveMessages(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "ViewDetailInfo": 
				CallBackDispatcherHelper.DispatchCallBack(ViewUserUnitDetailInfo, cbdArgs);
				break;
			default:
				break;
		}
	}

	void ViewUserUnitDetailInfo(object args){
		ModuleManger.Instance.ShowModule(ModuleEnum.UnitDetailModule);
		if (currentPickedUserUnit == null) return;
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, currentPickedUserUnit);
	}

	void Exit(){
		ClearInfo();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		view.CallbackView(cbdArgs);
	}

	void ClearInfo(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClearPanel", null);
		view.CallbackView(cbdArgs);
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
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshUnitInfoView", tuu);
		view.CallbackView(cbdArgs);
	}

	void RefreshLastLogin(uint unixTime){
		string text = TimeHelper.GetLatestPlayTime(unixTime).ToString();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshLastLogin", text);
		view.CallbackView(cbdArgs);
	}

	void RefreshRank(int rank){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshRank", rank.ToString());
		view.CallbackView(cbdArgs);
	}

	void RefreshUserName(string userName){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshUserName", userName);
		view.CallbackView(cbdArgs);
	}

}

