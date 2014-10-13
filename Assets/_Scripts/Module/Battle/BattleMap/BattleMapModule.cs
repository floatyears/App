using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleMapModule : ModuleBase {
	private GameObject rootObject;

	public BattleMapModule (UIConfigItem config) : base(  config) {
		CreateUI<BattleMapView> ();

	}

	
	public GameObject GetMapItem(int x, int y){
		return (view as BattleMapView).GetMapItem (x, y);
	}
}
