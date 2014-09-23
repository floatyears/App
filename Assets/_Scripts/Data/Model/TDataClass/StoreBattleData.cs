using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class StoreBattleData : ProtoBuf.IExtensible {

//	public RecoveBattleStep recoveBattleStep {
//		get { return instance.recoveBattleStep; }
//		set { instance.recoveBattleStep = value; }
//	}

	/// <summary>
	/// 0 == not battle, 1 == battle enemy, 2 == battle boss;
	/// </summary>
//	public int isBattle {
//		get { return instance.isBattle; }
//		set { instance.isBattle = value; }
//	}	

	public List<ClearQuestParam> QuestData {
		set { 
			_questData.Clear();
			_questData.AddRange(value);
		}
	}
	public List<EnemyInfo> EnemyInfo {
		set { 
			_enemyInfo.Clear();
			_enemyInfo.AddRange(value);
		}
	}

	public Coordinate roleCoordinate {
		get { return new Coordinate(xCoordinate,yCoordinate); }
		set { xCoordinate = value.x; yCoordinate = value.y; }
	}

	public void RemoveEnemyInfo (EnemyInfo ei) {
		enemyInfo.Remove (ei);
	}

	public ClearQuestParam GetLastQuestData(){
		return _questData[ _questData.Count > 0 ? (_questData.Count - 1) : 0 ];
	}
	
}
}

