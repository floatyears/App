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


    uint GetSearchFindId(object args){
        // TODO logic here;
        return 0;
    }
	void SearchFriendWithID(object args)
	{
//		TFriendInfo tfi;
		Debug.LogError("Receive the click, to search the friend with the id ....");
//		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, tfi);

	}

    public void onRearchFriendWithId(uint friendId){
        FindFriend.SendRequest(OnRspFindFriend, GetSearchFindId());
    }

    public void OnRspFindFriend(object data) {
        if (data == null)
            return;
        
        LogHelper.Log("TFriendList.OnRspDelFriend() begin");
        LogHelper.Log(data);
        bbproto.RspFindFriend rsp = data as bbproto.RspFindFriend;
        // first set it to null
        searchResult = null;
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            LogHelper.Log("OnRspFindFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            //            if (rsp.header.code == (int)ErrorCode.EF_FRIEND_NOT_EXISTS)
            //                return;
            if (rsp.code == ErrorCode.EF_FRIEND_NOT_EXISTS){
                ShowFriendNotExist();
            }
            return;
        }

        LogHelper.LogError("=========================OnRspFindFriend(), friendId {0}, friendName {1}", rsp.friend.userId, rsp.friend.nickName);

        TUserInfo searchResult = new TUserInfo(rsp.friend);
        ShowSearchFriendResult(searchResult);
    }

    public void ShowSearchFriendResult(TUserInfo resultInfo){
        // TODO show result here

    }

    public void ShowFriendNotExist(){
        // 
    }
}
