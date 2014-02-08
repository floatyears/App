using UnityEngine;
using System.Collections;
using bbproto;

public class RecoverSP : ActiveSkill, IActiveSkillExcute {
	public RecoverSP (object instance) : base (instance) { 
		skillBase = DeserializeData<SkillRecoverSP> ().baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}

	public object Excute (int userUnitID, int atk = -1) {
//		Debug.LogError ("RecoverSP excute : " + coolingDone);
		if (!coolingDone) {
			return null;	
		}
		SkillRecoverSP srs = DeserializeData<SkillRecoverSP> ();
		int step = (int)srs.value;
//		Debug.LogError ("step : " + step);
		MsgCenter.Instance.Invoke (CommandEnum.SkillRecoverSP, step);
		return step;
	}
}
