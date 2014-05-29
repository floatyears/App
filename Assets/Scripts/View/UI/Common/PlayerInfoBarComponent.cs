using UnityEngine;
using System.Collections;

public class PlayerInfoBarComponent : ConcreteComponent  {

	public PlayerInfoBarComponent(string uiName):base(uiName) {

	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		Debug.Log ("player info hide------------");
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
}
