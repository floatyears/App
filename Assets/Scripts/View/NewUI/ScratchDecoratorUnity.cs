using UnityEngine;
using System.Collections;

public class ScratchDecoratorUnity : UIComponentUnity {

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();

	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {

	}


}
