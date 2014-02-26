using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillDelayTime : ProtobufDataBase {
	private SkillDelayTime instance;
	public TSkillDelayTime (object instance) : base (instance) {
		this.instance = instance as SkillDelayTime;
	}
	
	public float DelayTime{
		get {
			return instance.value;
		}
	} 
}