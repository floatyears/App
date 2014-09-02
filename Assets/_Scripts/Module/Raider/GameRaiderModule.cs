using UnityEngine;
using System.Collections;

public class GameRaiderModule : ModuleBase {

	public GameRaiderModule(UIConfigItem config):base(  config) {
		CreateUI<GameRaiderView> ();
	}
	
	public override void InitUI () {
		base.InitUI ();
		Debug.Log ("raider create ui");
	}
	
	public override void ShowUI () {

		base.ShowUI ();
	}
	
	public override void HideUI () {

		base.HideUI ();

	}

}
