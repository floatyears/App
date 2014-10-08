using UnityEngine;
using System.Collections;
using bbproto;

public class ApplyMessageModule : ModuleBase{


	public ApplyMessageModule(UIConfigItem config, params object[] data) : base( config, data ){
		CreateUI<ApplyMessageView> ();
	}

}

