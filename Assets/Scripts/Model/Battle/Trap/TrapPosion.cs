using UnityEngine;
using System.Collections;

public class TrapPosion : TrapBase, ITrapExcute {
	public TrapPosion (object instance) : base (instance) {
		trapEffectType = TrapInjuredInfo.stateException;
		round = (int)GetInjuredValue.trapValue;
	}

	int round = 0;
	public void Excute () {
		if (round > 0) {
			MsgCenter.Instance.AddListener(CommandEnum.MoveToMapItem, RoleMove);
		}

		CountDownRound ();
	}

	void CountDownRound () {
		round--;
	}

	void RoleMove(object data) {
		if (round == 0) {
			MsgCenter.Instance.RemoveListener(CommandEnum.MoveToMapItem, RoleMove);
		}

		CountDownRound ();
	}
}
