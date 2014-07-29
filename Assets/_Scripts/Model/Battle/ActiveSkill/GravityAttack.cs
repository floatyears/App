using UnityEngine;
using System.Collections;
using bbproto;

public class GravityAttack : ActiveSkill {
	private SkillKillHP instance; 
	public GravityAttack (object instance) : base (instance) {
		this.instance = instance as SkillKillHP;
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
//
//	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		SkillKillHP skh = DeserializeData<SkillKillHP> ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.IgnoreDefense = true;
		ai.AttackValue = instance.value;
		ai.SkillID = skillBase.id;
		MsgCenter.Instance.Invoke(CommandEnum.SkillGravity, ai);
		return ai;
	}
}
