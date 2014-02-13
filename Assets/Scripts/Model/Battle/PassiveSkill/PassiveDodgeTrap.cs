using UnityEngine;
using System.Collections;
using bbproto;

public class PassiveDodgeTrap : ProtobufDataBase, IPassiveExcute {
	public PassiveDodgeTrap(object instance) : base (instance) {

	}

	public object Excute (object trapBase, IExcutePassiveSkill excutePS) {
		TrapBase tb = trapBase as TrapBase;
		if (tb == null) {
			return null;	
		}

		SkillDodgeTrap sdt = DeserializeData<SkillDodgeTrap> ();
		if (sdt.trapType == tb.GetTrapType() ) {
			if(tb.GetLevel == -1 || sdt.trapLevel >= tb.GetLevel) {
				excutePS.DisposeTrap(true);
				return null;
			}
		}
		excutePS.DisposeTrap(false);

		return null;
	}

}
