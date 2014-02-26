using UnityEngine;
using System.Collections;
using bbproto;

public class PassiveDodgeTrap : ProtobufDataBase, IPassiveExcute {
	private SkillDodgeTrap instance;
	public PassiveDodgeTrap(object instance) : base (instance) {
		this.instance = instance as SkillDodgeTrap;
	}

	public object Excute (object trapBase, IExcutePassiveSkill excutePS) {
		TrapBase tb = trapBase as TrapBase;
		if (tb == null) {
			return null;	
		}

//		SkillDodgeTrap sdt = DeserializeData<SkillDodgeTrap> ();
		if (instance.trapType == tb.GetTrapType() ) {
			if(tb.GetLevel == -1 || instance.trapLevel >= tb.GetLevel) {
				excutePS.DisposeTrap(true);
				return null;
			}
		}
		excutePS.DisposeTrap(false);

		return null;
	}

}
