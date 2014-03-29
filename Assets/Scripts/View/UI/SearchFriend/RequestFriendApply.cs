using UnityEngine;
using System.Collections;

public class RequestFriendApply : ApplyMessage{
	
	public RequestFriendApply(string uiName) : base(uiName){}

	public override void CreatUI(){
		base.CreatUI();
		CustomizeWindow();
	}
	public override void CallbackView(object data){
		base.CallbackView(data);
		
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (cbdArgs.funcName){
			case "ClickSure": 
				CallBackDispatcherHelper.DispatchCallBack(EnsureRequestApply, cbdArgs);
				break;
			case "ClickCancel": 
				CallBackDispatcherHelper.DispatchCallBack(CancelRequestApply, cbdArgs);
				break;
			default:
				break;
		}
	}

	void CancelRequestApply(object args){
		Debug.LogError("Receive view click, to cancel delete friend apply of player....");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);   
		ExcuteCallback(cbdArgs);
	}
	
	void EnsureRequestApply(object args){
		Debug.LogError("Receive view click, to ensure delete friend apply of player....");
		MsgCenter.Instance.Invoke(CommandEnum.SubmitFriendApply, null);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		ExcuteCallback(cbdArgs);
		
	}
	
	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}
	
	void CustomizeTitle(){
		string title = "Friend Apply";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		ExcuteCallback(cbdArgs);
	}
        
	void CustomizeNote(){
		string note = "Are you sure to submit your apply?";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeNote", note);
		ExcuteCallback(cbdArgs);
	}
}

