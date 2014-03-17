using UnityEngine;
using System.Collections;

public class SearchFriendController : ConcreteComponent
{
	TFriendInfo currentSearchFriend;

	public SearchFriendController(string uiName) : base( uiName ){}

	public override void CreatUI()
	{
		base.CreatUI();
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
			case "ClickSearch": 
				CallBackDispatcherHelper.DispatchCallBack(SearchFriendWithID, cbdArgs);
				break;
			default:
				break;
		}
	}
	
	void SearchFriendWithID(object args)
	{ 
		string idString = args as string;
		Debug.LogError("Receive the click, to search the friend with the id ....");
		if (idString == string.Empty)
		{
			Debug.LogError("Search ID Input can't be empty!!!!!");
			MsgCenter.Instance.Invoke(CommandEnum.NoteInformation, ConfigNoteMessage.inputIDEmpty);

		} else
		{
			uint id = System.Convert.ToUInt32(idString);
			Debug.LogError("SearchFriendController.SearchFriendWithID(), The ID input is " + id);
			OnRearchFriendWithId(id);
		}
	}

	public void OnRearchFriendWithId(uint friendId){
		FindFriend.SendRequest(OnRspFindFriend, friendId);
	}

	public void OnRspFindFriend(object data)
	{
		if (data == null)
			return;
        
		LogHelper.Log("TFriendList.OnRspDelFriend() begin");
		LogHelper.Log(data);
		bbproto.RspFindFriend rsp = data as bbproto.RspFindFriend;
		// first set it to null
		if (rsp.header.code != (int)ErrorCode.SUCCESS)
		{
			LogHelper.Log("OnRspFindFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			if (rsp.header.code == ErrorCode.EF_FRIEND_NOT_EXISTS)
			{
				ShowFriendNotExist();
			}
			else if (rsp.header.code == ErrorCode.EF_IS_ALREADY_FRIEND){
				ShowAlreadyFriend();
			}
			return;
		}

		TFriendInfo searchResult = new TFriendInfo(rsp.friend);
		ShowSearchFriendResult(searchResult);
	}

	public void ShowSearchFriendResult(TFriendInfo resultInfo)
	{
		currentSearchFriend = resultInfo;
		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, resultInfo);
	}

	public void ShowFriendNotExist()
	{
		currentSearchFriend = null;
		MsgCenter.Instance.Invoke(CommandEnum.NoteInformation, ConfigNoteMessage.searchFriendNotExist);
	}

	public void ShowAlreadyFriend()
	{

		currentSearchFriend = null;
		MsgCenter.Instance.Invoke(CommandEnum.NoteInformation, ConfigNoteMessage.alreadyFriend);
	}

//    //////////////////Test
//    public static void TestSearchFriendReq(){
//        LogHelper.Log("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS  TestSearchFriendReq() start");
//        // Test exists
//        SearchFriendController controller = new SearchFriendController("ssss");
//        controller.OnRearchFriendWithId(174);
//        controller.OnRearchFriendWithId(999);
//
//    }


	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.SubmitFriendApply, SubmitFriendApply);
	}

	void RmvCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.SubmitFriendApply, SubmitFriendApply);
	}

	void SubmitFriendApply(object msg){
		Debug.LogError("SearchFriendController.SubmitFriendApply(), to request to make friend with the search...");
		if(currentSearchFriend == null){
			Debug.LogError("SearchFriendController.SubmitFriendApply(), currentSearchFriend is null, return....");
			return;
		}

		
	}

}
