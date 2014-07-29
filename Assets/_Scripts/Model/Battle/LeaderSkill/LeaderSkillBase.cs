using UnityEngine;
using System.Collections;
using bbproto;

public class SkillBaseInfo : ProtobufDataBase {
	private int _initSkillCooling = 0;

	public int initSkillCooling {
		set { _initSkillCooling = value; } //Debug.LogError("initSkillCooling : " + value + " class : " + this); }
		get { return _initSkillCooling; }
	}

	private SkillBase _skillBase;

	public SkillBase skillBase {
		set { _skillBase = value; initSkillCooling = _skillBase.skillCooling; }
		get { return _skillBase; }
	}

//	public SkillBase BaseInfo {
//		get {
//			return skillBase;
//		}
//	}

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
