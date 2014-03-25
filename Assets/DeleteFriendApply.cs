using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeleteFriendApply : ApplyMessage{
	public DeleteFriendApply(string uiName) : base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		CustomizeWindow();
	}

	public override void Callback(object data){
		base.Callback(data);
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
	}

	void CancelDeleteApply(object args){
//		Debug.LogError("Receive view click, to cancel delete friend apply of player....");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);   
		ExcuteCallback(cbdArgs);
	}

	void EnsureDeleteApply(object args){
		Debug.LogError("Receive view click, to ensure delete friend apply of player....");
		MsgCenter.Instance.Invoke(CommandEnum.EnsureDeleteApply, null);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		ExcuteCallback(cbdArgs);

	}

	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}

	void CustomizeTitle(){
		string title = "Delete Your Apply";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		ExcuteCallback(cbdArgs);
	}

	void CustomizeNote(){
		string note = "Are you sure to delete your apply?";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeNote", note);
		ExcuteCallback(cbdArgs);
	}


}
