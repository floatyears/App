using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillStrengthenAttack : ActiveSkill {
		public SkillStrengthenAttack ( ) {
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
			ai.AttackType = (int)targetType;
			ai.AttackRace = (int)targetRace;
			ai.AttackValue = value;
			ai.AttackRound = periodValue;
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

}