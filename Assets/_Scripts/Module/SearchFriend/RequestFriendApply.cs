using UnityEngine;
using System.Collections;

public class RequestFriendApply : ApplyMessageModule{
	
	public RequestFriendApply(UIConfigItem config) : base(  config){
//		CreateUI
	}

	public override void InitUI(){
		base.InitUI();
		CustomizeWindow();


	}
	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (data[0].ToString()){
			case "ClickSure": 
				EnsureRequestApply(data[1]);
				break;
			case "ClickCancel": 
				CancelRequestApply(data[1]);
				break;
			default:
				break;
		}
	}

	void CancelRequestApply(object args){
//		Debug.LogError("Receive view click, to cancel delete friend apply of player....");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);   
		view.CallbackView("HidePanel");
	}
	
	void EnsureRequestApply(object args){
//		Debug.LogError("Receive view click, to ensure delete friend apply of player....");
		MsgCenter.Instance.Invoke(CommandEnum.SubmitFriendApply, null);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		view.CallbackView("HidePanel");
		
	}
	
	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}
	
	void CustomizeTitle(){
		string title = TextCenter.GetText ("FriendApply"); //"Friend Apply";
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		view.CallbackView("StylizeTitle", TextCenter.GetText ("FriendApply"));
	}
        
	void CustomizeNote(){
		string note = TextCenter.GetText ("ConfirmApply");// "Are you sure to submit your apply?";
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeNote", note);
		view.CallbackView("StylizeNote", TextCenter.GetText ("ConfirmApply"));
	}
}

