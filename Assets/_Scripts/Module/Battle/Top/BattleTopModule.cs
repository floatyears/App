using UnityEngine;
using System.Collections;

public class BattleTopModule : ModuleBase {

	public BattleTopModule(UIConfigItem config):base(config){
		CreateUI<BattleTopView> ();
	}
	
	public Vector3 GetCoinPos(){
		return (view as BattleTopView).coinLabel.transform.position;
	}
}
