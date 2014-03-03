using UnityEngine;
using System.Collections;

public class ErrorMsgComponent : ConcreteComponent {

	static IUICallback errorCall;

	public ErrorMsgComponent(string uiName):base(uiName) {}

	public override void CreatUI(){
//		Debug.Log("ErrorMsgComponent.CreateUI() : Start");
		base.CreatUI();
		errorCall = viewComponent as IUICallback;
	}

	public override void ShowUI(){
		base.ShowUI();
		AddListener();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveListener();
	}

	public override void DestoryUI(){
		base.DestoryUI();
	}

	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ErrorMsgShow , ShowErrorMsg);
	}
	
	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ErrorMsgShow , ShowErrorMsg);
	}

	public static void ShowErrorMsg(object error){
		if( errorCall == null ){
			Debug.LogError("ErrorMsgComponent.ShowErrorMsg(), ErrorCall is null, return");
		}
		errorCall.Callback( error as string );
	}

}
