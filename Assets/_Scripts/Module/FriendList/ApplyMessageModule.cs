using UnityEngine;
using System.Collections;

public class ApplyMessageModule : ModuleBase{


	public ApplyMessageModule(UIConfigItem config) : base( config ){}

	public override void ShowUI(){
		base.ShowUI();

		AddCommandListener();

//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);
		view.CallbackView("HidePanel");
	}

	public override void HideUI(){
		base.HideUI();
		RmvCommandListener();
	}

	public override void OnReceiveMessages(params object[] data){
//		base.CallbackView(data);

//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
//		switch (cbdArgs.funcName){
//			case "ClickCancel": 
//				CallBackDispatcherHelper.DispatchCallBack(ApplyCancel, cbdArgs);
//				break;
//			default:
//				break;
//		}
	}

	void ShowApplyInfo(object msg){
		TFriendInfo tfi = msg as TFriendInfo;
		RefreshApplyFriendInfo(tfi);
	}

	void ApplyCancel(object args){
//		Debug.LogError("ApplyCancel(), call view to close the window ...");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs();
		view.CallbackView("Cancel", null);
	}

	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ViewApplyInfo, ShowApplyInfo);
	}

	void RmvCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ViewApplyInfo, ShowApplyInfo);
	}

	void RefreshApplyFriendInfo(TFriendInfo tfi){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs();
		view.CallbackView("RefreshContent", tfi);
	}

}

