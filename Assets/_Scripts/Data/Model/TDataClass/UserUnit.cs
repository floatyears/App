using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class UserUnit : ProtoBuf.IExtensible {


	public void Init(){
		MsgCenter.Instance.AddListener(CommandEnum.StrengthenTargetType, StrengthenTargetType);
	}

	public bool isEnable = true;
	public bool isFocus = false;
		
    public void RemovevListener() {
        MsgCenter.Instance.RemoveListener(CommandEnum.StrengthenTargetType, StrengthenTargetType);
    }

	public void ResetState () {
		isEnable = true;
		isFocus = false;
	}
//    private int currentBlood = -1;
    private float attackMultiple = 1;
    public float AttackMultiple {
        get {
            return attackMultiple;
        }
    }
    private float hpMultiple = 1;
    public int unitBaseInfo = -1;

	public uint userID ;
	public string TUserUnitID {
		get {
			if (string.IsNullOrEmpty (userUnitID)) {
				return userID.ToString () + "_" + uniqueId.ToString ();	
			} else {
				return 	userUnitID;
			}
		}
	}
	private string userUnitID = string.Empty;
	public  string MakeUserUnitKey() {
		if (string.IsNullOrEmpty (userUnitID)) {
			return userID.ToString () + "_" + uniqueId.ToString ();	
		} else {
			return 	userUnitID;
		}
	} 

    NormalSkill[] normalSkill = new NormalSkill[2];

    public void SetAttack(float value, int type, EBoostTarget boostTarget, EBoostType boostType) {
        if (boostType == EBoostType.BOOST_HP) {
            if (boostTarget == EBoostTarget.UNIT_RACE) {
                SetHPByRace(value, type);
            } else {
                SetHPByType(value, type);
            }
        } else {
            if (boostTarget == EBoostTarget.UNIT_RACE) {
                SetAttackMultipeByRace(value, type);
            } else {
                SetAttackMultipleByType(value, type);	
            }
        }
    }

    void SetHPByType(float value, int type) {
        if (type == UnitType || type == 0) {
            hpMultiple *= value;
        }
    }

    void SetHPByRace(float value, int race) {
		if ((int)UnitInfo.race == race || UnitInfo.race == EUnitRace.ALL) {
            hpMultiple *= value;
        }
    }

    void SetAttackMultipleByType(float value, int type) {
        if (type == UnitType || type == 0) {
            attackMultiple *= value;
        }
    }

    void SetAttackMultipeByRace(float value, int race) {
//        UnitBaseInfo ubi = DataCenter.Instance.UnitBaseInfo[unitBaseInfo];
		if ((int)UnitInfo.race == race || UnitInfo.race == EUnitRace.ALL) {
            attackMultiple *= value;
        }
    }

	public List<NormalSkill> GetNormalSkill() {
		if (normalSkill[0] == null) {
			InitSkill();	
		}
		List<NormalSkill> ns = new List<NormalSkill> ();
		for (int i = 0; i < normalSkill.Length; i++) {
			if(normalSkill[i] != null) {
				ns.Add(normalSkill[i]);
			}
		}

		return ns;
	}

    public float CalculateInjured(int attackType, float attackValue) {
        int beRetraintType = DGTools.BeRestraintType(attackType);
        int retraintType = DGTools.RestraintType(attackType);
//		UserUnit uu = DeserializeData<UserUnit> ();
        float hurtValue = 0;

        if (beRetraintType == (int)UnitInfo.type) {
            hurtValue = attackValue * 0.5f;
        }
        else if (retraintType == (int)UnitInfo.type) {
            hurtValue = attackValue * 2f;
        }
        else {
            hurtValue = attackValue;
        }
        if (hurtValue <= 0) {
            hurtValue = 1;
        }
//        int hv = System.Convert.ToInt32(hurtValue);
//        currentBlood -= hv;
		return hurtValue;
    }

    void InitSkill() {
		UnitInfo ui = DataCenter.Instance.UnitData.GetUnitInfo (unitId);
        NormalSkill firstSkill = null;
        NormalSkill secondSkill = null;
        if (ui.skill1 > 0) {
			firstSkill = DataCenter.Instance.BattleData.GetSkill(MakeUserUnitKey(),ui.skill1,SkillType.NormalSkill) as NormalSkill; //Skill[ui.skill1] as TNormalSkill;	
        }
        if (ui.skill2 > 0) {
			secondSkill = DataCenter.Instance.BattleData.GetSkill(MakeUserUnitKey(),ui.skill2,SkillType.NormalSkill) as NormalSkill; //.Skill[ui.skill2] as TNormalSkill;	
        }
        AddSkill(firstSkill, secondSkill);
    }

    public List<AttackInfo> CaculateAttack(List<uint> card, List<int> ignorSkillID) {
        List<uint> copyCard = new List<uint>(card);
        List<AttackInfo> returnInfo = new List<AttackInfo>();
        if (normalSkill[0] == null) {
            InitSkill();	
        }
		UnitInfo tui = DataCenter.Instance.UnitData.GetUnitInfo (unitId);
		UnitInfo ui = tui;
        for (int i = 0; i < normalSkill.Length; i++) {
            NormalSkill tns = normalSkill[i];
			if(tns == null) {
				continue;
			}
		
            tns.DisposeUseSkillID(ignorSkillID);
            int count = tns.CalculateCard(copyCard);
            for (int j = 0; j < count; j++) {
				AttackInfo attack = AttackInfo.GetInstance(); //new AttackInfo();
                attack.AttackValue = CaculateAttack(this, ui, tns);
                attack.AttackType = tns.AttackType;
                attack.UserUnitID = MakeUserUnitKey();
                tns.GetSkillInfo(attack);
                returnInfo.Add(attack);
            }
        }
        return returnInfo;
    }

	public List<AttackInfo> CaculateAttack(CalculateSkillUtility csu) {
		List<uint> copyCard = new List<uint>(csu.haveCard);
		List<AttackInfo> returnInfo = new List<AttackInfo>();
		if (normalSkill[0] == null) {
			InitSkill();	
		}
		UnitInfo tui = DataCenter.Instance.UnitData.GetUnitInfo (unitId);
		UnitInfo ui = tui;
		for (int i = 0; i < normalSkill.Length; i++) {
			NormalSkill tns = normalSkill[i];
			if(tns == null) {
				continue;
			}
//			Debug.LogError(tns.Blocks.Count + "  normalskill : " + tns.Blocks[0]);
			tns.DisposeUseSkillID(csu.alreadyUseSkill);
			int count = tns.CalculateCard(copyCard);
			for (int j = 0; j < count; j++) {
				csu.alreadyUseSkill.Add(tns);
				csu.ResidualCard();
				AttackInfo attack = AttackInfo.GetInstance(); //new AttackInfo();
				attack.AttackValue = CaculateAttack(this, ui, tns);
				attack.AttackType = tns.AttackType;
				attack.UserUnitID = MakeUserUnitKey();
				tns.GetSkillInfo(attack);
				returnInfo.Add(attack);
			}
		}
		return returnInfo;
	}

	public int CaculateNeedCard (CalculateSkillUtility csu) {
		if (normalSkill[0] == null) {
			InitSkill();	
		}

		if (csu.haveCard.Count == 5) {
			return -1;
		}

		int index = -1;
		foreach (var item in normalSkill) {
			if(item == null) {
				continue;
			}
			if(item.Blocks.Count == 1) {
				index = (int)item.Blocks[0];
				break;
			}
			else{
				index = DGTools.NeedOneTriggerSkill(csu.haveCard, item.Blocks);
//				if(item.Blocks.Count == 5) {
////					Debug.LogError(MakeUserUnitKey() + "   index : " + index);
//				}
				if(index != -1) {
					break;
				}
			}

		}
		return index;
	}

    AttackInfo strengthenInfo = null;
    void StrengthenTargetType(object data) {
        AttackInfo ai = data as AttackInfo;
        if (ai == null) {
            return;	
        }
		if (ai.AttackType >= 0 && ai.AttackType!=(int)EUnitType.UALL && ai.AttackType != UnitType) {
            return;	
        }
		if (ai.AttackRace >= 0 && ai.AttackRace!=(int)EUnitRace.ALL && ai.AttackRace != UnitRace) {
			return;	
		}

        if (ai.AttackRound == 0) {
            strengthenInfo = null;
            return;
        }
        strengthenInfo = ai;
    }

    void AddSkill(NormalSkill firstSkill, NormalSkill secondSkill) {
        if (firstSkill == null && secondSkill != null) {
            normalSkill[0] = secondSkill;
        }

        if (firstSkill != null && secondSkill == null) {
            normalSkill[0] = firstSkill;
        }

        if (firstSkill != null && secondSkill != null) {
            if (secondSkill.GetActiveBlocks() > firstSkill.GetActiveBlocks()) { // second skill first excute
                normalSkill[0] = secondSkill;
                normalSkill[1] = firstSkill;
            }
            else {
                normalSkill[0] = firstSkill;
                normalSkill[1] = secondSkill;
            }
        }
    }

    protected int CaculateAttack(UserUnit uu, UnitInfo ui, NormalSkill tns) {
		float attack = tns.GetAttack(Attack) * attackMultiple;

        if (strengthenInfo != null) {
            attack *= strengthenInfo.AttackValue;
        }
        int value = System.Convert.ToInt32(attack);
        return value;
    }

    public int UnitType {
        get {
            return (int)UnitInfo.type;
        }
    }

	public int UnitRace {
		get {
			return (int)UnitInfo.race;
		}
	}

    public int LeadSKill {
        get {
            return UnitInfo.leaderSkill;
        }
    }

    public int ActiveSkill {
        get {
            return UnitInfo.activeSkill;
        }
    }

    public int PassiveSkill {
        get {
            return UnitInfo.passiveSkill;
        }
    }

    public UnitInfo UnitInfo {
        get {
//			Debug.LogError("instance : " + instance.uniqueId + " ubitid : " + instance.unitId);
			return DataCenter.Instance.UnitData.GetUnitInfo(unitId); //UnitInfo[instance.unitId];
        }
    }

	public int MultipleDevorExp (UserUnit baseUser) {
		if (baseUser == null) {
			return UnitInfo.DevourExp * level;
		}
		else{
				return System.Convert.ToInt32 (DGTools.AllMultiple (baseUser, this) * UnitInfo.DevourExp * level);
		}
	}

	public int MultipleMaterialExp(UserUnit baseUser) {
		if (baseUser == null) {
			return UnitInfo.DevourExp * level;
		}
		else{
				return System.Convert.ToInt32 (DGTools.OnlyTypeMultiple (baseUser, this) * UnitInfo.DevourExp * level);
		}
	}

	public int GetLevelCurveValue(int lv, PowerInfo pi) {
//		Debug.LogError("TUserUnit.GetLevelCurveValue..");
		if( lv == 1) {
			return UnitInfo.GetCurveValue( lv+1, pi);
		}
		return UnitInfo.GetCurveValue( lv+1, pi) - UnitInfo.GetCurveValue( lv, pi);
	}

	public int GetTotalCurveValue(int lv, PowerInfo pi) {
//		int total = 0;
//		for (int i=1; i<=level; i++) {
//			total += UnitInfo.GetCurveValue( Level, pi);
//		}
//		Debug.LogError("TUserUnit.GetTotalCurveValue.. Level:"+lv);
		return UnitInfo.GetCurveValue( lv, pi);
	}

    public int CurExp {
        get {
			int curExp = GetLevelCurveValue( level, UnitInfo.powerType.expType );
//			Debug.LogError(">>>> Level:"+(Level+1)+" => exp:"+curExp);
			curExp -= NextExp;
//			Debug.LogError(">>>> exp - nextExp => curExp:"+curExp);
            return curExp;
        }
    }


    public int NextExp {
        get {
			int nextexp = GetTotalCurveValue( level+1, UnitInfo.powerType.expType) - exp;
//			Debug.LogError("TUserUnit.NextExp :: Level:"+Level+" nextexp:"+nextexp+" instance.exp:"+instance.exp);
			if (nextexp < 0 || level == UnitInfo.maxLevel)
                nextexp = 0;
            return nextexp;
        }
    }

    public int InitBlood {
        get {
//            UnitInfo ui = UnitInfo.Object;
            int blood = this.Hp;

            float temp = blood * hpMultiple;
            return System.Convert.ToInt32(blood);
        }
    }



    public int Blood {
        get {
			return InitBlood;
        }
    }

    public int AddNumber {
        get {
            return addHp + addAttack;
        }
    }

    public int Attack {
        get {
			return addHp * 5 + UnitInfo.GetCurveValue( level, UnitInfo.powerType.attackType );
        }
    }

    public int Hp {
        get {
			return addHp * 10 + UnitInfo.GetCurveValue( level, UnitInfo.powerType.hpType );
        }
    }

	public int CalculateATK(UnitInfo tui) {
		if (tui == null) {
			return 0;	
		}
		return addHp * 5 + tui.GetCurveValue (level, tui.powerType.attackType );
	}

	public int CalculateHP(UnitInfo tui) {
//		Debug.LogError ("CalculateHP tui : " + tui);
		if (tui == null) {
			return 0;
		}

		return addHp * 10 + tui.GetCurveValue (level, tui.powerType.hpType);
	}
	
}
}

//A wrapper to manage userUnitInfo list
