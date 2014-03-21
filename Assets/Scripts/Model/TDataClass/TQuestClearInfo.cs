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

public class TQuestClearInfo : ProtobufDataBase {
	private QuestClearInfo	instance;
	private TStageClearItem storyClear;
	private List<TStageClearItem> eventClear;

	public TQuestClearInfo(QuestClearInfo inst) : base (inst) { 
        instance = inst;
		if (instance.storyClear != null) {
			storyClear = new TStageClearItem(instance.storyClear);
        }
    }


    //// property ////
	public	TStageClearItem			StoryClear { get { return this.storyClear; } }
	public	List<TStageClearItem>	EventClear { get { return this.eventClear; } }

	public	bool IsStoryQuestClear(uint stageId, uint questId) {
		if ( stageId < StoryClear.StageId ) { 
			return true;
		} else if ( stageId == StoryClear.StageId ) { 
			return ( questId <= StoryClear.QuestId );
		}

		return false;
	}

	public	bool IsEventQuestClear(uint stageId, uint questId) {
		foreach(TStageClearItem item in this.eventClear) {
			if ( item.StageId == stageId ) { 
				return ( questId <= item.QuestId);
			}
		}

		return false;
	}

	public	void UpdateStoryQuestClear(uint stageId, uint questId) {
		storyClear.StageId = stageId;
		storyClear.QuestId = questId;
	}

	public	void UpdateEventQuestClear(uint stageId, uint questId) {
		foreach(TStageClearItem item in this.eventClear) {
			if ( item.StageId == stageId ) { //found exists stageId, update the lastest cleared questId.
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
