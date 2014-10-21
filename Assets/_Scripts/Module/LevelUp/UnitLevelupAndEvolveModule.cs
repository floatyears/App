using UnityEngine;
using System.Collections;

public class UnitLevelupAndEvolveModule : ModuleBase {


	public UnitLevelupAndEvolveModule (UIConfigItem config, params object[] data):base(config,data){
		CreateUI<UnitLevelupAndEvolveView>();
	}
	
}
