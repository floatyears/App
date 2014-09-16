using UnityEngine;
using System.Collections;
using bbproto;

public class PassiveDodgeTrap : SkillBaseInfo {
	private SkillDodgeTrap instance;
	public PassiveDodgeTrap(object instance) : base (instance) {
		this.instance = instance as SkillDodgeTrap;
		skillBase = this.instance.baseInfo;
	}

	public object Excute (object trapBase) {
		TrapBase tb = trapBase as TrapBase;
		if (tb == null) {
			return null;	
		}

//		SkillDodgeTrap sdt = DeserializeData<SkillDodgeTrap> ();
		if (instance.trapType == tb.GetTrapType() || instance.trapType == ETrapType.All) {
			if(tb.GetLevel == -1 || instance.trapLevel >= tb.GetLevel) {
				BattleAttackManager.Instance.DisposeTrap(true);

				AudioManager.Instance.PlayAudio(AudioEnum.sound_ps_dodge_trap);

				return true;
			}
		}
		BattleAttackManager.Instance.DisposeTrap(false);

		return false;
	}


	public SkillBaseInfo skillBaseInfo {
		get {
			return this;
		}
	}
}
