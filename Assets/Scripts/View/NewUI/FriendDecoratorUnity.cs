using UnityEngine;
using System.Collections.Generic;

public class FriendDecoratorUnity : UIComponentUnity {

	private Dictionary< GameObject, SceneEnum > btns = new Dictionary< GameObject, SceneEnum >();

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
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

	private void FindBtns() {
//		GameObject btn;
//		btn = FindChild< UIButton >( "BtnList_Window/ImgBtn_FriendList" );
	}
}
