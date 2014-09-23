﻿using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class StageInfo : ProtoBuf.IExtensible {

	private string stageId;
	
	public string StageId {
		get { return stageId; }
	}
	
	private List<QuestInfo> questInfo;

	public void InitQuestInfo (StageInfo si) {
		questInfo = new List<QuestInfo> ();

		for (int i = 0; i < si.quests.Count; i++) {
//			Debug.LogError("InitQuestInfo : " + si.quests[i].id);
			questInfo.Add(quests[i]);
		}
		validTime.Sort((Period x, Period y)=>{return x.startTime.CompareTo(y.startTime);});
	}

	public uint StartTime {
		get { 
			uint currentTime = GameTimer.GetInstance().GetCurrentSeonds();
			uint lastTime = 0;
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
		get {return questInfo;}
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