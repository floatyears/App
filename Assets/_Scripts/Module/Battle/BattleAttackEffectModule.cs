using UnityEngine;
using System.Collections;

public class BattleAttackEffectModule : ModuleBase {

	public BattleAttackEffectModule(UIConfigItem config):base(config){
		CreateUI<BattleAttackEffectView> ();
	}
}
