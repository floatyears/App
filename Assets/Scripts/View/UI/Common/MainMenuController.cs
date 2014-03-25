using UnityEngine;
using System.Collections;

public class MainMenuController : ConcreteComponent, IUICallback {
	public MainMenuController (string uiName) : base(uiName) {}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
        //LogHelper.Log("MenuBtnsComponent.HideUI()");
		base.HideUI ();
	}

	public void Callback (object data){
		try {
			SceneEnum se = (SceneEnum)data;
			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}
}
