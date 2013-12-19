using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectView : UIBase
{
	private QuestSelectUnity window;

	public QuestSelectView(string uiName):base(uiName)
	{

	}
	public override void CreatUI ()
	{
		window = ViewManager.Instance.GetViewObject( "QuestSelectWindow" ) as QuestSelectUnity;
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
	}

}
