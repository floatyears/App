using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveAttackTargetType : ActiveSkill {
	private SkillTargetTypeAttack instance;

	/// <summary>
	/// 0=all,1=fire,2=water,3=wind,4=light,5=dark,6=none,7=heart.
	/// </summary>
	/// <value>The type of the attack.</value>
	public int AttackType {
		get { return (int)instance.hurtUnitType; }
	}
	
	/// <summary>
	/// 0=single, 1=all, 2=recoverhp.
	/// </summary>
	/// <value>The attack range.</value>
	public int AttackRange {
		get { return 1; }
	}
	
	/// <summary>
	/// Gets the type of the value.
	/// </summary>
	/// <value>The type of the value.</value>
	public EValueType ValueType {
		get { return instance.type; }
	}
	
	public float AttackValue {
		get { return instance.value; }
	}

	public ActiveAttackTargetType(object instance) : base (instance) {
		this.instance = instance as SkillTargetTypeAttack;
		skillBase = this.instance.baseInfo;
		if (skillBase.skillCooling == 0) {
			coolingDone = true;	
		}
	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
		AttackTargetType att = new AttackTargetType ();
		AttackInfo ai = AttackInfo.GetInstance ();
		ai.UserUnitID = userUnitID;
		if (instance.type == EValueType.MULTIPLE) {
			ai.AttackValue = atk * instance.value;	
		}
		else if(instance.type == EValueType.FIXED){
			ai.AttackValue = instance.value;
		}
		att.targetType = (int)instance.targetUnitType;
		ai.AttackType = (int)instance.hurtUnitType;
		att.attackInfo = ai;
		MsgCenter.Instance.Invoke(CommandEnum.AttackTargetType, att);
		return att;
	}
}

public class AttackTargetType {
	public AttackInfo attackInfo = null;
	public int targetType = -1;

}