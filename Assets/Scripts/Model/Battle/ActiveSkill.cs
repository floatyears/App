using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveSkillSingleAttack : ProtobufDataBase ,IActiveSkillExcute {
	private SkillBase skillBase;
	private bool coolingDone = false;
	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}
	public ActiveSkillSingleAttack(object instance) : base (instance) {
		skillBase = DeserializeData<SkillSingleAttack> ().baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

	public void RefreashCooling () {
		if (skillBase == null) {
			skillBase = DeserializeData<SkillSingleAttack> ().baseInfo;	
		}
		coolingDone = DGTools.CheckCooling (skillBase);
	}
	
	public object Excute (int userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;		
		}
		SkillSingleAttack ssa = DeserializeData<SkillSingleAttack> ();
		AttackInfo ai = new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.AttackType = (int)ssa.unitType;
		ai.AttackRange = (int)ssa.attackRange;
		if (ssa.type == EValueType.FIXED) {
			ai.AttackValue = ssa.value;	
		}
		else if(ssa.type == EValueType.MULTIPLE) {
			ai.AttackValue = ssa.value * atk;
		}
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		return ai;
	}
}