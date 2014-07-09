using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccpetFriendApply : ApplyMessage
{
	public AccpetFriendApply(string uiName) : base(uiName)
	{

	}

	public override void ShowUI()
	{
		base.ShowUI();
        CustomizeWindow();
    }

        public override void CallbackView(object data)
	{
		base.CallbackView(data);

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
		ExcuteCallback(cbdArgs);
	}

	void EnsureAcceptApply(object args){
//		Debug.LogError("Receive view click, to ensure delete friend apply of player....");
		MsgCenter.Instance.Invoke(CommandEnum.EnsureAcceptApply, null);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		ExcuteCallback(cbdArgs);
	}

	void EnsureDeleteApply(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureRefuseSingleApply, null);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("HidePanel", null); 
		ExcuteCallback(cbdArgs);
	}

	void CustomizeWindow(){
		CustomizeTitle();
		CustomizeNote();
	}

	void CustomizeTitle(){
		string title = TextCenter.GetText ("AcceptApply"); //"Accept Apply";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeTitle", title);
		ExcuteCallback(cbdArgs);
	}

	void CustomizeNote(){
		string note = TextCenter.GetText ("ConfirmAccept"); //"Are you sure to accept this apply?";
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("StylizeNote", note);
		ExcuteCallback(cbdArgs);
	}
	
}
