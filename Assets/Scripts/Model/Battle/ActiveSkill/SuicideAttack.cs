using UnityEngine;
using System.Collections;
using bbproto;

public class SuicideAttack : ActiveSkill, IActiveSkillExcute {
	private int blood = 0;
	public SuicideAttack (object instance) : base(instance) {
		skillBase = DeserializeData<SkillSuicideAttack> ().baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
		MsgCenter.Instance.AddListener (CommandEnum.UnitBlood, RecordBlood);
	}

	~SuicideAttack () {
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitBlood, RecordBlood);
	}

	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	void RecordBlood (object data) {
		blood = (int)data;
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}

	public object Excute (int userUnitID, int atk = -1) {
		if (blood <= 1) {
			return null;		
		}

		SkillSuicideAttack ssa = DeserializeData<SkillSuicideAttack> ();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		if (ssa.type == EValueType.FIXED) {
			ai.AttackValue = ssa.value;
		}
		else if (ssa.type == EValueType.MULTIPLE){
			ai.AttackValue = ssa.value * atk;
		}
		ai.AttackRange = (int)ssa.attackType;
		MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillAttack, ai);
		MsgCenter.Instance.Invoke (CommandEnum.SkillSucide, null);
		return ai;
	}
}
