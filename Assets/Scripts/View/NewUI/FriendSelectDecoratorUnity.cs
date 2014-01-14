using UnityEngine;
using System.Collections.Generic;

public class FriendSelectDecoratorUnity : UIComponentUnity, IUICallback {

	private GameObject msgBox;
	private UIImageButton btnStart;
	private UIButton btnSure;
	private UIButton btnCancel;
	private UIButton btnSeeInfo;

	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);
		InitUI();

	}
	
	public override void ShowUI () {
		base.ShowUI ();
		btnStart.isEnabled = false;
	}
	
	public override void HideUI () {
		base.HideUI ();
		//btnStart.isEnabled = false;
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {

		msgBox = FindChild("msg_box");
	
//		btnSure = FindChild< UIImageButton >( "btn_choose" );
//		btnCancel = FindChild< UIImageButton >( "btn_exit" );
//		btnSeeInfo = FindChild< UIImageButton >( "btn_see_info" );
		btnSure = FindChild("msg_box/btn_choose").GetComponent<UIButton>();
		btnCancel = FindChild("msg_box/btn_exit").GetComponent<UIButton>();
		btnSeeInfo = FindChild("msg_box/btn_see_info").GetComponent<UIButton>();

		btnStart = FindChild< UIImageButton >( "btn_quest_start" );


		UIEventListener.Get(btnStart.gameObject).onClick = ClickStartBtn;
		UIEventListener.Get(btnCancel.gameObject).onClick = ClickCancelBtn;
		UIEventListener.Get(btnSure.gameObject).onClick = ClickChooseBtn;
		UIEventListener.Get(btnSeeInfo.gameObject).onClick = ClickSeeInfoBtn;
		msgBox.SetActive( false );
	}

	void ClickCancelBtn(GameObject btn) {

		msgBox.SetActive( false );

	}

	void ClickChooseBtn(GameObject btn) {

		msgBox.SetActive( false );
		btnStart.isEnabled = true;
	}

	void ClickSeeInfoBtn(GameObject btn) {

		msgBox.SetActive( false );
	}

	void ClickStartBtn(GameObject btn) {

		btnStart.isEnabled = true;
	
	}

	public void Callback(object data)
	{
		bool canActivateMsgBox = (bool)data;
		msgBox.SetActive( canActivateMsgBox );
	}

}
