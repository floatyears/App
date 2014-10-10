using UnityEngine;
using System.Collections;

public class NoviceGuideTipsModule : ModuleBase {

	public NoviceGuideTipsModule(UIConfigItem config, params object[] data):base(config,data){
		CreateUI<NoviceGuideTipsView> ();
	}
}
