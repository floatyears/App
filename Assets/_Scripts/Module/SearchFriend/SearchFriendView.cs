using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchFriendView : ViewBase{
	UIButton buttonSearch;
	UIInput input;
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitWindow();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	void InitWindow(){
		buttonSearch = FindChild< UIButton >("Button_Search");
		UIEventListener.Get(buttonSearch.gameObject).onClick = ClickButton;
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
		ModuleManager.SendMessage (ModuleEnum.SearchFriendModule, "ClickSearch", input.value);
	}
	
}
