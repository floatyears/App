using UnityEngine;
using System.Collections;
using bbproto;

public class GravityAttack : ActiveSkill, IActiveSkillExcute {
	public GravityAttack (object instance) : base (instance) {
		skillBase = DeserializeData<SkillKillHP> ().baseInfo;	
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

	public object Excute (int userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		SkillKillHP skh = DeserializeData<SkillKillHP> ();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.IgnoreDefense = true;
		ai.AttackValue = skh.value;

		MsgCenter.Instance.Invoke(CommandEnum.SkillGravity, ai);
		return ai;
	}
}
