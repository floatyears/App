using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillRecoverSP : ActiveSkill, IActiveSkillExcute {
	private SkillRecoverSP instance;
	public TSkillRecoverSP (object instance) : base (instance) { 
		this.instance = instance as SkillRecoverSP;
		skillBase = this.instance.baseInfo;	
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

	public object Excute (uint userUnitID, int atk = -1) {
//		Debug.LogError ("RecoverSP excute : " + coolingDone);
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		SkillRecoverSP srs = DeserializeData<SkillRecoverSP> ();
		int step = (int)instance.value;
//		Debug.LogError ("step : " + step);
		MsgCenter.Instance.Invoke (CommandEnum.SkillRecoverSP, step);
		return step;
	}
}
