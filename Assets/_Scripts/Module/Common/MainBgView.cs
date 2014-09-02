using UnityEngine;
using System.Collections;

public class MainBackgroundView : ViewBase {
	private UISprite background;
	private UITexture otherBg;
	public override void Init (UIConfigItem config){
		base.Init (config);
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

//	void OnClickCallback(GameObject caller) {
//		if(origin != null && origin is IUICallback){
//			IUICallback callback = origin as IUICallback;
//			callback.CallbackView (caller);	
//		}
//	}

	private void ShowMask(object msg){
		bool isMask = (bool)msg;
//		Debug.LogError ("ShowMask : " + isMask);
		otherBg.enabled = isMask;
		background.enabled = !isMask;
	}
}
