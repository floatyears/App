using UnityEngine;
using System.Collections;

public class RewardComponent : ConcreteComponent {

	public RewardComponent(string uiName):base(uiName){}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void CallbackView(object data){
		base.CallbackView(data);
		
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
