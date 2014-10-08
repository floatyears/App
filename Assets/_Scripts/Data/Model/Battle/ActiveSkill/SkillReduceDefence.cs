using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillReduceDefence : ActiveSkill {
		public SkillReduceDefence(int dummy=0) {
	//		skillBase = this.instance.baseInfo;	
	//		if (skillBase.skillCooling == 0) {
	//			coolingDone = true;
	//		}
		}

		AttackInfoProto reduceDefense = null;
	//	bool b = false;
		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();

			AttackInfoProto ai = new AttackInfoProto(0);
			ai.userUnitID = userUnitID;
			ai.attackRound = period;
			ai.attackValue = value;
			ai.skillID = id;
	//		b = true;
			return ExcuteByDisk(ai);
		}

		public override AttackInfoProto ExcuteByDisk (AttackInfoProto ai) {
			reduceDefense = ai;
	//		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, reduceDefense);
			BattleAttackManager.Instance.ReduceDefense (reduceDefense);
			BattleConfigData.Instance.reduceDefenseAttack = reduceDefense;
			MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			return null;
		}

		void EnemyAttackEnd(object data) {
			reduceDefense.attackRound --;
	//		MsgCenter.Instance.Invoke (CommandEnum.ReduceDefense, reduceDefense);
			BattleAttackManager.Instance.ReduceDefense (reduceDefense);
			if (reduceDefense.attackRound <= 0) {
	//			Debug.LogWarning("remove EnemyAttackEnd");
	//			b = false;
				MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
				reduceDefense = null;
				BattleConfigData.Instance.reduceDefenseAttack = null;
			}
		}

	}
}