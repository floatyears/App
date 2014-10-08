using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
public partial class SkillSingleAttack : ActiveSkill {
//		skillBase = this.instance.baseInfo;	
//		if (skillBase.skillCooling == 0) {
//			coolingDone = true;
//		}

//	public bool CoolingDone {
//		get {
//			return coolingDone;
//		}
//	}
//
//	public void RefreashCooling () {
//		DisposeCooling ();
//	}

		public SkillSingleAttack(int dummy=0) {
			//		skillBase = this.instance.baseInfo;	
			//		initSkillCooling = skillBase.skillCooling;
			
			if (initSkillCooling == 0) {
				coolingDone = true;
			}
		}
		
		/// <value>The type of the attack.</value>
		public int AttackType {
			get { return (int)unitType; }
		}
		
		/// <summary>
		/// 0=single, 1=all, 2=recoverhp.
		/// </summary>
		/// <value>The attack range.</value>
		public int AttackRange {
			get { return (int)attackRange; }
		}


		public object ExcuteKonckDown (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();
	//		SkillSingleAttack ssa = DeserializeData<SkillSingleAttack>();
			AttackInfoProto ai = new AttackInfoProto(0);//new AttackInfo ();
			ai.userUnitID = userUnitID;
			ai.skillID = id;
			float value = DGTools.RandomToFloat ();
	//		Debug.LogError ("random value : " + value);
			if (value <= value) {
				ai.attackValue = int.MaxValue - 10000; //not minus 10000, number will be overflow.
			} 
			else {
				ai.attackValue = 1f;
			}
			ai.ignoreDefense = ignoreDefense;
			ai.attackRange = (int)attackRange;
			ai.attackType = 0;
	//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
			BattleAttackManager.Instance.ActiveSkillAttack (ai);
			return ai;
		}

		public override object Excute (string userUnitID, int atk = -1) {
			//		Debug.LogError ("TSkillSingleAttack cooling one : " + coolingDone);
			if (!coolingDone) {
				return null;		
			}
			BattleAttackManager.SetEffectTime(1f);
			//		Debug.LogError ("activeskill excute : ");
			InitCooling ();
			AttackInfoProto ai = new AttackInfoProto(0); //new AttackInfo ();
			ai.userUnitID = userUnitID;
			ai.skillID = id; 
			ai.attackType = (int)unitType;
			ai.attackRange = (int)attackRange;
			//		Debug.LogError ("instance attack range : " + instance.attackRange + " type : " + instance.type);
			if (attackRange == EAttackType.RECOVER_HP) {
				//			MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillRecoverHP, instance.value);
				BattleAttackManager.Instance.RecoveHPByActiveSkill(value);
			} else {
				if (type == EValueType.FIXED) {
					ai.attackValue = value;	
				} else if(type == EValueType.MULTIPLE) {
					ai.attackValue = value * atk;
				}	
			}
			
			ai.ignoreDefense = ignoreDefense;
			
			//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
			BattleAttackManager.Instance.ActiveSkillAttack (ai);
			return ai;
		}
	}
}
