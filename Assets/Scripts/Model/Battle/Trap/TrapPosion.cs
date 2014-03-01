using UnityEngine;
using System.Collections;

public class TrapPosion : TrapBase, ITrapExcute {
	public TrapPosion (object instance) : base (instance) {
		trapEffectType = TrapInjuredInfo.stateException;
		round = (int)GetTrap.effectType;
	}

	int round = 0;
	public void Excute () {
		if (round > 0) {
			MsgCenter.Instance.AddListener(CommandEnum.MoveToMapItem, RoleMove);
			MsgCenter.Instance.AddListener(CommandEnum.EnemyAttackEnd, EnemyAttak);
			CountDownRound ();
		}
	}
	
	void RoleMove(object data) {
		ExcuteTrap ();
	}

	void EnemyAttak (object data) {
		ExcuteTrap ();
	}

	void ExcuteTrap () {
		if (round == 0) {
			ViewManager.Instance.TrapLabel.text = "";
			MsgCenter.Instance.RemoveListener(CommandEnum.MoveToMapItem, RoleMove);
			MsgCenter.Instance.RemoveListener(CommandEnum.EnemyAttackEnd, EnemyAttak);
			MsgCenter.Instance.Invoke (CommandEnum.PlayerPosion, round);
			return;
		}

		CountDownRound ();
	}

	void CountDownRound () {
		ViewManager.Instance.TrapLabel.text = "posion trap hurt:" +  GetInjuredValue.trapValue +" round : " + round;
		MsgCenter.Instance.Invoke (CommandEnum.PlayerPosion, round);
		MsgCenter.Instance.Invoke(CommandEnum.InjuredNotDead, GetInjuredValue.trapValue);
		round--;
	}
}
