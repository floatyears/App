using UnityEngine;
using System.Collections.Generic;
using bbproto;
using System.Collections;

public class ConfigUnitInfo {
	public ConfigUnitInfo () {
		GenerateUnitInfo ();

		GenerateUserUnit ();

		GenerateUserUnitParty ();
	}
	
	private const int maxCount = 6;
	
	UnitInfo[] ui = new UnitInfo[maxCount];
	
	void GenerateUnitInfo () {
		for (int i = 1; i < maxCount; i++) {
			UnitInfo uiitem = new UnitInfo ();
			uiitem.id = i;
			uiitem.name = "unit_" + i;
			uiitem.type = i;
			uiitem.skill1 = (i - 1) * 2;
			uiitem.skill2 = (i - 1) * 2 + 1;
			for (int j = 0; j < 3; j++) {
				BattlePower bp = new BattlePower ();
				bp.attack = 10 + j * 10;
				bp.defense = 1 + j * 10;
				bp.hp = 100 + j * 10;
				bp.level = j + 1;
				uiitem.power.Add(bp);
			}
			uiitem.rare = i;
			uiitem.maxLevel = 10;
			uiitem.expType = 1;
		
			TempUnitInfo tui = new TempUnitInfo(uiitem);
			GlobalData.tempUnitInfo.Add(uiitem.id, tui);
		}
	}

	void GenerateUserUnit () {
		for (int i = 1; i < maxCount; i++) {
			UserUnit uu = new UserUnit ();
			uu.uniqueId = i;
			uu.id = i;
			uu.exp = 0;
			uu.level = 1;
			uu.addAttack = i;
			uu.addDefence = 0;
			uu.addHp = i;
			uu.limitbreakLv = 2;
			uu.getTime = 0;
			UserUnitInfo uui = new UserUnitInfo (uu);
			GlobalData.tempUserUnitInfo.Add (i, uui);
		}
	}

	void GenerateUserUnitParty () {
		UnitParty up = new UnitParty ();
		up.id = 0;
		for (int i = 1; i < 6; i++) {
			PartyItem pi = new PartyItem();
			pi.unitPos = i;
			pi.unitUniqueId = i;
			up.items.Add(pi);
		}
		UnitPartyInfo upi = new UnitPartyInfo (up);

		ModelManager.Instance.AddData (ModelEnum.UnitPartyInfo, upi);
	}
}

public class ConfigUserUnit {
	
}

public class TempUnitInfo : ProtobufDataBase {
	public TempUnitInfo (object instance) : base (instance) {
		
	}
}

public class UnitPartyInfo : ProtobufDataBase, IComparer {
	private List<PartyItem> partyItem = new List<PartyItem> ();							//skill sort

	/// <summary>
	/// key is area item. value is skill list. this area already use skill must record in this dic, avoidance redundant calculate.
	/// </summary>
	private Dictionary<int, CalculateSkillUtility> alreadyUse = new Dictionary<int, CalculateSkillUtility> ();	 
	private Dictionary<int, List<AttackInfo>> attack = new Dictionary<int, List<AttackInfo>> ();
	public UnitPartyInfo (object instance) : base (instance) { }
	~UnitPartyInfo () { }

	public List<AttackImageUtility> CalculateSkill(int areaItemID, int cardID) {
		CalculateSkillUtility skillUtility = CheckSkillUtility (areaItemID, cardID);
		Debug.LogError ("skillUtility haveCard : " + skillUtility.haveCard.Count + " skillUtility alreadyUseSkill : " +  skillUtility.alreadyUseSkill.Count);
		List<AttackInfo> areaItemAttackInfo = CheckAttackInfo (areaItemID);

		UserUnitInfo tempUnitInfo;
		List<AttackInfo> tempAttack = null;		
		List<AttackImageUtility> tempAttackType = new List<AttackImageUtility> ();

		for (int i = 0; i < partyItem.Count; i++) {
			tempUnitInfo = GlobalData.tempUserUnitInfo [partyItem [i].unitUniqueId];
			tempAttack = tempUnitInfo.CaculateAttack (skillUtility.haveCard, skillUtility.alreadyUseSkill);

			if (tempAttack.Count > 0) {
				for (int j = 0; j < tempAttack.Count; j++) {
					AttackInfo ai 			= tempAttack [j];
					areaItemAttackInfo.Add (ai);
					skillUtility.alreadyUseSkill.Add (ai.SkillID);
					AttackImageUtility aiu 	= new AttackImageUtility();
					aiu.attackProperty		= ai.AttackType;
					aiu.userProperty 		= GlobalData.tempUserUnitInfo[ai.UserUnitID].GetUnitType();
					aiu.skillID				= ai.SkillID;
					tempAttackType.Add (aiu);
				}
			}
		}
		return tempAttackType;
	}

