using UnityEngine;
using System.Collections;

public class SortController : ConcreteComponent {
	public SortController(string uiName) : base(uiName){}

	public override void HideUI () {
		base.HideUI ();
		base.DestoryUI ();
	}
}
