using UnityEngine;
using System.Collections;

public class EnvironmentTrap : TrapBase {
	public EnvironmentTrap (object instance) : base (instance) {
		trapEffectType = TrapInjuredInfo.environment;
	}

	public override void Excute () {
		Round =  (int)GetInjuredValue.trapValue;

		ExcuteByDisk ();
	}
	
	public override  void ExcuteByDisk () {
		MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, RoleMove);
		MsgCenter.Instance.Invoke (CommandEnum.ShieldMap, Round);
		ConfigBattleUseData.Instance.trapEnvironment = this;
	}

	void RoleMove (object instance) {
		MsgCenter.Instance.Invoke (CommandEnum.ShieldMap, Round);
		if (Round == 0) {
			MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, RoleMove);
			ConfigBattleUseData.Instance.trapEnvironment = null;
//			MsgCenter.Instance.Invoke (CommandEnum.ShieldMap, Round);
			return;
		}

		Round--;
	}
}
