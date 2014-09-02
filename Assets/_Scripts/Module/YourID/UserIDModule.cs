using UnityEngine;
using System.Collections;

public class UserIDComponent : ModuleBase
{

	public UserIDComponent(UIConfigItem config) : base(config )
	{
		CreateUI<UserIDView> ();
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
		view.CallbackView(cbdArgs);
	}



}