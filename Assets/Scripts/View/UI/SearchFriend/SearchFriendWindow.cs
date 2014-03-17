using UnityEngine;
using System.Collections;

public class SearchFriendWindow : UIComponentUnity
{
	UIButton buttonSearch;
	UILabel labelInput;


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
		labelInput = FindChild< UILabel >("Input/Label");
		labelInput.text = string.Empty;
	}

	void ClickButton(GameObject btn)
	{
		Debug.LogError("SearchFriendWindow.ClickButton(),  call controller respones....");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSearch");
//		ExcuteCallback(cbdArgs);
	}


}
