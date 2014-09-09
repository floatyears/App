using UnityEngine;
using System.Collections;
using bbproto;

public class TSkillSuicideAttack : ActiveSkill {
	private SkillSuicideAttack instance;
	private int blood = 0;

	public int AttackRange {
		get { return (int)instance.attackType; }
	}

	public int AttackUnitType {
		get { return (int)instance.unitType; }
	}

	public TSkillSuicideAttack (object instance) : base(instance) {
		this.instance = instance as SkillSuicideAttack;
		skillBase = this.instance.baseInfo;	
//		initSkillCooling = skillBase.skillCooling;
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
		AddListener ();
	}

	~TSkillSuicideAttack() {
		RemoveListener ();
	}

	public void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.UnitBlood, RecordBlood);
	}

	public void RemoveListener() {
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitBlood, RecordBlood);
	}

	void RecordBlood (object data) {
		blood = (int)data;
	}

	public override object Excute (string userUnitID, int atk = -1) {
		if (blood <= 1) {
			return null;		
		}
		InitCooling ();
		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo ();
		ai.UserUnitID = userUnitID;
		ai.SkillID = skillBase.id;
		if (instance.type == EValueType.FIXED) {
			ai.AttackValue = instance.value;
		}
		else if (instance.type == EValueType.MULTIPLE){
			ai.AttackValue = instance.value * atk;
		}
		ai.AttackRange = (int)instance.attackType;
		MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillAttack, ai);
		MsgCenter.Instance.Invoke (CommandEnum.SkillSucide, null);
		return ai;
	}
}
