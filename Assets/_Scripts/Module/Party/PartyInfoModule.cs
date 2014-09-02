using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoLogic : ModuleBase {
	public PartyInfoLogic(UIConfigItem config):base(  config) {}

	public override void HideUI () {
		base.HideUI ();
		base.DestoryUI ();
	}
}

public class UnitInfoLogic : ModuleBase {
	public UnitInfoLogic(UIConfigItem config):base(  config) {}

	public override void HideUI () {
		base.HideUI ();
		base.DestoryUI ();
	}
}