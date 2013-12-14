using UnityEngine;
using System.Collections;

public class FriendsView : UIBase 
{
	FriendsUnity window;
	FriendsUnity imgBtns;

	public FriendsView(string uiName) : base(uiName)
	{

	}

	public override void CreatUI ()
	{
		window = ViewManager.Instance.GetViewObject("Friends_InfoWindow") as FriendsUnity;
		imgBtns = ViewManager.Instance.GetViewObject("Friends_InfoList") as FriendsUnity;

		currentUIDic.Add(window.UIName, window);
		currentUIDic.Add(imgBtns.UIName, imgBtns);

		window.gameObject.transform.localPosition = new Vector3(0, 60, 0);
		imgBtns.gameObject.transform.localPosition = new Vector3(0, -100, 0);
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
		imgBtns.gameObject.SetActive(b);
	}

}
