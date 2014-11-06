using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
	public partial class CopyPassInfo : ProtoBuf.IExtensible {
		public ECopyType CopyType;

		public int GetQuestStar(uint stageId, uint questId) {
			StageState state = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryQuestState(stageId, questId, CopyType);
			if(state != StageState.CLEAR) {
				return 0;
			}

			foreach(QuestStarObj q in questStarList)  {
				if (q.questId == questId) {
					return q.star;
				}
			}
			return 3;
		}
		
		public int GetStageStar(uint stageId) {
			int stageStar = 3;

			StageState state = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState(stageId, this.CopyType);
			if ( state == StageState.LOCKED) 
				return 0;


			StageInfo stageInfo = DataCenter.Instance.QuestData.GetStageInfo(stageId);

			int clearQuestCount = 0;
			foreach(QuestInfo quest in stageInfo.quests ) {
				if( DataCenter.Instance.QuestData.QuestClearInfo.GetStoryQuestState(stageId, quest.id, this.CopyType) == StageState.CLEAR) {
					clearQuestCount += 1;
				}

				foreach(QuestStarObj q in questStarList)  {
					if (q.questId == quest.id) {
						if( q.star < stageStar)
							stageStar = q.star;
						break;
					}
				}
			}

			if ( clearQuestCount < stageInfo.quests.Count ) {
				if ( clearQuestCount > 0 )
					return 1; //star 1
				else 
					return 0;
			}

			return stageStar;
		}

		public void UpdateQuestStar(uint questId, int curStar) {
			bool isExists = false;
			foreach(QuestStarObj q in questStarList)  {
				if (q.questId == questId) {
					isExists = true;
					if( q.star < curStar )
						q.star = curStar;
					break;
				}
			}

			if(!isExists) {
				QuestStarObj starInfo=new QuestStarObj();
				starInfo.questId = questId;
				starInfo.star = curStar;
				questStarList.Add(starInfo);
			}
		}

		//标记为已领奖
		public void SetAcceptBonus(uint stageId) {
			this.stageIdList.Remove( stageId );
			this.acceptBonusStageList.Add( stageId );
		}

		public bool IsBonusAccept(uint stageId) {
			foreach(var sId in this.acceptBonusStageList ) {
				if( sId == stageId ) {
					return true;
				}
			}
			return false;
		}
	}
}

