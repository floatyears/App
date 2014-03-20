using UnityEngine;
using System.Collections;

public class SearchFriendWindow : UIComponentUnity
{
	UIButton buttonSearch;
	UIInput input;


	public override void Init(UIInsConfig config, IUICallback origin)
	{
		base.Init(config, origin);
		InitWindow();
	}

	public override void ShowUI()
	{
		base.ShowUI();
	}

	void InitWindow()
	{
		buttonSearch = FindChild< UIButton >("Button_Search");
		UIEventListener.Get(buttonSearch.gameObject).onClick = ClickButton;
		input = FindChild< UIInput >("Input");
		input.value = string.Empty;
	}

	void ClickButton(GameObject btn)
	{
		Debug.LogError("SearchFriendWindow.ClickButton(),  call controller respones....");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSearch", input.value);
		ExcuteCallback(cbdArgs);
	}


}
