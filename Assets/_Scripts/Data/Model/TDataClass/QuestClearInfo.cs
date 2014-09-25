using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageState {
	LOCKED = 0,
	CLEAR = 1,
	NEW = 2,
	EVENT_OPEN = 3,
	EVENT_CLOSE = 4
}

namespace bbproto{


	public partial class QuestClearInfo : ProtobufDataBase {

	    //// property ////
		public	StageClearItem			StoryClear { get { return this.storyClear; } }
		public	List<StageClearItem>	EventClear { get { return this.eventClear; } }

		public uint prevStageId(uint stageId) {
			uint cityId = stageId / 10;
			if ( stageId % 10 == 1 ) { // is frist stage of city
				if (cityId==1) { //first city
					return 0;
				}
				//get prev city's last stage
				CityInfo cityinfo = DataCenter.Instance.QuestData.GetCityInfo(cityId-1);
				return cityinfo.stages[cityinfo.stages.Count-1].id;
			}

			return stageId-1;
		}

		//return 0:locked  1:cleared 2: currentOpen
		public StageState GetStoryCityState(uint cityId) {
			CityInfo cityinfo = DataCenter.Instance.QuestData.GetCityInfo(cityId);
			bool isClear = true;
				foreach( StageInfo stage in cityinfo.stages ) {
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
			CityInfo prevCity = DataCenter.Instance.QuestData.GetCityInfo(cityId-1);
			bool prevIsClear = true;
				foreach( StageInfo stage in prevCity.stages ) {
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
			StageInfo stageinfo = DataCenter.Instance.QuestData.GetStageInfo(stageId);

			bool isClear = IsStoryStageClear(stageinfo);
			if ( isClear ) {
				return StageState.CLEAR;
			}

			// current isClear==false, but previous stage is Cleared.
			uint prevStage = prevStageId(stageId);
			if (prevStage == 0 || IsStoryStageClear( DataCenter.Instance.QuestData.GetStageInfo(prevStage) ) )
				return StageState.NEW;

			return StageState.LOCKED;
		}

		public	bool IsStoryStageClear(StageInfo stageInfo) {
			if (StoryClear == null || stageInfo == null) {
				return false;
			}

			if (stageInfo.id == StoryClear.stageId) {
				//Last quest of stage is clear, so the stage is clear.
				return ( StoryClear.questId >= stageInfo.QuestInfo[stageInfo.QuestInfo.Count-1].id );
			}

			return ( stageInfo.id < StoryClear.stageId );
		}

		public	bool IsEventStageClear(StageInfo stageInfo) {
			if (EventClear == null) {
				return false;
			}

			foreach(StageClearItem item in this.eventClear) {
				if ( item.stageId == stageInfo.id ) { 
					//Last quest of stage is clear, so the stage is clear.
					bool isClear = ( item.questId == stageInfo.QuestInfo[stageInfo.QuestInfo.Count-1].id );
					return isClear;
				}
			}
			
			return false;
		}

		public	bool IsStoryQuestClear(uint stageId, uint questId) {
			if (StoryClear == null) {
				return false;
			}
			if ( stageId < StoryClear.stageId ) { 
				return true;
			} else if ( stageId == StoryClear.stageId ) { 
				return ( questId <= StoryClear.questId );
			}

			return false;
		}

		public	bool IsEventQuestClear(uint stageId, uint questId) {
			if (this.eventClear == null) {
				return false;
			}

			foreach(StageClearItem item in this.eventClear) {
				if ( item.stageId == stageId ) { 
					return ( questId <= item.questId);
				}
			}

			return false;
		}

		public	void UpdateStoryQuestClear(uint stageId, uint questId) {
				if (storyClear == null) {
					storyClear = new StageClearItem();			
				}
			if( stageId > storyClear.stageId ) {
				storyClear.stageId = stageId;
			}
				
			if ( questId > storyClear.questId ) {
				storyClear.questId = questId;
			}
			if (stageId >= 17 && questId >= 175) {
				GameDataPersistence.Instance.StoreData("ResrouceDownload","Start");
			}
		}

		public	void UpdateEventQuestClear(uint stageId, uint questId) {
			foreach(StageClearItem item in this.eventClear) {
				if ( item.stageId == stageId ) { //found exists stageId, update the lastest cleared questId.
					if (questId > item.questId)
						item.questId = questId;
					return;
				}
			}

			// not found old stageId, so create new one & append it.
			StageClearItem sci = new StageClearItem();
			sci.stageId = stageId;
			sci.questId = questId;
			this.eventClear.Add(sci);
		}
	}
}
