using UnityEngine;
using System.Collections.Generic;
using bbproto;

//public class UserUnitParty {
//	public static Dictionary<int,UnitParty> userUnitPartyInfo = new Dictionary<int, UnitParty> ();
//
//}
//
//public class AddBlood {
//
//
//	public List<AttackInfo> CaculateAttack (List<uint> card,List<int> ignorSkillID) {
//		return null;
//	}
//}

public class TUserUnit : ProtobufDataBase {
    private UserUnit instance;

    public UserUnit Unit {
        get { return instance;}
    }

    public TUserUnit(UserUnit instance) : base (instance) { 
        MsgCenter.Instance.AddListener(CommandEnum.StrengthenTargetType, StrengthenTargetType);
        this.instance = instance as UserUnit;
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
        UnitBaseInfo ubi = DataCenter.Instance.UnitBaseInfo[unitBaseInfo];
        if ((int)ubi.race == race || race == (int)EUnitRace.ALL) {
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
        if ((int)ubi.race == race || race == (int)EUnitRace.ALL) {
            attackMultiple *= value;
        }
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
//		UserUnit uu 				= DeserializeData<UserUnit> ();
//		TUnitInfo tui 			= DataCenter.Instance.UnitInfo[instance.unitId];
		UnitInfo ui = DataCenter.Instance.GetUnitInfo (instance.unitId).Object;//UnitInfo[instance.unitId].Object;
        TNormalSkill firstSkill = null;
        TNormalSkill secondSkill = null;
        if (ui.skill1 > -1) {
            firstSkill = DataCenter.Instance.Skill[ui.skill1] as TNormalSkill;	
        }
        if (ui.skill2 > -1) {
            secondSkill = DataCenter.Instance.Skill[ui.skill2] as TNormalSkill;	
        }
        AddSkill(firstSkill, secondSkill);
    }

    public List<AttackInfo> CaculateAttack(List<uint> card, List<int> ignorSkillID) {
        List<uint> copyCard = new List<uint>(card);
        List<AttackInfo> returnInfo = new List<AttackInfo>();
        if (normalSkill[0] == null) {
            InitSkill();	
        }
		TUnitInfo tui = DataCenter.Instance.GetUnitInfo (instance.unitId); //UnitInfo[instance.unitId];
		UnitInfo ui = tui.Object;
        for (int i = 0; i < normalSkill.Length; i++) {
            TNormalSkill tns = normalSkill[i];
//			Debug.LogError("tns : " + tns.SkillID );
            tns.DisposeUseSkillID(ignorSkillID);
            int count = tns.CalculateCard(copyCard);
            for (int j = 0; j < count; j++) {
                AttackInfo attack = new AttackInfo();
                attack.AttackValue = CaculateAttack(instance, ui, tns);
                attack.AttackType = tns.AttackType;
                attack.UserUnitID = instance.uniqueId;
                tns.GetSkillInfo(attack);
                returnInfo.Add(attack);
            }
        }

//		Debug.LogError ("instance.uniqueId : " + instance.uniqueId + " returnInfo.Count: " + returnInfo.Count);
        return returnInfo;
    }

    AttackInfo strengthenInfo = null;
    void StrengthenTargetType(object data) {
        AttackInfo ai = data as AttackInfo;
        if (ai == null) {
            return;	
        }
        if (ai.AttackType != UnitType) {
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
        int addAttack = uu.addAttack * 50;

        float attack = addAttack + DataCenter.Instance.GetUnitValue(ui.powerType.attackType, uu.level); //ui.power [uu.level].attack;
        attack = tns.GetAttack(attack) * attackMultiple;

        if (strengthenInfo != null) {
            attack *= strengthenInfo.AttackValue;
        }
//		Debug.LogError ("addAttack : " + addAttack + "attack : " + uu.addAttack );
        int value = System.Convert.ToInt32(attack);
        return value;
    }

    public int UnitType {
        get {
            return (int)UnitInfo.Object.type;
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
//		Debug.LogError("MultipleDevorExp :: unitId:"+UnitInfo.ID+" UnitInfo.DevourExp:"+UnitInfo.DevourExp);
		return System.Convert.ToInt32 (DGTools.AllMultiple (baseUser, this) * UnitInfo.DevourExp * Level);
	}

    public int Exp {
        get{
            return instance.exp;
        }
    }

    public int CurExp {
        get {
            int curExp = DataCenter.Instance.GetUnitValueTotal(UnitInfo.ExpType, instance.level) - NextExp;
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
            //		UserUnit uu = DeserializeData<UserUnit>();
            UnitInfo ui = UnitInfo.Object;
            int blood = 0;
            blood += DGTools.CaculateAddBlood(instance.addHp, instance, ui);
            //		blood += DataCenter.Instance.GetUnitValue (ui.powerType.hpType, uu.level); //ui.power [uu.level].hp;
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
        get {
            return instance.level;
        }
		set {

		}
    }

    public string AddNumber {
        get {
            return (AddHP + AddAttack).ToString();
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
            Debug.Log("Cannot find key " + key + " in Global.userUnitInfo");
            return null;
        }
	
        TUserUnit tuu = userUnitInfo[key];
//		Debug.LogError (" tuu : " + tuu);
        return tuu;
    }

    public  TUserUnit GetMyUnit(uint uniqueId) {
        if (DataCenter.Instance.UserInfo == null) {
            Debug.LogError("TUserUnit.GetMyUnit() : Global.userInfo=null");
            return null;
        }
		
        return Get(DataCenter.Instance.UserInfo.UserId, uniqueId);
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
        string key = MakeUserUnitKey(userId, uniqueId);
        if (!userUnitInfo.ContainsKey(key))
            userUnitInfo.Add(key, uu);
        else {
            userUnitInfo[key] = uu;
        }
    }

    public void AddMyUnit(UserUnit unit) {
//        LogHelper.LogError("============================before AddMyUnit(), count {0}, new unitId {1} new uniqueID  {2}",
//                           userUnitInfo.Count, unit.unitId, unit.uniqueId);
        Add(DataCenter.Instance.UserInfo.UserId, unit.uniqueId, new TUserUnit(unit));
        // test
//        LogHelper.LogError("============================after AddMyUnit(), count {0}", userUnitInfo.Count);
        foreach (var item in userUnitInfo) {
            TUserUnit tUnit = item.Value as TUserUnit;
//            LogHelper.Log("========================================unit.ID {0}=================================", tUnit.ID);
        }
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