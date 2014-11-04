using UnityEngine;
using System.Collections;

public class BattleFailModule : ModuleBase {

	public BattleFailModule(UIConfigItem config):base(config){
		CreateUI<BattleFailView> ();
	}
}
