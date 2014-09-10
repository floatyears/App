using UnityEngine;
using System.Collections;

public class OperationNoticeModule : ModuleBase {

	public OperationNoticeModule(UIConfigItem config):base(  config){
		CreateUI<OperationNoticeView> ();
	}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (data[0].ToString()){
		case "SyncFriendList": 
//			CallBackDispatcherHelper.DispatchCallBack(SyncFriendList, cbdArgs);
			break;
		default:
			break;
		}
	}


}
