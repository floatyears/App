using UnityEngine;
using System.Collections;

public class VicotoryEffectControl : ModuleBase {
	public VicotoryEffectControl(UIConfigItem config) : base (  config) {

	}

	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}
}
