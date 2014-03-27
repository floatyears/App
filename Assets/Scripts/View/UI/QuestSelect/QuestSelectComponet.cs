using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectComponent : ConcreteComponent{
	TStageInfo currentStageInfo;
	int currentQuestIndex;

	private TEvolveStart evolveStageInfo;

	public QuestSelectComponent(string uiName):base(uiName){
        MsgCenter.Instance.AddListener(CommandEnum.ChangeScene, ResetUI);
    }
	
	public override void CreatUI(){

		base.CreatUI();

	}
	
	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedStage, SelectedStage);
		MsgCenter.Instance.AddListener (CommandEnum.EvolveStart, EvolveStartQuest);
	}
	
	public override void HideUI() {
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedStage, SelectedStage);
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
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		int index = (int)args;
		currentQuestIndex = index;
		Dictionary<string, object> info = new Dictionary<string, object>();
		info.Add("position", index);
		info.Add("data", currentStageInfo);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowInfoPanel",  info);
		ExcuteCallback(cbdArgs);
	}
	
	void TurnToFriendSelect(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		bool b = (bool)args;
		TStageInfo tsi = null;
		uint questID = 0;

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

    void ResetUI(object args){
        SceneEnum nextScene = (SceneEnum)args;
        LogHelper.Log("ResetUI(), currentScene {0}, nextScene {1}", UIManager.Instance.baseScene.CurrentScene, nextScene);
        if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.FriendSelect && nextScene == SceneEnum.QuestSelect){
            LogHelper.Log("QuestSelect ResetUI(), no reset");
            return;
        }

        if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.QuestSelect && nextScene == SceneEnum.FriendSelect){
            LogHelper.Log("QuestSelect ResetUI(), no reset");
            return;
        }
        QuestSelectDecoratorUnity view = viewComponent as QuestSelectDecoratorUnity;
        view.ResetUIWhenChange();
    }

}
