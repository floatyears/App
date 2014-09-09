using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchFriendModule : ModuleBase{
	TFriendInfo currentSearchFriend;
    uint searchFriendUid;

	public SearchFriendModule(UIConfigItem config) : base(   config ){
		CreateUI<SearchFriendView> ();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCommandListener();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCommandListener();
	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (data[0].ToString()){
			case "ClickSearch": 
				SearchFriendWithID(data[1]);
				break;
			default:
				break;
		}
	}



	void SearchFriendWithID(object args){ 
		string idString = args as string;
		Debug.LogError("Receive the click, to search the friend with the id ....");
		if (idString == string.Empty){
			Debug.LogError("Search ID Input can't be empty!!!!!");
//			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetSearchIdEmptyMsgWindowParams());
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("InputError"),TextCenter.GetText("InputEmpty"),TextCenter.GetText("OK"));

		}
		else{
            searchFriendUid = System.Convert.ToUInt32(idString);
			if(searchFriendUid < 10000){
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(-203);
			}else{
				Debug.LogError("SearchFriendController.SearchFriendWithID(), The ID input is " + searchFriendUid);
				OnRearchFriendWithId(searchFriendUid);
			}
            
		}
	}

	public void OnRearchFriendWithId(uint friendId){
		FindFriend.SendRequest(OnRspFindFriend, friendId);
	}

	public void OnRspFindFriend(object data){
		if (data == null)
			return;
        
        LogHelper.Log("TFriendList.OnRspFindFriend() begin");
		LogHelper.Log(data);
		bbproto.RspFindFriend rsp = data as bbproto.RspFindFriend;
		// first set it to null
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.Log("OnRspFindFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}

		TFriendInfo searchResult = new TFriendInfo(rsp.friend);
		ShowSearchFriendResult(searchResult);
	}
	
	void AddFriendApplication(uint friendUid){
		LogHelper.Log("AddFriendApplication () start");
		AddFriend.SendRequest(OnAddFriend, friendUid);
	}

	void OnAddFriend(object data){
		if (data == null)
			return;
		
		LogHelper.Log("TFriendList.OnRspAddFriend() begin");
		LogHelper.Log(data);
		bbproto.RspAddFriend rsp = data as bbproto.RspAddFriend;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.Log("RspAddFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            if (rsp.header.code == ErrorCode.EF_IS_ALREADY_FRIEND){
                ShowAlreadyFriend();
            }else {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			}
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
        LogHelper.Log("OnAddFriend(), rsp.friends {0}", inst);
        LogHelper.Log("OnAddFriend(), friendlist {0}, friendList == null {1}", DataCenter.Instance.FriendList, DataCenter.Instance.FriendList == null);
        DataCenter.Instance.SetFriendList(inst);
	}

	public void ShowSearchFriendResult(TFriendInfo resultInfo){
		currentSearchFriend = resultInfo;
		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, resultInfo);
	}

	public void ShowFriendNotExist(){
        LogHelper.Log("ShowFriendNotExist() start");
		currentSearchFriend = null;
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetSearchIdNotExistMsgWindowParams());
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("SearchError"), TextCenter.GetText ("SearchFriendNotExist", searchFriendUid), TextCenter.GetText ("OK"));
    }

	public void ShowAlreadyFriend(){
		currentSearchFriend = null;
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetSearchIdAlreadyFriendMsgWindowParams());
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("SearchError"), TextCenter.GetText ("UserAlreadyFriend", searchFriendUid), TextCenter.GetText ("OK"));
    }

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

		AddFriendApplication(currentSearchFriend.UserId);
	}

}
