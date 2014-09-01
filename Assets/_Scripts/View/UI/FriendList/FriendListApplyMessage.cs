using UnityEngine;
using System.Collections;

public class FriendListApplyMessage : ApplyMessage {

	public FriendListApplyMessage(string uiName) : base(uiName){}
	
	public override void ShowUI(){
		base.ShowUI();
		CustomizeWindow();
	}
	
	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (cbdArgs.funcName){
		case "ClickSure": 
			CallBackDispatcherHelper.DispatchCallBack(EnsureDeleteApply, cbdArgs);
			break;
			
		case "ClickCancel": 
			CallBackDispatcherHelper.DispatchCallBack(CancelDeleteApply, cbdArgs);
			break;
		default:
			break;
		}

		CallBackDispatcherArgs callbackArgs = new CallBackDispatcherArgs("HidePanel", null); 
		ExcuteCallback(callbackArgs);
	}
	
	void CancelDeleteApply(object args){

	}
	
	void EnsureDeleteApply(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureDeleteApply, null);
	}
	
	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}
	
	void CustomizeTitle(){
		string title = TextCenter.GetText ("DeleteApply");//"";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		ExcuteCallback(cbdArgs);
	}
	
	void CustomizeNote(){
		string note = TextCenter.GetText ("ConfirmDelete");// "Are you sure to delete your apply?";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeNote", note);
		ExcuteCallback(cbdArgs);
	}

}
