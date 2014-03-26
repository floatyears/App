using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillPoison : ActiveSkill, IActiveSkillExcute {
	private SkillPoison instance;
	public TSkillPoison(object instance) : base (instance) { 
		this.instance = instance as SkillPoison;
		skillBase = this.instance.baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}

//		Debug.LogError ("TSkillPoison : " + skillBase.skillCooling);
	}

	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}
	AttackInfo posionInfo = null;
	public object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackValue = atk * instance.value;
		ai.AttackRound = instance.roundValue;
		ai.IgnoreDefense = true;
		ai.AttackType = 0;
		posionInfo = ai;
		MsgCenter.Instance.Invoke(CommandEnum.BePosion, ai);
		return null;
	}

	void AttackEnemyEnd(object data) {
		if (posionInfo == null) {
			return;	
		}
		posionInfo.AttackRound --;
		MsgCenter.Instance.Invoke (CommandEnum.SkillPosion, posionInfo);
		if (posionInfo.AttackRound == 0) {
			posionInfo = null;
			MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		}
	}
}
