using UnityEngine;
using System.Collections;

public class ShowNewCardModule : ModuleBase {
	public ShowNewCardModule(UIConfigItem config,params object[] data) : base(config, data) {
		CreateUI<ShowNewCardView> ();
	}
	
}
