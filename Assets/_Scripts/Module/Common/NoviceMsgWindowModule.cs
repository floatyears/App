using UnityEngine;
using System.Collections.Generic;

public class NoviceMsgWindowModule : ModuleBase{
	public NoviceMsgWindowModule(UIConfigItem config, params object[] data):base(config,data){
		CreateUI<NoviceMsgWindowView> ();
	}

	
}