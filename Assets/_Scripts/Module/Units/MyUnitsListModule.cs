using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitsListModule : ModuleBase {
	public MyUnitsListModule(UIConfigItem config):base(  config){
		CreateUI<MyUnitsListView> ();
	}
}
