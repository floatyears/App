using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class SearchFriendView : ViewBase{
	UIButton buttonSearch;
	UIInput input;
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);

		buttonSearch = FindChild< UIButton >("Button_Search");
		UIEventListenerCustom.Get(buttonSearch.gameObject).onClick = ClickButton;
		input = FindChild< UIInput >("Input");
		input.value = string.Empty;
		input.validation = UIInput.Validation.Integer;
		
		FindChild<UILabel> ("Label_Title").text = TextCenter.GetText ("FriendSearch_Title");
		FindChild<UILabel> ("Label_Introduction").text = TextCenter.GetText ("FriendSearch_Content");
		FindChild<UILabel> ("Button_Search/Label").text = TextCenter.GetText ("Btn_Submit_SearchFriend");
	}

	void ClickButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		//Debug.LogError("SearchFriendWindow.ClickButton(),  call controller respones....");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSearch", input.value);
//		ExcuteCallback(cbdArgs);modu
		SearchFriendWithID( input.value);
	}
	
	void SearchFriendWithID(string idString){ 
		uint searchFriendUid = System.Convert.ToUInt32(idString);
		Debug.LogError("Receive the click, to search the friend with the id ....");
		if (searchFriendUid == 0){
			Debug.LogError("Search ID Input can't be empty!!!!!");
			//			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetSearchIdEmptyMsgWindowParams());
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("InputError"),TextCenter.GetText("InputEmpty"),TextCenter.GetText("OK"));
		}
		else{
			if(searchFriendUid < 10000){
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(-203);
			}else{
				Debug.LogError("SearchFriendController.SearchFriendWithID(), The ID input is " + searchFriendUid);
				FriendController.Instance.FindFriend(OnRspFindFriend, searchFriendUid);
			}
			
		}
	}
	
	void OnRspFindFriend(object data){
		if (data == null)
			return;

		bbproto.RspFindFriend rsp = data as bbproto.RspFindFriend;
		// first set it to null
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.Log("OnRspFindFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		ModuleManager.Instance.ShowModule (ModuleEnum.ApplyMessageModule, "data", rsp.friend,"title",TextCenter.GetText ("FriendApply"),"content",TextCenter.GetText ("ConfirmApply"));
	}
}
