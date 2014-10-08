using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class PartyModule : ModuleBase{
	public PartyModule(UIConfigItem config) : base(  config){
		CreateUI<PartyView> ();
	}

}
