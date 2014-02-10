using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class LeaderSkillMultipleAttack : ProtobufDataBase {
	public LeaderSkillMultipleAttack(object instance) : base (instance) {

	}

	public float MultipeAttack (List<AttackInfo> attackInfo) {
		SkillMultipleAttack sma = DeserializeData<SkillMultipleAttack> ();
		float multiple = 1f;
		List<int> tempAttackType = new List<int> ();
		for (int i = 0; i < attackInfo.Count; i++) {
			if(tempAttackType.Contains(attackInfo[i].AttackType) || attackInfo[i].AttackRange == 2) {
				continue;
			}
			tempAttackType.Add(attackInfo[i].AttackType);
		}

		if (tempAttackType.Count >= sma.unitTypeCount) {
			multiple = 2.5f;
		}

		return multiple;
	}
}
