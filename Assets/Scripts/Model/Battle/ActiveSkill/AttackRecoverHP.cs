using UnityEngine;
using System.Collections;
using bbproto;

public class AttackRecoverHP : ActiveSkill ,IActiveSkillExcute{
	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	public AttackRecoverHP(object instance) : base (instance) {
		skillBase = DeserializeData<SkillSingleAtkRecoverHP> ().baseInfo;	
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
		SkillSingleAtkRecoverHP ssarh = DeserializeData<SkillSingleAtkRecoverHP> ();
		AttackInfo ai = new AttackInfo ();
		ai.AttackType = (int)ssarh.unitType;
		if (ssarh.type == EValueType.MULTIPLE) {
			ai.AttackValue = atk * ssarh.value;		
		} else if(ssarh.type == EValueType.FIXED) {
			ai.AttackValue = ssarh.value;
		}
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillDrawHP, null);
		return ai;
	}

	


}
