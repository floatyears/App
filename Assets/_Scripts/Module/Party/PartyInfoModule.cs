using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoModule : ModuleBase {
	public PartyInfoModule(UIConfigItem config):base(  config) {}

	public override void HideUI () {
		base.HideUI ();
		base.DestoryUI ();
	}
}
