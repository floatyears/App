using UnityEngine;
using System.Collections;
using bbproto;

public class TEnemyInfo : ProtobufDataBase {
	private EnemyInfo instance;

	public TEnemyInfo (EnemyInfo instance) : base (instance) {
		this.instance = instance;
	}

	public void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.AddListener (CommandEnum.DeferAttackRound, DeferAttackRound);
	}

	public void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.RemoveListener (CommandEnum.DeferAttackRound, DeferAttackRound);
	}

	public EnemyInfo EnemyInfo() {
		return instance;
	}

	public int initBlood {
		get { return instance.currentHp; }
		set { instance.currentHp = value; }
	}

	public int initAttackRound {
		get { return instance.currentNext; }
		set { instance.currentNext = value; }
	}

	public bool isDeferAttackRound = false;
	private AttackInfo posionAttack;

	public TDropUnit drop;

	public bool IsInjured () {
		if (initBlood > 0 && initBlood < instance.hp) { return true; }
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
		if (initBlood <= 0) {
			return;	
		}
		posionAttack = data as AttackInfo;
		if (posionAttack == null) {
			return;	
		}

		int value = System.Convert.ToInt32 (posionAttack.AttackValue);
		KillHP (value);
	}

	public void KillHP(int hurtValue) {
		initBlood -= hurtValue;
		bool one = ConfigBattleUseData.Instance.NotDeadEnemy;
		bool two = NoviceGuideStepEntityManager.CurrentNoviceGuideStage != NoviceGuideStage.FIRST_ATTACK_TWO;
		bool three = NoviceGuideStepEntityManager.CurrentNoviceGuideStage != NoviceGuideStage.BOSS_ATTACK_BOOST;
		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage != NoviceGuideStage.NONE) {
			if(one || (two && three)) {
				if (initBlood <= 0) {
					initBlood = 10;
					IsDead = false;
				}
			}
			MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
			return;
		}
		if (initBlood <= 0) {
			initBlood = 0;	
			IsDead = true;
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
		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
	}

	public void FirstAttack () {
		initAttackRound++;
		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
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

	public int GetInitRound() {
		return instance.nextAttack;
	}

	public int GetInitBlood () {
		return instance.hp;
	}

	public int GetRound () {
		return initAttackRound;
	}

	public int DropUnit () {
		return -1;
	}

	public int GetUnitType () {
		return (int)EnemyInfo ().type;
	}

	private bool isDead = false;
	public bool IsDead {
		get { return isDead; }
		set {
			isDead = value; 
			if(isDead) {
				RemoveListener();
			}
		}
	}
}

public class EnemySortByHP : IComparer {
	public int Compare (object x, object y)
	{
		TEnemyInfo tex = (TEnemyInfo)x;
		TEnemyInfo tey = (TEnemyInfo)y;
		return tex.initBlood.CompareTo (tey.initBlood);
	}
}
