using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillExtraAttack : ProtobufDataBase {
	public TSkillExtraAttack (object instance) : base (instance) {
		
	}
	
	public AttackInfo AttackValue (float attackValue, uint id) {
		AttackInfo ai = new AttackInfo ();
		ai.AttackValue = attackValue * DeserializeData<SkillExtraAttack> ().attackValue;
		ai.AttackType = (int)DeserializeData<SkillExtraAttack> ().unitType;
		ai.AttackRange = 1;//attack all enemy
		ai.UserUnitID = id;
		return ai;
	}
}