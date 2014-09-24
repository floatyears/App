using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NicknameView : ViewBase {

	UIButton okButton;
	UIButton CancelButton;
	UIInput nickNameInput;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null){
		FindUIElement();
		SetNickNamePanel ();
		//		SetOption();
		base.Init (config,data);
	}
	
	public override void ShowUI(){
		base.ShowUI ();
		//		SetUIElement();

//		nickNameInput.isSelected = true;
//		nickNameInput.gameObject.SendMessage ("OnPress",true);
//		MsgCenter.Instance.Invoke (CommandEnum.SetBlocker,new BlockerMaskParams(BlockerReason.MessageWindow,true));
	}
	
	public override void HideUI(){
		base.HideUI ();
		//		ResetUIElement();

//		MsgCenter.Instance.Invoke (CommandEnum.SetBlocker,new BlockerMaskParams(BlockerReason.MessageWindow,false));
	}
	
	public override void DestoryUI(){
		UIEventListener.Get( okButton.gameObject ).onClick = null;
		base.DestoryUI ();
	}

	void FindUIElement(){
		FindChild< UILabel > ("OKButton/Label").text = TextCenter.GetText ("OK");
		FindChild< UILabel > ("CancelButton/Label").text = TextCenter.GetText ("Cancel");
		FindChild< UILabel > ("Title").text = TextCenter.GetText ("Game_Setting_Option_NickName");

		CancelButton = FindChild<UIButton> ("CancelButton");
		okButton = FindChild< UIButton >("OKButton" );
		nickNameInput = FindChild< UIInput >("NickNameInput" );
//		nickNameInput.

		if (string.IsNullOrEmpty(DataCenter.Instance.UserData.UserInfo.nickName)) {
			nickNameInput.value = TextCenter.GetText ("Default_Nickname");
//			FindChild<UILabel>("NickNameInput/Label").text = TextCenter.GetText ("Default_Nickname");	
		}

	}

	void SetNickNamePanel(){
		UIEventListener.Get( okButton.gameObject ).onClick = ClickOkButton;
		UIEventListener.Get( CancelButton.gameObject ).onClick = ClickCancelButton;
	}
	
	void ClickOkButton( GameObject go ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		LogHelper.Log("ClickOkButton to rename");
//		MsgCenter.Instance.Invoke( CommandEnum.ReqRenameNick, nickNameInput.value );
		if (string.IsNullOrEmpty(nickNameInput.value) && string.IsNullOrEmpty(nickNameInput.defaultText)){// == null || nickNameInput.value == "") {

			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("NameIsNullTitle"),TextCenter.GetText("NameIsNullContent"),TextCenter.GetText("OK"));
		} else {
			ChangeName (!string.IsNullOrEmpty(nickNameInput.value) ? nickNameInput.value : nickNameInput.defaultText);
		}

	}

	void ClickCancelButton(GameObject go){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		//		MsgCenter.Instance.Invoke( CommandEnum.ReqRenameNick, nickNameInput.value );
		ModuleManager.Instance.ShowModule(ModuleEnum.OthersModule);
	}


	// leiliang--------------------------------------------------------------------
	public void ChangeName(object  name)
	{
		//        LogHelper.Log("ChangeName(), start");
		//		if (changeName == null)
		//		{

		//			ModuleManger.Instance.ShowModule(ModuleEnum.Start);
		//		}
		UserController.Instance.RenameNick (ReName, (string)name );
	}

	
	void ReName(object data){
		if (data != null && DataCenter.Instance.UserData.UserInfo != null)
		{
			bbproto.RspRenameNick rspRenameNick = data as bbproto.RspRenameNick;
			Debug.Log("rename response newNickName : " + rspRenameNick.newNickName);
			
			if (rspRenameNick.header.code != (int)ErrorCode.SUCCESS) {
				Debug.LogError("Rsp code: "+rspRenameNick.header.code+", error:"+rspRenameNick.header.error);
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspRenameNick.header.code);	
				return;
			}
			
			bool renameSuccess = (rspRenameNick.header.code == 0);
			if (renameSuccess && rspRenameNick.newNickName != null)
			{
				DataCenter.Instance.UserData.UserInfo.nickName = rspRenameNick.newNickName;
				
				GameObject.Find("PlayerInfoBar(Clone)").GetComponent<PlayerInfoBarView>().UpdateData();
			} else
			{
				//TODO: show error msgbox.
			}
		}
		
		ModuleManager.Instance.ShowModule(ModuleEnum.OthersModule);
//		HideUI ();
	}

	
//	void ClickNameChangeButton( GameObject go ){
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		
//		Debug.Log( "OthersWindow ClickNameChangeButton() : Start");
//		RequestNameChange( GetInputText() );
//		Debug.Log( "OthersWindow ClickNameChangeButton() : End");
//	}
//	
//	string GetInputText(){
//		return nickNameInput.label.text;
//	}
//
//	
//	void ResetUIElement(){
//		nickNameInput.label.text = string.Empty;
//	}
//	
//	void RequestNameChange(string name){
//		Debug.Log( "OthersWindow RequestNameChange() : Start");
//		Debug.Log( "OthersWindow RequestNameChange() : End");
//	}
}
