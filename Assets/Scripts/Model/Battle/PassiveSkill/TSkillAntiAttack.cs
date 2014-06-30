using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillAntiAttack : SkillBaseInfo, IPassiveExcute {
	private SkillAntiAttack instance;
	public TSkillAntiAttack(object instance) : base (instance) {
		this.instance = instance as SkillAntiAttack;
		skillBase = this.instance.baseInfo;
	}

	public object Excute (object trapBase, IExcutePassiveSkill excutePS) {
		if (trapBase is TrapBase) {
			excutePS.DisposeTrap(false);
			return null;	
		}

		int type = (int)trapBase;
		EUnitType et = (EUnitType)type;
//		Debug.LogError ("TSkillAntiAttack et : " + et + " instance.attackSource : " + instance.attackSource);
		if (instance.attackSource == EUnitType.UALL || et == instance.attackSource) {
			float value = DGTools.RandomToFloat ();
//			Debug.LogError ("random ratio : " + value + " instance.antiAtkRatio : " + instance.antiAtkRatio);
			if (value <= instance.antiAtkRatio) {
				AttackInfo ai = AttackInfo.GetInstance(); //new AttackInfo();
				ai.AttackValue = instance.probability;
				ai.AttackType = (int)instance.antiAttack;
				return ai;
			}	
		}
		return null;
	}

	public SkillBaseInfo skillBaseInfo {
		get {
			return this;
		}
	}
}
