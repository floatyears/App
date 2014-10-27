using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class UnitParty : ProtoBuf.IExtensible{
 
	public UnitParty(int dummy=0) { 
		
    }

	public void Init(){
		reAssignData();
		GetSkillCollection();
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
                for (int i = 0; i < items.Count; i++) {
						userUnit[items[i].unitPos] = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(items[i].unitUniqueId);
                }
            }
            return userUnit;
        }
    }

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

	
    public void GetSkillCollection() {


		if (items.Count > 0) {
			uint id = items[0].unitUniqueId;
			UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(id);
			AddLeadSkill(tuu);
		}
		
		if (BattleConfigData.Instance.BattleFriend != null) {
			UserUnit tuu = BattleConfigData.Instance.BattleFriend.UserUnit;
			AddLeadSkill(tuu);
		}

		items.Sort ((first,second)=>{
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
		});
    }

	public List<NormalSkill> GetNormalSkill() {
		List<NormalSkill> temp = new List<NormalSkill>();
		UserUnit tuu;
		foreach (var item in items) {
			uint id = item.unitUniqueId;
			tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(id);
			if(tuu != null)
				temp.AddRange(tuu.GetNormalSkill());
		}                

		if (BattleConfigData.Instance.BattleFriend != null) {
			tuu = BattleConfigData.Instance.BattleFriend.UserUnit;
			temp.AddRange(tuu.GetNormalSkill());
		}
		return temp;
	}

    void AddLeadSkill(UserUnit tuu) {
		if (tuu == null) {
			return;
		}
		string uniqueID = tuu.MakeUserUnitKey();
		SkillBase pdb = DataCenter.Instance.BattleData.GetSkill (tuu.MakeUserUnitKey (), tuu.LeadSKill, SkillType.LeaderSkill); //Skill[tuu.LeadSKill];
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
                userUnit[item.unitPos] = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(item.unitUniqueId);
            //LogHelper.LogError(" SetPartyItem:: => pos:{0} uniqueId:{1}",item.unitPos, item.unitUniqueId);
            this.reAssignData();
        }
        else {
            //LogHelper.LogError ("SetPartyItem :: item.unitPos:{0} is invalid.", pos);
        }
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


    NormalSkill GetSecondSkill(PartyItem pi) {
        UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(pi.unitUniqueId);
        if (tuu == null) {
            return null;
        }

        UnitInfo ui1 = tuu.UnitInfo;		
		if (ui1.skill2 == 0) {
			return null;		
		}
		NormalSkill ns = DataCenter.Instance.BattleData.GetSkill (tuu.MakeUserUnitKey (), ui1.skill2, SkillType.NormalSkill) as NormalSkill; //Skill[ui1.skill2] as TNormalSkill;
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
			UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(item.unitUniqueId);
//			Debug.LogError("GetUserUnit end : " + tuu + "     ---------");
			temp.Add(tuu);
		}
        return temp;
    }

    public SkillBase GetLeaderSkillInfo() {
        UserUnit uui = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(items[0].unitUniqueId);
        if (uui == null)
            return null;

		SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (uui.MakeUserUnitKey (), uui.LeadSKill, SkillType.LeaderSkill); //Skill[uui.LeadSKill];
        return sbi;
    }

	public UserUnit GetPartyItem(int index){
		if (index < 0 || index >= items.Count)
						return null;
		Debug.LogError ("index : " + index + "instance.items [index].unitUniqueId : " + items [index].unitUniqueId); 
		for (int i = 0; i < items.Count; i++) {
			Debug.LogError("GetPartyItem i : " + i + " instance.items : " + items[i]);
				}
	 	return DataCenter.Instance.UnitData.UserUnitList.GetMyUnit( items [index].unitUniqueId);
	}
	}
}