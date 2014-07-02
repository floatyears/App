using UnityEngine;
using System.Collections;

public class MainBgView : UIComponentUnity {
	private UISprite background;
	private UITexture otherBg;
	public override void Init (UIInsConfig config, IUICallback origin){
		base.Init (config,origin);
		background = transform.FindChild("HomeBG").GetComponent<UISprite>();
		otherBg = FindChild<UITexture> ("OtherBG");
		otherBg.enabled = false;
	}

	public override void ShowUI () {
		base.ShowUI();
//		UIEventListener.Get (gameObject).onClick = OnClickCallback;
//		NGUITools.AddWidgetCollider (gameObject);

		MsgCenter.Instance.AddListener(CommandEnum.ShowHomeBgMask, ShowMask);
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowHomeBgMask, ShowMask);
	}

	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowHomeBgMask, ShowMask);
		base.DestoryUI ();
	}

	void OnClickCallback(GameObject caller) {
		if(origin != null && origin is IUICallback){
			IUICallback callback = origin as IUICallback;
			callback.CallbackView (caller);	
		}
	}

	private void ShowMask(object msg){
		bool isMask = (bool)msg;
		if(isMask){
			otherBg.enabled = true;
			background.enabled = false;
		}
		else{
			otherBg.enabled = false;
			background.enabled = true;
		}
	}
}
