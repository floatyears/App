using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitListModule : ModuleBase {
	public MyUnitListModule(UIConfigItem config):base(  config){
		CreateUI<MyUnitListView> ();
	}
}
