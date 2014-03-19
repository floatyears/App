using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectComponent : ConcreteComponent{
	TStageInfo currentStageInfo;
	public QuestSelectComponent(string uiName):base(uiName){}
	
	public override void CreatUI(){
		base.CreatUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedStage, SelectedStage);
	}
	
	public override void HideUI()
	{
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedStage, SelectedStage);
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}

	void SelectedStage(object data) {
		currentStageInfo = data as TStageInfo;
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateQuestList", currentStageInfo);
		ExcuteCallback(cbdArgs);
	}


	public override void Callback(object data){
		base.Callback(data);
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
		int index = (int)args;
		Dictionary<string, object> info = new Dictionary<string, object>();
		info.Add("position", index);
		info.Add("data", currentStageInfo);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowInfoPanel",  info);
		ExcuteCallback(cbdArgs);
		Debug.LogError("ShowQuestInfo(), index is " + index);
	}


	void TurnToFriendSelect(object args){
		//change scene to friendSelect
	}
}
