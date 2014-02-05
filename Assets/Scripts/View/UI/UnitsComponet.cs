using UnityEngine;
using System.Collections;

public class UnitsComponent : ConcreteComponent,IUICallback {
	
	public UnitsComponent(string uiName):base(uiName) {}
	
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
		try {
			SceneEnum se = (SceneEnum)data;

			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}
}
