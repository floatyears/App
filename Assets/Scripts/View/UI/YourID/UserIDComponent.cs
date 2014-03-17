using UnityEngine;
using System.Collections;

public class UserIDComponent : ConcreteComponent
{

	public UserIDComponent(string uiName) : base( uiName )
	{
	}

	public override void ShowUI()
	{
		base.ShowUI();
		ShowUsedID();
	}
	
	public override void HideUI()
	{
		base.HideUI();
	}


	void ShowUsedID()
	{
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowUserID", DataCenter.Instance.UserInfo.UserId);
		ExcuteCallback(cbdArgs);
	}



}