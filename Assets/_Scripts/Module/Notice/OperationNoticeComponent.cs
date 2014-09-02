using UnityEngine;
using System.Collections;

public class OperationNoticeModule : ModuleBase {

	public OperationNoticeModule(UIConfigItem config):base(  config){}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void OnReceiveMessages(object data){
		base.OnReceiveMessages(data);
		
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (cbdArgs.funcName){
		case "SyncFriendList": 
			//CallBackDispatcherHelper.DispatchCallBack(SyncFriendList, cbdArgs);
			break;
		default:
			break;
		}
	}


}
