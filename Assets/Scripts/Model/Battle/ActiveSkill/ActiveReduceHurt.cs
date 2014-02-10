using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveReduceHurt : ActiveSkill, IActiveSkillExcute {
	public ActiveReduceHurt (object instance) : base (instance) {
		skillBase = DeserializeData<SkillReduceHurt> ().baseInfo;
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
	AttackInfo ai = null;
	public object Excute (uint userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackValue = srh.value;
		ai.AttackRound = srh.periodValue;
		MsgCenter.Instance.Invoke(CommandEnum.ActiveReduceHurt,ai);
		ai.AttackRound --;
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);

		return ai;
	}

	void EnemyAttackEnd(object data) {
		if (ai == null) {
			return;
		}
		MsgCenter.Instance.Invoke(CommandEnum.ActiveReduceHurt,ai);
		if (ai.AttackRound <= 0) {
			MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			ai = null;
		} 
		else {
			ai.AttackRound --;
		}
	}
}
