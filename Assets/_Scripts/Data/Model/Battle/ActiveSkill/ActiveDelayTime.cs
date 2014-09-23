using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveDelayTime : ActiveSkill {
	private SkillDelayTime instance;
	public ActiveDelayTime(object instance) :base(instance){
		this.instance = instance as SkillDelayTime;
//		skillBase = this.instance.baseInfo;
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

	SkillDelayTime  sdt = null;

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
//	 	sdt = DeserializeData<SkillDelayTime> ();
		MsgCenter.Instance.Invoke (CommandEnum.DelayTime, instance.value);
		return null;
	}
}
