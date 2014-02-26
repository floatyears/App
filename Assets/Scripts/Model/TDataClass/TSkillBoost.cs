using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillBoost : ProtobufDataBase {
	public TSkillBoost (object instance) : base (instance) {
		
	}
	
	SkillBoost Get(){
		return DeserializeData<SkillBoost> ();
	}
	
	public float GetBoostValue {
		get{
			return DeserializeData<SkillBoost> ().boostValue;
		}
	}
	
	public int GetTargetValue {
		get{
			return DeserializeData<SkillBoost> ().targetValue;
		}
	}
	
	/// <summary>
	/// attack = 0, hp = 1
	/// </summary>
	/// <returns>The boost type.</returns>
	public EBoostType GetBoostType {
		get{
			return Get ().boostType;
		}
	}
	
	/// <summary>
	/// race = 0, type = 1
	/// </summary>
	/// <returns>The target type.</returns>
	public EBoostTarget GetTargetType { 
		get {
			return Get ().targetType;
		}
	}
}