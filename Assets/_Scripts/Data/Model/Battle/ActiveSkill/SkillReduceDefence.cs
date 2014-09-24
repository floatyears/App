using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillReduceDefence : ActiveSkill {
		public SkillReduceDefence() {
	//		skillBase = this.instance.baseInfo;	
	//		if (skillBase.skillCooling == 0) {
	//			coolingDone = true;
	//		}
		}

		AttackInfo reduceDefense = null;
	//	bool b = false;
		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();

			AttackInfo ai = AttackInfo.GetInstance ();
			ai.UserUnitID = userUnitID;
			ai.AttackRound = period;
			ai.AttackValue = value;
			ai.SkillID = id;
	//		b = true;
			return ExcuteByDisk(ai);
		}

		public override AttackInfo ExcuteByDisk (AttackInfo ai) {
			reduceDefense = ai;
	//		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, reduceDefense);
			BattleAttackManager.Instance.ReduceDefense (reduceDefense);
			BattleConfigData.Instance.reduceDefenseAttack = reduceDefense;
			MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			return null;
		}

		void EnemyAttackEnd(object data) {
			reduceDefense.AttackRound --;
	//		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, reduceDefense);
			BattleAttackManager.Instance.ReduceDefense (reduceDefense);
			if (reduceDefense.AttackRound <= 0) {
	//			Debug.LogWarning("remove EnemyAttackEnd");
	//			b = false;
				MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
				reduceDefense = null;
				BattleConfigData.Instance.reduceDefenseAttack = null;
			}
		}

	}
}