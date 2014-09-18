using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveReduceHurt : ActiveSkill {
	private SkillReduceHurt instance;
	public ActiveReduceHurt (object instance) : base (instance) {
		this.instance = instance as SkillReduceHurt;
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


	AttackInfo reduceHurtAttack = null;
	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackValue = instance.value;
		ai.AttackRound = instance.periodValue;
		ai.SkillID = skillBase.id;
		return ExcuteByDisk(ai);
	}

	public override AttackInfo ExcuteByDisk(AttackInfo ai) {
		reduceHurtAttack = ai;
		BattleConfigData.Instance.reduceHurtAttack = reduceHurtAttack;
		MsgCenter.Instance.Invoke(CommandEnum.ActiveReduceHurt,reduceHurtAttack);
		reduceHurtAttack.AttackRound --;
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		return reduceHurtAttack;
	}


	void EnemyAttackEnd(object data) {
		if (reduceHurtAttack == null) {
			return;
		}
		MsgCenter.Instance.Invoke(CommandEnum.ActiveReduceHurt,reduceHurtAttack);
		if (reduceHurtAttack.AttackRound <= 0) {
			BattleConfigData.Instance.reduceHurtAttack = null;
			MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			reduceHurtAttack = null;
		} 
		else {
			reduceHurtAttack.AttackRound --;
		}
	}
}
