using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillRecoverHP : SkillBaseInfo {
	private SkillRecoverHP instance;
	public TSkillRecoverHP (object instance) : base (instance) {
		this.instance = instance as SkillRecoverHP;
		skillBase = this.instance.baseInfo;
	}
	
	/// <summary>
	/// Recovers the H.
	/// </summary>
	/// <returns>The H.</returns>
	/// <param name="blood">Blood.</param>
	/// <param name="type">1 = right now. 2 = every round. 3 = every step.</param>
	public int RecoverHP (int blood,int type) {
		if(type == (int)instance.period){
			float tempBlood = blood;
			if(instance.type == EValueType.FIXED) {
				tempBlood += instance.value;
			}
			else if(instance.type == EValueType.PERCENT) {
				tempBlood *= (1 + instance.value);
			}
			blood = System.Convert.ToInt32(tempBlood);
		}
		return blood;
	}
}
