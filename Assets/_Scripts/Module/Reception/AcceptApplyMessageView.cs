using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcceptApplyMessageView : ApplyMessageView{
	UIButton deleteButton;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config,data);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(params object[] args){
		base.CallbackView(args);
	}

	void InitUIElement(){
		deleteButton = FindChild<UIButton>("Window/Button_Delete");
		UIEventListenerCustom.Get(deleteButton.gameObject).onClick = ClickDeleteButton;
	}

	void ClickDeleteButton(GameObject btn){
		//Debug.LogError("Click the delete button, call controller to response...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickDelete", null);
//		ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.AcceptApplyMessageModule, "ClickDelete");
	}

}

