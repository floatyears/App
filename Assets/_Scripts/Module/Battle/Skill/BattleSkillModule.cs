using UnityEngine;
using System.Collections;

public class BattleSkillModule : ModuleBase {

	public BattleSkillModule(UIConfigItem config):base(config){
		CreateUI<BattleSkillView> ();
	}
}
