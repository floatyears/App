using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillReduceHurt : LeaderSkillBase {
	private SkillReduceHurt instance;
	private int useCount = 0;
	public TSkillReduceHurt (object instance) : base (instance) {
		this.instance = instance as SkillReduceHurt;
		skillBase = this.instance.baseInfo;
	}
	
	public float ReduceHurt (float attackValue,int type) {
//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (instance.unitType == EUnitType.UALL || instance.unitType == (EUnitType)type) {
			if(instance.value > 100f) {
				Debug.LogError("ReduceHurt error : reduce proportion error ! ");
			}
			else{
				float proportion = 1f - (float)instance.value / 100f;
				attackValue *= proportion;
			}
		}
		if (instance.periodValue != 0) {
			useCount ++;
		}
		return attackValue;
	}
	
	public bool CheckUseDone () {
//		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (instance.periodValue == 0) {
			return false;
		}
		
		if (useCount >= instance.periodValue) {
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
		return (int)instance.period;
	}
}