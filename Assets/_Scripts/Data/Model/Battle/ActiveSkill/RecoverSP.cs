using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillRecoverSP : ActiveSkill {
	private SkillRecoverSP instance;
	public TSkillRecoverSP (object instance) : base (instance) { 
		this.instance = instance as SkillRecoverSP;
//		skillBase = this.instance.baseInfo;	
////		initSkillCooling = skillBase.skillCooling;
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

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
		BattleAttackManager.SetEffectTime(1f);
//		Debug.LogError("tskillrecoversp excute");
		int step = (int)instance.value;

//		MsgCenter.Instance.Invoke (CommandEnum.SkillRecoverSP, step);
		BattleAttackManager.Instance.RecoverEnergePoint (step);
		return step;
	}
}
