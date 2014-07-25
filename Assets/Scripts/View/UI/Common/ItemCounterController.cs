using UnityEngine;
using System.Collections;

public class ItemCounterController : ConcreteComponent{
	public ItemCounterController(string name) : base(name){}

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
