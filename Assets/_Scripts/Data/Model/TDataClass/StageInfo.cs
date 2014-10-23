using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class StageInfo : ProtoBuf.IExtensible {

	private string stageId;
	
	public string StageId {
		get { return stageId; }
	}
	
	public ECopyType CopyType = ECopyType.CT_NORMAL;

	private List<QuestInfo> questInfo;

//	public void InitQuestInfo (StageInfo si) {
//		questInfo = new List<QuestInfo> ();
//
//		for (int i = 0; i < si.quests.Count; i++) {
////			Debug.LogError("InitQuestInfo : " + si.quests[i].id);
//			questInfo.Add(quests[i]);
//		}
//		
//	}

	public uint StartTime {
		get { 
			uint currentTime = GameTimer.GetInstance().GetCurrentSeonds();
			uint lastTime = 0;
			//前提要求：服务器返回的validTime列表是按时间先后排序的。
			foreach (var item in validTime) {
				if(currentTime >= item.startTime){
					if(currentTime <= item.endTime){
//						break;
						return item.startTime;
					}
				}else if(currentTime > lastTime){
					return item.startTime;
//					break;
				}

				lastTime = item.endTime;
			}

			return 0;
		}
	}

	public uint EndTime {
		get { 
			uint currentTime = GameTimer.GetInstance().GetCurrentSeonds();
			uint lastTime = 0;
			//前提要求：服务器返回的validTime列表是按时间先后排序的。
			foreach (var item in validTime) {
				if(currentTime >= item.startTime){
					if(currentTime <= item.endTime){
						//						break;
						return item.endTime;
					}
				}else if(currentTime > lastTime){
					return item.endTime;
					//					break;
				}
				
				lastTime = item.endTime;
			}
			
			return 0;
		}
	}

	public int PosX {
		get { return pos.x; }
	}

	public int PosY {
			get { return pos.y; }
	}

	public List<QuestInfo> QuestInfo {
//		set{
//				validTime.Sort((Period x, Period y)=>{return x.startTime.CompareTo(y.startTime);});
//		}
		get {return quests;}
	}

	public void InitStageId(uint cityId){
		stageId = string.Format("{0}_{1}", cityId, id);
	}

	private uint questID;
	public uint QuestId {
		get { return questID; }
		set { questID = value; }
	}
}

}