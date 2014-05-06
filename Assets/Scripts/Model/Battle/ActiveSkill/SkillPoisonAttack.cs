using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillPoison : ActiveSkill {
	private SkillPoison instance;
	public TSkillPoison(object instance) : base (instance) { 
		this.instance = instance as SkillPoison;
		skillBase = this.instance.baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

//	public bool CoolingDone {
//		get {
//			return coolingDone;
//		}
//	}

//	public void RefreashCooling () {
//		DisposeCooling ();
//	}

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
		ConfigBattleUseData.Instance.posionAttack = posionInfo;
		ExcuteByDisk (ai);
		return null;
	}

	public override AttackInfo ExcuteByDisk(AttackInfo ai) {
		posionInfo = ai;
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.Invoke(CommandEnum.BePosion, ai);
		return posionInfo;
	}

	void AttackEnemyEnd(object data) {
		if (posionInfo == null) {
			return;	
		}
		posionInfo.AttackRound --;
		MsgCenter.Instance.Invoke (CommandEnum.SkillPosion, posionInfo);
		if (posionInfo.AttackRound == 0) {
			posionInfo = null;
			ConfigBattleUseData.Instance.posionAttack = null;
			MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		}
	}
}
