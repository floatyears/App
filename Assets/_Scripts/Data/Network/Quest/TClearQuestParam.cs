using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TClearQuestParam : ProtobufDataBase {
	public TClearQuestParam(ClearQuestParam ins) : base (ins) {
		instance = ins;
	} 

	public ClearQuestParam instance;

	public uint questId {
		get { return instance.questID; }
		set { instance.questID = value; }
	}

	public int getMoney {
		get { return instance.getMoney; }
		set { instance.getMoney = value; }
	}
	public List<uint> getUnit {
		get { return instance.getUnit; }
//		set { instance.getUnit = value; }
	}
	public List<uint> hitGrid  {
		get { return instance.hitGrid; }
//		set { instance.hitGrid = value; }
	}
//	public Coordinate currentCoor;
}
