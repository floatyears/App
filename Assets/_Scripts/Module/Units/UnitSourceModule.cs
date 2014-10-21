using UnityEngine;
using System.Collections;

public class UnitSourceModule : ModuleBase {

	public UnitSourceModule(UIConfigItem config,params object[] data):base(config, data){
		CreateUI<UnitSourceView> ();
	}
}
