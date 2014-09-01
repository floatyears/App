using UnityEngine;
using System.Collections;

public class UserBriefInfoLogic : ModuleBase{
	TUserUnit currentPickedUserUnit;

	public UserBriefInfoLogic(string uiName):base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		AddEventListener();

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		ExcuteCallback(cbdArgs);
	}

	public override void HideUI(){
		base.HideUI();
		RemoveEventListener();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);

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
		UIManager.Instance.ChangeScene(ModuleEnum.UnitDetailModule);
		if (currentPickedUserUnit == null) return;
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
		currentPickedUserUnit = tfi.UserUnit;
		RefreshUnitInfo(tfi.UserUnit);
		RefreshRank(tfi.Rank);
		RefreshUserName(tfi.NickName);
		RefreshLastLogin(tfi.LastPlayTime);
	}

	void RefreshUnitInfo(TUserUnit tuu){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshUnitInfoView", tuu);
		ExcuteCallback(cbdArgs);
	}

	void RefreshLastLogin(uint unixTime){
		string text = TimeHelper.GetLatestPlayTime(unixTime).ToString();
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

