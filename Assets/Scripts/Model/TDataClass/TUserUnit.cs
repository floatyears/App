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
	public TUserUnit(UserUnit instance) : base (instance) { 
		MsgCenter.Instance.AddListener (CommandEnum.StrengthenTargetType, StrengthenTargetType);
		this.instance = instance as UserUnit;
	} 

	public void RemovevListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.StrengthenTargetType, StrengthenTargetType);
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

	public void SetAttack(float value, int type, EBoostTarget boostTarget,EBoostType boostType) {
		if (boostType == EBoostType.BOOST_HP) {
			if (boostTarget == EBoostTarget.UNIT_RACE){
				SetHPByRace(value,type);
			}
			else{
				SetHPByType(value,type);
			}
		} else {
			if (boostTarget == EBoostTarget.UNIT_RACE) {
				SetAttackMultipeByRace (value, type);
			}
			else {
				SetAttackMultipleByType(value,type);	
			}
		}
	}

	void SetHPByType (float value, int type) {
		if (type == UnitType || type == 0) {
			hpMultiple *= value;
		}
	}

	void SetHPByRace (float value, int race) {
		UnitBaseInfo ubi = GlobalData.unitBaseInfo [unitBaseInfo];
		if ((int)ubi.race == race || race == (int)EUnitRace.ALL) {
			hpMultiple *= value;
		}
	}

	void SetAttackMultipleByType (float value,int type) {
		if (type == UnitType || type == 0) {
			attackMultiple *= value;
		}
	}

	void SetAttackMultipeByRace(float value,int race) {
		UnitBaseInfo ubi = GlobalData.unitBaseInfo [unitBaseInfo];
		if ((int)ubi.race == race || race == (int)EUnitRace.ALL) {
			attackMultiple *= value;
		}
	}

	public float CalculateInjured(int attackType, float attackValue) {
		int beRetraintType = DGTools.BeRestraintType (attackType);
		int retraintType = DGTools.RestraintType (attackType);
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
		int hv = System.Convert.ToInt32 (hurtValue);
		currentBlood -= hv;
		return hv;
	}

	void InitSkill () {
//		UserUnit uu 				= DeserializeData<UserUnit> ();
//		TUnitInfo tui 			= GlobalData.unitInfo[instance.unitId];
		UnitInfo ui				= GlobalData.unitInfo[instance.unitId].Object;
		TNormalSkill firstSkill = null;
		TNormalSkill secondSkill = null;
		if (ui.skill1 > -1) {
			firstSkill	= GlobalData.skill [ui.skill1] as TNormalSkill;	
		}
		if (ui.skill2 > -1) {
			secondSkill = GlobalData.skill [ui.skill2] as TNormalSkill;	
		}
		AddSkill(firstSkill,secondSkill);
	}

	public List<AttackInfo> CaculateAttack (List<uint> card,List<int> ignorSkillID) {
		List<uint> copyCard 		= new List<uint> (card);
		List<AttackInfo> returnInfo = new List<AttackInfo> ();
		if (normalSkill [0] == null) {
			InitSkill();	
		}
//		UserUnit uu 				= DeserializeData<UserUnit> ();
		TUnitInfo tui 			= GlobalData.unitInfo[instance.unitId];
		UnitInfo ui				= GlobalData.unitInfo[instance.unitId].Object;
		for (int i = 0; i < normalSkill.Length; i++) {
			TNormalSkill tns 	= normalSkill[i];
			tns.DisposeUseSkillID(ignorSkillID);
			int count = tns.CalculateCard(copyCard);
			for (int j = 0; j < count; j++) {
				AttackInfo attack	= new AttackInfo();
				attack.AttackValue	= CaculateAttack(instance,ui,tns);
				attack.AttackType	= (int)ui.type;
				attack.UserUnitID	= instance.uniqueId;
				tns.GetSkillInfo(attack);
				returnInfo.Add(attack);
			}
		}
		return returnInfo;
	}

	AttackInfo strengthenInfo = null;
	void StrengthenTargetType(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		if (ai.AttackType != UnitType ) {
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

	protected int CaculateAttack (UserUnit uu, UnitInfo ui, TNormalSkill tns) {
		int addAttack = uu.addAttack * 50;
		float attack = addAttack + GlobalData.Instance.GetUnitValue(ui.powerType.attackType, uu.level); //ui.power [uu.level].attack;
		attack = tns.GetAttack(attack) * attackMultiple;

		if (strengthenInfo != null) {
			attack *= strengthenInfo.AttackValue;
		}
		int value = System.Convert.ToInt32 (attack);
		return value;
	}

	public int UnitType {
		get {
			return (int)UnitInfo.Object.type;
		}
	}

	public int LeadSKill  {
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
		return GlobalData.unitInfo [instance.unitId];
		}
	}


	public int Exp {
		get {
			return instance.exp;
		}
	}
	public int InitBlood {
		get {
	//		UserUnit uu = DeserializeData<UserUnit>();
			UnitInfo ui = UnitInfo.Object;
			int blood = 0;
			blood +=  DGTools.CaculateAddBlood (instance.addHp,instance,ui);
	//		blood += GlobalData.Instance.GetUnitValue (ui.powerType.hpType, uu.level); //ui.power [uu.level].hp;
			float temp = blood * hpMultiple;
			return System.Convert.ToInt32(blood);
		}
	}

	public int Blood {
		get {
			if (currentBlood == -1) {
	//			UserUnit uu = DeserializeData<UserUnit>();
				UnitInfo ui = UnitInfo.Object ;
				currentBlood += DGTools.CaculateAddBlood (instance.addHp,instance,ui);
	//			currentBlood += GlobalData.Instance.GetUnitValue(ui.powerType.hpType,uu.level); //ui.power [uu.level].hp;
			}
			float blood = currentBlood * hpMultiple;
			return System.Convert.ToInt32(blood);
		}
	}

	public UserUnit Object{
		get { return instance; }
	}

	public int Level{
		get {
			return instance.level;
		}
	}

	public int Attack {
		get {
			int addAttack = Object.addAttack * 50;
			UnitInfo ui = UnitInfo.Object ;
			return addAttack + GlobalData.Instance.GetUnitValue(ui.powerType.attackType,Object.level); //ui.power [GetObject.level].attack;
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

	public int AddAttack{
		get{
			return instance.addAttack;
		}
	}

	public int AddHP{
		get{
			return instance.addHp;
		}
	}
}

//A wrapper to manage userUnitInfo list
public class UserUnitList {
	private UserUnitList instance;
	private Dictionary<string, TUserUnit> userUnitInfo;
	public UserUnitList(){ 
		userUnitInfo = new Dictionary<string, TUserUnit>(); //key: "{userid}_{unitUniqueId}"
	}

	public  string MakeUserUnitKey(uint userId, uint uniqueId) {
		return userId.ToString () + "_" + uniqueId.ToString ();
	}

	public  TUserUnit Get(uint userId, uint uniqueId) {
		string key = MakeUserUnitKey(userId, uniqueId);
		if (!userUnitInfo.ContainsKey (key))
			return null;
		return userUnitInfo [key];
	}

	public  TUserUnit GetMyUnit(uint uniqueId) {
		return Get(GlobalData.userInfo.UserId, uniqueId);
	}

	public  void Add(uint userId, uint uniqueId, TUserUnit uu) {
		string key = MakeUserUnitKey(userId, uniqueId);
		if ( !userUnitInfo.ContainsKey (key) )
			userUnitInfo.Add(key, uu);
		else{
			userUnitInfo [key] = uu;
		}
	}

	public  void Del(uint userId, uint uniqueId) {
		string key = MakeUserUnitKey(userId, uniqueId);
		if (userUnitInfo.ContainsKey (key))
			userUnitInfo.Remove(key);
	}
}

//public class PartyItemInfo : ProtobufDataBase {
//	public PartyItemInfo (PartyItem instance) : base (instance) {
//		UnitInfo UI = new UnitInfo ();
//	}
//}