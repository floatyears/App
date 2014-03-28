using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class QuestController : ConcreteComponent{

	List<TCityInfo> storyCityList = new List<TCityInfo>();
	List<TStageInfo> storyStageList = new List<TStageInfo>();

	public QuestController(string uiName):base(uiName){}
	public override void CreatUI(){ base.CreatUI();}
	
	public override void ShowUI(){
		base.ShowUI();
		GetStoryCityList();
		CreateStage();
	}
	
	public override void HideUI(){
		base.HideUI();
		ClearStage();
	}

	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "ClickStoryItem" : 
				CallBackDispatcherHelper.DispatchCallBack(TurnToSelectQuest, cbdArgs);
				break;
			default:
				break;
		}
	}

	void GetStoryCityList(){
		uint nowCityIDForTemp = 1;
		storyCityList.Add(DataCenter.Instance.GetCityInfo(nowCityIDForTemp));
//		Debug.LogError("storyCityList(), storyStageList's count is " + storyCityList.Count);
		GetStoryStageList();
	}

	void GetStoryStageList(){
		//Debug.LogError("QuestController.GetStoryStageList()......Story City Count : " + storyCityList.Count);
		for (int cityIndex = 0; cityIndex < storyCityList.Count; cityIndex++){
			TCityInfo tci = storyCityList[ cityIndex ];
			for (int stageIndex = 0; stageIndex < tci.Stages.Count; stageIndex++) {
				TStageInfo tsi = tci.Stages[ stageIndex ];
				storyStageList.Add(tsi);
				//Debug.LogError("tsi id : " + tsi.State.ToString());
			}
		}
		//Debug.LogError("QuestController.GetStoryStageList()......Story Stage Count is " + storyStageList.Count);
	}

	void CreateStage(){
		CallBackDispatcherArgs storyArgs = new CallBackDispatcherArgs("CreateStoryView", storyCityList);
		ExcuteCallback(storyArgs);
	}

	void ClearStage(){
		storyCityList.Clear();
	}

	void TurnToSelectQuest(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TStageInfo stageSelected =args as TStageInfo ; 
		if(stageSelected == null) {
			return;
		}

//		Debug.LogError("TurnToSelectQuest(), selected Stage's index is " + stageSelected);
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
		MsgCenter.Instance.Invoke(CommandEnum.GetSelectedStage, stageSelected);
	}


}
