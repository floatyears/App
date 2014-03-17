using UnityEngine;
using System.Collections;
using bbproto;

public class TQuestInfo : ProtobufDataBase {
	private QuestInfo instance;
	public TQuestInfo (QuestInfo qi) : base (qi) {
		instance = qi;
	}

	public QuestInfo questInfo {
		get {return instance;}
	}


}
