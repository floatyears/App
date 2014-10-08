using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillAttackRecoverHP : ActiveSkill {
	//	public bool CoolingDone {
	//		get {
	//			return coolingDone;
	//		}
	//	}

		public SkillAttackRecoverHP(int dummy=0) {
	//		skillBase = DeserializeData<SkillSingleAtkRecoverHP> ().baseInfo;	
	//		skillBase = this.instance.baseInfo;
	////		initSkillCooling = skillBase.skillCooling;
	//		if (skillBase.skillCooling == 0) {
	//			coolingDone = true;
	//		}
		}

	//	public void RefreashCooling () {
	//		DisposeCooling ();
	//	}

		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();
	//		SkillSingleAtkRecoverHP ssarh = DeserializeData<SkillSingleAtkRecoverHP> ();
			AttackInfoProto ai = new AttackInfoProto(0); //new AttackInfo ();
			ai.attackType = (int)unitType;
			ai.attackRange = (int)attackType;
			ai.userUnitID = userUnitID;
			ai.skillID = id;
			if (type == EValueType.MULTIPLE) {
				ai.attackValue = atk * value;		
			} else if(type == EValueType.FIXED) {
				ai.attackValue = value;
			}
	//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
			BattleAttackManager.Instance.ActiveSkillAttack (ai);
	//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillDrawHP, null);
			BattleAttackManager.Instance.RecoveHPByActiveSkill ();
			return ai;
		}

		


	}
}
