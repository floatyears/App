using UnityEngine;
using System.Collections;

public class PartyView : UIBase
{
	PartyView partyWindow;

	private TopUI topUI;

	public PartyView(string uiName) : base(uiName)
	{
		
	}
	
	public override void CreatUI ()
	{
		//add top and bottom UI
		topUI = ViewManager.Instance.GetViewObject("MenuTop") as TopUI;
		topUI.transform.parent = viewManager.TopPanel.transform;
		topUI.transform.localPosition = Vector3.zero;
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
		topUI.gameObject.SetActive(b);
	}
}

