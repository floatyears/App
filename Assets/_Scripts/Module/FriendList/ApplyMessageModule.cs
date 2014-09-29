using UnityEngine;
using System.Collections;
using bbproto;

public class ApplyMessageModule : ModuleBase{


	public ApplyMessageModule(UIConfigItem config, params object[] data) : base( config, data ){
		CreateUI<ApplyMessageView> ();
	}

//	void InitUIElement(){
//		deleteButton = FindChild<UIButton>("Window/Button_Delete");
//		UIEventListenerCustom.Get(deleteButton.gameObject).onClick = ClickDeleteButton;
//	}
//	
//	void ClickDeleteButton(GameObject btn){
//		//Debug.LogError("Click the delete button, call controller to response...");
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickDelete", null);
//		//		ExcuteCallback(cbdArgs);
//		ModuleManager.SendMessage (ModuleEnum.AcceptApplyMessageModule, "ClickDelete");
//	}
}

