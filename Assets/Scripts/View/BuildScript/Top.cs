using UnityEngine;
using System.Collections;

public class Top : ConcreteComponent  {
	public Top(string uiName):base(uiName) {

	}

	public override void CreatUI () {
		base.CreatUI ();
		viewComponent.Init (uiConfig);
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
}
