using UnityEngine;
using System.Collections.Generic;

public class FriendSelectDecoratorUnity : UIComponentUnity {

	private GameObject msgBox;
	private UIImageButton btnStart;
	private UIButton btnSure;
	private UIButton btnCancel;
	private UIButton btnSeeInfo;

	private DragPanel friendsScroller;
	private GameObject friendItem;

	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);
		InitUI();

	}
	
	public override void ShowUI () {
		base.ShowUI ();
		btnStart.isEnabled = false;
		friendsScroller.RootObject.gameObject.SetActive(true);
	}
	
	public override void HideUI () {
		base.HideUI ();
		//btnStart.isEnabled = false;
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {

		msgBox = FindChild("Window/msg_box");
	
		btnSure = FindChild< UIButton >( "Window/msg_box/btn_choose" );
		btnCancel = FindChild< UIButton >( "Window/msg_box/btn_exit" );
		btnSeeInfo = FindChild< UIButton >( "Window/msg_box/btn_see_info" );
	
		btnStart = FindChild< UIImageButton >( "ScrollView/btn_quest_start" );


		UIEventListener.Get(btnStart.gameObject).onClick = ClickStartBtn;
		UIEventListener.Get(btnCancel.gameObject).onClick = ClickCancelBtn;
		UIEventListener.Get(btnSure.gameObject).onClick = ClickChooseBtn;
		UIEventListener.Get(btnSeeInfo.gameObject).onClick = ClickSeeInfoBtn;
		msgBox.SetActive( false );

		friendItem = Resources.Load("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
		friendsScroller = new DragPanel ("FriendSelectScroller", friendItem);
		friendsScroller.CreatUI();
		friendsScroller.AddItem (13);
		friendsScroller.RootObject.SetItemWidth(140);

		friendsScroller.RootObject.gameObject.transform.parent = gameObject.transform.FindChild("ScrollView");
		friendsScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		friendsScroller.RootObject.gameObject.transform.localPosition = -115*Vector3.up;
		
		for(int i = 0; i < friendsScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(friendsScroller.ScrollItem[ i ].gameObject).onClick = PickFriend;
		}
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
	
	void PickFriend(GameObject btn)
	{
		msgBox.SetActive( true );
	}

}
