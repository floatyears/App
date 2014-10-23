using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
	public partial class CopyPassInfo : ProtoBuf.IExtensible {
		
		public int GetQuestStar(uint questId) {
			foreach(QuestStarObj q in questStarList)  {
				if (q.questId == questId)
					return q.star;
			}
			return 3;
		}
		
		public int GetStageStar(uint stageId) {
			int stageStar = 3;
			StageInfo stageInfo = DataCenter.Instance.QuestData.GetStageInfo(stageId);

			StageState state = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState(stageId, stageInfo.CopyType);
			if ( state == StageState.LOCKED) 
				return 0; 
			
			foreach(QuestInfo quest in stageInfo.quests ) {
				foreach(QuestStarObj q in questStarList)  {
					if (q.questId == quest.id) {
						if( q.star < stageStar)
							stageStar = q.star;
						break;
					}
				}
			}
			
			return stageStar;
		}
		
	}
}

