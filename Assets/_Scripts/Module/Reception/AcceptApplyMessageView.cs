using UnityEngine;
using System.Collections;

public class AcceptApplyMessageView : ApplyMessageView{
	UIButton deleteButton;

	public override void Init(UIConfigItem config){
		base.Init(config);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
	}

	void InitUIElement(){
		deleteButton = FindChild<UIButton>("Window/Button_Delete");
		UIEventListener.Get(deleteButton.gameObject).onClick = ClickDeleteButton;
	}

	void ClickDeleteButton(GameObject btn){
		//Debug.LogError("Click the delete button, call controller to response...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickDelete", null);
//		ExcuteCallback(cbdArgs);
	}

}

