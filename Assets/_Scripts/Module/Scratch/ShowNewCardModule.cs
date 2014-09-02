using UnityEngine;
using System.Collections;

public class ShowNewCardModule : ModuleBase {
	public ShowNewCardModule(UIConfigItem config) : base(  config) {
		CreateUI<ShowNewCardView> ();
	}
}
