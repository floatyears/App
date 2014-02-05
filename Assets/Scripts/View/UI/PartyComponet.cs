using UnityEngine;
using System.Collections;

public class PartyComponent : ConcreteComponent {
	
	public PartyComponent(string uiName):base(uiName) {}
	
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
			SceneEnum scene = (SceneEnum)data;
			UIManager.Instance.ChangeScene( scene );

		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}
}
