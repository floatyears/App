using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendHelperBriefInfo : UserBriefInfoLogic {
	public FriendHelperBriefInfo(string uiName):base(uiName){}

	public override void CreatUI(){
		base.CreatUI();
		//StylizeView();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "Choose" : 
				CallBackDispatcherHelper.DispatchCallBack(ChooseHelper, cbdArgs);
				break;
			default:
				break;
		}
	}

	void StylizeView(){
		Dictionary<string,string> styleArgs = new Dictionary<string, string>();
		styleArgs.Add("ButtonTop", "Choose");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("Stylize", styleArgs);
		ExcuteCallback(styleArgs);
	}

	void ChooseHelper(object args){
		MsgCenter.Instance.Invoke(CommandEnum.ChooseHelper, null);
	}

}
