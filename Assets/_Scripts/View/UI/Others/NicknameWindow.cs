﻿using UnityEngine;
using System.Collections;

public class NicknameWindow : UIComponentUnity {

	UIButton okButton;
	UIButton CancelButton;
	UIInput nickNameInput;

	public override void Init ( UIInsConfig config, IUICallback origin ){
		FindUIElement();
		SetNickNamePanel ();
		//		SetOption();
		base.Init (config, origin);
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

		if (string.IsNullOrEmpty(DataCenter.Instance.UserInfo.NickName)) {
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
			MsgWindowParams mwp = new MsgWindowParams ();
			//mwp.btnParams = new BtnParam[1];
			mwp.btnParam = new BtnParam ();
			mwp.titleText = TextCenter.GetText("NameIsNullTitle");
			mwp.contentText = TextCenter.GetText("NameIsNullContent");
			
			BtnParam sure = new BtnParam ();
			sure.callback = null;
			sure.text = TextCenter.GetText("OK");
			mwp.btnParam = sure;
			
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
		} else {
			ChangeName (!string.IsNullOrEmpty(nickNameInput.value) ? nickNameInput.value : nickNameInput.defaultText);
		}

	}

	void ClickCancelButton(GameObject go){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		//		MsgCenter.Instance.Invoke( CommandEnum.ReqRenameNick, nickNameInput.value );
		UIManager.Instance.ChangeScene(SceneEnum.Others);
	}


	// leiliang--------------------------------------------------------------------
	private INetBase changeName;
	public void ChangeName(object  name)
	{
		//        LogHelper.Log("ChangeName(), start");
		//		if (changeName == null)
		//		{
		changeName = new RenameNick();
		changeName.OnRequest(name, ReName);
		//			UIManager.Instance.ChangeScene(SceneEnum.Start);
		//		}
	}

	
	void ReName(object data){
		if (data != null && DataCenter.Instance.UserInfo != null)
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
				DataCenter.Instance.UserInfo.NickName = rspRenameNick.newNickName;
				
				GameObject.Find("PlayerInfoBar(Clone)").GetComponent<PlayerInfoBar>().UpdateData();
			} else
			{
				//TODO: show error msgbox.
			}
		}
		
		UIManager.Instance.ChangeScene(SceneEnum.Others);
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
