using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveSkill : SkillBaseInfo {
//	protected SkillBase skillBase;
	protected int initSkillCooling = 0;
	protected bool coolingDone = false;
	public ActiveSkill (object instance) : base (instance) {
//		skillBase = 
	}

	~ActiveSkill () {

	}

	protected void DisposeCooling () {
		coolingDone = DGTools.CheckCooling (skillBase);
	}

	protected void InitCooling() {
		skillBase.skillCooling = initSkillCooling;
		if (skillBase.skillCooling > 0) {
			coolingDone = false;
		}
	}

	public SkillBase GetSkillInfo () {
		return skillBase;
	}
}

public class TSkillSingleAttack : ActiveSkill ,IActiveSkillExcute {
	private SkillSingleAttack instance;
	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}
	public TSkillSingleAttack(object instance) : base (instance) {
		this.instance = instance as SkillSingleAttack;
		skillBase = this.instance.baseInfo;	
		initSkillCooling = skillBase.skillCooling;
	
		if (initSkillCooling == 0) {
			coolingDone = true;
		}
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}
	
	public object Excute (uint userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;		
		}
		InitCooling ();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackType = (int)instance.unitType;
		ai.AttackRange = (int)instance.attackRange;
		if (instance.attackRange == EAttackType.RECOVER_HP) {
			MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillRecoverHP, instance.value);
		} 
		else {
			if (instance.type == EValueType.FIXED) {
				ai.AttackValue = instance.value;	
			}
			else if(instance.type == EValueType.MULTIPLE) {
				ai.AttackValue = instance.value * atk;
			}	
		}

		ai.IgnoreDefense = instance.ignoreDefense;
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		return ai;
	}
}