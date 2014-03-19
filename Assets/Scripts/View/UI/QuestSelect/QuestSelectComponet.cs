using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectComponent : ConcreteComponent{
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
		TStageInfo tsi = data as TStageInfo;
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateQuestList", tsi);
		ExcuteCallback(cbdArgs);
	}


	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "ClickFriendSelect" : 
				CallBackDispatcherHelper.DispatchCallBack(TurnToFriendSelect, cbdArgs);
				break;
 			default:
				break;
		}
	}

	void TurnToFriendSelect(object args){
		//change scene to friendSelect
	}
}
