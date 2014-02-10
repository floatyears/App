using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveAttackTargetType : ActiveSkill, IActiveSkillExcute {
	public ActiveAttackTargetType(object instance) : base (instance) {
		skillBase = DeserializeData<SkillTargetTypeAttack> ().baseInfo;
		if (skillBase.skillCooling == 0) {
			coolingDone = true;	
		}
	}

	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}
	
	public void RefreashCooling () {
		DisposeCooling ();
	}

	public object Excute (int userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
		SkillTargetTypeAttack stta = DeserializeData<SkillTargetTypeAttack> ();
		AttackTargetType att = new AttackTargetType ();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		if (stta.type == EValueType.MULTIPLE) {
			ai.AttackValue = atk * stta.value;	
		}
		else if(stta.type == EValueType.FIXED){
			ai.AttackValue = stta.value;
		}
		att.targetType = (int)stta.targetUnitType;
		ai.AttackType = (int)stta.hurtUnitType;
		att.attackInfo = ai;
		MsgCenter.Instance.Invoke(CommandEnum.AttackTargetType, att);
		return att;
	}
}

public class AttackTargetType {
	public AttackInfo attackInfo = null;
	public int targetType = -1;

}