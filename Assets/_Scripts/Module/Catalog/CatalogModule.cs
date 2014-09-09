using UnityEngine;
using System.Collections;

public class CatalogModule : ModuleBase {
	public CatalogModule(UIConfigItem config):base(  config) {
		CreateUI<CatalogView> ();
	}
}
