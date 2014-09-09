using UnityEngine;
using System.Collections;

public class FightReadyModule : ModuleBase {
	public FightReadyModule(UIConfigItem config, params object[] data) : base(  config,data){
		CreateUI<FightReadyView> ();
	}
}
