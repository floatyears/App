using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
public partial class SkillExtraAttack : SkillBase, ProtoBuf.IExtensible {

	public AttackInfo AttackValue (float attackValue, UserUnit id) {
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.AttackValue = attackValue * attackValue;
		ai.AttackType = (int)unitType;
		ai.AttackRange = 1;//attack all enemy
		ai.SkillID = baseInfo.id;
		ai.UserUnitID = id.MakeUserUnitKey();
		return ai;
	}
}
}