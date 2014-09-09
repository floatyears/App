using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendHelperBriefInfo : UserBriefInfoModule {
	public FriendHelperBriefInfo(UIConfigItem config):base(  config){}

	public override void InitUI(){
		base.InitUI();
		//StylizeView();
	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (data[0].ToString()){
			case "Choose" : 
				ChooseHelper(data[1]);
				break;
			default:
				break;
		}
	}

	void StylizeView(){
//		Dictionary<string,string> styleArgs = ;
//		styleArgs.Add();
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("Stylize", styleArgs);
		view.CallbackView("Stylize", new Dictionary<string, string>(){{"ButtonTop", "Choose"}});
	}

	void ChooseHelper(object args){
		MsgCenter.Instance.Invoke(CommandEnum.ChooseHelper, null);
	}

}
