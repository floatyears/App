using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillPoison : ActiveSkill {
	private SkillPoison instance;
	public TSkillPoison(object instance) : base (instance) { 
		this.instance = instance as SkillPoison;
//		skillBase = this.instance.baseInfo;	
//		if (skillBase.skillCooling == 0) {
//			coolingDone = true;
//		}
	}

	AttackInfo posionInfo = null;
	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackValue = atk * instance.value;
		ai.AttackRound = instance.roundValue;
		ai.IgnoreDefense = true;
		ai.AttackType = 0; //0 = ATK_SINGLE
		ai.SkillID = id;
		ai.AttackRange = 1;
		BattleConfigData.Instance.posionAttack = ai;
		MsgCenter.Instance.Invoke (CommandEnum.PlayAllEffect, ai);
		ExcuteByDisk (ai);
		return null;
	}

	public override AttackInfo ExcuteByDisk(AttackInfo ai) {
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
		posionInfo.AttackRound --;
		Debug.LogError("TSkillPoison attack enemy end : " + posionInfo.AttackRound);
		MsgCenter.Instance.Invoke (CommandEnum.SkillPosion, posionInfo);
		if (posionInfo.AttackRound == 0) {
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
