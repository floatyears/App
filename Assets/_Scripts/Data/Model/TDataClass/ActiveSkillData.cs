using UnityEngine;
using System.Collections.Generic;


namespace bbproto{
public partial class ActiveSkillData : ProtoBuf.IExtensible {

//	public ActiveSkillData ActiveSkillCooling{
//		get { return instance; }
//	}

	public void InitActiveSkill(ActiveSkill activeSkillExcte) {
		ActiveSkill activeSkill = activeSkillExcte as ActiveSkill;

		skillId = activeSkill.id;
		round = activeSkill.skillCooling;
	}

	public void RefreshData() {

	}
}
}