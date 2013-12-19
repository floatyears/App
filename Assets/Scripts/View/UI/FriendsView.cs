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

		window.Init ("Friends_InfoWindow");
		imgBtns.Init ("Friends_InfoList");

		currentUIDic.Add(window.UIName, window);
		currentUIDic.Add(imgBtns.UIName, imgBtns);

		window.gameObject.transform.localPosition = 220*Vector3.up;
		imgBtns.gameObject.transform.localPosition = -100*Vector3.up;
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
