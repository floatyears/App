using UnityEngine;
using System.Collections;

public class BattleFullScreenTipsModule : ModuleBase {

	public BattleFullScreenTipsModule(UIConfigItem config):base(config){
		CreateUI<BattleFullScreenTipsView> ();
	}
	
}
