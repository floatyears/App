using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillAntiAttack : ProtobufDataBase, IPassiveExcute {
	private SkillAntiAttack instance;
	public TSkillAntiAttack(object instance) : base (instance) {
		this.instance = instance as SkillAntiAttack;
	}

	public object Excute (object trapBase, IExcutePassiveSkill excutePS) {
		if (trapBase is TrapBase) {
			return null;	
		}

//		SkillAntiAttack saa = DeserializeData<SkillAntiAttack> ();
		int type = (int)trapBase;
		EUnitType et = (EUnitType)type;
		if (instance.attackSource == EUnitType.UALL || et == instance.attackSource) {
			float value = DGTools.RandomToFloat ();
			if (value <= instance.antiAtkRatio) {
				AttackInfo ai = new AttackInfo();
				ai.AttackValue = instance.probability;
				ai.AttackType = (int)instance.antiAttack;
				return ai;
			}	
		}
		return null;
	}
}
