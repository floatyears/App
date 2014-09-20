using UnityEngine;
using System.Collections;

public class BattleResultModule : ModuleBase {

	public BattleResultModule(UIConfigItem config, params object[] data):base(config,data){
		CreateUI<BattleResultView> ();
	}
}
