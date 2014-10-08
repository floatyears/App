using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillReduceHurt : ActiveSkill {
		public SkillReduceHurt (int dummy=0) {
	//		skillBase = this.instance.baseInfo;
	//		if (skillBase.skillCooling == 0) {
	//			coolingDone = true;		
	//		}
		}

	//	public bool CoolingDone {
	//		get {
	//			return coolingDone;
	//		}
	//	}
	//	
	//	public void RefreashCooling () {
	//		DisposeCooling ();
	//	}


		AttackInfoProto reduceHurtAttack = null;
		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;
			}
			InitCooling ();
	//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
			AttackInfoProto ai = new AttackInfoProto(0); //new AttackInfo ();
			ai.userUnitID = userUnitID;
			ai.attackValue = value;
			ai.attackRound = periodValue;
			ai.skillID = id;
			return ExcuteByDisk(ai);
		}

		public override AttackInfoProto ExcuteByDisk(AttackInfoProto ai) {
			reduceHurtAttack = ai;
			BattleConfigData.Instance.reduceHurtAttack = reduceHurtAttack;
			MsgCenter.Instance.Invoke(CommandEnum.ActiveReduceHurt,reduceHurtAttack);
			reduceHurtAttack.attackRound --;
			MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
			return reduceHurtAttack;
		}


		void EnemyAttackEnd(object data) {
			if (reduceHurtAttack == null) {
				return;
			}
			MsgCenter.Instance.Invoke(CommandEnum.ActiveReduceHurt,reduceHurtAttack);
			if (reduceHurtAttack.attackRound <= 0) {
				BattleConfigData.Instance.reduceHurtAttack = null;
				MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
				reduceHurtAttack = null;
			} 
			else {
				reduceHurtAttack.attackRound --;
			}
		}

		private int useCount = 0;
		
		public float ReduceHurt (float attackValue,int type) {
			//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
			if (unitType == EUnitType.UALL || unitType == (EUnitType)type) {
				if(value > 100f) {
					Debug.LogError("ReduceHurt error : reduce proportion error ! ");
				}
				else{
					float proportion = 1f - (float)value / 100f;
					attackValue *= proportion;
				}
			}
			if (periodValue != 0) {
				useCount ++;
			}
			return attackValue;
		}
		
		public bool CheckUseDone () {
			//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
			if (periodValue == 0) {
				return false;
			}
			
			if (useCount >= periodValue) {
				useCount = 0;
				return true;
			} 
			else {
				return false;
			}
		}
		
		/// <summary>
		/// 0 = right now, 1 = every round, 2 = every step.
		/// </summary>
		/// <returns>The duration.</returns>
		public int GetDuration() {
			return (int)period;
		}
	}
}
