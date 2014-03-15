using UnityEngine;
using System.Collections;

public class FriendListUserBriefInfo : UserBriefInfoLogic
{
	
	public FriendListUserBriefInfo(string uiName):base(uiName)
	{

	}

	public override void ShowUI()
	{
		base.ShowUI();
		EnableDeleteFriend();
	}

	public override void Callback(object data)
	{
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName)
		{
			case "ClickDelete": 
				CallBackDispatcherHelper.DispatchCallBack(DeleteFriend, cbdArgs);
				break;
			default:
				break;
		}
	}
	
	void DeleteFriend(object args)
	{
//		int position = (int)args;
		Debug.LogError("Delete Friend.... ");
		MsgCenter.Instance.Invoke(CommandEnum.EnsureDeleteFriend, null);
	}

	void EnableDeleteFriend()
	{
//		Debug.LogError("Call View to Enable Delete Friend...");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableDeleteFriend", null);
		ExcuteCallback(cbdArgs);
	}

}

