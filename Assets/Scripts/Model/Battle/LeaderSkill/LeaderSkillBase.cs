using UnityEngine;
using System.Collections;
using bbproto;

public class SkillBaseInfo : ProtobufDataBase {
	protected SkillBase skillBase;
	public SkillBase BaseInfo {
		get {
			return skillBase;
		}
	}
	public SkillBaseInfo(object instance) : base (instance) {
		
	}
	public SkillBase GetSkillInfo () {
		return skillBase;
	}

	public string SkillName {
		get { return skillBase.name; }
	}

	public string SkillDescribe {
		get {return skillBase.description;}
	}
}
