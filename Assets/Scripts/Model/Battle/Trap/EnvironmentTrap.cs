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
		MsgCenter.Instance.Invoke (CommandEnum.ShieldMap, true);
		ConfigBattleUseData.Instance.trapEnvironment = this;
		Round--;
	}

	void RoleMove (object instance) {
		if (Round == 0) {
			MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, RoleMove);
			ConfigBattleUseData.Instance.trapEnvironment = null;
			MsgCenter.Instance.Invoke (CommandEnum.ShieldMap, false);
			return;
		}

		Round--;
	}
}
