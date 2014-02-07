using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveSkill : ProtobufDataBase {
	protected SkillBase skillBase;
	protected bool coolingDone = false;
	public ActiveSkill (object instance) : base (instance) {
		skillBase = DeserializeData<SkillSingleAttack> ().baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

	protected void DisposeCooling () {
		if (skillBase == null) {
			skillBase = DeserializeData<SkillSingleAttack> ().baseInfo;	
		}
		coolingDone = DGTools.CheckCooling (skillBase);
	}
}

public class ActiveSkillSingleAttack : ActiveSkill ,IActiveSkillExcute {
	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}
	public ActiveSkillSingleAttack(object instance) : base (instance) {
	
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}
	
	public object Excute (int userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;		
		}
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