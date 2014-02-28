using bbproto;
using UnityEngine;
using System.Collections;

//public class TempEnemySortByHP : IComparer {
//	public int Compare (object x, object y)
//	{
//		TEnemyInfo tex = (TEnemyInfo)x;
//		TEnemyInfo tey = (TEnemyInfo)y;
//		return tex.GetBlood ().CompareTo (tey.GetBlood ());
//	}
//}

public class ConfigEnermy {
	public ConfigEnermy() {
		GenerateEnemy ();
	}
	void GenerateEnemy () {
		EnemyInfo ei = new EnemyInfo ();
		ei.enemyId = 1;
		ei.unitId = 1;
		ei.attack = 200;
		ei.nextAttack = 1;
		ei.defense = 10;
		ei.hp = 500;
		ei.type = (EUnitType)1;
		TEnemyInfo te = new TEnemyInfo (ei);
		GlobalData.enemyInfo.Add (ei.enemyId,te);
		
		ei = new EnemyInfo ();
		ei.enemyId = 2;
		ei.unitId = 2;
		ei.attack = 20;
		ei.nextAttack = 1;
		ei.defense = 100;
		ei.hp = 500;
		ei.type = (EUnitType)2;
		te = new TEnemyInfo (ei);
		GlobalData.enemyInfo.Add (ei.enemyId,te);
		
		ei = new EnemyInfo();
		ei.enemyId = 3;
		ei.unitId = 3;
		ei.attack = 500;
		ei.defense = 100;
		ei.type = EUnitType.UNONE;
		ei.hp = 1000;
		ei.nextAttack = 1;
		te = new TEnemyInfo (ei);
		GlobalData.enemyInfo.Add (ei.enemyId,te);

		ei = new EnemyInfo();
		ei.enemyId = 4;
		ei.unitId = 4;
		ei.attack = 500;
		ei.defense = 100;
		ei.type = EUnitType.UNONE;
		ei.hp = 1000;
		ei.nextAttack = 1;
		te = new TEnemyInfo (ei);
		GlobalData.enemyInfo.Add (ei.enemyId,te);

		ei = new EnemyInfo();
		ei.enemyId = 5;
		ei.unitId = 5;
		ei.attack = 500;
		ei.defense = 100;
		ei.type = EUnitType.UNONE;
		ei.hp = 1000;
		ei.nextAttack = 1;
		te = new TEnemyInfo (ei);
		GlobalData.enemyInfo.Add (ei.enemyId,te);
	}
}