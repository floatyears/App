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

	public override void Callback(object data){
		base.Callback(data);
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
		connecting.enabled = false;
	}
	void SetMaskActive(object args){
		bool isActive = (bool)args;
		background.enabled = isActive;
	}

	void SetConnectActive(object args){
		bool isActive = (bool)args;
		connecting.enabled = isActive;
	}

}
