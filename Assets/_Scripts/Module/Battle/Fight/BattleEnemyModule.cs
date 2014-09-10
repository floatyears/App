using UnityEngine;
using System.Collections;

public class BattleEnemyModule : ModuleBase {

	public BattleEnemyModule(UIConfigItem config):base(config){
		CreateUI<BattleEnemyView> ();
	}
}
