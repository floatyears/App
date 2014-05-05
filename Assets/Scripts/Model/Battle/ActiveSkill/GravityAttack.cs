using UnityEngine;
using System.Collections;
using bbproto;

public class GravityAttack : ActiveSkill, IActiveSkillExcute {
	private SkillKillHP instance; 
	public GravityAttack (object instance) : base (instance) {
		this.instance = instance as SkillKillHP;
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

	}

	public object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		SkillKillHP skh = DeserializeData<SkillKillHP> ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.IgnoreDefense = true;
		ai.AttackValue = instance.value;

		MsgCenter.Instance.Invoke(CommandEnum.SkillGravity, ai);
		return ai;
	}
}
