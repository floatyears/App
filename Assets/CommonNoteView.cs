using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonNoteView : UIComponentUnity
{
	GameObject window;
	UILabel titleLabel;
	UILabel noteContentLabel;
	UIButton cancelButton;
	UIButton sureButton;
	int originLayer;

	public override void Init(UIInsConfig config, IUICallback origin)
	{
		FindUIElement();

		base.Init(config, origin);
	}

	public override void ShowUI()
	{
		base.ShowUI();
		SetUIElement();
	}

	public override void HideUI()
	{
		base.HideUI();
		ResetUIElement();
		ShowSelf(false);
	}

	public override void DestoryUI()
	{
		base.DestoryUI();
	}

	void FindUIElement()
	{
		window = FindChild("Window");
		sureButton = FindChild<UIButton>("Window/Button_Left");
		cancelButton = FindChild<UIButton>("Window/Button_Right");
		titleLabel = FindChild<UILabel>("Window/Label_Title");
		noteContentLabel = FindChild<UILabel>("Window/Label_Note_Content");

		UIEventListener.Get(cancelButton.gameObject).onClick = ClickCancelButton;
		UIEventListener.Get(sureButton.gameObject).onClick = ClickSureButton;
		originLayer = Main.Instance.NguiCamera.eventReceiverMask;
	}

	void ShowSelf(bool canShow)
	{
		this.gameObject.SetActive(canShow);
		if (canShow)
		{
			SetScreenShelt("ScreenShelt");
			window.transform.localScale = new Vector3(1f, 0f, 1f);
			iTween.ScaleTo(window, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
		} else
		{
			SetScreenShelt("Default");
		}
	}

	void SetScreenShelt(string layerName)
	{
		if (layerName == "ScreenShelt")
			Main.Instance.NguiCamera.eventReceiverMask = LayerMask.NameToLayer(layerName) << 15;
		else
			Main.Instance.NguiCamera.eventReceiverMask = originLayer;
	}

	void SetUIElement()
	{
		this.gameObject.SetActive(false);
	}

	void ClickButton(GameObject go)
	{
		this.gameObject.SetActive(false);
		UIEventListener.Get(cancelButton.gameObject).onClick -= ClickButton;
	}

	void ShowErrorMsg(string text)
	{
		UIEventListener.Get(cancelButton.gameObject).onClick += ClickButton;
		this.gameObject.SetActive(true);
		titleLabel.text = text;
	}

	void ResetUIElement()
	{
		titleLabel.text = string.Empty;
	}

	public override void Callback(object data)
	{
		ShowSelf(true);  
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName)
		{
			case "NoteUpdateFriend": 
				CallBackDispatcherHelper.DispatchCallBack(UpdateNotePanel, cbdArgs);
				break;
			case "NoteRefuseApply": 
				CallBackDispatcherHelper.DispatchCallBack(UpdateNotePanel, cbdArgs);
				break;
			default:
				break;
		}
	}

	void ClickCancelButton(GameObject btn)
	{
		ShowSelf(false);
	}

	void ClickSureButton(GameObject btn)
	{
//		MsgCenter.Instance.Invoke(CommandEnum.EnsureUpdateFriend, null);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSure", null);
		ExcuteCallback(cbdArgs);
		ShowSelf(false);
	}

	void UpdateNotePanel(object args)
	{
		Dictionary<string,string> noteContentTextDic = args as Dictionary<string,string>;
		titleLabel.text = noteContentTextDic ["title"];
		noteContentLabel.text = noteContentTextDic ["content"];
	}
}
