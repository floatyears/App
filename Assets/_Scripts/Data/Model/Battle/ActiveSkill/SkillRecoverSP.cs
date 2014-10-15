using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillRecoverSP : ActiveSkill {
		public SkillRecoverSP (int dummy=0) { 
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
			int step = (int)value;

	//		MsgCenter.Instance.Invoke (CommandEnum.SkillRecoverSP, step);
			BattleAttackManager.Instance.RecoverEnergePoint (step);
			return step;
		}

		public object Excute1 (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;	
			}
			InitCooling ();
			int step = (int)value;
			//		MsgCenter.Instance.Invoke (CommandEnum.SkillRecoverSP, step);
			BattleAttackManager.Instance.RecoverEnergePoint (step);
			return step;
		}

		public override SkillBase GetBaseInfo ()
		{
			return baseInfo;
		}
	}
}