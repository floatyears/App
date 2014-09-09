using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDetailModule : ModuleBase {
	public UnitDetailModule(UIConfigItem config, params object[] data):base(  config, data) {
		CreateUI<UnitDetailView> ();
	}
}
