using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectController : ConcreteComponent{
	TStageInfo currentStageInfo;
	int currentQuestIndex;

	private TEvolveStart evolveStageInfo;

	public QuestSelectController(string uiName):base(uiName){
//        MsgCenter.Instance.AddListener(CommandEnum.ChangeScene, ResetUI);
    }
	
	public override void CreatUI(){
		base.CreatUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
//		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedStage, SelectedStage);
		MsgCenter.Instance.AddListener (CommandEnum.EvolveStart, EvolveStartQuest);
	}
	
	public override void HideUI() {
		base.HideUI();
//		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedStage, SelectedStage);
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveStart, EvolveStartQuest);
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}

	void SelectedStage(object data) {
		currentStageInfo = data as TStageInfo;
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateQuestList", currentStageInfo);
		ExcuteCallback(cbdArgs);
	}

	void EvolveStartQuest (object data) {
		evolveStageInfo = data as TEvolveStart;
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EvolveQuestList", evolveStageInfo.StageInfo);
		ExcuteCallback(cbdArgs);
	}


	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "ClickFriendSelect" : 
				CallBackDispatcherHelper.DispatchCallBack(TurnToFriendSelect, cbdArgs);
				break;
			case "ClickQuestItem" : 
				CallBackDispatcherHelper.DispatchCallBack(ShowQuestInfo, cbdArgs);
				break;
 			default:
				break;
		}
	}

	void ShowQuestInfo(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		int index = (int)args;
		currentQuestIndex = index;
//		Dictionary<string, object> info = new Dictionary<string, object>();
//		info.Add("position", index);
//		info.Add("data", currentStageInfo);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowInfoPanel",  info);
//		ExcuteCallback(cbdArgs);
	}

	
	//MsgWindow show, note stamina is not enough.
	bool CheckStaminaEnough(int staminaNeed, int staminaNow){
		if(staminaNeed > staminaNow) return true;
		else return false;
	}

	MsgWindowParams GetStaminaLackMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.Instace.GetCurrentText("StaminaLackNoteTitle");
		msgParams.contentText = TextCenter.Instace.GetCurrentText("StaminaLackNoteContent");
		msgParams.btnParam = new BtnParam();
		return msgParams;
	}

	void TurnToFriendSelect(object args){
		bool b = (bool)args;
		TStageInfo tsi = null;
		uint questID = 0;

		int staminaNeed = currentStageInfo.QuestInfo[ currentQuestIndex ].Stamina;
		int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
//		Debug.Log("TurnToFriendSelect()......staminaNeed is : " + staminaNeed);
//		Debug.Log("TurnToFriendSelect()......staminaNow is : " + staminaNow);
		if(CheckStaminaEnough(staminaNeed, staminaNow)){
			Debug.LogError("TurnToFriendSelect()......Stamina is not enough, MsgWindow show...");
			//MsgWindowShow
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
			return;
		}

		UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);
		if (b) {
			MsgCenter.Instance.Invoke( CommandEnum.EvolveSelectQuest, evolveStageInfo);
		}
		else {
			tsi = currentStageInfo;
			questID = tsi.QuestInfo[ currentQuestIndex ].ID;
			uint stageID = tsi.ID;
			Dictionary<string,uint> idArgs = new Dictionary<string, uint>();
			idArgs.Add("QuestID", questID);
			idArgs.Add("StageID", stageID);
			MsgCenter.Instance.Invoke( CommandEnum.GetSelectedQuest, idArgs);
		}
	}


}
