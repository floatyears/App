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

//		public List<ClearQuestParam> QuestData {
//			set { 
//				_questData.Clear();
//				_questData.AddRange(value);
//			}
//		}

		public StoreBattleData(){
			roleCoordinate = new Coordinate (2, 0);
		}
		public List<EnemyInfo> EnemyInfo {
			set { 
				if(_enemyInfo == value)
					return;
				_enemyInfo.Clear();
				_enemyInfo.AddRange(value);
			}
			get{
				return _enemyInfo;
			}
		}

		public Coordinate roleCoordinate {
			get { return new Coordinate(xCoordinate,yCoordinate); }
			set { xCoordinate = value.x; yCoordinate = value.y; }
		}

		public ClearQuestParam GetLastQuestData(){
			if (_questData.Count == 0) {
				_questData.Add(new ClearQuestParam());
			}
			return _questData[(_questData.Count - 1)];
		}
	
	}
}

