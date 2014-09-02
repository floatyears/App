using UnityEngine;
using System.Collections;

public class ReceptionController : ModuleBase {
	public ReceptionController(UIConfigItem config) : base(   config ){
		CreateUI<ReceptionView> ();
	}


}
