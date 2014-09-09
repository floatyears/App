using UnityEngine;
using System.Collections;

public class PlayerInfoBarModule : ModuleBase  {

	public PlayerInfoBarModule(UIConfigItem config):base(  config) {
		CreateUI<PlayerInfoBarView> ();
	}
}
