using UnityEngine;
using System.Collections;

public class ShopDecoratorUnity : UIComponentUnity {
	
	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		//ShowTweeners();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
}
