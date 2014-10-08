using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto	{
	public partial class SkillPoison : ActiveSkill {

		public SkillPoison(int dummy=0) { 
	//		skillBase = this.instance.baseInfo;	
			if (skillCooling == 0) {
				coolingDone = true;
			}
		}

		AttackInfoProto posionInfo = null;
		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;
			}
			InitCooling ();
			AttackInfoProto ai = new AttackInfoProto(0); //new AttackInfo ();
			ai.userUnitID = userUnitID;
			ai.attackValue = atk * value;
			ai.attackRound = roundValue;
			ai.ignoreDefense = true;
			ai.attackType = 0; //0 = ATK_SINGLE
			ai.skillID = id;
			ai.attackRange = 1;
			BattleConfigData.Instance.posionAttack = ai;
			MsgCenter.Instance.Invoke (CommandEnum.PlayAllEffect, ai);
			ExcuteByDisk (ai);
			return null;
		}

		public override AttackInfoProto ExcuteByDisk(AttackInfoProto ai) {
			posionInfo = ai;
			MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
	//		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
	//		Debug.LogError ("TSkillPoison ai.AttackRound : " + ai.AttackRound + " value :  " + ai.AttackValue);
			MsgCenter.Instance.Invoke(CommandEnum.BePosion, ai);
			return posionInfo;
		}

		void AttackEnemyEnd(object data) {
			if (posionInfo == null) {
				return;	
			}
			posionInfo.attackRound --;
			Debug.LogError("TSkillPoison attack enemy end : " + posionInfo.attackRound);
			MsgCenter.Instance.Invoke (CommandEnum.SkillPosion, posionInfo);
			if (posionInfo.attackRound == 0) {
				posionInfo = null;
				BattleConfigData.Instance.posionAttack = null;
				MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
	//			MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
			}
		}

	//	void BattleEnd(object data) {
	//		ConfigBattleUseData.Instance.posionAttack = null;
	////		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
	//	}
	}
}
