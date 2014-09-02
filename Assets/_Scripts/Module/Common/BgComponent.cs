using UnityEngine;
using System.Collections;

public class BgComponent : ModuleBase {
	
	public BgComponent(UIConfigItem config) : base (  config) {}

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
