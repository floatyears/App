using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveDelayTime : ActiveSkill, IActiveSkillExcute {
	public ActiveDelayTime(object instance) : base (instance) {
		skillBase = DeserializeData<SkillDelayTime> ().baseInfo;
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

	SkillDelayTime  sdt = null;

	public object Excute (uint userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
	 	sdt = DeserializeData<SkillDelayTime> ();
		MsgCenter.Instance.Invoke (CommandEnum.DelayTime, sdt.value);
		return null;
	}
}
