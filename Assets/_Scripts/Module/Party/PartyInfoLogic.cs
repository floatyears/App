using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoLogic : ModuleBase {
	public PartyInfoLogic(string uiName):base(uiName) {}

	public override void HideUI () {
		base.HideUI ();
		base.DestoryUI ();
	}
}

public class UnitInfoLogic : ModuleBase {
	public UnitInfoLogic(string uiName):base(uiName) {}

	public override void HideUI () {
		base.HideUI ();
		base.DestoryUI ();
	}
}