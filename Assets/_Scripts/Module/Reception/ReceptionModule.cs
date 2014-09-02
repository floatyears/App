using UnityEngine;
using System.Collections;

public class ReceptionModule : ModuleBase {
	public ReceptionModule(UIConfigItem config) : base(   config ){
		CreateUI<ReceptionView> ();
	}


}
