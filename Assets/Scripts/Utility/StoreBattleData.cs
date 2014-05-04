using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TStoreBattleData : ProtobufDataBase {
	public StoreBattleData instance;
	public TStoreBattleData(StoreBattleData ins) : base (ins){
		instance = ins;
	}

	public int attackRound = 0;

	public bool HitKey = false;

	public int colorIndex {
		get { return instance.colorIndex; }
		set { instance.colorIndex = value; }
	}
	public int hp {
		get { return instance.hp; }
		set { instance.hp = value; }
	}
	public int sp {
		get { return instance.sp; }
		set { instance.sp = value; }//Debug.LogError("sp value : " + value);}
	}
	/// <summary>
	/// 0 == not battle, 1 == battle enemy, 2 == battle boss;
	/// </summary>
	public int isBattle {
		get { return instance.isBattle; }
		set { instance.isBattle = value; }
	}	

	public List<TClearQuestParam> questData {
		get { 
			List<TClearQuestParam> clear = new List<TClearQuestParam>();
			for (int i = 0; i < instance.questData.Count; i++) {
				TClearQuestParam tqp = new TClearQuestParam(instance.questData[i]);
				clear.Add(tqp);
			}
			return clear;
		}
		set { 
			instance.questData.Clear();
			for (int i = 0; i < value.Count; i++) {
				instance.questData.Add(value[i].instance);
			}
		}
	}
	public List<EnemyInfo> enemyInfo {
		get { 
			return instance.enemyInfo;
		}
		set { 
			instance.enemyInfo.Clear();
			for (int i = 0; i < value.Count; i++) {
				instance.enemyInfo.Add(value[i]);
			}
		}
	}

	public List<TEnemyInfo> tEnemyInfo {
		get {
			List<TEnemyInfo> temp = new List<TEnemyInfo>();
			for (int i = 0; i < instance.enemyInfo.Count; i++) {
				TEnemyInfo tei = new TEnemyInfo(instance.enemyInfo[i]);
				tei.EnemySymbol = (uint)i;
				temp.Add(tei);
			}
			return temp;
		}
		set {
			instance.enemyInfo.Clear();
			for (int i = 0; i < value.Count; i++) {
				instance.enemyInfo.Add(value[i].EnemyInfo());
//				Debug.LogError( value[i].EnemySymbol + " hp: " + instance.enemyInfo[i].currentHp + " next : " + instance.enemyInfo[i].currentNext);
			}
		}
	}

	public Coordinate roleCoordinate {
		get { return new Coordinate(instance.xCoordinate,instance.yCoordinate); }
		set { instance.xCoordinate = value.x; instance.yCoordinate = value.y; }
	}

	public void RemoveEnemyInfo (EnemyInfo ei) {
		enemyInfo.Remove (ei);
	}
}

