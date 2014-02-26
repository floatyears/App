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
		if (type == GetUnitType () || type == 0) {
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
		if (type == GetUnitType () || type == 0) {
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

		if (beRetraintType == (int)GetUnitInfo ().type) {
			hurtValue = attackValue * 0.5f;
		} 
		else if (retraintType == (int)GetUnitInfo ().type) {
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
		TUnitInfo tui 			= GlobalData.unitInfo[instance.unitId];
		UnitInfo ui					= tui.DeserializeData<UnitInfo>();
		TNormalSkill firstSkill = null;
		TNormalSkill secondSkill = null;
		if (ui.skill1 > -1) {
			firstSkill	= GlobalData.normalSkill [ui.skill1] as TNormalSkill;	
		}
		if (ui.skill2 > -1) {
			secondSkill = GlobalData.normalSkill [ui.skill2] as TNormalSkill;	
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
		UnitInfo ui					= tui.DeserializeData<UnitInfo>();
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
		if (ai.AttackType != GetUnitType ()) {
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

	public int GetUnitType (){
		return (int)GetUnitInfo().type;
	}

	public int GetLeadSKill () {
		return GetUnitInfo().leaderSkill;
	}

	public int GetActiveSkill () {
		return GetUnitInfo ().activeSkill;
	}

	public int GetPassiveSkill () {
		return GetUnitInfo ().passiveSkill;
	}

	public UnitInfo GetUnitInfo() {
//		UserUnit userUnit = instance;//DeserializeData () as UserUnit;
		return GlobalData.unitInfo [instance.unitId].GetObject;
	}

	public int GetInitBlood () {
//		UserUnit uu = DeserializeData<UserUnit>();
		UnitInfo ui = GetUnitInfo() ;
		int blood = 0;
		blood +=  DGTools.CaculateAddBlood (instance.addHp,uu,ui);
//		blood += GlobalData.Instance.GetUnitValue (ui.powerType.hpType, uu.level); //ui.power [uu.level].hp;
		float temp = blood * hpMultiple;
		return System.Convert.ToInt32(blood);
	}

	public int GetBlood () {
		if (currentBlood == -1) {
//			UserUnit uu = DeserializeData<UserUnit>();
			UnitInfo ui = GetUnitInfo() ;
			currentBlood += DGTools.CaculateAddBlood (instance.addHp,uu,ui);
//			currentBlood += GlobalData.Instance.GetUnitValue(ui.powerType.hpType,uu.level); //ui.power [uu.level].hp;
		}
		float blood = currentBlood * hpMultiple;
		return System.Convert.ToInt32(blood);
	}

	public UserUnit GetObject{
		get { return instance; }
	}

	public int GetLevel{
		get {
			return GetObject.level;
		}
	}

	public int GetAttack {
		get {
			int addAttack = GetObject.addAttack * 50;
			UnitInfo ui = GetUnitInfo() ;
			return addAttack + GlobalData.Instance.GetUnitValue(ui.powerType.attackType,GetObject.level); //ui.power [GetObject.level].attack;
		}
	}

	public uint GetUnitID {
		get {
			return GetUnitInfo().id;
		}
	}

	public uint GetID {
		get {
			return DeserializeData<UserUnit>().uniqueId;
		}
	}
}

//public class PartyItemInfo : ProtobufDataBase {
//	public PartyItemInfo (PartyItem instance) : base (instance) {
//		UnitInfo UI = new UnitInfo ();
//	}
//}