using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveSkill : ProtobufDataBase {
	protected SkillBase skillBase;
	protected int initSkillCooling = 0;
	protected bool coolingDone = false;
	public ActiveSkill (object instance) : base (instance) {

	}

	~ActiveSkill () {

	}

	protected void DisposeCooling () {
		coolingDone = DGTools.CheckCooling (skillBase);
	}

	protected void InitCooling() {
		coolingDone = false;
		skillBase.skillCooling = initSkillCooling;
	}
}

public class ActiveSkillSingleAttack : ActiveSkill ,IActiveSkillExcute {
	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}
	public ActiveSkillSingleAttack(object instance) : base (instance) {
		skillBase = DeserializeData<SkillSingleAttack> ().baseInfo;	
		initSkillCooling = skillBase.skillCooling;
		if (skillBase.skillCooling == 0) {
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
		SkillSingleAttack ssa = DeserializeData<SkillSingleAttack> ();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackType = (int)ssa.unitType;
		ai.AttackRange = (int)ssa.attackRange;
		if (ssa.attackRange == EAttackType.RECOVER_HP) {
			MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillRecoverHP, ssa.value);
		} 
		else {
			if (ssa.type == EValueType.FIXED) {
				ai.AttackValue = ssa.value;	
			}
			else if(ssa.type == EValueType.MULTIPLE) {
				ai.AttackValue = ssa.value * atk;
			}	
		}

		ai.IgnoreDefense = ssa.ignoreDefense;
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		return ai;
	}
}