using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManipulationModule : ModuleBase {


	public BattleManipulationModule(UIConfigItem config):base(config){
		CreateUI<BattleManipulationView> ();
	}

}
