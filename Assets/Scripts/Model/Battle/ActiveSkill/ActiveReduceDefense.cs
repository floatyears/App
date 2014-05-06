using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveReduceDefense : ActiveSkill {
	private SkillReduceDefence instance;
	public ActiveReduceDefense(object instance) : base (instance) {
		this.instance = instance as SkillReduceDefence;
		skillBase = this.instance.baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}
//	TClass<string, int, float> tc;

//	public bool CoolingDone {
//		get {
//			return coolingDone;
//		}
//	}
//
//	public void RefreashCooling () {
//		DisposeCooling ();
//	}

	AttackInfo reduceDefense = null;

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		Debug.LogError("ActiveReduceDefense excute ");
//		SkillReduceDefence srd = DeserializeData<SkillReduceDefence> ();
//		tc = new TClass<string, int, float> ();
//		tc.arg1 = userUnitID;
//		tc.arg2 = (int)instance.period;
//		tc.arg3 = instance.value; // in percent

		AttackInfo ai = AttackInfo.GetInstance ();
		ai.UserUnitID = userUnitID;
		ai.AttackRound = instance.period;
		ai.AttackValue = instance.value;
		return ExcuteByDisk(ai);
	}

	public override AttackInfo ExcuteByDisk (AttackInfo ai) {
		reduceDefense = ai;

		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, reduceDefense);
		ConfigBattleUseData.Instance.reduceDefenseAttack = reduceDefense;
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		return null;
	}

	void EnemyAttackEnd(object data) {
//		Debug.LogError ("excute EnemyAttackEnd");
		reduceDefense.AttackRound --;
		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, reduceDefense);
		if (reduceDefense.AttackRound <= 0) {
//			Debug.LogWarning("remove EnemyAttackEnd");
			MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			reduceDefense = null;
			ConfigBattleUseData.Instance.reduceDefenseAttack = null;
		}
	}

}
