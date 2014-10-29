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
		public const int MAX_STAGE_ID = 57; //当前版本最后一个StageId
		public const int STAGE_COUNT_OF_CITY = 7; 


		public StageClearItem GetClearItem(ECopyType copyType) {
			return (copyType==ECopyType.CT_NORMAL) ? storyClear : eliteClear;
		}

		//返回最新可进入的CityId
		public uint GetNewestCity(ECopyType copyType) {
			StageClearItem clearInfo = GetClearItem(copyType);
			uint lastestCityId = ( clearInfo==null ? 0 : clearInfo.stageId/10 );
			if( lastestCityId==0 ) 
				lastestCityId = 1;

			if( GetStoryCityState(lastestCityId, copyType) == StageState.CLEAR ) 
				lastestCityId += 1; //当前City已通关，返回下一City

			return lastestCityId;
		}

		//返回最新可进入的StageId
		public uint GetNewestStage(ECopyType copyType) {
			StageClearItem clearInfo = GetClearItem(copyType);
			uint lastestStageId = (clearInfo==null ? 0 : clearInfo.stageId);
			if( lastestStageId==0 ) 
				lastestStageId = 11;
			if( GetStoryStageState(lastestStageId, copyType) == StageState.CLEAR ) 
				lastestStageId = nextStageId(lastestStageId); //当前Stage已通关，返回下一Stage
			
			return lastestStageId;
		}

		public uint nextStageId(uint stageId) {
			uint cityId = stageId / 10;
			if (stageId == MAX_STAGE_ID ) { //last stage of all
				return stageId;
			}

			if ( stageId % 10 == STAGE_COUNT_OF_CITY ) { // is last stage of city
				//get next city's first stage
				CityInfo cityinfo = DataCenter.Instance.QuestData.GetCityInfo(cityId+1);
				return cityinfo.stages[0].id;
			}

			return stageId+1;
		}

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
		public StageState GetStoryCityState(uint cityId, ECopyType copyType) {
			CityInfo cityinfo = DataCenter.Instance.QuestData.GetCityInfo(cityId);
			bool isClear = true;
				foreach( StageInfo stage in cityinfo.stages ) {
				if (!IsStoryStageClear(stage, copyType)){
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
				if (!IsStoryStageClear(stage, copyType)){
					prevIsClear = false;
					break;
				}
			}
			if(prevIsClear) return StageState.NEW;

			return StageState.LOCKED;

		}

			//return 0:locked  1:cleared 2: currentOpen
		public StageState GetStoryStageState(uint stageId, ECopyType copyType) {
			StageInfo stageinfo = DataCenter.Instance.QuestData.GetStageInfo(stageId);

			bool isClear = IsStoryStageClear(stageinfo, copyType);
			if ( isClear ) {
				return StageState.CLEAR;
			}

			// current isClear==false, but previous stage is Cleared.
			uint prevStage = prevStageId(stageId);
			if (prevStage == 0 || IsStoryStageClear( DataCenter.Instance.QuestData.GetStageInfo(prevStage), copyType ) )
				return StageState.NEW;

			return StageState.LOCKED;
		}

		private	bool IsStoryStageClear(StageInfo stageInfo, ECopyType copyType) {

			StageClearItem clearItem = (stageInfo.CopyType==ECopyType.CT_NORMAL) ? storyClear : eliteClear;
			if( stageInfo.CopyType != copyType ) {
				clearItem = (copyType==ECopyType.CT_NORMAL) ? storyClear : eliteClear;
			}

			if (clearItem == null || stageInfo == null) {
				return false;
			}

			if (stageInfo.id == clearItem.stageId) {
				//Last quest of stage is clear, so the stage is clear.
				return ( clearItem.questId >= stageInfo.QuestInfo[stageInfo.QuestInfo.Count-1].id );
			}

			return ( stageInfo.id < clearItem.stageId );
		}

		public	bool IsEventStageClear(StageInfo stageInfo) {
			if (eventClear == null) {
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

		public	bool IsStoryQuestClear(uint stageId, uint questId, ECopyType copyType) {
			StageClearItem clearItem = (copyType==ECopyType.CT_NORMAL) ? storyClear : eliteClear;
			if (clearItem == null) {
				return false;
			}
			if ( stageId < clearItem.stageId ) { 
				return true;
			} else if ( stageId == clearItem.stageId ) { 
				return ( questId <= clearItem.questId );
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

		public	void UpdateStoryQuestClear(uint stageId, uint questId, ECopyType copyType) {
			if (storyClear == null) {
				storyClear = new StageClearItem();
			}
			if (eliteClear == null) {
				eliteClear = new StageClearItem();
			}

			StageClearItem clearItem = (copyType==ECopyType.CT_NORMAL) ? storyClear : eliteClear;

			if( stageId > clearItem.stageId ) {
				clearItem.stageId = stageId;
				if(questId == 133){
					NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.NoviceGuideStepD_1;
				}else if(questId == 143){
					NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.NoviceGuideStepE_1;
				}
			}
				
			if ( questId > clearItem.questId ) {
				clearItem.questId = questId;
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
