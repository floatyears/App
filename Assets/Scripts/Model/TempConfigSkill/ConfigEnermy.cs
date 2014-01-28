using UnityEngine;
using System.Collections;
using bbproto;

public class TempEnemy : ProtobufDataBase {
	public TempEnemy (object instance) : base (instance) {

	}

	EnemyInfo GetEnemyInfo() {
		return DeserializeData<EnemyInfo> ();
	}

	private int initBlood = -1;
	private int initAttackRound = -1;

	public bool IsInjured () {
		if (GetEnemyInfo ().hp < initBlood) {
			return true;
		}
		else {
			return false;
		}
	}

	public int CalculateInjured (float attackInfo, bool restraint, int unitType) {
		int injured = 0;
		if (restraint) {
			injured = (int)(attackInfo * 2 - GetDefense ());
		} 
		else {
			int beRestraint = DGTools.BeRestraintType(unitType);
			if(beRestraint > 0) {
				injured = (int) (attackInfo * 0.5f - GetDefense());
			}
			else{
				injured = (int)(attackInfo - GetDefense());
			}
		}
		initBlood -= injured;
		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
		return injured;
	}

	public void Reset() {
		initBlood = GetEnemyInfo ().hp;
		initAttackRound = GetEnemyInfo ().attackRound;
	}

	public void ResetAttakAround () {
		initAttackRound = GetEnemyInfo().attackRound;
	}

	public void Next () {
		initAttackRound --;
	}

	public int GetAttack () {
		return GetEnemyInfo().attack;
	}

	public int GetID () {
		return GetEnemyInfo().unitId;
	}

	public int GetDefense () {
		return GetEnemyInfo ().defense;
	}

	public int GetRound () {
		return initAttackRound;
	}

	public int GetBlood () {
		return initBlood;
	}

	public int DropUnit () {
		return -1;
	}

	public int GetUnitType () {
		return GetEnemyInfo ().type;
	}
}

public class TempEnemySortByHP : IComparer {
	public int Compare (object x, object y)
	{
		TempEnemy tex = (TempEnemy)x;
		TempEnemy tey = (TempEnemy)y;
		return tex.GetBlood ().CompareTo (tey.GetBlood ());
	}
}

public class ConfigEnermy {
	public ConfigEnermy() {
		GenerateEnemy ();
	}

	void GenerateEnemy () {
		EnemyInfo ei = new EnemyInfo ();
		ei.unitId = 1;
		ei.attack = 400;
		ei.attackRound = 1;
		ei.defense = 10;
		ei.hp = 400;
		ei.type = 1;
		ei.dropUnitId = 10;
		ei.dropRate = 0.15f;
		TempEnemy te = new TempEnemy (ei);
		GlobalData.tempEnemyInfo.Add (ei.unitId,te);

		ei = new EnemyInfo ();
		ei.unitId = 2;
		ei.attack = 800;
		ei.attackRound = 1;
		ei.defense = 20;
		ei.hp = 1500;
		ei.type = 2;
		ei.dropUnitId = 11;
		ei.dropRate = 0.2f;
		te = new TempEnemy (ei);
		GlobalData.tempEnemyInfo.Add (ei.unitId,te);
	}
}
