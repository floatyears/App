using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TActiveSkillData : ProtobufDataBase {
	private  ActiveSkillData instance;
	public TActiveSkillData (ActiveSkillData ins) : base(ins)  {
		instance = ins;
	}

	public ActiveSkillData ActiveSkillCooling{
		get { return instance; }
	}

	public void InitActiveSkill(ActiveSkill activeSkillExcte) {
		ActiveSkill activeSkill = activeSkillExcte as ActiveSkill;

		instance.skillId = activeSkill.skillBase.id;
		instance.round = activeSkill.skillBase.skillCooling;
	}

	public void RefreshData() {

	}
}
