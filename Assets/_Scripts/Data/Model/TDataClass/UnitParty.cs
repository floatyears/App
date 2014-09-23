using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class UnitParty : ProtoBuf.IExtensible,IComparer{
    private List<PartyItem> partyItem = new List<PartyItem>();		
 
	public UnitParty() { 
		
    }

	public void Init(){
		AddListener ();
		reAssignData();
		GetSkillCollection();
	}

	public void AddListener() {
		MsgCenter.Instance.AddListener(CommandEnum.ActiveReduceHurt, ReduceHurt);      
		MsgCenter.Instance.AddListener (CommandEnum.EnterBattle, EnterBattle);
		MsgCenter.Instance.AddListener (CommandEnum.LeftBattle, LeftBattle);
	}

    public void RemoveListener() {
		MsgCenter.Instance.RemoveListener (CommandEnum.LeftBattle, LeftBattle);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnterBattle, EnterBattle);
        MsgCenter.Instance.RemoveListener(CommandEnum.ActiveReduceHurt, ReduceHurt);
    }

    public bool HasUnit(uint uniqueId) {
        if (UserUnit == null) {
            return false;
        }
                
        foreach (UserUnit tUserUnit in UserUnit) {
            if (tUserUnit == null) {
                continue;
            }

            if (uniqueId == tUserUnit.uniqueId) {
                return true;
            }
        }
        return false;
    }

	private List<NormalSkill> normalSkill = new List<NormalSkill>();

	private Dictionary<string,SkillBase> leaderSkill = new Dictionary<string,SkillBase>();
	public Dictionary<string,SkillBase> LeadSkill {
        get { return leaderSkill; }
    }

    private List<UserUnit> userUnit;
	public List<UserUnit> UserUnit {
        get {
            if (userUnit == null) {
				userUnit = new List<UserUnit>(){null,null,null,null,null};
                for (int i = 0; i < partyItem.Count; i++) {
					userUnit[partyItem[i].unitPos] = DataCenter.Instance.UserUnitList.GetMyUnit(partyItem[i].unitUniqueId);
                }
            }
            return userUnit;
        }
    }

	void EnterBattle(object data) {
		if (BattleConfigData.Instance.BattleFriend == null) {
			UserUnit[DataCenter.friendPos] = null;
		}
		else {
			if (id == DataCenter.Instance.PartyInfo.CurrentPartyId) {
				UserUnit tuu = BattleConfigData.Instance.BattleFriend.UserUnit;
				DataCenter.Instance.UserUnitList.Add(tuu.userID, tuu.uniqueId, tuu);
				UserUnit[DataCenter.friendPos] = tuu;
				
				cardCount ++;
			}
		}

		for (int i = 0; i < partyItem.Count; i++) {
			if(partyItem[i].unitUniqueId > 0) {
				cardCount ++;
			}
		}
	}

	void LeftBattle (object data) {
		UserUnit[DataCenter.friendPos] = null;
	}

    AttackInfo reduceHurt = null;

    private int totalHp = 0;
    private int totalCost = 0;
    private Dictionary<EUnitType, int>  typeAttackValue = new Dictionary<EUnitType, int>();

    private void initTypeAtk(Dictionary<EUnitType, int> atkVal, EUnitType type) {
        if (!atkVal.ContainsKey(type))
            atkVal.Add(type, 0);
        else
            atkVal[type] = 0;
    }

    private void reAssignData() {
        List<UserUnit> uu = GetUserUnit();
        if (uu == null) {
            Debug.LogError("TUnitParty.AssignData :: GetUserUnit() return null.");
            return;
        }

        totalHp = totalCost = 0;
        initTypeAtk(typeAttackValue, EUnitType.UWIND);
        initTypeAtk(typeAttackValue, EUnitType.UFIRE);
        initTypeAtk(typeAttackValue, EUnitType.UWATER);
        initTypeAtk(typeAttackValue, EUnitType.ULIGHT);
        initTypeAtk(typeAttackValue, EUnitType.UDARK);
        initTypeAtk(typeAttackValue, EUnitType.UNONE);

        foreach (PartyItem item in items) {
            if (item.unitPos >= uu.Count) {
                LogHelper.LogError("  Calculate party attack: INVALID item.unitPos{0} > count:{1}", item.unitPos, uu.Count);
                continue;
            }
            if (uu[item.unitPos] == null) //it's empty party item
                continue;

			UnitInfo ui = uu[item.unitPos].UnitInfo;

            EUnitType unitType = ui.type;

			if ( typeAttackValue.ContainsKey(unitType) ) {
				typeAttackValue[unitType] += uu[item.unitPos].Attack;
			}else {
				Debug.LogError("FATAL ERROR: unknown unitType:"+unitType+" in typeAttackValue.");
			}
            
            totalHp += uu[item.unitPos].Hp;
            totalCost += uu[item.unitPos].UnitInfo.cost;
        }
    }

    public Dictionary<EUnitType, int> TypeAttack { get { return typeAttackValue; } }
    public int TotalHp		{ get { return totalHp; } }
	public int GetTotalAtk()		{ 
		int total = 0;
		foreach (var item in typeAttackValue){
			total += item.Value;
		}
		return total;
	}
    public int TotalCost	{ get { return totalCost; } }

    //skill sort
    private CalculateRecoverHP crh ;
    /// <summary>
    /// key is area item. value is skill list. this area already use skill must record in this dic, avoidance redundant calculate.
    /// </summary>
    private Dictionary<int, CalculateSkillUtility> alreadyUse = new Dictionary<int, CalculateSkillUtility>();	 
    private Dictionary<int, List<AttackInfo>> attack = new Dictionary<int, List<AttackInfo>>();
    public Dictionary<int, List<AttackInfo>> Attack {
        get { return attack;}
    }

    void ReduceHurt(object data) {
        reduceHurt = data as AttackInfo;
        if (reduceHurt != null) {
            if (reduceHurt.AttackRound == 0) {
                reduceHurt = null;
            }
        }
    }

	private int cardCount = 0;
    public int CaculateInjured(int attackType, float attackValue) {
		float Proportion = 1f / cardCount;
        float attackV = attackValue * Proportion;
        float hurtValue = 0;

		foreach (var item in UserUnit) {
			if(item != null) {
				hurtValue += item.CalculateInjured(attackType, attackV);
			}
		}

        if (reduceHurt != null) {
            float value = hurtValue * reduceHurt.AttackValue;
            hurtValue -= value;
        }

        return System.Convert.ToInt32(hurtValue);
    }

	public bool CalculateNeedCard(int areaItemID, int index) {
		if (crh == null) {
			crh = new CalculateRecoverHP();		
		}

		CalculateSkillUtility csu = CheckSkillUtility (areaItemID);
		if (csu.haveCard.Count == 5) {
			return false;	
		}

		if (crh.RecoverHPNeedCard (csu) == index) {
			return true;	
		}

		foreach (var item in UserUnit) {
			if(item == null) {
				LogHelper.Log("skip empty partyItem:"+item);
				continue;
			}

			if(item.CaculateNeedCard(csu) == index) {
				return true;
			}
		}
		return false;
	}
	
	public List<AttackInfo> CalculateSkill(int areaItemID, int cardID, int blood, float boostValue = 1f) {
        if (crh == null) {
            crh = new CalculateRecoverHP();
        }

        CalculateSkillUtility skillUtility = CheckSkillUtility(areaItemID, cardID);
        List<AttackInfo> areaItemAttackInfo = CheckAttackInfo(areaItemID);
        areaItemAttackInfo.Clear();
        UserUnit tempUnitInfo;
        List<AttackInfo> tempAttack = new List<AttackInfo>();
		List<AttackInfo> tempAttackType = new List<AttackInfo>();

		//===== calculate fix recover hp.
		AttackInfo recoverHp = crh.RecoverHP(skillUtility, blood);
		if (recoverHp != null) {
			recoverHp.AttackValue *= boostValue;
			recoverHp.UserUnitID = UserUnit[0].MakeUserUnitKey();
			recoverHp.UserPos = 0; // 0 == self leder position
			tempAttack.Add(recoverHp);
		}
		int len = UserUnit.Count;
		for (int i = 0; i < len; i++) {
			var item = UserUnit[i];
			if(item == null) {
				LogHelper.Log("skip empty partyItem:"+item);
				continue;
			}
			tempAttack.AddRange(item.CaculateAttack(skillUtility));
			if (tempAttack.Count > 0) {
				for (int j = 0; j < tempAttack.Count; j++) {
					AttackInfo ai = tempAttack[j];
					ai.UserPos = i;

					ai.AttackValue *= boostValue;

					areaItemAttackInfo.Add(ai);
					tempAttackType.Add(ai);
				}
			}
			tempAttack.Clear();
		}
		CalculateAttackCount ();
        return tempAttackType;
    }
	int prevAttackCount = 0;
	void CalculateAttackCount() {
		int attackCount = 0;
		foreach (var item in attack) {
			if(item.Value != null) {
				attackCount += item.Value.Count;
			}
		}

		if (attackCount == 0 || attackCount == prevAttackCount) {
			return;	
		}

		prevAttackCount = attackCount;

		switch (attackCount) {
			case 1:
				AudioManager.Instance.PlayAudio(AudioEnum.sound_attack_increase_1);
				break;
			case 2:
				AudioManager.Instance.PlayAudio(AudioEnum.sound_attack_increase_2);
				break;
			case 3:
				AudioManager.Instance.PlayAudio(AudioEnum.sound_attack_increase_3);
				break;
			case 4:
				AudioManager.Instance.PlayAudio(AudioEnum.sound_attack_increase_4);
				break;
			case 5:
				AudioManager.Instance.PlayAudio(AudioEnum.sound_attack_increase_5);
				break;
			case 6:
				AudioManager.Instance.PlayAudio(AudioEnum.sound_attack_increase_6);
				break;
			default:
				AudioManager.Instance.PlayAudio(AudioEnum.sound_attack_increase_7);
				break;
		}
	}

    public void ClearData() {
        AttackInfo.ClearData();
        alreadyUse.Clear();
        attack.Clear();
    }
	
    public void GetSkillCollection() {
        partyItem = new List<PartyItem>();
        for (int i 		= 0; i < items.Count; i++) {
            partyItem.Add(items[i]);
        }

        GetLeaderSkill();
        DGTools.InsertSort<PartyItem,IComparer>(partyItem, this);
    }

	public List<NormalSkill> GetNormalSkill() {
		List<NormalSkill> temp = new List<NormalSkill>();
		UserUnit tuu;
		foreach (var item in items) {
			uint id = item.unitUniqueId;
			tuu = DataCenter.Instance.UserUnitList.GetMyUnit(id);
			if(tuu != null)
				temp.AddRange(tuu.GetNormalSkill());
		}                

		if (BattleConfigData.Instance.BattleFriend != null) {
			tuu = BattleConfigData.Instance.BattleFriend.UserUnit;
			temp.AddRange(tuu.GetNormalSkill());
		}
		return temp;
	}

	
    void GetLeaderSkill() {
        if (items.Count > 0) {
            uint id = items[0].unitUniqueId;
			UserUnit tuu = DataCenter.Instance.UserUnitList.GetMyUnit(id);
			AddLeadSkill(tuu);
        }

		if (BattleConfigData.Instance.BattleFriend != null) {
			UserUnit tuu = BattleConfigData.Instance.BattleFriend.UserUnit;
			AddLeadSkill(tuu);
		}
    }

    void AddLeadSkill(UserUnit tuu) {
		if (tuu == null) {
			return;
		}
		string uniqueID = tuu.MakeUserUnitKey();
		SkillBase pdb = DataCenter.Instance.GetSkill (tuu.MakeUserUnitKey (), tuu.LeadSKill, SkillType.LeaderSkill); //Skill[tuu.LeadSKill];
			if (leaderSkill.ContainsKey(uniqueID)) {
			leaderSkill[uniqueID] = pdb;
	    }
	    else {
			leaderSkill.Add(uniqueID, pdb);
	    }
    }
	
    public void SetPartyItem(int pos, uint unitUniqueId) {

        if (pos < items.Count) {
            PartyItem item = new PartyItem();
            item.unitPos = pos;
            item.unitUniqueId = unitUniqueId;

            //update instance and userUnit
            items[item.unitPos] = item;
            if (userUnit != null && userUnit[item.unitPos] != null)
                userUnit[item.unitPos] = DataCenter.Instance.UserUnitList.GetMyUnit(item.unitUniqueId);
            //LogHelper.LogError(" SetPartyItem:: => pos:{0} uniqueId:{1}",item.unitPos, item.unitUniqueId);
            this.reAssignData();
        }
        else {
            //LogHelper.LogError ("SetPartyItem :: item.unitPos:{0} is invalid.", pos);
        }
    }

    public int Compare(object first, object second) {
        PartyItem firstUU = (PartyItem)first;
        PartyItem secondUU = (PartyItem)second;
	
        NormalSkill ns1 = GetSecondSkill(firstUU);
        NormalSkill ns2 = GetSecondSkill(secondUU);
        if (ns1 == null && ns2 == null)
            return 0;
        else if (ns1 == null)
            return 1;
        else
            return -1;
        return ns1.activeBlocks.Count.CompareTo(ns2.activeBlocks.Count);
    }
	
    public int GetInitBlood() {
        int bloodNum = 0;
		foreach (var item in UserUnit) {
			if(item != null)
			bloodNum += item.InitBlood;
		}
        return bloodNum;
    }
	
    public int GetBlood() {
        int bloodNum = 0;
		foreach (var item in UserUnit) {
			bloodNum += item.Blood;
		}
        return bloodNum;
    }
	
    public Dictionary<int,uint> GetPartyItem() {
        Dictionary<int,uint> temp = new Dictionary<int, uint>();
        for (int i = 0; i < items.Count; i++) {
            PartyItem pi = items[i];
            temp.Add(pi.unitPos, pi.unitUniqueId);
        }
        return temp;
    }

	CalculateSkillUtility CheckSkillUtility(int areaItemID) {
		CalculateSkillUtility skillUtility;	
		if (!alreadyUse.TryGetValue(areaItemID, out skillUtility)) {
			skillUtility = new CalculateSkillUtility();
			alreadyUse.Add(areaItemID, skillUtility);
		}
		return skillUtility;
	}

    CalculateSkillUtility CheckSkillUtility(int areaItemID, int cardID) {
        CalculateSkillUtility skillUtility;												//-- find or creat  have card and use skill record data
        if (!alreadyUse.TryGetValue(areaItemID, out skillUtility)) {
            skillUtility = new CalculateSkillUtility();
            alreadyUse.Add(areaItemID, skillUtility);
        }
        skillUtility.haveCard.Add((uint)cardID);
        return skillUtility;
    }
	
    List<AttackInfo> CheckAttackInfo(int areaItemID) {
        List<AttackInfo> areaItemAttackInfo = null;										//-- find or creat attack data;
        if (!attack.TryGetValue(areaItemID, out areaItemAttackInfo)) {
            areaItemAttackInfo = new List<AttackInfo>();
            attack.Add(areaItemID, areaItemAttackInfo);
        }
        return areaItemAttackInfo;
    }
	
    NormalSkill GetSecondSkill(PartyItem pi) {
        UserUnit tuu = DataCenter.Instance.UserUnitList.GetMyUnit(pi.unitUniqueId);
        if (tuu == null) {
            return null;
        }

        UnitInfo ui1 = tuu.UnitInfo;		
		if (ui1.skill2 == 0) {
			return null;		
		}
		NormalSkill ns = DataCenter.Instance.GetSkill (tuu.MakeUserUnitKey (), ui1.skill2, SkillType.NormalSkill) as NormalSkill; //Skill[ui1.skill2] as TNormalSkill;
		if (ns == null) {
			return null;	
		} 
		else {
			return ns;
		}
    }
	
    public List<UserUnit> GetUserUnit() {
        List<UserUnit> temp = new List<UserUnit>();
        foreach (var item in items) {
//			Debug.LogError("GetUserUnit befoure : " + ID + " item.unitUniqueId : " + item.unitUniqueId + "  item.pos : " + item.unitPos);
			UserUnit tuu = DataCenter.Instance.UserUnitList.GetMyUnit(item.unitUniqueId);
//			Debug.LogError("GetUserUnit end : " + tuu + "     ---------");
			temp.Add(tuu);
		}
        return temp;
    }

    public SkillBase GetLeaderSkillInfo() {
        UserUnit uui = DataCenter.Instance.UserUnitList.GetMyUnit(items[0].unitUniqueId);
        if (uui == null)
            return null;

		SkillBase sbi = DataCenter.Instance.GetSkill (uui.MakeUserUnitKey (), uui.LeadSKill, SkillType.LeaderSkill); //Skill[uui.LeadSKill];
        return sbi;
    }

	public UserUnit GetPartyItem(int index){
		if (index < 0 || index >= partyItem.Count)
						return null;
		Debug.LogError ("index : " + index + "instance.items [index].unitUniqueId : " + items [index].unitUniqueId); 
		for (int i = 0; i < items.Count; i++) {
			Debug.LogError("GetPartyItem i : " + i + " instance.items : " + items[i]);
				}
	 	return DataCenter.Instance.UserUnitList.GetMyUnit( items [index].unitUniqueId);
	}
	}
}