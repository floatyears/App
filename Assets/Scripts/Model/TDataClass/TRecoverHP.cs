using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillRecoverHP : ProtobufDataBase {
	public TSkillRecoverHP (object instance) : base (instance) {
		
	}
	
	/// <summary>
	/// Recovers the H.
	/// </summary>
	/// <returns>The H.</returns>
	/// <param name="blood">Blood.</param>
	/// <param name="type">1 = right now. 2 = every round. 3 = every step.</param>
	public int RecoverHP (int blood,int type) {
		SkillRecoverHP srhp = DeserializeData<SkillRecoverHP> ();
		if(type == (int)srhp.period){
			float tempBlood = blood;
			if(srhp.type == EValueType.FIXED) {
				tempBlood += srhp.value;
			}
			else if(srhp.type == EValueType.PERCENT) {
				tempBlood *= (1 + srhp.value);
			}
			blood = System.Convert.ToInt32(tempBlood);
		}
		return blood;
	}
}
