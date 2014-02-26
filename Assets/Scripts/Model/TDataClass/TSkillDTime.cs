using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillDelayTime : LeaderSkillBase {
	private SkillDelayTime instance;
	public TSkillDelayTime (object instance) : base (instance) {
		this.instance = instance as SkillDelayTime;
		skillBase = this.instance.baseInfo;
	}
	
	public float DelayTime{
		get { return instance.value; }
	} 
}