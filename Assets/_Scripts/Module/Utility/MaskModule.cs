using UnityEngine;
using System.Collections;

public class MaskModule : ModuleBase {
	public MaskModule(UIConfigItem config) : base(  config){
		CreateUI<MaskView> ();
    }


}
