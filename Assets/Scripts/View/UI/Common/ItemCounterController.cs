using UnityEngine;
using System.Collections;

public class ItemCounterController : ConcreteComponent{
	public ItemCounterController(string name) : base(name){}
	public override void HideUI () {
		base.HideUI ();
//		base.DestoryUI ();
	}
}
