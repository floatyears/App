using UnityEngine;
using System.Collections;

public class MaskView : UIComponentUnity {
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (call.funcName){
			case "ShowMask" :
				CallBackDispatcherHelper.DispatchCallBack(SetUIActive, call);
				break;
			default:
				break;
		}
	}

	void SetUIActive(object args){
		bool isActive = (bool)args;
		gameObject.SetActive(isActive);
	}

}
