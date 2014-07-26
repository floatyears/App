using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TUnitParty : ProtobufDataBase, IComparer, ILeaderSkill {
    private List<PartyItem> partyItem = new List<PartyItem>();		
 
    private UnitParty instance;
	public TUnitParty(object ins) : base (ins) { 
		this.instance = ins as UnitParty;
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
                
        foreach (TUserUnit tUserUnit in UserUnit.Values) {
            if (tUserUnit == null) {
                continue;
            }
            if (uniqueId == tUserUnit.ID) {
                return true;
            }
        }
        return false;
    }

	private List<TNormalSkill> normalSkill = new List<TNormalSkill>();

	private Dictionary<string,ProtobufDataBase> leaderSkill = new Dictionary<string,ProtobufDataBase>();
    public Dictionary<string,ProtobufDataBase> LeadSkill {
        get { return leaderSkill; }
    }

    private Dictionary<int,TUserUnit> userUnit;
    public Dictionary<int,TUserUnit> UserUnit {
        get {
            if (userUnit == null) {
                userUnit = new Dictionary<int,TUserUnit>();
                for (int i = 0; i < partyItem.Count; i++) {
                    TUserUnit uui = DataCenter.Instance.UserUnitList.GetMyUnit(partyItem[i].unitUniqueId);
                    userUnit.Add(partyItem[i].unitPos, uui);
                }
            }
            return userUnit;
        }
    }

	void EnterBattle(object data) {
		if (ConfigBattleUseData.Instance.BattleFriend == null) {
			UserUnit.Add(DataCenter.friendPos, null);
		}
		else {
			if (ID == DataCenter.Instance.PartyInfo.CurrentPartyId) {
				TUserUnit tuu = ConfigBattleUseData.Instance.BattleFriend.UserUnit;
				DataCenter.Instance.UserUnitList.Add(tuu.userID, tuu.ID, tuu);
				UserUnit.Add(DataCenter.friendPos, tuu);
				
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
		if (UserUnit.ContainsKey (DataCenter.friendPos)) {
			UserUnit.Remove(DataCenter.friendPos);
		}
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
        List<TUserUnit> uu = GetUserUnit();
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

        foreach (PartyItem item in instance.items) {
            if (item.unitPos >= uu.Count) {
                LogHelper.LogError("  Calculate party attack: INVALID item.unitPos{0} > count:{1}", item.unitPos, uu.Count);
                continue;
            }
            if (uu[item.unitPos] == null) //it's empty party item
                continue;

			TUnitInfo ui = uu[item.unitPos].UnitInfo;

            EUnitType unitType = ui.Type;

			if ( typeAttackValue.ContainsKey(unitType) ) {
				typeAttackValue[unitType] += uu[item.unitPos].Attack;
			}else {
				Debug.LogError("FATAL ERROR: unknown unitType:"+unitType+" in typeAttackValue.");
			}
            
            totalHp += uu[item.unitPos].Hp;
            totalCost += uu[item.unitPos].UnitInfo.Cost;
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

		foreach (var item in UserUnit.Values) {
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
			if(item.Value == null) {
				LogHelper.Log("skip empty partyItem:"+item.Key);
				continue;
			}

			if(item.Value.CaculateNeedCard(csu) == index) {
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
        TUserUnit tempUnitInfo;
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

		foreach (var item in UserUnit) {
			if(item.Value == null) {
				LogHelper.Log("skip empty partyItem:"+item.Key);
				continue;
			}
			tempAttack.AddRange(item.Value.CaculateAttack(skillUtility));
			if (tempAttack.Count > 0) {
				for (int j = 0; j < tempAttack.Count; j++) {
					AttackInfo ai = tempAttack[j];
					ai.UserPos = item.Key;

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
//		Debug.LogError ("CalculateAttackCount attackCount : " + attackCount);

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
        for (int i 		= 0; i < instance.items.Count; i++) {
            partyItem.Add(instance.items[i]);
        }

        GetLeaderSkill();
        DGTools.InsertSort<PartyItem,IComparer>(partyItem, this);
    }

	public List<TNormalSkill> GetNormalSkill() {
		List<TNormalSkill> temp = new List<TNormalSkill>();
		TUserUnit tuu;
		foreach (var item in instance.items) {
			uint id = item.unitUniqueId;
			tuu = DataCenter.Instance.UserUnitList.GetMyUnit(id);
			if(tuu != null)
				temp.AddRange(tuu.GetNormalSkill());
		}                

		if (ConfigBattleUseData.Instance.BattleFriend != null) {
			tuu = ConfigBattleUseData.Instance.BattleFriend.UserUnit;
			temp.AddRange(tuu.GetNormalSkill());
		}
		return temp;
	}

	
    void GetLeaderSkill() {
        if (instance.items.Count > 0) {
            uint id = instance.items[0].unitUniqueId;
			TUserUnit tuu = DataCenter.Instance.UserUnitList.GetMyUnit(id);
			AddLeadSkill(tuu);
        }

		if (ConfigBattleUseData.Instance.BattleFriend != null) {
			TUserUnit tuu = ConfigBattleUseData.Instance.BattleFriend.UserUnit;
			AddLeadSkill(tuu);
		}
    }

    void AddLeadSkill(TUserUnit tuu) {
		if (tuu == null) {
			return;
		}
		string uniqueID = tuu.MakeUserUnitKey();
		ProtobufDataBase pdb = DataCenter.Instance.GetSkill (tuu.MakeUserUnitKey (), tuu.LeadSKill, SkillType.LeaderSkill); //Skill[tuu.LeadSKill];
			if (leaderSkill.ContainsKey(uniqueID)) {
			leaderSkill[uniqueID] = pdb;
	    }
	    else {
			leaderSkill.Add(uniqueID, pdb);
	    }
    }
	
    public UnitParty Object {
        get { return instance; }
    } 

    public int ID { //party id
        get { return instance.id; }
    }

    public void SetPartyItem(int pos, uint unitUniqueId) {

        if (pos < instance.items.Count) {
            PartyItem item = new PartyItem();
            item.unitPos = pos;
            item.unitUniqueId = unitUniqueId;

            //update instance and userUnit
            instance.items[item.unitPos] = item;
            if (userUnit != null && userUnit.ContainsKey(item.unitPos))
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
		foreach (var item in UserUnit.Values) {
			if(item != null)
			bloodNum += item.InitBlood;
		}
        return bloodNum;
    }
	
    public int GetBlood() {
        int bloodNum = 0;
		foreach (var item in UserUnit.Values) {
			bloodNum += item.Blood;
		}
        return bloodNum;
    }
	
    public Dictionary<int,uint> GetPartyItem() {
        Dictionary<int,uint> temp = new Dictionary<int, uint>();
        for (int i = 0; i < instance.items.Count; i++) {
            PartyItem pi = instance.items[i];
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
        TUserUnit tuu = DataCenter.Instance.UserUnitList.GetMyUnit(pi.unitUniqueId);
        if (tuu == null) {
            return null;
        }

        UnitInfo ui1 = tuu.UnitInfo.Object;		
		if (ui1.skill2 == 0) {
			return null;		
		}
		TNormalSkill ns = DataCenter.Instance.GetSkill (tuu.MakeUserUnitKey (), ui1.skill2, SkillType.NormalSkill) as TNormalSkill; //Skill[ui1.skill2] as TNormalSkill;
		if (ns == null) {
			return null;	
		} 
		else {
			return ns.Object;
		}
    }
	
    public List<TUserUnit> GetUserUnit() {
        List<TUserUnit> temp = new List<TUserUnit>();
        foreach (var item in instance.items) {
			TUserUnit tuu = DataCenter.Instance.UserUnitList.GetMyUnit(item.unitUniqueId);
			temp.Add(tuu);
		}
        return temp;
    }

    public SkillBase GetLeaderSkillInfo() {
        TUserUnit uui = DataCenter.Instance.UserUnitList.GetMyUnit(instance.items[0].unitUniqueId);
        if (uui == null)
            return null;

		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (uui.MakeUserUnitKey (), uui.LeadSKill, SkillType.LeaderSkill); //Skill[uui.LeadSKill];
        if (sbi == null)
            return null;
        return sbi.GetSkillInfo();
    }
}