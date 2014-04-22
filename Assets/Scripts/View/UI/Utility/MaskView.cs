using UnityEngine;
using System.Collections;

public class MaskView : UIComponentUnity {
	UISprite background;
	UISprite connecting;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (call.funcName){
			case "ShowMask" :
				CallBackDispatcherHelper.DispatchCallBack(SetMaskActive, call);
				break;
			case "ShowConnect" :
				CallBackDispatcherHelper.DispatchCallBack(SetConnectActive, call);
				break;
			default:
				break;
		}
	}

	void InitUI(){
		background = FindChild<UISprite>("Sprite_Mask");
		connecting = FindChild<UISprite>("Sprite_Connect");
		background.enabled = false;
		connecting.transform.parent.gameObject.SetActive(false);
	}
	void SetMaskActive(object args){
		if (background == null) {
			return;	
		}
		bool isActive = (bool)args;
		background.enabled = isActive;
		gameObject.SetActive(isActive);

	}

	void SetConnectActive(object args){
		if (connecting == null) {
			return;	
		}
		bool isActive = (bool)args;
		connecting.gameObject.SetActive(isActive);
		gameObject.SetActive(isActive);
	}

}
