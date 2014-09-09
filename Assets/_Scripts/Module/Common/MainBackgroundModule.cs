using UnityEngine;
using System.Collections;

public class MainBackgroundModule : ModuleBase {
	
	public MainBackgroundModule(UIConfigItem config) : base (  config) {
		CreateUI<MainBackgroundView> ();
	}

	public override void InitUI () {
		base.InitUI ();
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
	
	public void OnReceiveMessage (object caller) {

	}
	
}
