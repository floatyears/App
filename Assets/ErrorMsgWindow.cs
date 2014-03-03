using UnityEngine;
using System.Collections;

public class ErrorMsgWindow : UIComponentUnity {

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
		Debug.Log("ErrorMsgWindow.FindUIElement() : Start ");
		errorNoteLabel = FindChild<UILabel>("Label_Error");
		closeButton = FindChild<UIButton>("Button");
		Debug.Log("ErrorMsgWindow.FindUIElement() : End ");

	}

	void SetUIElement(){
		AddListener();
	}

	void ClickButton(GameObject go){
		this.gameObject.SetActive(false);
		UIEventListener.Get(closeButton.gameObject).onClick -= ClickButton;
	}

	void ShowErrorMsg(object msg){
		string errorText = msg as string;
		UIEventListener.Get(closeButton.gameObject).onClick += ClickButton;
		this.gameObject.SetActive(true);
		errorNoteLabel.text = errorText;
	}

	void ResetUIElement(){
		errorNoteLabel.text = string.Empty;
		RemoveListener();
	}

	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ErrorMsgShow , ShowErrorMsg);
	}

	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ErrorMsgShow , ShowErrorMsg);
	}

}
