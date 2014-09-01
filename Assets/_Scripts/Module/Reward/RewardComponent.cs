using UnityEngine;
using System.Collections;

public class RewardComponent : ModuleBase {

	public RewardComponent(string uiName):base(uiName){}
	public override void ShowUI(){
		base.ShowUI();

//		ShowUIAnimation ();
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

	
//	void ShowUIAnimation(){
//		view.gameObject.transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
//		iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
//	}
}
