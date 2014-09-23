using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
public partial class SkillReduceHurt : SkillBase, ProtoBuf.IExtensible {

	private int useCount = 0;
	
	public float ReduceHurt (float attackValue,int type) {
//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (unitType == EUnitType.UALL || unitType == (EUnitType)type) {
			if(value > 100f) {
				Debug.LogError("ReduceHurt error : reduce proportion error ! ");
			}
			else{
				float proportion = 1f - (float)value / 100f;
				attackValue *= proportion;
			}
		}
		if (periodValue != 0) {
			useCount ++;
		}
		return attackValue;
	}
	
	public bool CheckUseDone () {
//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (periodValue == 0) {
			return false;
		}
		
		if (useCount >= periodValue) {
			useCount = 0;
			return true;
		} 
		else {
			return false;
		}
	}
	
	/// <summary>
	/// 0 = right now, 1 = every round, 2 = every step.
	/// </summary>
	/// <returns>The duration.</returns>
	public int GetDuration() {
		return (int)period;
	}
}
}