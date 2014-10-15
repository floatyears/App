using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillStrengthenAttack : ActiveSkill {
		public SkillStrengthenAttack (int dummy=0) {
	//		skillBase = this.instance.baseInfo;
	//		if (skillBase.skillCooling == 0) {
	//			coolingDone = true;	
	//		}
		}
		
		AttackInfoProto strengthenAttack = null;
		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();
			AttackInfoProto ai = new AttackInfoProto(0);//new AttackInfo ();
			ai.userUnitID = userUnitID;
			ai.attackType = (int)targetType;
			ai.attackRace = (int)targetRace;
			ai.attackValue = value;
			ai.attackRound = periodValue;
			return ExcuteByDisk (ai);
		}

		public override AttackInfoProto ExcuteByDisk (AttackInfoProto ai) {
			strengthenAttack = ai;
			BattleConfigData.Instance.strengthenAttack = strengthenAttack;
			MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, strengthenAttack);
			MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			strengthenAttack.attackRound --;
			return strengthenAttack;
		}

		void EnemyAttackEnd(object data) {
			if (strengthenAttack == null) {
				return;	
			}
			if (strengthenAttack.attackRound <= 0) {
				BattleConfigData.Instance.strengthenAttack = null;
				MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, strengthenAttack);
				MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			}
			else{
				MsgCenter.Instance.Invoke(CommandEnum.StrengthenTargetType, strengthenAttack);
				strengthenAttack.attackRound--;
			}
		}

		public override SkillBase GetBaseInfo ()
		{
			return baseInfo;
		}
	}

}