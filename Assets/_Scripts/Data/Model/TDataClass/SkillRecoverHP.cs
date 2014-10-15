using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillRecoverHP : ActiveSkill {
	
	/// <summary>
	/// Recovers the H.
	/// </summary>
	/// <returns>The H.</returns>
	/// <param name="blood">Blood.</param>
	/// <param name="type">1 = right now. 2 = every round. 3 = every step.</param>
	public int RecoverHP (int blood,int type) {
		if(type == (int)period){
//			float tempBlood = blood;
			float tempBlood = 0;
				if((EValueType)type == EValueType.FIXED) {
				tempBlood += value;
			}
				else if((EValueType)type == EValueType.PERCENT) {
				tempBlood += blood * value;
			}
			blood = System.Convert.ToInt32(tempBlood);
		}
		return blood;
	}
		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();
			float tempBlood = 0f;
			if ( period == EPeriod.EP_RIGHT_NOW) {
				if(type == EValueType.FIXED) {
					tempBlood += value;
				}
				else if(type == EValueType.PERCENT) {
					tempBlood += atk *  BattleAttackManager.Instance.maxBlood;
				}	
			}
			
			//		MsgCenter.Instance.Invoke (CommandEnum.RecoverHP, tempBlood);
			//		BattleAttackManager.Instance.RecoverHP (tempBlood);
			return tempBlood;
		}

//		public SkillRecoverHP () { 
//			if (skillCooling == 0) {
//				coolingDone = true;
//			}
//		}
		public override SkillBase GetBaseInfo ()
		{
			return baseInfo;
		}
}
	
}