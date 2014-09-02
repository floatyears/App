using UnityEngine;
using System.Collections;

public class UserIDView : ViewBase{
	UILabel idLabel;

	public override void Init(UIConfigItem config){
		base.Init(config);
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

	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "ShowUserID": 
				CallBackDispatcherHelper.DispatchCallBack(ShowUserID, cbdArgs);
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
		ModuleManger.Instance.ShowModule(ModuleEnum.FriendsModule);
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
