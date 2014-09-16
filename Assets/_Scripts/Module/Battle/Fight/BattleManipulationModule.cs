using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManipulationModule : ModuleBase {


	public BattleManipulationModule(UIConfigItem config):base(config){
		CreateUI<BattleManipulationView> ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data [1].ToString ()) {
		case "":
			break;
		}
	}

}
