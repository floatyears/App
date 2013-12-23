using UnityEngine;
using System.Collections;

public class FriendSelectView : UIBase
{
	FriendSelectUnity window;

	private ScrollView friendListScroller;

	public UIImageButton startBtn;

	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	public FriendSelectView(string uiName) : base(uiName)
	{

	}

	public override void CreatUI ()
	{
		//add scene info bar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();

		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();

		window = ViewManager.Instance.GetViewObject("FriendSelectWindow") as FriendSelectUnity;
		window.Init ("FriendSelectWindow");

		friendListScroller = new ScrollView(window.LeftTransform, window.RightTransform, window.FriendItem);
		friendListScroller.ShowData(5);
		
		for(int i= 0; i<friendListScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get( friendListScroller.DragList[ i ] );	
			listen.onClick = ClickFriend;
		}

		startBtn = window.GetComponentInChildren<UIImageButton>();
		startBtn.isEnabled = false;
		UIEventListener.Get(startBtn.gameObject).onClick = StartQuest;

	}

	private void BackToPreScene(GameObject btn)
	{
		ChangeScene(SceneEnum.QuestSelect);
	}

	void StartQuest(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Fight);
	}

	void ClickFriend(GameObject btn)
	{
		startBtn.isEnabled = true;
	}

	void SetActive(bool b)
	{
		window.gameObject.SetActive(b);
		friendListScroller.insUIObject.SetActive(b);
		
		sceneInfoBar.gameObject.SetActive(b);
	}

	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = true;
		sceneInfoLab.text = uiName;
		UIEventListener.Get(backBtn.gameObject).onClick += BackToPreScene;
	}
	
	public override void HideUI ()
	{
		UIEventListener.Get(backBtn.gameObject).onClick -= BackToPreScene;
		SetActive(false);
	}
	
	public override void DestoryUI ()
	{
		
	}
}
