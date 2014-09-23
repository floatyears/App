using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillBoost : SkillBase {
	private SkillBoost instance;
	public TSkillBoost (object instance) {
		this.instance = instance as SkillBoost;
//		skillBase = this.instance.baseInfo;
	}
	
	SkillBoost Get(){
		return instance;
	}
	
	public float GetBoostValue {
		get{
			return instance.boostValue;
		}
	}
	
	public int GetTargetValue {
		get{
			return instance.targetValue;
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