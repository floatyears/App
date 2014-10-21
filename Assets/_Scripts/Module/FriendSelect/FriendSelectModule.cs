using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class FriendSelectModule : ModuleBase{


	public FriendSelectModule( UIConfigItem config, params object[] data):base( config, data) {
		CreateUI<FriendSelectView> ();

	}


}
