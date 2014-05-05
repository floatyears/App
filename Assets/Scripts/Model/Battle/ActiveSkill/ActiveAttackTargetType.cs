using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveAttackTargetType : ActiveSkill, IActiveSkillExcute {
	private SkillTargetTypeAttack instance;
	public ActiveAttackTargetType(object instance) : base (instance) {
		this.instance = instance as SkillTargetTypeAttack;
		skillBase = this.instance.baseInfo;
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

	public object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
//		SkillTargetTypeAttack stta = DeserializeData<SkillTargetTypeAttack> ();
		AttackTargetType att = new AttackTargetType ();

//		bbproto.AttackInfoProto aip = new bbproto.AttackInfoProto();
//		AttackInfo ai = new AttackInfo (aip);
//		 ai = new AttackInfo ();
		AttackInfo ai = AttackInfo.GetInstance ();
		ai.UserUnitID = userUnitID;
		if (instance.type == EValueType.MULTIPLE) {
			ai.AttackValue = atk * instance.value;	
		}
		else if(instance.type == EValueType.FIXED){
			ai.AttackValue = instance.value;
		}
		att.targetType = (int)instance.targetUnitType;
		ai.AttackType = (int)instance.hurtUnitType;
		att.attackInfo = ai;
		MsgCenter.Instance.Invoke(CommandEnum.AttackTargetType, att);
		return att;
	}
}

public class AttackTargetType {
	public AttackInfo attackInfo = null;
	public int targetType = -1;

}