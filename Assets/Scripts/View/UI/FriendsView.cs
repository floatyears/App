using UnityEngine;
using System.Collections;

public class FriendsView : UIBase 
{
	private FriendsUnity window;
	private SceneInfoBar sceneInfoBar;

	public FriendsView(string uiName) : base(uiName){}

	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject( UIConfig.friendPath + "FriendWindow") as FriendsUnity;
		window.Init ("FriendWindow");
		currentUIDic.Add(window.UIName, window);
		window.gameObject.transform.localPosition = -135*Vector3.up;
	}

	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = false;
		sceneInfoBar.UITitleLab.text = UIName;
	}
	
	public override void HideUI ()
	{
		SetUIActive(false);
	}
	
	void SetUIActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

}
