using UnityEngine;
using System.Collections;
using bbproto;

public class TempEnemy : ProtobufDataBase {
	public TempEnemy (object instance) : base (instance) {
		MsgCenter.Instance.AddListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.AddListener (CommandEnum.DeferAttackRound, DeferAttackRound);
	}

	public void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.RemoveListener (CommandEnum.DeferAttackRound, DeferAttackRound);
	}

	EnemyInfo GetEnemyInfo() {
		return DeserializeData<EnemyInfo> ();
	}

	private int initBlood = -1;
	private int initAttackRound = -1;

	public bool isDeferAttackRound = false;
	public bool isPosion = false;

	public bool IsInjured () {
		if (GetEnemyInfo ().hp < initBlood) {
			return true;
		}
		else {
			return false;
		}
	}

	public int CalculateInjured (AttackInfo attackInfo, bool restraint) {//, int unitType, bool ignoreDefense = false) {
		float injured = 0;
		bool ignoreDefense = attackInfo.IgnoreDefense;
		int unitType = attackInfo.AttackType;
		float attackvalue = attackInfo.AttackValue;
		if (restraint) {
			injured = attackvalue * 2;
		} 
		else {
			int beRestraint = DGTools.BeRestraintType(unitType);
			if(beRestraint == (int)GetEnemyInfo().type) {
				injured = attackvalue * 0.5f;
			}
			else{
				injured = attackvalue;
			}
		}
		if (!ignoreDefense) {
			injured -= GetDefense();
		}
		if (injured < 1) {
			injured = 1;
		}
		int value = System.Convert.ToInt32 (injured);
		KillHP (value);
		return value;
	}

	float reduceProportion = 0f;
	public void ReduceDefense(float value) {
		reduceProportion = value;
	}

	void SkillPosion(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		int value = System.Convert.ToInt32 (ai.AttackValue);
		KillHP (value);
	}

	public void KillHP(int hurtValue) {
		initBlood -= hurtValue;
		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
	}

	public void Reset() {
		initBlood = GetInitBlood ();
		initAttackRound = GetEnemyInfo ().nextAttack;
	}

	public void ResetAttakAround () {
		isDeferAttackRound = false;
		initAttackRound = GetEnemyInfo().nextAttack;
	}

	public void Next () {
		initAttackRound --;
	}

	void DeferAttackRound(object data) {
		int value = (int)data;
		if (initBlood > 0) {
			initAttackRound += value;
			isDeferAttackRound = true;
			MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
		}
	}

	public int GetAttack () {
		return GetEnemyInfo().attack;
	}

	public uint GetID () {
		return GetEnemyInfo().unitId;
	}

	public int GetDefense () {
		int defense = GetEnemyInfo ().defense;
		defense = defense - System.Convert.ToInt32 (defense * reduceProportion);
		return defense;
	}

	public int GetRound () {
		return initAttackRound;
	}

	public int GetInitBlood () {
		return GetEnemyInfo ().hp;
	}

	public int GetBlood () {
		return initBlood;
	}

	public int DropUnit () {
		return -1;
	}

	public int GetUnitType () {
		return (int)GetEnemyInfo ().type;
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
		ei.attack = 200;
		ei.nextAttack = 1;
		ei.defense = 100;
		ei.hp = 500;
		ei.type = (EUnitType)1;
		TempEnemy te = new TempEnemy (ei);
		GlobalData.tempEnemyInfo.Add (ei.unitId,te);

		ei = new EnemyInfo ();
		ei.unitId = 2;
		ei.attack = 300;
		ei.nextAttack = 1;
		ei.defense = 100;
		ei.hp = 500;
		ei.type = (EUnitType)2;
		te = new TempEnemy (ei);
		GlobalData.tempEnemyInfo.Add (ei.unitId,te);

		ei = new EnemyInfo();
		ei.unitId = 3;
		ei.attack = 500;
		ei.defense = 100;
		ei.type = EUnitType.UNONE;
		ei.hp = 1000;
		ei.nextAttack = 1;
		te = new TempEnemy (ei);
		GlobalData.tempEnemyInfo.Add (ei.unitId,te);
	}
}
