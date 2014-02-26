using UnityEngine;
using System.Collections;
using bbproto;

public class PassiveAntiAttack : ProtobufDataBase, IPassiveExcute {
	public PassiveAntiAttack(object instance) : base (instance) {

	}

	public object Excute (object trapBase, IExcutePassiveSkill excutePS) {
		if (trapBase is TrapBase) {
			return null;	
		}

		SkillAntiAttack saa = DeserializeData<SkillAntiAttack> ();
		int type = (int)trapBase;
		EUnitType et = (EUnitType)type;
		if (saa.attackSource == EUnitType.UALL || et == saa.attackSource) {
			float value = DGTools.RandomToFloat ();
			if (value <= saa.antiAtkRatio) {
				AttackInfo ai = new AttackInfo();
				ai.AttackValue = saa.probability;
				ai.AttackType = (int)saa.antiAttack;
				return ai;
			}	
		}
		return null;
	}
}
