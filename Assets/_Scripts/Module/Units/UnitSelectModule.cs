using UnityEngine;
using System.Collections;

public class UnitSelectModule : ModuleBase {

	public UnitSelectModule(UIConfigItem config, params object[] data):base(config,data){
		CreateUI<UnitSelectView> ();
	}
}
