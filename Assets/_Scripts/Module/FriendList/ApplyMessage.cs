using UnityEngine;
using System.Collections;

public class ApplyMessage : ModuleBase{
	public ApplyMessage(string uiName) : base( uiName ){}

	public override void ShowUI(){
		base.ShowUI();
		AddCommandListener();

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		ExcuteCallback(cbdArgs);
	}

	public override void HideUI(){
		base.HideUI();
		RmvCommandListener();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "ClickCancel": 
				CallBackDispatcherHelper.DispatchCallBack(ApplyCancel, cbdArgs);
				break;
			default:
				break;
		}
	}

	void ShowApplyInfo(object msg){
		TFriendInfo tfi = msg as TFriendInfo;
		RefreshApplyFriendInfo(tfi);
	}

	void ApplyCancel(object args){
//		Debug.LogError("ApplyCancel(), call view to close the window ...");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("Cancel", null);
		ExcuteCallback(cbdArgs);
	}

	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ViewApplyInfo, ShowApplyInfo);
	}

	void RmvCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ViewApplyInfo, ShowApplyInfo);
	}

	void RefreshApplyFriendInfo(TFriendInfo tfi){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshContent", tfi);
		ExcuteCallback(cbdArgs);
	}

}

