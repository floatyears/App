using UnityEngine;
using System.Collections;

public class ErrorMsgWindow : UIComponentUnity,IUICallback {

	UILabel errorNoteLabel;
	UIButton closeButton;

	public override void Init(UIInsConfig config, IUIOrigin origin){
		FindUIElement();

		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
	}

	public override void HideUI(){
		base.HideUI();
		ResetUIElement();
	}

	public override void DestoryUI(){
		base.DestoryUI();
	}

	void FindUIElement(){
//		Debug.Log("ErrorMsgWindow.FindUIElement() : Start ");
		errorNoteLabel = FindChild<UILabel>("Label_Error");
		closeButton = FindChild<UIButton>("Button");
//		Debug.Log("ErrorMsgWindow.FindUIElement() : End ");

	}

	void SetUIElement(){
		this.gameObject.SetActive(false);
	}

	void ClickButton(GameObject go){
		this.gameObject.SetActive(false);
		UIEventListener.Get(closeButton.gameObject).onClick -= ClickButton;
	}

	void ShowErrorMsg(string text){
		UIEventListener.Get(closeButton.gameObject).onClick += ClickButton;
		this.gameObject.SetActive(true);
		errorNoteLabel.text = text;
	}

	void ResetUIElement(){
		errorNoteLabel.text = string.Empty;
	}

	public void Callback(object data){
		string errorText = data as string;
		ShowErrorMsg( errorText );
	}
}
