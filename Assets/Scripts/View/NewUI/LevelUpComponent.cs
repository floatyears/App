using UnityEngine;
using System.Collections;

public class LevelUpComponent : ConcreteComponent, IUICallback {

	public LevelUpComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
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

	public void Callback (object data)
	{
		IUICallback call = viewComponent as IUICallback;
		if(call != null) {
			call.Callback(data);
		}
	}
}
