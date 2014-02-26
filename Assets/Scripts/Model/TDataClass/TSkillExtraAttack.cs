using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillExtraAttack : SkillBaseInfo {
	private SkillExtraAttack instance;
	public TSkillExtraAttack (object instance) : base (instance) {
		this.instance = instance as SkillExtraAttack;
		skillBase = this.instance.baseInfo;
	}
	
	public AttackInfo AttackValue (float attackValue, uint id) {
		AttackInfo ai = new AttackInfo ();
		ai.AttackValue = attackValue * DeserializeData<SkillExtraAttack> ().attackValue;
		ai.AttackType = (int)instance.unitType;
		ai.AttackRange = 1;//attack all enemy
		ai.UserUnitID = id;
		return ai;
	}
}