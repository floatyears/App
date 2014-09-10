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
//			float tempBlood = blood;
			float tempBlood = 0;
			if(instance.type == EValueType.FIXED) {
				tempBlood += instance.value;
			}
			else if(instance.type == EValueType.PERCENT) {
				tempBlood += blood *  instance.value;
			}
			blood = System.Convert.ToInt32(tempBlood);
		}
		return blood;
	}
}

public class TAcitveRecoverHP : ActiveSkill {
	private SkillRecoverHP instance;
	public TAcitveRecoverHP (object instance) : base (instance) { 
		this.instance = instance as SkillRecoverHP;
		skillBase = this.instance.baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}

	/// <summary>
	/// ***** ATK value is blood. **** Excute the specified userUnitID and atk.
	/// </summary>
	/// <param name="userUnitID">User unit I.</param>
	/// <param name="atk">Atk.</param>
	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;	
		}
		InitCooling ();
		float tempBlood = 0f;
		if (instance.period == EPeriod.EP_RIGHT_NOW) {
			if(instance.type == EValueType.FIXED) {
				tempBlood += instance.value;
			}
			else if(instance.type == EValueType.PERCENT) {
				tempBlood += atk *  BattleMapModule.bud.maxBlood;
			}	
		}

		MsgCenter.Instance.Invoke (CommandEnum.RecoverHP, tempBlood);
		return tempBlood;
	}
}