using UnityEngine;
using System.Collections;
using bbproto;

public class KnockdownAttack : ActiveSkill, IActiveSkillExcute {
	public KnockdownAttack (object instance) : base (instance){ 
		skillBase = DeserializeData<SkillSingleAttack> ().baseInfo;	
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

	public object Excute (uint userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
		SkillSingleAttack ssa = DeserializeData<SkillSingleAttack>();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		float value = DGTools.RandomToFloat ();
//		Debug.LogError ("random value : " + value);
		if (value <= ssa.value) {
			ai.AttackValue = int.MaxValue - 10000; //not minus 10000, number will be overflow.
		} 
		else {
			ai.AttackValue = 1f;
		}
		ai.IgnoreDefense = ssa.ignoreDefense;
		ai.AttackRange = (int)ssa.attackRange;
		ai.AttackType = 0;
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		return ai;
	}
}
