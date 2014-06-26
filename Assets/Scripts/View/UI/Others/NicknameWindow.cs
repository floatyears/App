using UnityEngine;
using System.Collections;

public class NicknameWindow : UIComponentUnity {

	UIButton okButton;
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
	}
	
	public override void HideUI(){
		base.HideUI ();
		//		ResetUIElement();
	}
	
	public override void DestoryUI(){
		UIEventListener.Get( okButton.gameObject ).onClick = null;
		base.DestoryUI ();
	}

	void FindUIElement(){
		okButton = FindChild< UIButton >("OKButton" );
		nickNameInput = FindChild< UIInput >("NickNameInput" );
	}

	void SetNickNamePanel(){
		UIEventListener.Get( okButton.gameObject ).onClick = ClickOkButton;
	}
	
	void ClickOkButton( GameObject go ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		LogHelper.Log("ClickOkButton to rename");
//		MsgCenter.Instance.Invoke( CommandEnum.ReqRenameNick, nickNameInput.value );
		ChangeName (nickNameInput.value);
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
