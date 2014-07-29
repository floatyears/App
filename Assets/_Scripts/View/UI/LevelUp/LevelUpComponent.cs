using UnityEngine;
using System.Collections;

public class LevelUpComponent : ConcreteComponent, IUICallback {

	public LevelUpComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
//		Debug.LogError ("LevelUpComponent : CreatUI");
	}
	
	public override void ShowUI () {
//		Debug.LogError("LevelUpComponent ");
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void CallbackView (object data)
	{
		IUICallback call = viewComponent as IUICallback;
		if(call != null) {
			call.CallbackView(data);
		}
	}
}
