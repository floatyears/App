using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TUnitParty : ProtobufDataBase, IComparer, ILeaderSkill {
    private List<PartyItem> partyItem = new List<PartyItem>();		
 
    private UnitParty instance;
    public TUnitParty(object instance) : base (instance) { 
        this.instance = instance as UnitParty;
        MsgCenter.Instance.AddListener(CommandEnum.ActiveReduceHurt, ReduceHurt);      
		MsgCenter.Instance.AddListener (CommandEnum.EnterBattle, EnterBattle);
		MsgCenter.Instance.AddListener (CommandEnum.LeftBattle, LeftBattle);
        reAssignData();
        GetSkillCollection();
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
		UserUnit.Add(DataCenter.friendPos, DataCenter.Instance.BattleFriend.UserUnit);
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

//			Debug.LogError("uu[item.unitPos].UnitInfo="+uu[item.unitPos].UnitInfo);
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
	
    public int CaculateInjured(int attackType, float attackValue) {
        float Proportion = 1f / (float)partyItem.Count;
        float attackV = attackValue * Proportion;
        float hurtValue = 0;
        for (int i = 0; i < partyItem.Count; i++) {
			if(partyItem[i]==null || partyItem[i].unitUniqueId==0) {
				continue;
			}
            TUserUnit unitInfo = DataCenter.Instance.UserUnitList.GetMyUnit(partyItem[i].unitUniqueId);
            hurtValue += unitInfo.CalculateInjured(attackType, attackV);
        }
		
        if (reduceHurt != null) {
            float value = hurtValue * reduceHurt.AttackValue;
            hurtValue -= value;
        }
		
        return System.Convert.ToInt32(hurtValue);
    }
	
    public List<AttackImageUtility> CalculateSkill(int areaItemID, int cardID, int blood) {
        if (crh == null) {
            crh = new CalculateRecoverHP();		
        }
        CalculateSkillUtility skillUtility = CheckSkillUtility(areaItemID, cardID);
        List<AttackInfo> areaItemAttackInfo = CheckAttackInfo(areaItemID);
        areaItemAttackInfo.Clear();
        TUserUnit tempUnitInfo;
        List<AttackInfo> tempAttack = new List<AttackInfo>();		
        List<AttackImageUtility> tempAttackType = new List<AttackImageUtility>();
		
        for (int i = 0; i < partyItem.Count; i++) {
			if (partyItem[i]==null || partyItem[i].unitUniqueId == 0 ){
				LogHelper.Log("skip empty partyItem:"+i+" partyItem[i]:"+partyItem[i]);
				continue;
			}
            if (i == 0) {
                AttackInfo recoverHp = crh.RecoverHP(skillUtility.haveCard, skillUtility.alreadyUseSkill, blood);
                if (recoverHp != null) {
					TUserUnit tuu = DataCenter.Instance.MyUnitList.GetMyUnit(partyItem[i].unitUniqueId);
					recoverHp.UserUnitID = tuu.MakeUserUnitKey();
                    recoverHp.UserPos = partyItem[i].unitPos;
                    tempAttack.Add(recoverHp);
                }
            }
            tempUnitInfo = DataCenter.Instance.UserUnitList.GetMyUnit(partyItem[i].unitUniqueId);
            tempAttack.AddRange(tempUnitInfo.CaculateAttack(skillUtility.haveCard, skillUtility.alreadyUseSkill));
            if (tempAttack.Count > 0) {
                for (int j = 0; j < tempAttack.Count; j++) {
                    AttackInfo ai = tempAttack[j];
                    ai.UserPos = partyItem[i].unitPos;
                    areaItemAttackInfo.Add(ai);
                    skillUtility.alreadyUseSkill.Add(ai.SkillID);
                    AttackImageUtility aiu = new AttackImageUtility();
                    aiu.attackProperty = ai.AttackType;

					if ( DataCenter.Instance.UserUnitList.GetMyUnit(ai.UserUnitID) == null)
						Debug.LogError("DataCenter.Instance.UserUnitList.GetMyUnit(ai.UserUnitID)== null");

                    aiu.userProperty = DataCenter.Instance.UserUnitList.GetMyUnit(ai.UserUnitID).UnitType;
                    aiu.skillID = ai.SkillID;
                    aiu.attackID = ai.AttackID;
                    tempAttackType.Add(aiu);
                }     
            }
            tempAttack.Clear();
        }
		
        return tempAttackType;
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
	
    void GetLeaderSkill() {
        if (instance.items.Count > 0) {
            uint id = instance.items[0].unitUniqueId;
			TUserUnit tuu = DataCenter.Instance.MyUnitList.GetMyUnit(instance.items[0].unitUniqueId);
			AddLeadSkill(tuu);
        }
		if (DataCenter.Instance.BattleFriend != null) {
			ProtobufDataBase pdb = DataCenter.Instance.Skill[DataCenter.Instance.BattleFriend.UserUnit.LeadSKill];
			TUserUnit tuu = DataCenter.Instance.BattleFriend.UserUnit;
			AddLeadSkill(tuu);
		}
    }

    void AddLeadSkill(TUserUnit tuu) {
		if (tuu == null) {
			return;
		}
		string uniqueID = tuu.MakeUserUnitKey();
		ProtobufDataBase pdb = DataCenter.Instance.Skill[tuu.LeadSKill];
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
//		UnitParty up = DeserializeData<UnitParty> ();
        int bloodNum = 0;
        for (int i = 0; i < instance.items.Count; i++) {
            uint unitUniqueID = instance.items[i].unitUniqueId;
			if ( unitUniqueID == 0 ) {
				continue;
			}
			TUserUnit uu = DataCenter.Instance.UserUnitList.GetMyUnit(unitUniqueID);
			if (uu != null ) {
				bloodNum += uu.InitBlood;
			}
        }
        return bloodNum;
    }
	
    public int GetBlood() {
//		UnitParty up = DeserializeData<UnitParty> ();
        int bloodNum = 0;
        for (int i = 0; i < instance.items.Count; i++) {
            uint unitUniqueID = instance.items[i].unitUniqueId;
            bloodNum += DataCenter.Instance.UserUnitList.GetMyUnit(unitUniqueID).Blood;
        }
        return bloodNum;
    }
	
    public Dictionary<int,uint> GetPartyItem() {
        Dictionary<int,uint> temp = new Dictionary<int, uint>();
//		UnitParty up = DeserializeData<UnitParty> ();
        for (int i = 0; i < instance.items.Count; i++) {
            PartyItem pi = instance.items[i];
            temp.Add(pi.unitPos, pi.unitUniqueId);
//			Debug.LogError("pi.unitPos : " + pi.unitPos + " pi.unitUniqueId : " + pi.unitUniqueId);
        }
        return temp;
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
        UserUnit uu1 = tuu.Object;
        UnitInfo ui1 = tuu.UnitInfo.Object;			 //DataCenter.Instance.UnitInfo[uu1.unitId].DeserializeData<UnitInfo>();
        TNormalSkill ns = DataCenter.Instance.Skill[ui1.skill2] as TNormalSkill;
        return ns.Object;
    }
	
    public List<TUserUnit> GetUserUnit() {
        List<TUserUnit> temp = new List<TUserUnit>();
        foreach (var item in instance.items) {
			TUserUnit tuu = DataCenter.Instance.MyUnitList.GetMyUnit(item.unitUniqueId);
			temp.Add(tuu);
		}
        return temp;
    }

    public SkillBase GetLeaderSkillInfo() {
        TUserUnit uui = DataCenter.Instance.UserUnitList.GetMyUnit(instance.items[0].unitUniqueId);
        if (uui == null)
            return null;

        SkillBaseInfo sbi = DataCenter.Instance.Skill[uui.LeadSKill];
        if (sbi == null)
            return null;
        return sbi.GetSkillInfo();
    }

//    public Dictionary<int, TUserUnit> GetPosUnitInfo() {
//        Dictionary<int,TUserUnit> temp = new Dictionary<int,TUserUnit>();
//        foreach (var item in instance.items) {
//            TUserUnit uui = DataCenter.Instance.UserUnitList.GetMyUnit(item.unitUniqueId);
//            temp.Add(item.unitPos, uui);
//        }
//        return temp;
//    }
}