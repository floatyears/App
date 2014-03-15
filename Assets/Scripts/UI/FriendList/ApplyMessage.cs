using UnityEngine;
using System.Collections;

public class ApplyMessage : ConcreteComponent
{
	protected TFriendInfo curPickedApplyFriend;
	public ApplyMessage(string uiName) : base( uiName )
	{
	}

	public override void ShowUI()
	{
		base.ShowUI();
		AddCommandListener();
	}

	public override void HideUI()
	{
		base.HideUI();
		RmvCommandListener();
	}

	public override void Callback(object data)
	{
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName)
		{
			case "ClickCancel": 
				CallBackDispatcherHelper.DispatchCallBack(ApplyCancel, cbdArgs);
				break;
			
			default:
				break;
		}
	}

	void ShowApplyInfo(object msg)
	{
		Debug.LogError("333333333333");
		TFriendInfo tfi = msg as TFriendInfo;
		RefreshApplyFriendInfo(tfi);
	}

	void ApplyCancel(object args)
	{

		Debug.LogError("ApplyCancel(), call view to close the window ...");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("Cancel", null);
		ExcuteCallback(cbdArgs);
	}

	void AddCommandListener()
	{
		MsgCenter.Instance.AddListener(CommandEnum.ViewApplyInfo, ShowApplyInfo);
	}

	void RmvCommandListener()
	{
		MsgCenter.Instance.RemoveListener(CommandEnum.ViewApplyInfo, ShowApplyInfo);
                
	}

	void RefreshApplyFriendInfo(TFriendInfo tfi)
	{
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshContent", tfi);
		ExcuteCallback(cbdArgs);
	}


}

