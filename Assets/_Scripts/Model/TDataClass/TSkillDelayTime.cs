using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillTime : ProtobufDataBase {
	private SkillDelayTime instance;
	public TSkillTime (object instance) : base (instance) {
		this.instance = instance as SkillDelayTime;
	}
	
	public float DelayTime{
		get {
			return instance.value;
		}
	} 
}