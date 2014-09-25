using UnityEngine;
using System.Collections;

public class BattleSkillModule : ModuleBase {

	public BattleSkillModule(UIConfigItem config, params object[] data):base(config, data){
		CreateUI<BattleSkillView> ();
	}
}
