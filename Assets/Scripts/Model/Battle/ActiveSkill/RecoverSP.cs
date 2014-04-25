using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillRecoverSP : ActiveSkill, IActiveSkillExcute {
	private SkillRecoverSP instance;
	public TSkillRecoverSP (object instance) : base (instance) { 
		this.instance = instance as SkillRecoverSP;
		skillBase = this.instance.baseInfo;	
		initSkillCooling = skillBase.skillCooling;
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

	public object Excute (string userUnitID, int atk = -1) {
//		Debug.LogError ("RecoverSP excute befoure : " + coolingDone + " skillBase.skillCooling " + skillBase.skillCooling);
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
//		Debug.LogError ("RecoverSP excute end : " + coolingDone + " skillBase.skillCooling " + skillBase.skillCooling);
//		SkillRecoverSP srs = DeserializeData<SkillRecoverSP> ();
		int step = (int)instance.value;
//		Debug.LogError ("step : " + step);
		MsgCenter.Instance.Invoke (CommandEnum.SkillRecoverSP, step);
		return step;
	}
}
