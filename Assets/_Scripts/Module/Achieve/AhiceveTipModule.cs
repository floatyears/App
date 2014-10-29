using UnityEngine;
using System.Collections;

public class AchieveTipModule : ModuleBase {

	public AchieveTipModule(UIConfigItem config, params object[] data):base(config, data){
		CreateUI<AchieveTipView>();
	}
}
