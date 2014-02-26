using UnityEngine;
using System.Collections;
using bbproto;

public class SkillBaseInfo : ProtobufDataBase {
	protected SkillBase skillBase;
	public SkillBaseInfo(object instance) : base (instance) {
		
	}
	public SkillBase GetSkillInfo () {
		return skillBase;
	}
}
