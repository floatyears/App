using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestModule : ModuleBase{
	private List<TCityInfo> storyCityList = new List<TCityInfo>();
	private List<TStageInfo> storyStageList = new List<TStageInfo>();

	public QuestModule(UIConfigItem config):base(  config){
//		CreateUI<quevi
	}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
		ClearStage();

		base.DestoryUI();
	}

//	public override void DestoryUI ()
//	{
//		base.DestoryUI ();
//	}

	public override void OnReceiveMessages(object data){
		base.OnReceiveMessages(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "ClickStoryItem" : 
				CallBackDispatcherHelper.DispatchCallBack(TurnToSelectQuest, cbdArgs);
				break;
			default:
				break;
		}
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
		view.CallbackView(storyArgs);
	}

	void ClearStage(){
		storyCityList.Clear();
	}

	void TurnToSelectQuest(object args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TStageInfo stageSelected =args as TStageInfo ; 
		if (stageSelected != null) {

				ConfigBattleUseData.Instance.currentStageInfo = stageSelected;
				ModuleManger.Instance.ShowModule(ModuleEnum.StageSelectModule);
				MsgCenter.Instance.Invoke(CommandEnum.GetSelectedStage, stageSelected);

		}

	}


}
