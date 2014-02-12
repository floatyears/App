using UnityEngine;
using System.Collections;

public class ExcutePassiveSkill  {
	private ILeaderSkill leaderSkill;
	private ExcuteTrap excuteTrap;
	public ExcutePassiveSkill(ILeaderSkill ls) {
		leaderSkill = ls;
		excuteTrap = new ExcuteTrap ();

		MsgCenter.Instance.AddListener (CommandEnum.MeetTrap, DisposeTrap);
	}

	~ ExcutePassiveSkill () {
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetTrap, DisposeTrap);
	}

	void DisposeTrap(object data) {
		TrapBase tb = data as TrapBase;
		if (tb == null) {
			return;		
		}

		ITrapExcute ie = tb as ITrapExcute;
		excuteTrap.Excute (ie);
	}
}
