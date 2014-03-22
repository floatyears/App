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

	public TStageInfo (StageInfo si) : base (si) {
		instance = si;
		InitQuestInfo (si);
	}

	void InitQuestInfo (StageInfo si) {
		questInfo = new List<TQuestInfo> ();
		
		for (int i = 0; i < si.quests.Count; i++) {
			TQuestInfo tqi = new TQuestInfo(instance.quests[i]);
			questInfo.Add(tqi);
		}
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
		get { return instance.startTime; }
	}

	public uint endTime {
		get { return instance.endTime; }
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
