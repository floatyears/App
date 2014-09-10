using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeleteFriendApply : ApplyMessageModule{
	public DeleteFriendApply(UIConfigItem config) : base(  config){}

	public override void ShowUI(){
		base.ShowUI();
		CustomizeWindow();
	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (data[0].ToString()){
			case "ClickSure": 
				EnsureDeleteApply(data[1]);
				break;

			case "ClickCancel": 
				CancelDeleteApply(data[1]);
				break;
			default:
				break;
		}
	}

	void CancelDeleteApply(object args){
//		Debug.LogError("Receive view click, to cancel delete friend apply of player....");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs(, null);   
		view.CallbackView("HidePanel");
	}

	void EnsureDeleteApply(object args){
//		Debug.LogError("Receive view click, to ensure delete friend apply of player....");
		MsgCenter.Instance.Invoke(CommandEnum.EnsureDeleteApply, null);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		view.CallbackView("HidePanel");

	}

	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}

	void CustomizeTitle(){
//		string title = TextCenter.GetText ("DeleteApply");//"";
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		view.CallbackView("StylizeTitle", TextCenter.GetText ("DeleteApply"));
	}

	void CustomizeNote(){
//		string note = TextCenter.GetText ("ConfirmDelete");// "Are you sure to delete your apply?";
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeNote", note);
		view.CallbackView("StylizeNote", TextCenter.GetText ("ConfirmDelete"));
	}


}
