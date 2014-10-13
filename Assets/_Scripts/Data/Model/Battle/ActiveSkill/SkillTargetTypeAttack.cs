using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
public partial class SkillTargetTypeAttack : ActiveSkill {

	/// <summary>
	/// 0=all,1=fire,2=water,3=wind,4=light,5=dark,6=none,7=heart.
	/// </summary>
	/// <value>The type of the attack.</value>
	public int AttackType {
		get { return (int)hurtUnitType; }
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
	public SkillTargetTypeAttack(object instance){
		if (skillCooling == 0) {
			coolingDone = true;	
		}
	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
		AttackTargetType att = new AttackTargetType ();
		AttackInfoProto ai = new AttackInfoProto(0);
		ai.userUnitID = userUnitID;
		ai.skillID = id;
		if (type == EValueType.MULTIPLE) {
			ai.attackValue = atk * value;	
		}
		else if(type == EValueType.FIXED){
			ai.attackValue = value;
		}
		att.targetType = (int)targetUnitType;
		ai.attackType = (int)hurtUnitType;
		att.attackInfo = ai;
//		MsgCenter.Instance.Invoke(CommandEnum.AttackTargetType, att);
		BattleAttackManager.Instance.AttackTargetTypeEnemy (att);
		return att;
	}

		public override SkillBase GetBaseInfo ()
		{
			return baseInfo;
		}
}

public class AttackTargetType {
	public AttackInfoProto attackInfo = null;
	public int targetType = -1;

}

}