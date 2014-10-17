using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsListModule : ModuleBase {
	public UnitsListModule(UIConfigItem config):base(  config){
		CreateUI<UnitsListView> ();
	}
}
