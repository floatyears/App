using UnityEngine;
using System.Collections;

public class PlayerInfoBarModule : ModuleBase  {

	public PlayerInfoBarModule(UIConfigItem config):base(  config) {

	}

	public override void InitUI () {
		base.InitUI ();
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
