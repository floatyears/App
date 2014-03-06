using UnityEngine;
using System.Collections;
using bbproto;

public class TEnemyInfo : ProtobufDataBase {
	private EnemyInfo instance;

	public TEnemyInfo (EnemyInfo instance) : base (instance) {
		this.instance = instance;
		initBlood = GetInitBlood ();
		initAttackRound = instance.nextAttack;

		MsgCenter.Instance.AddListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.AddListener (CommandEnum.DeferAttackRound, DeferAttackRound);
	}

	public void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.RemoveListener (CommandEnum.DeferAttackRound, DeferAttackRound);
	}

	EnemyInfo EnemyInfo() {
		return instance;
	}

	private int initBlood = -1;
	private int initAttackRound = -1;
	public bool isDeferAttackRound = false;
	public bool isPosion = false;


	public bool IsInjured () {
		if (EnemyInfo ().hp < initBlood) { return true; }
		else { return false; }
	}

	public int CalculateInjured (AttackInfo attackInfo, bool restraint) {
		float injured = 0;
		bool ignoreDefense = attackInfo.IgnoreDefense;
		int unitType = attackInfo.AttackType;
		float attackvalue = attackInfo.AttackValue;
		if (restraint) {
			injured = attackvalue * 2;
		} 
		else {
			int beRestraint = DGTools.BeRestraintType(unitType);
			if(beRestraint == (int)EnemyInfo().type) {
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
		if (initBlood < 0) {
			initBlood = 0;	
		}
		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
	}

	public void Reset() {
		initBlood = GetInitBlood ();
		initAttackRound = EnemyInfo ().nextAttack;
	}

	public void ResetAttakAround () {
		isDeferAttackRound = false;
		initAttackRound = EnemyInfo().nextAttack;
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

	public int AttackValue{
		get{
			return EnemyInfo().attack;
		}
	}

	private uint enemySymbol = 0;
	public uint EnemySymbol {
		get{ return enemySymbol; }
		set{ enemySymbol = value; }
	}

	public uint EnemyID {
		get{
			return EnemyInfo().enemyId;
		}
	}

	public uint UnitID{
		get{
			return EnemyInfo ().unitId;
		}
	}

	public int GetDefense () {
		int defense = EnemyInfo ().defense;
		defense = defense - System.Convert.ToInt32 (defense * reduceProportion);
		return defense;
	}

	public int GetRound () {
		return initAttackRound;
	}

	public int GetInitBlood () {
		return EnemyInfo ().hp;
	}

	public int GetBlood () {
		return initBlood;
	}

	public int DropUnit () {
		return -1;
	}

	public int GetUnitType () {
		return (int)EnemyInfo ().type;
	}
}

public class EnemySortByHP : IComparer {
	public int Compare (object x, object y)
	{
		TEnemyInfo tex = (TEnemyInfo)x;
		TEnemyInfo tey = (TEnemyInfo)y;
		return tex.GetBlood ().CompareTo (tey.GetBlood ());
	}
}
