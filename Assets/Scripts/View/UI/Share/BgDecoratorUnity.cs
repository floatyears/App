using UnityEngine;
using System.Collections;

public class BgDecoratorUnity : UIComponentUnity {
	private UISprite sprite;

	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config,origin);
		sprite = GetComponent<UISprite> ();
	}

	public override void ShowUI () {
		base.ShowUI();
		UIEventListener.Get (gameObject).onClick = OnClickCallback;
		NGUITools.AddWidgetCollider (gameObject);
	}

	public override void HideUI () {
		base.HideUI();

	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void OnClickCallback(GameObject caller) {
//		Debug.LogError ("origin``" + origin == null + "```type``" + origin.GetType());
		if(origin != null && origin is IUICallback){
			IUICallback callback = origin as IUICallback;
			callback.Callback (caller);	
		}
	}
}
