using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
public partial class SkillExtraAttack : SkillBase, ProtoBuf.IExtensible {

	public AttackInfoProto AttackValue (float attackValue, UserUnit id) {
		AttackInfoProto ai = new AttackInfoProto(); //new AttackInfo ();
		ai.attackValue = attackValue * attackValue;
		ai.attackType = (int)unitType;
		ai.attackRange = 1;//attack all enemy
		ai.skillID = baseInfo.id;
		ai.userUnitID = id.MakeUserUnitKey();
		return ai;
	}
}
}