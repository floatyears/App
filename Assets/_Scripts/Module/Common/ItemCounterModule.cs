using UnityEngine;
using System.Collections;

public class ItemCounterModule : ModuleBase{
	public ItemCounterModule(UIConfigItem config) : base(  config){
		CreateUI<ItemCounterView> ();
	}

	public override void ShowUI () {
//		Debug.LogError("ItemCounterController showui 1 ");
		base.ShowUI ();
//		Debug.LogError("ItemCounterController showui 2 ");
	}

	public override void HideUI () {
//		Debug.LogError("ItemCounterController hide ui 1");
		base.HideUI ();
//		Debug.LogError("ItemCounterController hide ui 2");
//		base.DestoryUI ();
	}
}
