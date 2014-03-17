using UnityEngine;
using System.Collections;

public class SearchFriendController : ConcreteComponent
{

	public SearchFriendController(string uiName) : base( uiName )
	{
	}

	public override void CreatUI()
	{
		base.CreatUI();
	}

	public override void ShowUI()
	{
		base.ShowUI();
	}

	public override void Callback(object data)
	{
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName)
		{
			case "ClickSearch": 
				CallBackDispatcherHelper.DispatchCallBack(SearchFriendWithID, cbdArgs);
				break;
			default:
				break;
		}
	}

	void SearchFriendWithID(object args)
	{
//		TFriendInfo tfi;
		Debug.LogError("Receive the click, to search the friend with the id ....");
//		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, tfi);


	}


}
