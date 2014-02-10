using UnityEngine;
using System.Collections;
using bbproto;

public class SkillPoisonAttack : ActiveSkill, IActiveSkillExcute {
	public SkillPoisonAttack(object instance) : base (instance) { 
		skillBase = DeserializeData<SkillPoison> ().baseInfo;	
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
	AttackInfo posionInfo = null;
	public object Excute (int userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);

		SkillPoison sp = DeserializeData<SkillPoison>();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackValue = atk * sp.value;
		ai.AttackRound = sp.roundValue;
		ai.IgnoreDefense = true;
		ai.AttackType = 0;
		posionInfo = ai;
		MsgCenter.Instance.Invoke(CommandEnum.BePosion, ai);
		return null;
	}

	void AttackEnemyEnd(object data) {
		if (posionInfo == null) {
			return;	
		}
		posionInfo.AttackRound --;
		MsgCenter.Instance.Invoke (CommandEnum.SkillPosion, posionInfo);
		if (posionInfo.AttackRound == 0) {
			posionInfo = null;
			MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		}
	}
}
