using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TQuestInfo : ProtobufDataBase {
	private QuestInfo instance;
	public TQuestInfo (QuestInfo qi) : base (qi) {
		instance = qi;
	}

	public QuestInfo questInfo {
		get { return instance; }
	}

	public uint ID {
		get { return instance.id; }
	}

	public EQuestState state {
		get { return instance.state; }
	}

	public int No {
		get { return instance.no; }
	}

	public string Name {
		get { return instance.name; }
	}

	public string Story {
		get { return instance.story; }
	}

	public int Stamina {
		get { return instance.stamina; }
	}

	public int Floor {
		get { return instance.floor; }
	}
	public int RewardExp {
		get { return instance.rewardExp; }
	}

	public int RewardMoney {
		get { return instance.rewardMoney; }
	}

	public List<uint> BossID {
		get { return instance.bossId; }
	}

	public List<uint> EnemyID {
		get { return instance.enemyId; }
	}

	public Position Pos {
		get { return instance.pos; }
	}
}
