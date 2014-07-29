using UnityEngine;
using System.Collections;

public class GameRaiderComponent : ConcreteComponent {

	public GameRaiderComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
		Debug.Log ("raider create ui");
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {

		base.HideUI ();

	}

}
