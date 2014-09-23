using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class HomeModule : ModuleBase{
	private List<CityInfo> storyCityList = new List<CityInfo>();
	private List<StageInfo> storyStageList = new List<StageInfo>();

	public HomeModule(UIConfigItem config):base(  config){
		CreateUI<HomeView> ();
	}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
		ClearStage();
	}

//	public override void DestoryUI ()
//	{
//		base.DestoryUI ();
//	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (data[0].ToString()){
			case "ClickStoryItem" : 
				TurnToSelectQuest(data[1]);
				break;
			default:
				break;
		}
	}


	void GetStoryStageList(){
		//Debug.LogError("QuestController.GetStoryStageList()......Story City Count : " + storyCityList.Count);
		for (int cityIndex = 0; cityIndex < storyCityList.Count; cityIndex++){
			CityInfo tci = storyCityList[ cityIndex ];
			for (int stageIndex = 0; stageIndex < tci.stages.Count; stageIndex++) {
				StageInfo tsi = tci.stages[ stageIndex ];
				storyStageList.Add(tsi);
				//Debug.LogError("tsi id : " + tsi.State.ToString());
			}
		}
		//Debug.LogError("QuestController.GetStoryStageList()......Story Stage Count is " + storyStageList.Count);
	}

	void CreateStage(){
//		CallBackDispatcherArgs storyArgs = new CallBackDispatcherArgs("CreateStoryView", storyCityList);
		view.CallbackView("CreateStoryView", storyCityList);
	}

	void ClearStage(){
		storyCityList.Clear();
	}

	void TurnToSelectQuest(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		StageInfo stageSelected =args as StageInfo ; 
		if (stageSelected != null) {

			BattleConfigData.Instance.currentStageInfo = stageSelected;
			ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",stageSelected);

		}

	}


}
