using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveStrengthenAttack : ActiveSkill {
	private SkillStrengthenAttack instance;
	public ActiveStrengthenAttack (object instance) : base (instance) {
		this.instance = instance as SkillStrengthenAttack;
//		skillBase = this.instance.baseInfo;
//		if (skillBase.skillCooling == 0) {
//			coolingDone = true;	
//		}
	}
	
	AttackInfo strengthenAttack = null;
	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
		AttackInfo ai = AttackInfo.GetInstance ();//new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackType = (int)instance.targetType;
		ai.AttackRace = (int)instance.targetRace;
		ai.AttackValue = instance.value;
		ai.AttackRound = instance.periodValue;
		return ExcuteByDisk (ai);
	}

	public override AttackInfo ExcuteByDisk (AttackInfo ai) {
		strengthenAttack = ai;
		BattleConfigData.Instance.strengthenAttack = strengthenAttack;
		MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, strengthenAttack);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		strengthenAttack.AttackRound --;
		return strengthenAttack;
	}

	void EnemyAttackEnd(object data) {
		if (strengthenAttack == null) {
			return;	
		}
		if (strengthenAttack.AttackRound <= 0) {
			BattleConfigData.Instance.strengthenAttack = null;
			MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, strengthenAttack);
			MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		}
		else{
			MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, strengthenAttack);
			strengthenAttack.AttackRound--;
		}
	}
}
