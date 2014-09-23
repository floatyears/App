using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveDeferAttackRound : ActiveSkill {
	private SkillDeferAttackRound instance;
	public ActiveDeferAttackRound (object instance):base(instance) {
		this.instance = instance as SkillDeferAttackRound;
//		skillBase = this.instance.baseInfo;
////		Debug.LogError ("ActiveDeferAttackRound.skillCooling : " + skillBase.skillCooling);
//		if (skillBase.skillCooling == 0) {
//			Debug.LogError ("ActiveDeferAttackRound is boost ");
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
		Debug.LogError ("InitCooling : " + skillCooling);
//		SkillDeferAttackRound sdar = DeserializeData<SkillDeferAttackRound> ();
		int roundValue = System.Convert.ToInt32 (instance.value);
		MsgCenter.Instance.Invoke(CommandEnum.DeferAttackRound, roundValue);
		return null;
	}
}
