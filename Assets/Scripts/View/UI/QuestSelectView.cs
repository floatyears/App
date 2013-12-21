using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectView : UIBase
{
	private QuestSelectUnity window;

	private TopUI topUI;
	private BottomUI bottomUI;

	public QuestSelectView(string uiName):base(uiName)
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

		window = ViewManager.Instance.GetViewObject( "QuestSelectWindow" ) as QuestSelectUnity;
		window.Init ("QuestSelectWindow");
		currentUIDic.Add( window.UIName, window );
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

		topUI.gameObject.SetActive(b);
		bottomUI.gameObject.SetActive(b);
	}

}
