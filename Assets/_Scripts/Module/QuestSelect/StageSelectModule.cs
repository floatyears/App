using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelectModule: ModuleBase{
	public StageSelectModule(UIConfigItem config, params object[] data):base(config,data){
		CreateUI<StageSelectView> ();
	}
	
}
