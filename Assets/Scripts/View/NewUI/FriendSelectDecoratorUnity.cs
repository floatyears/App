using UnityEngine;
using System.Collections.Generic;

public class FriendSelectDecoratorUnity : UIComponentUnity, IUICallback {

	private GameObject msgBox;
	private UIImageButton btnStart;

	public override void Init (UIInsConfig config, IUIOrigin origin) {
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

		msgBox = FindChild("msg_box");
		msgBox.SetActive( false );

		btnStart = FindChild< UIImageButton >( "btn_quest_start" );
		btnStart.isEnabled = false;

	}

	public void Callback (object data)
	{
		bool b = (bool)data;
		btnStart.isEnabled = b;
	}
}
