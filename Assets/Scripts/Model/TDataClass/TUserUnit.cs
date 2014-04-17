using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TUserUnit : ProtobufDataBase {
    private UserUnit instance;

    public UserUnit Unit {
        get { return instance;}
    }

    public TUserUnit(UserUnit instance) : base (instance) { 
        MsgCenter.Instance.AddListener(CommandEnum.StrengthenTargetType, StrengthenTargetType);
        this.instance = instance as UserUnit;
    } 

	public static TUserUnit GetUserUnit(uint id, UserUnit uu)  {
		TUserUnit tuu =  new TUserUnit(uu);
		tuu.userID = id;
		return tuu;
	}
		
    public void RemovevListener() {
        MsgCenter.Instance.RemoveListener(CommandEnum.StrengthenTargetType, StrengthenTargetType);
    }

    private int currentBlood = -1;
    private float attackMultiple = 1;
    public float AttackMultiple {
        get {
            return attackMultiple;
        }
    }
    private float hpMultiple = 1;
    public int unitBaseInfo = -1;

	public uint userID ;
	private string userUnitID = string.Empty;
	public  string MakeUserUnitKey() {
		if (string.IsNullOrEmpty (userUnitID)) {
			return userID.ToString () + "_" + ID.ToString ();	
		} else {
			return 	userUnitID;
		}
	} 

    TNormalSkill[] normalSkill = new TNormalSkill[2];

    public void SetAttack(float value, int type, EBoostTarget boostTarget, EBoostType boostType) {
        if (boostType == EBoostType.BOOST_HP) {
            if (boostTarget == EBoostTarget.UNIT_RACE) {
                SetHPByRace(value, type);
            }
            else {
                SetHPByType(value, type);
            }
        }
        else {
            if (boostTarget == EBoostTarget.UNIT_RACE) {
                SetAttackMultipeByRace(value, type);
            }
            else {
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
		if ((int)UnitInfo.Race == race || UnitInfo.Race == EUnitRace.ALL) {
            hpMultiple *= value;
        }
    }

    void SetAttackMultipleByType(float value, int type) {
        if (type == UnitType || type == 0) {
            attackMultiple *= value;
        }
    }

    void SetAttackMultipeByRace(float value, int race) {
        UnitBaseInfo ubi = DataCenter.Instance.UnitBaseInfo[unitBaseInfo];
		if ((int)UnitInfo.Race == race || UnitInfo.Race == EUnitRace.ALL) {
            attackMultiple *= value;
        }
    }

	public List<TNormalSkill> GetNormalSkill() {
		if (normalSkill[0] == null) {
			InitSkill();	
		}
		List<TNormalSkill> ns = new List<TNormalSkill> ();
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

        if (beRetraintType == (int)UnitInfo.Object.type) {
            hurtValue = attackValue * 0.5f;
        }
        else if (retraintType == (int)UnitInfo.Object.type) {
            hurtValue = attackValue * 2f;
        }
        else {
            hurtValue = attackValue;
        }
        if (hurtValue <= 0) {
            hurtValue = 1;
        }
        int hv = System.Convert.ToInt32(hurtValue);
        currentBlood -= hv;
        return hv;
    }

    void InitSkill() {
		UnitInfo ui = DataCenter.Instance.GetUnitInfo (instance.unitId).Object;
        TNormalSkill firstSkill = null;
        TNormalSkill secondSkill = null;
        if (ui.skill1 > 0) {
			firstSkill = DataCenter.Instance.GetSkill(MakeUserUnitKey(),ui.skill1,SkillType.NormalSkill) as TNormalSkill; //Skill[ui.skill1] as TNormalSkill;	
        }
        if (ui.skill2 > 0) {
			secondSkill = DataCenter.Instance.GetSkill(MakeUserUnitKey(),ui.skill2,SkillType.NormalSkill) as TNormalSkill; //.Skill[ui.skill2] as TNormalSkill;	
        }
        AddSkill(firstSkill, secondSkill);
    }

    public List<AttackInfo> CaculateAttack(List<uint> card, List<int> ignorSkillID) {
        List<uint> copyCard = new List<uint>(card);
        List<AttackInfo> returnInfo = new List<AttackInfo>();
        if (normalSkill[0] == null) {
            InitSkill();	
        }
		TUnitInfo tui = DataCenter.Instance.GetUnitInfo (instance.unitId);
		UnitInfo ui = tui.Object;
        for (int i = 0; i < normalSkill.Length; i++) {
            TNormalSkill tns = normalSkill[i];
			if(tns == null) {
				continue;
			}
		
            tns.DisposeUseSkillID(ignorSkillID);
            int count = tns.CalculateCard(copyCard);
            for (int j = 0; j < count; j++) {
                AttackInfo attack = new AttackInfo();
                attack.AttackValue = CaculateAttack(instance, ui, tns);
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
		TUnitInfo tui = DataCenter.Instance.GetUnitInfo (instance.unitId);
		UnitInfo ui = tui.Object;
		for (int i = 0; i < normalSkill.Length; i++) {
			TNormalSkill tns = normalSkill[i];
			if(tns == null) {
				continue;
			}
//			Debug.LogError(tns.Blocks.Count + "  normalskill : " + tns.Blocks[0]);
			tns.DisposeUseSkillID(csu.alreadyUseSkill);
			int count = tns.CalculateCard(copyCard);
			for (int j = 0; j < count; j++) {
				csu.alreadyUseSkill.Add(tns);
				csu.ResidualCard();
				AttackInfo attack = new AttackInfo();
				attack.AttackValue = CaculateAttack(instance, ui, tns);
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

    void AddSkill(TNormalSkill firstSkill, TNormalSkill secondSkill) {
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

    protected int CaculateAttack(UserUnit uu, UnitInfo ui, TNormalSkill tns) {
		float attack = DGTools.CaculateAddAttack (uu.addAttack, uu, ui); //addAttack + DataCenter.Instance.GetUnitValue(ui.powerType.attackType, uu.level); //ui.power [uu.level].attack;
        attack = tns.GetAttack(attack) * attackMultiple;

        if (strengthenInfo != null) {
            attack *= strengthenInfo.AttackValue;
        }
        int value = System.Convert.ToInt32(attack);
        return value;
    }

    public int UnitType {
        get {
            return (int)UnitInfo.Object.type;
        }
    }

	public int UnitRace {
		get {
			return (int)UnitInfo.Object.race;
		}
	}

    public int LeadSKill {
        get {
            return UnitInfo.Object.leaderSkill;
        }
    }

    public int ActiveSkill {
        get {
            return UnitInfo.Object.activeSkill;
        }
    }

    public int PassiveSkill {
        get {
            return UnitInfo.Object.passiveSkill;
        }
    }

    public TUnitInfo UnitInfo {
        get {
//		UserUnit userUnit = instance;//DeserializeData () as UserUnit;
			return DataCenter.Instance.GetUnitInfo(instance.unitId); //UnitInfo[instance.unitId];
        }
    }

	public int MultipleDevorExp (TUserUnit baseUser) {
		if (baseUser == null) {
			return UnitInfo.DevourExp * Level;
		}
		else{
			return System.Convert.ToInt32 (DGTools.AllMultiple (baseUser, this) * UnitInfo.DevourExp * Level);
		}
	}



    public int Exp {
        get{
            return instance.exp;
        }
    }

    public int CurExp {
        get {
            int curExp = DataCenter.Instance.GetUnitValue(UnitInfo.ExpType, instance.level) - NextExp;
            return curExp;
        }
    }


    public int NextExp {
        get {
            int nextexp = DataCenter.Instance.GetUnitValueTotal(UnitInfo.ExpType, instance.level) - instance.exp;
            if (nextexp < 0)
                nextexp = 0;
            return nextexp;
        }
    }

    public int InitBlood {
        get {
            UnitInfo ui = UnitInfo.Object;
            int blood = 0;
//			Debug.LogError(MakeUserUnitKey() + " instance.addHp : " + instance.addHp + " GetValue (uu, ui) : " +DGTools.GetValue (instance, ui.powerType.hpType));
            blood += DGTools.CaculateAddBlood(instance.addHp, instance, ui);
            float temp = blood * hpMultiple;
            return System.Convert.ToInt32(blood);
        }
    }

    public int Blood {
        get {
            if (currentBlood == -1) {
                //			UserUnit uu = DeserializeData<UserUnit>();
                UnitInfo ui = UnitInfo.Object;
                currentBlood = DGTools.CaculateAddBlood(instance.addHp, instance, ui);
                //			currentBlood += DataCenter.Instance.GetUnitValue(ui.powerType.hpType,uu.level); //ui.power [uu.level].hp;
            }
            float blood = currentBlood * hpMultiple;
            return System.Convert.ToInt32(blood);
        }
    }

    public UserUnit Object {
        get { return instance; }
    }

    public int Level {
        get { return instance.level; }
		set { instance.level = value; }
    }

	public int ActiveSkillLevel {
		get { return instance.activeSkillLevel; }
		set { instance.activeSkillLevel = value; }
	}

    public int AddNumber {
        get {
            return AddHP + AddAttack;
        }
    }

    public int Attack {
        get {
            return DGTools.CaculateAddAttack(instance.addAttack, instance, UnitInfo.Object);
        }
    }

    public int Hp {
        get {
            return DGTools.CaculateAddBlood(instance.addHp, instance, UnitInfo.Object);
        }
    }

    public uint UnitID {
        get {
            return UnitInfo.Object.id;
        }
    }

    public uint ID {
        get {
            return instance.uniqueId;
//			return DeserializeData<UserUnit>().uniqueId;
        }
    }

    public int AddAttack {
        get {
            return instance.addAttack;
        }
    }

    public int AddHP {
        get {
            return instance.addHp;
        }
    }

    /// <summary>
    ///  0 = false. 1 = true.
    /// </summary>
    /// <value>The is favorate.</value>
    public int isFavorate {
        get {
            return instance.isFavorite;
        }
    } 
}

//A wrapper to manage userUnitInfo list
public class UserUnitList {
    private Dictionary<string, TUserUnit> userUnitInfo;
    public UserUnitList() { 
        userUnitInfo = new Dictionary<string, TUserUnit>(); //key: "{userid}_{unitUniqueId}"
    }

    public  string MakeUserUnitKey(uint userId, uint uniqueId) {
        return userId.ToString() + "_" + uniqueId.ToString();
    }

    public  Dictionary<string, TUserUnit> GetAll() {
        return userUnitInfo;
    }

    public  void Clear() {
        userUnitInfo.Clear();
    }

    public int Count{
        get { return userUnitInfo.Count;}
    }

    public  TUserUnit Get(uint userId, uint uniqueId) {
        string key = MakeUserUnitKey(userId, uniqueId);
//		Debug.LogError (" key : " + key);
        if (!userUnitInfo.ContainsKey(key)) {
//            Debug.Log("Cannot find key " + key + " in Global.userUnitInfo");
            return null;
        }
	
        TUserUnit tuu = userUnitInfo[key];
//		Debug.LogError (" tuu : " + tuu);
        return tuu;
    }

	public TUserUnit Get (string UserId) {
		if (!userUnitInfo.ContainsKey(UserId)) {
//			Debug.Log("Cannot find key " + UserId + " in Global.userUnitInfo");
			return null;
		}
		
		TUserUnit tuu = userUnitInfo[UserId];
		return tuu;
	}

    public  TUserUnit GetMyUnit(uint uniqueId) {
        if (DataCenter.Instance.UserInfo == null) {
//            Debug.LogError("TUserUnit.GetMyUnit() : Global.userInfo=null");
            return null;
        }
		
        return Get(DataCenter.Instance.UserInfo.UserId, uniqueId);
    }

	public  TUserUnit GetMyUnit(string id) {
		if (DataCenter.Instance.UserInfo == null) {
//			Debug.LogError("TUserUnit.GetMyUnit() : Global.userInfo=null");
			return null;
		}

		return Get(id);
	}

//    public bool HasUnit(uint uniqueId){
//        !userUnitInfo.ContainsKey(key)
//    }

    public  void DelMyUnit(uint uniqueId) {
        if (DataCenter.Instance.UserInfo == null) {
            Debug.LogError ("TUserUnit.GetMyUnit() : Global.userInfo=null");
            return;
        }
//        LogHelper.LogError("============================before DelMyUnit(), count {0}, del uniqueId {1}", userUnitInfo.Count, uniqueId);
        Del(DataCenter.Instance.UserInfo.UserId, uniqueId);
        // test
//        LogHelper.LogError("============================after DelMyUnit(), count {0}", userUnitInfo.Count);
        foreach (var item in userUnitInfo) {
            TUserUnit tUnit = item.Value as TUserUnit;
//            LogHelper.Log("========================================unit.ID {0}=================================", tUnit.ID);
        }
    }

    public  void Add(uint userId, uint uniqueId, TUserUnit uu) {
		string key = uu.MakeUserUnitKey();
        if (!userUnitInfo.ContainsKey(key))
            userUnitInfo.Add(key, uu);
        else {
            userUnitInfo[key] = uu;
        }
    }

    public List<uint> GetMyUnitUniqueIdList(){
        List <uint> uniqueIdList = new List<uint>();
        foreach (var item in userUnitInfo) {
            TUserUnit tUnit = item.Value as TUserUnit;
            if (GetMyUnit(tUnit.UnitID) != null){
                uniqueIdList.Add(tUnit.UnitID);
            }
        }
        return uniqueIdList;
    }

    public void AddMyUnit(UserUnit unit) {
		TUserUnit tuu = TUserUnit.GetUserUnit(DataCenter.Instance.UserInfo.UserId, unit); 
		Add(DataCenter.Instance.UserInfo.UserId, unit.uniqueId, tuu);
    }

    public void AddMyUnitList(List <UserUnit> unitList){
        for (int i = 0; i < unitList.Count; i++){
            AddMyUnit(unitList[i]);
        }
    }

    public void DelMyUnitList(List <uint> uniqueIds){
        for (int i = 0; i < uniqueIds.Count; i++){
            DelMyUnit(uniqueIds[i]);
        }
    }

    public  void Del(uint userId, uint uniqueId) {
        string key = MakeUserUnitKey(userId, uniqueId);
        if (userUnitInfo.ContainsKey(key))
            userUnitInfo.Remove(key);
    }


}

//public class PartyItemInfo : ProtobufDataBase {
//	public PartyItemInfo (PartyItem instance) : base (instance) {
//		UnitInfo UI = new UnitInfo ();
//	}
//}