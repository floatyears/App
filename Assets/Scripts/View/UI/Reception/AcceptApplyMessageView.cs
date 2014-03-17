using UnityEngine;
using System.Collections;

public class AcceptApplyMessageView : ApplyMessageView
{

	UIButton deleteButton;

	public override void Init(UIInsConfig config, IUICallback origin)
	{
		base.Init(config, origin);

		InitUIElement();
	}

	public override void ShowUI()
	{
		base.ShowUI();
	}

	public override void HideUI()
	{
		base.HideUI();
	}

	public override void Callback(object data)
	{
		base.Callback(data);
	}

	void InitUIElement(){
		deleteButton = FindChild<UIButton>("Window/Button_Delete");
		UIEventListener.Get(deleteButton.gameObject).onClick = ClickDeleteButton;
	}

	void ClickDeleteButton(GameObject btn){
		Debug.LogError("Click the delete button, call controller to response...");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickDelete", null);
		ExcuteCallback(cbdArgs);
	}

}

