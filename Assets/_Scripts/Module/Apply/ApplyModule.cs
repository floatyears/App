using UnityEngine;
using System.Collections;

public class ApplyModule : ModuleBase{
	public ApplyModule(UIConfigItem config) : base(   config ){
		CreateUI<ApplyView> ();
	}
	
}
