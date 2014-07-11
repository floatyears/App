using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillSingleAttack : ActiveSkill  {
	private SkillSingleAttack instance;

	/// <summary>
	/// 0=all,1=fire,2=water,3=wind,4=light,5=dark,6=none,7=heart.
	/// </summary>
	/// <value>The type of the attack.</value>
	public int AttackType {
		get { return (int)instance.unitType; }
	}

	/// <summary>
	/// 0=single, 1=all, 2=recoverhp.
	/// </summary>
	/// <value>The attack range.</value>
	public int AttackRange {
		get { return (int)instance.attackRange; }
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

	public TSkillSingleAttack(object instance) : base (instance) {
		this.instance = instance as SkillSingleAttack;
		skillBase = this.instance.baseInfo;	
		initSkillCooling = skillBase.skillCooling;
		
		if (initSkillCooling == 0) {
			coolingDone = true;
		}
	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;		
		}
		AttackController.SetEffectTime(1f);
		Debug.LogError ("activeskill excute : ");
		InitCooling ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.SkillID = skillBase.id;
		ai.AttackType = (int)instance.unitType;
		ai.AttackRange = (int)instance.attackRange;
		if (instance.attackRange == EAttackType.RECOVER_HP) {
			MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillRecoverHP, instance.value);
		} else {
			if (instance.type == EValueType.FIXED) {
				ai.AttackValue = instance.value;	
			} else if(instance.type == EValueType.MULTIPLE) {
				ai.AttackValue = instance.value * atk;
			}	
		}

		ai.IgnoreDefense = instance.ignoreDefense;

		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillAttack, ai);
		return ai;
	}
}