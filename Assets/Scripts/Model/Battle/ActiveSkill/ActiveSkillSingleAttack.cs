using UnityEngine;
using System.Collections;
using bbproto;

//public class ActiveSkill : SkillBaseInfo, IActiveSkillExcute {
//	protected int initSkillCooling = 0;
//	protected bool coolingDone = false;
//	public ActiveSkill (object instance) : base (instance) { }
//
//	~ActiveSkill () {
//
//	}
//
//	public void RefreashCooling () {
//		DisposeCooling ();
//	}
//
//	public bool CoolingDone {
//		get {
//			return coolingDone;
//		}
//	}
//
//	protected void DisposeCooling () {
//		coolingDone = DGTools.CheckCooling (skillBase);
//	}
//
//	protected void InitCooling() {
//		skillBase.skillCooling = initSkillCooling;
//		if (skillBase.skillCooling > 0) {
//			coolingDone = false;
//		}
//	}
//	
//	public virtual AttackInfo ExcuteByDisk (AttackInfo ai) {
//		return null;
//	}
//
//	public virtual object Excute (string userUnitID, int atk = -1) {
//		return null;
//	}
//}

public class TSkillSingleAttack : ActiveSkill  {
	private SkillSingleAttack instance;
	public TSkillSingleAttack(object instance) : base (instance) {
		this.instance = instance as SkillSingleAttack;
		skillBase = this.instance.baseInfo;	
		initSkillCooling = skillBase.skillCooling;
	
		if (initSkillCooling == 0) {
			coolingDone = true;
		}
	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;		
		}
		InitCooling ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackType = (int)instance.unitType;
		ai.AttackRange = (int)instance.attackRange;
		if (instance.attackRange == EAttackType.RECOVER_HP) {
			MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillRecoverHP, instance.value);
		} 
		else {
			if (instance.type == EValueType.FIXED) {
				ai.AttackValue = instance.value;	
			}
			else if(instance.type == EValueType.MULTIPLE) {
				ai.AttackValue = instance.value * atk;
			}	
		}

		ai.IgnoreDefense = instance.ignoreDefense;
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		return ai;
	}
}