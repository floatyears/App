using UnityEngine;
using System.Collections;

public class NoviceMsgWindowLogic : ConcreteComponent{
	public NoviceMsgWindowLogic(string uiName):base(uiName){
		AddListener ();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void DestoryUI(){
		RemoveListener ();
		base.DestoryUI();
	}
	
	void AddListener(){
		//LogHelper.Log ("------------open novice guide msg window add listener");
		MsgCenter.Instance.AddListener(CommandEnum.OpenGuideMsgWindow, OpenGuideMsgWindow);
		MsgCenter.Instance.AddListener(CommandEnum.CloseGuideMsgWindow, CloseGuideMsgWindow);

		MsgCenter.Instance.AddListener (CommandEnum.DestoryGuideMsgWindow, DestoryGuideMsgWindow);
	}
	
	
	void RemoveListener(){
		//LogHelper.Log ("------------open novice guide msg window remove listener");
		MsgCenter.Instance.RemoveListener(CommandEnum.OpenGuideMsgWindow, OpenGuideMsgWindow);
		MsgCenter.Instance.RemoveListener(CommandEnum.CloseGuideMsgWindow, CloseGuideMsgWindow);

		MsgCenter.Instance.RemoveListener (CommandEnum.DestoryGuideMsgWindow, DestoryGuideMsgWindow);
	}
	
	
	void OpenGuideMsgWindow(object msg){ 
		LogHelper.Log ("------------open novice guide msg window"+msg.ToString());
		//		Debug.LogError ("MsgWindowLogic : OpenMsgWindow ");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowMsg", msg);
		ExcuteCallback(cbdArgs);
	}
	
	void CloseGuideMsgWindow(object msg){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CloseMsg", msg);
		ExcuteCallback(cbdArgs);
	}

	void DestoryGuideMsgWindow(object data) {
		DestoryUI ();
	}
	
}