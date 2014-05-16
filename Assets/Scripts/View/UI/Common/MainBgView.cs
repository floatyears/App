using UnityEngine;
using System.Collections;

public class MainBgView : UIComponentUnity {
	private UISprite maskSpr;

	public override void Init (UIInsConfig config, IUICallback origin){
		base.Init (config,origin);
		maskSpr = FindChild<UISprite>("Mask");
		maskSpr.enabled = false;
	}

	public override void ShowUI () {
		base.ShowUI();
		UIEventListener.Get (gameObject).onClick = OnClickCallback;
		NGUITools.AddWidgetCollider (gameObject);

		MsgCenter.Instance.AddListener(CommandEnum.ShowHomeBgMask, ShowMask);
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowHomeBgMask, ShowMask);
	}

	void OnClickCallback(GameObject caller) {
		if(origin != null && origin is IUICallback){
			IUICallback callback = origin as IUICallback;
			callback.CallbackView (caller);	
		}
	}

	private void ShowMask(object msg){
		//Debug.Log("Receive msg, show bg mask");
		maskSpr.enabled = (bool)msg;
	}
}
