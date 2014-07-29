using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TStageClearItem : ProtobufDataBase {
	private StageClearItem	instance;
	public TStageClearItem(StageClearItem inst) : base (inst) { 
		instance = inst;
	}

	//// property ////
	public	uint	StageId { get { return instance.stageId; } set {instance.stageId = value;} }
	public	uint	QuestId { get { return instance.questId; } set {instance.questId = value;} }
}

public enum StageState {
	LOCKED = 0,
	CLEAR = 1,
	NEW = 2,
	EVENT_OPEN = 3,
	EVENT_CLOSE = 4
}

public class TQuestClearInfo : ProtobufDataBase {
	private QuestClearInfo	instance ;
	private TStageClearItem storyClear;
	private List<TStageClearItem> eventClear = new List<TStageClearItem>();

	public TQuestClearInfo(QuestClearInfo inst) : base (inst) { 
        instance = inst;
		if (instance.storyClear == null) {
			instance.storyClear = new StageClearItem();
        }

		storyClear = new TStageClearItem(instance.storyClear);
	}


    //// property ////
	public	TStageClearItem			StoryClear { get { return this.storyClear; } }
	public	List<TStageClearItem>	EventClear { get { return this.eventClear; } }

	public uint prevStageId(uint stageId) {
		uint cityId = stageId / 10;
		if ( stageId % 10 == 1 ) { // is frist stage of city
			if (cityId==1) { //first city
				return 0;
			}
			//get prev city's last stage
			TCityInfo cityinfo = DataCenter.Instance.GetCityInfo(cityId-1);
			return cityinfo.Stages[cityinfo.Stages.Count-1].ID;
		}

		return stageId-1;
	}

	//return 0:locked  1:cleared 2: currentOpen
	public StageState GetStoryCityState(uint cityId) {
		TCityInfo cityinfo = DataCenter.Instance.GetCityInfo(cityId);
		bool isClear = true;
		foreach( TStageInfo stage in cityinfo.Stages ) {
			if (!IsStoryStageClear(stage)){
				isClear = false;
				break;
			}
		}
		if(isClear) 
			return StageState.CLEAR;

		if(!isClear && cityId==1) 
			return StageState.NEW;

		//curr cityId is not clear, check prevCity
		TCityInfo prevCity = DataCenter.Instance.GetCityInfo(cityId-1);
		bool prevIsClear = true;
		foreach( TStageInfo stage in prevCity.Stages ) {
			if (!IsStoryStageClear(stage)){
				prevIsClear = false;
				break;
			}
		}
		if(prevIsClear) return StageState.NEW;

		return StageState.LOCKED;

	}

		//return 0:locked  1:cleared 2: currentOpen
	public StageState GetStoryStageState(uint stageId) {
		TStageInfo stageinfo = DataCenter.Instance.GetStageInfo(stageId);

		bool isClear = IsStoryStageClear(stageinfo);
		if ( isClear ) {
			return StageState.CLEAR;
		}

		// current isClear==false, but previous stage is Cleared.
		uint prevStage = prevStageId(stageId);
		if (prevStage == 0 || IsStoryStageClear( DataCenter.Instance.GetStageInfo(prevStage) ) )
			return StageState.NEW;

		return StageState.LOCKED;
	}

	public	bool IsStoryStageClear(TStageInfo stageInfo) {
		if (StoryClear == null || stageInfo == null) {
			return false;
		}

		if (stageInfo.ID == StoryClear.StageId) {
			//Last quest of stage is clear, so the stage is clear.
			bool isClear = ( StoryClear.QuestId == stageInfo.QuestInfo[stageInfo.QuestInfo.Count-1].ID );
			return isClear;
		}

		return ( stageInfo.ID < StoryClear.StageId );
	}

	public	bool IsEventStageClear(TStageInfo stageInfo) {
		if (EventClear == null) {
			return false;
		}

		foreach(TStageClearItem item in this.eventClear) {
			if ( item.StageId == stageInfo.ID ) { 
				//Last quest of stage is clear, so the stage is clear.
				bool isClear = ( item.QuestId == stageInfo.QuestInfo[stageInfo.QuestInfo.Count-1].ID );
				return isClear;
			}
		}
		
		return false;
	}

	public	bool IsStoryQuestClear(uint stageId, uint questId) {
		if (StoryClear == null) {
			return false;
		}
		if ( stageId < StoryClear.StageId ) { 
			return true;
		} else if ( stageId == StoryClear.StageId ) { 
			return ( questId <= StoryClear.QuestId );
		}

		return false;
	}

	public	bool IsEventQuestClear(uint stageId, uint questId) {
		if (this.eventClear == null) {
			return false;
		}

		foreach(TStageClearItem item in this.eventClear) {
			if ( item.StageId == stageId ) { 
				return ( questId <= item.QuestId);
			}
		}

		return false;
	}

	public	void UpdateStoryQuestClear(uint stageId, uint questId) {
		if( stageId > storyClear.StageId ) {
			storyClear.StageId = stageId;
		}
			
		if ( questId > storyClear.QuestId ) {
			storyClear.QuestId = questId;
		}
	}

	public	void UpdateEventQuestClear(uint stageId, uint questId) {
		foreach(TStageClearItem item in this.eventClear) {
			if ( item.StageId == stageId ) { //found exists stageId, update the lastest cleared questId.
				if (questId > item.QuestId)
					item.QuestId = questId;
				return;
			}
		}

		// not found old stageId, so create new one & append it.
		StageClearItem sci = new StageClearItem();
		sci.stageId = stageId;
		sci.questId = questId;
		TStageClearItem newItem = new TStageClearItem(sci);
		this.eventClear.Add(newItem);
	}
}
