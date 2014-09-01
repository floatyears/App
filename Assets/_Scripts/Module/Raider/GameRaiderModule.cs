using UnityEngine;
using System.Collections;

public class GameRaiderModule : ModuleBase {

	public GameRaiderModule(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
		Debug.Log ("raider create ui");
	}
	
	public override void ShowUI () {

		base.ShowUI ();

		ShowUIAnimation ();
	}
	
	public override void HideUI () {

		base.HideUI ();

	}

	void ShowUIAnimation(){
		view.transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
		iTween.MoveTo(view.gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
	}
}
