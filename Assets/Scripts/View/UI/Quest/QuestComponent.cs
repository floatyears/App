using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class QuestComponent : ConcreteComponent{

	List<TCityInfo> storyCityList = new List<TCityInfo>();
	List<TStageInfo> storyStageList = new List<TStageInfo>();

	public QuestComponent(string uiName):base(uiName){}
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
		storyCityList.Add(DataCenter.Instance.GetCityInfo(1));
//		Debug.LogError("storyCityList(), storyStageList's count is " + storyCityList.Count);

		GetStoryStageList();
	}

	void GetStoryStageList(){
		for (int cityIndex = 0; cityIndex < storyCityList.Count; cityIndex++){
			TCityInfo tci = storyCityList[ cityIndex ];
//			Debug.LogError("tci id : " + tci.ID);
			//TODO
			for (int stageIndex = 0;  stageIndex < tci.Stages.Count; stageIndex ++) {
				TStageInfo tsi = tci.Stages[ stageIndex ];
				storyStageList.Add(tsi);
//				Debug.LogError("tsi id : " + tsi.ID);
			}
		}

//		Debug.LogError("GetStoryStageList(), storyStageList's count is " + storyStageList.Count);
	}

	void CreateStage(){
		CallBackDispatcherArgs storyArgs = new CallBackDispatcherArgs("CreateStoryView", storyCityList);
		ExcuteCallback(storyArgs);
	}

	void ClearStage(){
		storyCityList.Clear();
	}

	void TurnToSelectQuest(object args){
		TStageInfo stageSelected =args as TStageInfo ; 
		if(stageSelected == null) {
			return;
		}

//		Debug.LogError("TurnToSelectQuest(), selected Stage's index is " + stageSelected);
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
		MsgCenter.Instance.Invoke(CommandEnum.GetSelectedStage, stageSelected);
	}


}
