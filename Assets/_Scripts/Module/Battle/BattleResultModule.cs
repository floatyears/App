using UnityEngine;
using System.Collections;

public class BattleResultModule : ModuleBase {

	public BattleResultModule(UIConfigItem config):base(config){
		CreateUI<BattleResultView> ();
	}
}
