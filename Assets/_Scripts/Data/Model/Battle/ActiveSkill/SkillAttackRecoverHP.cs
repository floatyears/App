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

		public SkillAttackRecoverHP( ) {
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
			AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
			ai.AttackType = (int)unitType;
			ai.AttackRange = (int)attackType;
			ai.UserUnitID = userUnitID;
			ai.SkillID = id;
			if (type == EValueType.MULTIPLE) {
				ai.AttackValue = atk * value;		
			} else if(type == EValueType.FIXED) {
				ai.AttackValue = value;
			}
	//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
			BattleAttackManager.Instance.ActiveSkillAttack (ai);
	//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillDrawHP, null);
			BattleAttackManager.Instance.RecoveHPByActiveSkill ();
			return ai;
		}

		


	}
}
