using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillAntiAttack : SkillBase {
	private SkillAntiAttack instance;
	public EUnitType AttackSource {
		get { return instance.attackSource; }
	}
	public TSkillAntiAttack(object instance){
		this.instance = instance as SkillAntiAttack;
//		skillBase = this.instance.baseInfo;
	}

	public object Excute (object trapBase) {
		if (trapBase is TrapBase) {
			BattleAttackManager.Instance.DisposeTrap(false);
			return null;	
		}

		int type = (int)trapBase;
		EUnitType et = (EUnitType)type;
//		Debug.LogError ("TSkillAntiAttack et : " + et + " instance.attackSource : " + instance.attackSource);
		if (instance.attackSource == EUnitType.UALL || et == instance.attackSource) {
			float value = DGTools.RandomToFloat ();
//			Debug.LogError ("random ratio : " + value + " instance.antiAtkRatio : " + instance.antiAtkRatio);
			if (value <= instance.antiAtkRatio) {
				AttackInfoProto ai = new AttackInfoProto(0); //new AttackInfo();
				ai.attackValue = instance.probability;
				ai.attackType = (int)instance.antiAttack;
				return ai;
			}	
		}
		return null;
	}

	public SkillBase skillBaseInfo {
		get {
			return this;
		}
	}
}
