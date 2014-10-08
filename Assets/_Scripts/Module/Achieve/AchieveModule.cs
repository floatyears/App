using UnityEngine;
using System.Collections;

public class AchieveModule : ModuleBase {

	public AchieveModule(UIConfigItem config):base(config){
		CreateUI<AchieveView> ();
	}
}
