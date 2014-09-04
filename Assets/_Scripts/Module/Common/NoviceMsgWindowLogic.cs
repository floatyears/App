using UnityEngine;
using System.Collections.Generic;

public class NoviceMsgWindowLogic : ModuleBase{
	public NoviceMsgWindowLogic(UIConfigItem config):base(  config){
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

	void AddListener(){https://files.slack.com/files-pri/T02FJAEE5-F02H47HB0/___qq______.png
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
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowMsg", msg);
		view.CallbackView("ShowMsg", msg);
	}
	
	void CloseGuideMsgWindow(object msg){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CloseMsg", msg);
		view.CallbackView("CloseMsg", msg);
	}

	void DestoryGuideMsgWindow(object data) {
		DestoryUI ();
	}
	
}