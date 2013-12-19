using UnityEngine;
using System.Collections;

public class FriendSelectView : UIBase
{
	FriendSelectUnity window;

	private ScrollView friendListScroller;

	public UIImageButton startBtn;

	public FriendSelectView(string uiName) : base(uiName)
	{

	}

	public override void CreatUI ()
	{
		window = ViewManager.Instance.GetViewObject("FriendSelectWindow") as FriendSelectUnity;
		window.Init ("FriendSelectWindow");
		currentUIDic.Add (window.UIName, window);

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
	}
	
	public override void HideUI ()
	{
		SetActive(false);
	}
	
	public override void DestoryUI ()
	{
		
	}
	
	void SetActive(bool b)
	{
		window.gameObject.SetActive(b);
		friendListScroller.insUIObject.SetActive(b);
	}

}
