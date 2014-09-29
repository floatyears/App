using UnityEngine;
using System.Collections;

public class FriendMainModule : ModuleBase{
	public FriendMainModule(UIConfigItem config):base(  config){
		CreateUI<FriendMainView> ();
	}

}
