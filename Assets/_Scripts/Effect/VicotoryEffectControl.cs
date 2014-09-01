using UnityEngine;
using System.Collections;

public class VicotoryEffectControl : ModuleBase {
	public VicotoryEffectControl(string name) : base (name) {

	}

	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}
}
