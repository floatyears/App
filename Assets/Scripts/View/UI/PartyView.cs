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
		LogHelper.Log("1111112222222233333333444444");
		StartView.playerInfoBar.gameObject.SetActive(false);
		StartView.menuBtns.gameObject.SetActive(false);
		StartView.mainBg.gameObject.SetActive(false);
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

