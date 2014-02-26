using UnityEngine;
using System.Collections;
using bbproto;

public class RecoverSP : ActiveSkill, IActiveSkillExcute {
	private SkillRecoverSP instance;
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

	public object Excute (uint userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
		int step = (int)instance.value;
		MsgCenter.Instance.Invoke (CommandEnum.SkillRecoverSP, step);
		return step;
	}
}
