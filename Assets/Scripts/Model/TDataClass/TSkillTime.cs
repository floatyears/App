using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillTime : ProtobufDataBase {
	public TSkillTime (object instance) : base (instance) {
		
	}
	
	public float DelayTime{
		get {
			return DeserializeData<SkillDelayTime>().value;
		}
	} 
}