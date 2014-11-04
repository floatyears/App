using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class SellUnitModule : ModuleBase {
	int maxPickCount = 12;
	int totalSaleValue = 0;


	public SellUnitModule(UIConfigItem config):base(  config) {
		CreateUI<SellUnitView> ();
	}
    
}
