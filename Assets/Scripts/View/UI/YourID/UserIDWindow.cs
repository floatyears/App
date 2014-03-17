using UnityEngine;
using System.Collections;

public class UserIDWindow : UIComponentUnity, IUICallback
{

	UILabel idLabel;

	public override void Init(UIInsConfig config, IUICallback origin)
	{
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI()
	{
		base.ShowUI();
		ShowTween();
	}

	public override void HideUI()
	{
		base.HideUI();
	}

	public override void DestoryUI()
	{
		base.DestoryUI();
	}

	public override void Callback(object data)
	{
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName)
		{
			case "ShowUserID": 
				CallBackDispatcherHelper.DispatchCallBack(ShowUserID, cbdArgs);
				break;
			default:
				break;
		}
	}
	
	void ShowUserID(object args)
	{
		uint id = (uint)args;
		Debug.LogError("IDSURE : " + id);
		idLabel.text = id.ToString();
	}

	private void InitUI()
	{
		idLabel = FindChild<UILabel>("Label_ID_Vaule");

		UIButton buttonOK = FindChild< UIButton >("Button_OK");
		UIEventListener.Get(buttonOK.gameObject).onClick = ClickButton;
	}

	void ClickButton(GameObject go)
	{
		UIManager.Instance.ChangeScene(SceneEnum.Friends);
	}

	private void ShowTween()
	{
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

}
