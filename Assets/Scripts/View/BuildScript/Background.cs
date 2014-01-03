using UnityEngine;
using System.Collections;

public class Background : ConcreteComponent , IUICallback {
	
	public Background(string uiName) : base (uiName) {

	}

	public override void CreatUI () {
		base.CreatUI ();
		//viewComponent.Init (uiConfig, this);
		
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	public void Callback (object caller) {
		UIManager.Instance.ChangeScene (SceneEnum.Quest);
	}
}
