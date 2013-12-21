using UnityEngine;
using System.Collections;

public class UnitView : UIBase
{
	UnitUnity topWindow;
	UnitUnity centerWindow;
	UnitUnity bottomWindow;

	private TopUI topUI;
	private BottomUI bottomUI;

	private GameObject partyBtn;

	public UnitView(string uiName) : base(uiName)
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
		topUI.callback += Handlecallback;
		topWindow = ViewManager.Instance.GetViewObject("UnitTopWindow") as UnitUnity;
		centerWindow = ViewManager.Instance.GetViewObject("UnitCenterWindow") as UnitUnity;
		bottomWindow = ViewManager.Instance.GetViewObject("UnitBottomWindow") as UnitUnity;
		topWindow.Init ("UnitTopWindow");
		centerWindow.Init ("UnitCenterWindow");
		bottomWindow.Init ("UnitBottomWindow");

		partyBtn = bottomWindow.transform.FindChild("Party").gameObject;
		UIEventListener.Get(partyBtn.gameObject).onClick = TurnToParty;

		currentUIDic.Add(topWindow.UIName, topWindow);
		currentUIDic.Add(bottomWindow.UIName, bottomWindow);
		currentUIDic.Add(centerWindow.UIName, centerWindow);
		
		topWindow.gameObject.transform.localPosition = 330*Vector3.up;
		centerWindow.gameObject.transform.localPosition = 160*Vector3.up;
		bottomWindow.gameObject.transform.localPosition = -90*Vector3.up;
	}

	void Handlecallback (GameObject caller)
	{
		controllerManger.HideActor();
	}

	void TurnToParty(GameObject go)
	{
		controllerManger.ShowActor(1);

		//topUI.ShowUI();
		topUI.gameObject.SetActive(true);
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
		topWindow.gameObject.SetActive(b);
		centerWindow.gameObject.SetActive(b);
		bottomWindow.gameObject.SetActive(b);

		topUI.gameObject.SetActive(b);
		bottomUI.gameObject.SetActive(b);
	}
}

