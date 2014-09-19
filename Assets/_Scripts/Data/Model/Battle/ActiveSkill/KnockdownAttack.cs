using UnityEngine;
using System.Collections;
using bbproto;

public class KnockdownAttack : ActiveSkill {
	private SkillSingleAttack instance;
	public KnockdownAttack (object instance) : base (instance){ 
		this.instance = instance as SkillSingleAttack;
		skillBase = this.instance.baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

//	public bool CoolingDone {
//		get {
//			return coolingDone;
//		}
//	}
//
//	public void RefreashCooling () {
//		DisposeCooling ();
//	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		SkillSingleAttack ssa = DeserializeData<SkillSingleAttack>();
		AttackInfo ai = AttackInfo.GetInstance ();//new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.SkillID = skillBase.id;
		float value = DGTools.RandomToFloat ();
//		Debug.LogError ("random value : " + value);
		if (value <= instance.value) {
			ai.AttackValue = int.MaxValue - 10000; //not minus 10000, number will be overflow.
		} 
		else {
			ai.AttackValue = 1f;
		}
		ai.IgnoreDefense = instance.ignoreDefense;
		ai.AttackRange = (int)instance.attackRange;
		ai.AttackType = 0;
//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		BattleAttackManager.Instance.ActiveSkillAttack (ai);
		return ai;
	}
}
