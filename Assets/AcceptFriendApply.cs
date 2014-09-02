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

    public override void OnReceiveMessages(object data)
	{
		base.OnReceiveMessages(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName)
		{
			case "ClickSure": 
				CallBackDispatcherHelper.DispatchCallBack(EnsureAcceptApply, cbdArgs);
				break;

			case "ClickCancel": 
				CallBackDispatcherHelper.DispatchCallBack(Exit, cbdArgs);
				break;

			case "ClickDelete": 
				CallBackDispatcherHelper.DispatchCallBack(EnsureDeleteApply, cbdArgs);
                break;
                default:
				break;
		}
	}

	void Exit(object args){
//		Debug.LogError("Receive view click, to cancel delete friend apply of player....");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null);   
		view.CallbackView(cbdArgs);
	}

	void EnsureAcceptApply(object args){
//		Debug.LogError("Receive view click, to ensure delete friend apply of player....");
		MsgCenter.Instance.Invoke(CommandEnum.EnsureAcceptApply, null);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		view.CallbackView(cbdArgs);
	}

	void EnsureDeleteApply(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureRefuseSingleApply, null);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		view.CallbackView(cbdArgs);
	}

	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}

	void CustomizeTitle(){
		string title = TextCenter.GetText ("AcceptApply"); //"Accept Apply";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		view.CallbackView(cbdArgs);
	}

	void CustomizeNote(){
		string note = TextCenter.GetText ("ConfirmAccept"); //"Are you sure to accept this apply?";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeNote", note);
		view.CallbackView(cbdArgs);
	}
	
}
