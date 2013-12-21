using UnityEngine;
using System.Collections;

public class PartyView : UIBase
{
	PartyView partyWindow;

	public PartyView(string uiName) : base(uiName)
	{
		
	}
	
	public override void CreatUI ()
	{
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

	}
}

