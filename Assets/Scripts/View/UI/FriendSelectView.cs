using UnityEngine;
using System.Collections;

public class FriendSelectView : UIBase
{
	FriendSelectUnity window;

	//private ScrollView friendListScroller;

	public UIImageButton btnStart;
	private GameObject msgBox;
	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UIButton btnMsgChoose;
	private UIButton btnMsgSeeInfo;
	private UIButton btnMsgExit;
	private UILabel sceneInfoLab;

	private DragPanel friendsScroller;
	private GameObject friendItem;

	public FriendSelectView(string uiName) : base(uiName)
	{

	}

	public override void CreatUI ()
	{
		//Add Share UI -- SceneInfoBar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;
		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();
		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();
		window = ViewManager.Instance.GetViewObject("FriendSelectWindow") as FriendSelectUnity;
		window.Init ("FriendSelectWindow");
		window.transform.parent = viewManager.TopPanel.transform;

		//Add Scroller -- FriendScroller
		friendItem = Resources.Load("Prefabs/FriendScrollerItem") as GameObject;
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
		LogHelper.Log("11111111111");
		msgBox.SetActive(false);
		friendsScroller.RootObject.gameObject.SetActive(true);
		btnStart.isEnabled = true;
	}
	void SeeFriendInfo(GameObject go)
	{
		LogHelper.Log("2222222222");
		msgBox.SetActive(false);
		friendsScroller.RootObject.gameObject.SetActive(true);
	}

	void CancelChoose(GameObject go)
	{
		LogHelper.Log("33333333333333");
		msgBox.SetActive(false);
		friendsScroller.RootObject.gameObject.SetActive(true);
	}
	private void BackToPreScene(GameObject btn)
	{
		btnStart.isEnabled = false;
		ChangeScene(SceneEnum.QuestSelect);
	}

	void StartQuest(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Fight);
	}

	void SetActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
		friendsScroller.RootObject.gameObject.SetActive(b);
	}

	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = true;
		sceneInfoLab.text = uiName;
		UIEventListener.Get(backBtn.gameObject).onClick += BackToPreScene;
		UIEventListener.Get(btnStart.gameObject).onClick += StartQuest;
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
		UIEventListener.Get(backBtn.gameObject).onClick -= BackToPreScene;
		UIEventListener.Get(btnStart.gameObject).onClick -= StartQuest;
		for(int i = 0; i < friendsScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(friendsScroller.ScrollItem[ i ].gameObject).onClick -= PickFriend;
		}
		SetActive(false);
	}
	
	public override void DestoryUI ()
	{
		
	}
}
