using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccpetFriendApply : ApplyMessageModule
{
	public AccpetFriendApply(UIConfigItem config) : base(  config)
	{

	}

	public override void ShowUI()
	{
		base.ShowUI();
        CustomizeWindow();
    }

    public override void OnReceiveMessages(params object[] data)
	{
//		base.OnReceiveMessages(data);

//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (data[0].ToString())
		{
			case "ClickSure": 
				EnsureAcceptApply(data[1]);
				break;

			case "ClickCancel": 
				Exit(data[1]);
				break;

			case "ClickDelete": 
				EnsureDeleteApply(data[1]);
                break;
                default:
				break;
		}
	}

	void Exit(object args){
//		Debug.LogError("Receive view click, to cancel delete friend apply of player....");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);   
		view.CallbackView("HidePanel");
	}

	void EnsureAcceptApply(object args){
//		Debug.LogError("Receive view click, to ensure delete friend apply of player....");
		MsgCenter.Instance.Invoke(CommandEnum.EnsureAcceptApply, null);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		view.CallbackView("HidePanel");
	}

	void EnsureDeleteApply(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureRefuseSingleApply, null);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		view.CallbackView("HidePanel");
	}

	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}

	void CustomizeTitle(){
//		string title = ; //"Accept Apply";
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		view.CallbackView("StylizeTitle", TextCenter.GetText ("AcceptApply"));
	}

	void CustomizeNote(){
//		string note = ; //"Are you sure to accept this apply?";
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs();
		view.CallbackView("StylizeNote", TextCenter.GetText ("ConfirmAccept"));
	}
	
}
