using UnityEngine;
using System.Collections.Generic;

public class NoviceMsgWindowModule : ModuleBase{
	public NoviceMsgWindowModule(UIConfigItem config):base(  config){
		CreateUI<NoviceMsgWindowView> ();
	}

	
}