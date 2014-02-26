using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillReduceHurt : ProtobufDataBase {
	private int useCount = 0;
	public TSkillReduceHurt (object instance) : base (instance) {
		
	}
	
	public float ReduceHurt (float attackValue,int type) {
		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (srh.unitType == EUnitType.UALL || srh.unitType == (EUnitType)type) {
			if(srh.value > 100f) {
				Debug.LogError("ReduceHurt error : reduce proportion error ! ");
			}
			else{
				float proportion = 1f - (float)srh.value / 100f;
				attackValue *= proportion;
			}
		}
		if (srh.periodValue != 0) {
			useCount ++;
		}
		return attackValue;
	}
	
	public bool CheckUseDone () {
		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (srh.periodValue == 0) {
			return false;
		}
		
		if (useCount >= srh.periodValue) {
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
		return (int)DeserializeData<SkillReduceHurt> ().period;
	}
}