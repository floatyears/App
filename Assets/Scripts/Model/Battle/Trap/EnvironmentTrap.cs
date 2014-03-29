using UnityEngine;
using System.Collections;

public class EnvironmentTrap : TrapBase, ITrapExcute {
	public EnvironmentTrap (object instance) : base (instance) {
		trapEffectType = TrapInjuredInfo.environment;
		round = (int)GetInjuredValue.trapValue;
//		round = trapValueIndex;
//		Debug.LogError ("EnvironmentTrap : " + round);
	}

	int round = 0;

	public void Excute () {
		if (round > 0) {
			MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, RoleMove);
			MsgCenter.Instance.Invoke (CommandEnum.ShieldMap, true);
			round--;
		}

//		Debug.LogError ("Excute EnvironmentTrap : " + round);
	}


	void RoleMove (object instance) {
		if (round == 0) {
			round = (int)GetInjuredValue.trapValue;
			MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, RoleMove);
			MsgCenter.Instance.Invoke (CommandEnum.ShieldMap, false);
			return;
		}

		round--;
	}
}
