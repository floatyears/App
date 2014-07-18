using UnityEngine;
using System.Collections;
using bbproto;

public class SkillBaseInfo : ProtobufDataBase {
	protected int initSkillCooling = 0;
	private SkillBase _skillBase;
	protected SkillBase skillBase {
		set { _skillBase = value;  initSkillCooling = _skillBase.skillCooling;}
		get { return _skillBase; }
	}
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
		set {skillBase.description = value; }
	}
}
