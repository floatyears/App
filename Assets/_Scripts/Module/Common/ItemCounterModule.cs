using UnityEngine;
using System.Collections;

public class ItemCounterModule : ModuleBase{
	public ItemCounterModule(UIConfigItem config, params object[] data) : base(config,data){
		CreateUI<ItemCounterView> ();
	}
}
