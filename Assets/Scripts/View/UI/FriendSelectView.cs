using UnityEngine;
using System.Collections;

public class FriendSelectView : UIBase
{
	private FriendSelectUnity window;
	private SceneInfoBar sceneInfoBar;

	public UIImageButton btnStart;
	private GameObject msgBox;
	
	private UIButton btnMsgChoose;
	private UIButton btnMsgSeeInfo;
	private UIButton btnMsgExit;

	private DragPanel friendsScroller;
	private GameObject friendItem;

	public FriendSelectView(string uiName) : base(uiName){}

	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject( UIConfig.friendPath + "FriendSelectWindow") as FriendSelectUnity;
		window.Init ("FriendSelectWindow");
		window.transform.parent = viewManager.TopPanel.transform;

		friendItem = Resources.Load("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
		friendsScroller = new DragPanel ("FriendSelectScroller", friendItem);
		friendsScroller.CreatUI();
		friendsScroller.AddItem (13);
		friendsScroller.RootObject.SetItemWidth(140);
		friendsScroller.RootObject.gameObject.transform.localPosition = -680*Vector3.up;

		btnStart = window.GetComponentInChildren<UIImageButton>();
		btnStart.isEnabled = false;

		msgBox = window.gameObject.transform.Find("msg_box").gameObject;
		msgBox.transform.localPosition = new Vector3(0, 0, -5);
		btnMsgChoose = msgBox.transform.Find("btn_choose").GetComponent<UIButton>();
		btnMsgSeeInfo = msgBox.transform.Find("btn_see_info").GetComponent<UIButton>();
		btnMsgExit = msgBox.transform.Find("btn_exit").GetComponent<UIButton>();

		msgBox.SetActive(false);

	}

	void PickFriend(GameObject go)
	{
		friendsScroller.RootObject.gameObject.SetActive(false);
		msgBox.SetActive(true);
	}

	void ChooseFriend(GameObject go)
	{
		msgBox.SetActive(false);
		friendsScroller.RootObject.gameObject.SetActive(true);
		btnStart.isEnabled = true;
	}
	void SeeFriendInfo(GameObject go)
	{
		msgBox.SetActive(false);
		friendsScroller.RootObject.gameObject.SetActive(true);
	}

	void CancelChoose(GameObject go)
	{
		msgBox.SetActive(false);
		friendsScroller.RootObject.gameObject.SetActive(true);
	}

	private void JumpToQuest(GameObject btn)
	{
		StartView.playerInfoBar.gameObject.SetActive(false);
		StartView.menuBtns.gameObject.SetActive(false);
		StartView.mainBg.gameObject.SetActive(false);

		ControllerManager.Instance.ChangeScene(SceneEnum.Fight);
	}
	
	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = true;
		sceneInfoBar.UITitleLab.text = UIName;
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick += BackUI;

		UIEventListener.Get(btnStart.gameObject).onClick += JumpToQuest;
		UIEventListener.Get(btnMsgChoose.gameObject).onClick += ChooseFriend;
		UIEventListener.Get(btnMsgSeeInfo.gameObject).onClick += SeeFriendInfo;
		UIEventListener.Get(btnMsgExit.gameObject).onClick += CancelChoose;

		for(int i = 0; i < friendsScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(friendsScroller.ScrollItem[ i ].gameObject).onClick += PickFriend;
		}
	}

	public override void HideUI ()
	{
		SetUIActive(false);
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick -= BackUI;
		UIEventListener.Get(btnStart.gameObject).onClick -= JumpToQuest;
		for(int i = 0; i < friendsScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(friendsScroller.ScrollItem[ i ].gameObject).onClick -= PickFriend;
		}
	}

	private void SetUIActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
		friendsScroller.RootObject.gameObject.SetActive(b);
	}

	private void BackUI(GameObject btn)
	{
		btnStart.isEnabled = false;
		controllerManger.BackToPrevScene();
	}
}
