using UnityEngine;
using System.Collections;
using bbproto;

public class LeaderSkillBase : ProtobufDataBase {
	protected SkillBase skillBase;
	public LeaderSkillBase(object instance) : base (instance) {
		
	}
	public SkillBase GetSkillInfo () {
		return skillBase;
	}
}
