using UnityEngine;
using System.Collections;

public class FriendSelectView : UIBase
{
	FriendSelectUnity window;

	private ScrollView friendListScroller;

	public UIImageButton startBtn;

	private TopUI topUI;
	private BottomUI bottomUI;

	public FriendSelectView(string uiName) : base(uiName)
	{

	}

	public override void CreatUI ()
	{
		//add top and bottom UI
		topUI = ViewManager.Instance.GetViewObject("MenuTop") as TopUI;
		bottomUI = ViewManager.Instance.GetViewObject("MenuBottom") as BottomUI;
		topUI.transform.parent = viewManager.TopPanel.transform;
		bottomUI.transform.parent = viewManager.BottomPanel.transform;
		topUI.transform.localPosition = Vector3.zero;
		bottomUI.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject("FriendSelectWindow") as FriendSelectUnity;
		window.Init ("FriendSelectWindow");
		//currentUIDic.Add (window.UIName, window);

		friendListScroller = new ScrollView(window.LeftTransform, window.RightTransform, window.FriendItem);
		friendListScroller.ShowData(5);
		
		for(int i= 0; i<friendListScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get( friendListScroller.DragList[ i ] );	
			listen.onClick = ClickFriend;
		}

		startBtn = window.GetComponentInChildren<UIImageButton>();
		startBtn.isEnabled = false;
		LogHelper.Log(startBtn.gameObject.name);
		UIEventListener.Get(startBtn.gameObject).onClick = StartQuest;

	}
	void StartQuest(GameObject btn)
	{
		LogHelper.Log("Fight Start...........");
		ControllerManager.Instance.ChangeScene(SceneEnum.Fight);
	}

	void ClickFriend(GameObject btn)
	{
		LogHelper.Log("Picked Up One Friend......");
		startBtn.isEnabled = true;
	}
	public override void ShowUI ()
	{
		SetActive(true);
		//base.ShowUI ();
	}
	
	public override void HideUI ()
	{
		SetActive(false);
		//base.HideUI ();
	}
	
	public override void DestoryUI ()
	{
		
	}
	
	void SetActive(bool b)
	{
		LogHelper.Log("friend select ```` " + b);
		window.gameObject.SetActive(b);
		friendListScroller.insUIObject.SetActive(b);

		topUI.gameObject.SetActive(b);
		bottomUI.gameObject.SetActive(b);
	}

}
