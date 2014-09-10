using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserIDView : ViewBase{
	UILabel idLabel;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void DestoryUI(){
		base.DestoryUI();
	}

	public override void CallbackView(params object[] args){
//		base.CallbackView(data);

//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (args[0].ToString()){
			case "ShowUserID": 
				ShowUserID(args[1]);
				break;
			default:
				break;
		}
	}
	
	void ShowUserID(object args){
		uint id = (uint)args;
//		Debug.LogError("IDSURE : " + id);
		idLabel.text = id.ToString();
	}

	private void InitUI(){
		idLabel = FindChild<UILabel>("Label_ID_Vaule");

		UIButton buttonOK = FindChild< UIButton >("Button_OK");
		UIEventListener.Get(buttonOK.gameObject).onClick = ClickButton;
	}

	void ClickButton(GameObject go){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		ModuleManager.Instance.ShowModule(ModuleEnum.FriendMainModule);
	}

	void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)
				continue;
			tweenPos.ResetToBeginning();
			tweenPos.PlayForward();
		}
	}

}
