using UnityEngine;
using System.Collections;

public class BattleBottomModule : ModuleBase {

	public BattleBottomModule(UIConfigItem config):base(config){
		CreateUI<BattleBottomView> ();
	}
}
