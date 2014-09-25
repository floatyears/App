using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillKillHP : ActiveSkill {
		public SkillKillHP ( ) {
	//		skillBase = this.instance.baseInfo;	
	//		if (skillBase.skillCooling == 0) {
	//			coolingDone = true;
	//		}
		}

	//	public bool CoolingDone {
	//			return coolingDone;
	//		}
	//	}
	//
	//	public void RefreashCooling () {
	//
	//	}

		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();
	//		SkillKillHP skh = DeserializeData<SkillKillHP> ();
			AttackInfoProto ai = new AttackInfoProto(); //new AttackInfo ();
			ai.userUnitID = userUnitID;
			ai.ignoreDefense = true;
			ai.attackValue = value;
			ai.skillID = id;
	//		MsgCenter.Instance.Invoke(CommandEnum.SkillGravity, ai);
			BattleAttackManager.Instance.SkillGravity (ai);
			return ai;
		}
	}
}