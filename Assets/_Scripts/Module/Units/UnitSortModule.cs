using UnityEngine;
using System.Collections;

public class UnitSortModule : ModuleBase {

	public UnitSortModule(UIConfigItem config, params object[] data):base(config, data){
		CreateUI<UnitSortView> ();
	}

}
