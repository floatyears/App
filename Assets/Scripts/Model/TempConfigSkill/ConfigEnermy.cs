using UnityEngine;
using System.Collections;
using bbproto;

public class TempEnemy : ProtobufDataBase {
	public TempEnemy (object instance) : base (instance) {

	}

	EnemyInfo GetEnemyInfo() {
		return DeserializeData<EnemyInfo> ();
	}

	public int GetID () {
		return GetEnemyInfo().unitId;
	}

	public int GetRound () {
		return GetEnemyInfo ().attackRound;
	}

	public int GetBlood () {
		return GetEnemyInfo ().hp;
	}

	public int DropUnit () {
		return -1;
	}
}

public class ConfigEnermy {

	public ConfigEnermy() {
		GenerateEnemy ();
	}

	void GenerateEnemy () {
		EnemyInfo ei = new EnemyInfo ();
		ei.unitId = 1;
		ei.attack = 10;
		ei.attackRound = 1;
		ei.defense = 10;
		ei.hp = 1000;
		ei.type = 1;
		ei.dropUnitId = 10;
		ei.dropRate = 0.15f;
		TempEnemy te = new TempEnemy (ei);
		GlobalData.tempEnemyInfo.Add (ei.unitId,te);

		ei = new EnemyInfo ();
		ei.unitId = 2;
		ei.attack = 20;
		ei.attackRound = 3;
		ei.defense = 20;
		ei.hp = 2000;
		ei.type = 2;
		ei.dropUnitId = 11;
		ei.dropRate = 0.2f;
		te = new TempEnemy (ei);
		GlobalData.tempEnemyInfo.Add (ei.unitId,te);
	}
}
