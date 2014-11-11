using UnityEngine;
using System.Collections;

public class NicknameModule : ModuleBase {

	public NicknameModule(UIConfigItem config):base(  config){
		CreateUI<NicknameView> ();
	}

}
