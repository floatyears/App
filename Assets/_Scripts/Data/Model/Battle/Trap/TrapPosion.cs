using UnityEngine;
using System.Collections;

public class TrapPosion : TrapBase {
	public TrapPosion (object instance) : base (instance) {
		trapEffectType = TrapInjuredInfo.stateException;
	}
	
	public override void Excute () {
		Round = (int)GetTrap.effectType;
		BattleConfigData.Instance.trapPoison = this;
		ExcuteByDisk ();
	}

	public override void ExcuteByDisk() {
//		Debug.LogError("trapposion excute by disk ");
		MsgCenter.Instance.AddListener(CommandEnum.MoveToMapItem, RoleMove);
		MsgCenter.Instance.AddListener(CommandEnum.EnemyAttackEnd, EnemyAttak);
		CountDownRound ();
	}
	
	void RoleMove(object data) {
		ExcuteTrap ();
		if (Round != 0) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_attack);
		}
	}

	void EnemyAttak (object data) {
		ExcuteTrap ();
	}

	void ExcuteTrap () {
		if (Round == 0) {
			BattleConfigData.Instance.trapPoison = null;
			MsgCenter.Instance.RemoveListener(CommandEnum.MoveToMapItem, RoleMove);
			MsgCenter.Instance.RemoveListener(CommandEnum.EnemyAttackEnd, EnemyAttak);
			MsgCenter.Instance.Invoke (CommandEnum.PlayerPosion, Round);
			return;
		}
		CountDownRound ();
	}

	void CountDownRound () {
		MsgCenter.Instance.Invoke (CommandEnum.PlayerPosion, Round);
		MsgCenter.Instance.Invoke(CommandEnum.InjuredNotDead, GetInjuredValue.trapValue);
		Round--;
	}
}
