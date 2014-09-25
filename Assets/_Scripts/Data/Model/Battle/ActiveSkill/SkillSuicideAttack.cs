using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillSuicideAttack : ActiveSkill {


		public SkillSuicideAttack () {
	//		skillBase = this.instance.baseInfo;	
	////		initSkillCooling = skillBase.skillCooling;
	//		if (skillBase.skillCooling == 0) {
	//			coolingDone = true;
	//		}
		}

	//	~TSkillSuicideAttack() {
	//		RemoveListener ();
	//	}

	//	public void AddListener () {
	//		MsgCenter.Instance.AddListener (CommandEnum.UnitBlood, RecordBlood);
	//	}
	//
	//	public void RemoveListener() {
	//		MsgCenter.Instance.RemoveListener (CommandEnum.UnitBlood, RecordBlood);
	//	}
	//
	//	void RecordBlood (object data) {
	//		blood = (int)data;
	//	}

		public int AttackRange {
			get { return (int)attackType; }
		}
		
		public int AttackUnitType {
			get { return (int)unitType; }
		}


		public override object Excute (string userUnitID, int atk = -1) {
			if (BattleConfigData.Instance.storeBattleData.hp <= 1) {
				return null;		
			}
			InitCooling ();
			AttackInfoProto ai = new AttackInfoProto(); //new AttackInfo ();
			ai.userUnitID = userUnitID;
			ai.skillID = id;
			if (type == EValueType.FIXED) {
				ai.attackValue = value;
			}
			else if (type == EValueType.MULTIPLE){
				ai.attackValue = value * atk;
			}
			ai.attackRange = (int)attackType;
	//		MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillAttack, ai);
			BattleAttackManager.Instance.ActiveSkillAttack (ai);
	//		MsgCenter.Instance.Invoke (CommandEnum.SkillSucide, null);
			BattleAttackManager.Instance.Sucide (null);
			return ai;
		}
	}
}
