using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TStageInfo : ProtobufDataBase {
	private StageInfo instance;
	private List<TQuestInfo> questInfo;
	private string stageId;

	public string StageId {
		get { return stageId; }
	}

	public uint CityId {
		get { return instance.cityId; }
		set { instance.cityId = value; }
	}

	public TStageInfo (StageInfo si) : base (si) {
		instance = si;
		InitQuestInfo (si);
	}

	void InitQuestInfo (StageInfo si) {
		questInfo = new List<TQuestInfo> ();

		for (int i = 0; i < si.quests.Count; i++) {
//			Debug.LogError("InitQuestInfo : " + si.quests[i].id);
			TQuestInfo tqi = new TQuestInfo(instance.quests[i]);
			questInfo.Add(tqi);
		}
		instance.validTime.Sort((Period x, Period y)=>{return x.startTime.CompareTo(y.startTime);});
	}

	public StageInfo stageInfo {
		get { return instance;}
	}

	public uint ID {
		get {return instance.id;}
	}

	public EQuestState State {
		get {return instance.state;}
	}

	public bool IsClear {
		get {return (instance.state == EQuestState.QS_CLEARED);}
	}

	public QuestType Type {
		get {return instance.type; }
	}

	public string StageName {
		get { return instance.stageName; }
	}

	public string Description {
		get { return instance.description; }
	}

	public uint StartTime {
		get { 
			uint currentTime = GameTimer.GetInstance().GetCurrentSeonds();
			uint lastTime = 0;
			foreach (var item in instance.validTime) {
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

	public uint endTime {
		get { 
			uint currentTime = GameTimer.GetInstance().GetCurrentSeonds();
			uint lastTime = 0;
			foreach (var item in instance.validTime) {
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

	public List<bbproto.Period> ValidTime {
		get { return instance.validTime; }
	}

	public QuestBoost Boost {
		get { return instance.boost; }
	}

	public Position Pos {
		get { return instance.pos; }
	}

	public int PosX {
		get { return Pos.x; }
	}

	public int PosY {
		get { return Pos.y; }
	}

	public List<TQuestInfo> QuestInfo {
		get {return questInfo;}
	}

	public void InitStageId(uint cityId){
		stageId = string.Format("{0}_{1}", cityId, ID);
	}

	private uint questID;
	public uint QuestId {
		get { return questID; }
		set { questID = value; }
	}
}
