using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendHelperBriefInfo : UserBriefInfoModule {
	public FriendHelperBriefInfo(UIConfigItem config):base(  config){}

	public override void InitUI(){
		base.InitUI();
		//StylizeView();
	}

	public override void OnReceiveMessages(object data){
		base.OnReceiveMessages(data);
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
		view.CallbackView(styleArgs);
	}

	void ChooseHelper(object args){
		MsgCenter.Instance.Invoke(CommandEnum.ChooseHelper, null);
	}

}