	public void ClearData () {
		alreadyUse.Clear ();
		attack.Clear ();
	}
	
	public void GetSkillCollection() {
		partyItem 		= new List<PartyItem>();
		UnitParty up 	= DeserializeData<UnitParty> ();
		for (int i 		= 0; i < up.items.Count; i++) {
			partyItem.Add(up.items[i]);
		}
		DGTools.InsertSort<PartyItem,IComparer> (partyItem, this);
	}
	
	public int Compare (object first, object second)
	{
		PartyItem firstUU 	= (PartyItem)first;
		PartyItem secondUU 	= (PartyItem)second;
		NormalSkill ns1 	= GetSecondSkill (firstUU);
		NormalSkill ns2 	= GetSecondSkill (secondUU);
		return ns1.activeBlocks.Count.CompareTo(ns2.activeBlocks.Count);
	}

	public int GetBlood () {
		UnitParty up = DeserializeData<UnitParty> ();
		int bloodNum = 0;
		for (int i = 0; i < up.items.Count; i++) {
			int unitUniqueID = up.items [i].unitUniqueId;
			UserUnit uu = GlobalData.tempUserUnitInfo [unitUniqueID].DeserializeData<UserUnit> ();
			UnitInfo ui = GlobalData.tempUnitInfo [uu.id].DeserializeData<UnitInfo> ();
			bloodNum += DGTools.CaculateAddBlood (uu.addHp);
			bloodNum += ui.power [uu.level].hp;
		}
		return bloodNum;
	}

	CalculateSkillUtility CheckSkillUtility (int areaItemID, int cardID) {
		CalculateSkillUtility skillUtility;												//-- find or creat  have card and use skill record data
		if (!alreadyUse.TryGetValue (areaItemID, out skillUtility)) {
			skillUtility = new CalculateSkillUtility();
			alreadyUse.Add(areaItemID,skillUtility);
		}
		skillUtility.haveCard.Add ((uint)cardID);
		return skillUtility;
	}

	List<AttackInfo> CheckAttackInfo (int areaItemID) {
		List<AttackInfo> areaItemAttackInfo = null;										//-- find or creat attack data;
		if (!attack.TryGetValue (areaItemID, out areaItemAttackInfo)) {
			areaItemAttackInfo 				= new List<AttackInfo>();
			attack.Add(areaItemID,areaItemAttackInfo);
		}
		return areaItemAttackInfo;
	}

	NormalSkill GetSecondSkill (PartyItem pi) {
		UserUnit uu1 = GlobalData.tempUserUnitInfo[pi.unitUniqueId].DeserializeData() as UserUnit;
		UnitInfo ui1 = GlobalData.tempUnitInfo[uu1.id].DeserializeData<UnitInfo>();
		return GlobalData.tempNormalSkill [ui1.skill2].DeserializeData<NormalSkill> ();
	}
}

public class CalculateSkillUtility {
	public List<uint> haveCard = new List<uint>();
	public List<int> alreadyUseSkill = new List<int>();
}

public class AttackImageUtility {
	public int attackProperty = -1;
	public int userProperty = -1;

	//------------test need data, delete it behind test done------------//
	//------------------------------------------------------------------//
	public int skillID = -1;

}

public class AttackInfo {
	private int userUnitID = -1;
	public int UserUnitID {
		get { return userUnitID; }
		set { userUnitID = value; }
	}
	
	private int skillID = -1;
	public int SkillID {
		get {return skillID;}
		set {skillID = value;}
	}
	
	private int attackType = 0;
	public int AttackType {
		get { return attackType; }
		set {attackType = value; }
	}
	
	private int attackRange ;
	/// <summary>
	/// 0 = single attack
	/// 1 = all attack
	/// 2 = recover hp
	/// </summary>
	/// <value>The attack range.</value>
	public int AttackRange {
		get { return attackRange; }
		set { attackRange = value; }
	}
	
	private float attackValue ;
	public float AttackValue {
		get {return attackValue; }
		set {attackValue = value;}
	}

	//------------test need data, delete it behind test done------------//
	//------------------------------------------------------------------//
}

