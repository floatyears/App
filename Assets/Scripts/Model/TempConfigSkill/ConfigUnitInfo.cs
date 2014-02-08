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
			UnitInfo uiitem 	= new UnitInfo ();
			uiitem.id 			= i;
			uiitem.name			= "unit_" + i;
			uiitem.type 		= i;
			uiitem.skill1 		= (i - 1) * 2;
			uiitem.skill2 		= (i - 1) * 2 + 1;
			for (int j = 0; j < 3; j++) {
				BattlePower bp 	= new BattlePower ();
				bp.attack 		= 10 + j * 10;
				//bp.defense 		= 1 + j * 10;
				bp.hp 			= 100 + j * 10;
				bp.level 		= j + 1;
				uiitem.power.Add(bp);
			}
			uiitem.rare 		= i;
			uiitem.maxLevel 	= 10;
			uiitem.expType 		= 1;
			if(i == 1){
				uiitem.leaderSkill = 19;
			}
			if(i == 5) {
				uiitem.leaderSkill = 20;
			}
			TempUnitInfo tui = new TempUnitInfo(uiitem);
			GlobalData.tempUnitInfo.Add(uiitem.id, tui);
		}
//		Debug.LogError(GlobalData.tempUnitInfo [1].)
	}

	void GenerateUserUnit () {
		for (int i = 1; i < maxCount; i++) {
			UserUnit uu 		= new UserUnit ();
			uu.uniqueId 		= i;
			uu.id 				= i;
			uu.exp 				= 0;
			uu.level 			= 1;
			uu.addAttack 		= i;
			uu.addDefence		= 0;
			uu.addHp 			= i;
			uu.limitbreakLv 	= 2;
			uu.getTime 			= 0;
			UserUnitInfo uui 	= new UserUnitInfo (uu);
			GlobalData.tempUserUnitInfo.Add (i, uui);
		}

		GlobalData.tempUserUnitInfo [1].unitBaseInfo = 181;
		GlobalData.tempUserUnitInfo [2].unitBaseInfo = 85;
		GlobalData.tempUserUnitInfo [3].unitBaseInfo = 89;
		GlobalData.tempUserUnitInfo [4].unitBaseInfo = 80;
		GlobalData.tempUserUnitInfo [5].unitBaseInfo = 87;
	}

	void GenerateUserUnitParty () {
		UnitParty up = new UnitParty ();
		up.id = 0;
		for (int i = GlobalData.posStart; i <  GlobalData.posEnd; i++) {
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

public class UnitPartyInfo : ProtobufDataBase, IComparer, ILeadSkill {
	private List<PartyItem> partyItem = new List<PartyItem> ();		
	private Dictionary<int,ProtobufDataBase> leaderSkill = new Dictionary<int,ProtobufDataBase> ();
	public Dictionary<int,ProtobufDataBase> LeadSkill {
		get {
//			Debug.LogError("UnitPartyInfo : " + leaderSkill.Count);
			return leaderSkill;
		}
	}
	private Dictionary<int,UserUnitInfo> userUnit ;
	public Dictionary<int,UserUnitInfo> UserUnit {
		get {
			if(userUnit == null) {
				userUnit = new Dictionary<int,UserUnitInfo>();
				for (int i = 0; i < partyItem.Count; i++) {
					UserUnitInfo uui = GlobalData.tempUserUnitInfo[partyItem[i].unitUniqueId];
					userUnit.Add(partyItem[i].unitPos,uui);
				}
			}
			return userUnit;
		}
	}

//skill sort
	private CalculateRecoverHP crh ;
	/// <summary>
	/// key is area item. value is skill list. this area already use skill must record in this dic, avoidance redundant calculate.
	/// </summary>
	private Dictionary<int, CalculateSkillUtility> alreadyUse = new Dictionary<int, CalculateSkillUtility> ();	 
	private Dictionary<int, List<AttackInfo>> attack = new Dictionary<int, List<AttackInfo>> ();
	public Dictionary<int, List<AttackInfo>> Attack {
		get {return attack;}
	}
	public UnitPartyInfo (object instance) : base (instance) {  }
	~UnitPartyInfo () {  }

	public int CaculateInjured (int attackType, float attackValue) {
		float Proportion = 1f / (float)partyItem.Count;
//		Debug.LogError ("CaculateInjured : " + Proportion);
		float attackV = attackValue * Proportion;
//		Debug.LogError ("attackV : " + attackV);
		float hurtValue = 0;
		for (int i = 0; i < partyItem.Count; i++) {
			UserUnitInfo unitInfo = GlobalData.tempUserUnitInfo [partyItem [i].unitUniqueId];
			hurtValue += unitInfo.CalculateInjured(attackType, attackV);
		}
		return System.Convert.ToInt32(hurtValue);
	}

	public List<AttackImageUtility> CalculateSkill(int areaItemID, int cardID, int blood) {
		if (crh == null) {
			crh = new CalculateRecoverHP ();		
		}
		CalculateSkillUtility skillUtility = CheckSkillUtility (areaItemID, cardID);
		List<AttackInfo> areaItemAttackInfo = CheckAttackInfo (areaItemID);
		areaItemAttackInfo.Clear ();
		UserUnitInfo tempUnitInfo;
		List<AttackInfo> tempAttack = new List<AttackInfo>();		
		List<AttackImageUtility> tempAttackType = new List<AttackImageUtility> ();

		for (int i = 0; i < partyItem.Count; i++) {
			if(i == 0) {
				AttackInfo recoverHp = crh.RecoverHP (skillUtility.haveCard, skillUtility.alreadyUseSkill, blood);
				if(recoverHp != null) {
					recoverHp.UserUnitID = partyItem [i].unitUniqueId;
					recoverHp.UserPos = partyItem[i].unitPos;
					tempAttack.Add(recoverHp);
				}
			}

			tempUnitInfo = GlobalData.tempUserUnitInfo [partyItem [i].unitUniqueId];
			tempAttack.AddRange(tempUnitInfo.CaculateAttack (skillUtility.haveCard, skillUtility.alreadyUseSkill));

			if (tempAttack.Count > 0) {
				for (int j = 0; j < tempAttack.Count; j++) {
					AttackInfo ai 			= tempAttack [j];
					ai.UserPos 				= partyItem[i].unitPos;
					areaItemAttackInfo.Add (ai);
					skillUtility.alreadyUseSkill.Add (ai.SkillID);
					AttackImageUtility aiu 	= new AttackImageUtility();
					aiu.attackProperty		= ai.AttackType;
					aiu.userProperty 		= GlobalData.tempUserUnitInfo[ai.UserUnitID].GetUnitType();
					aiu.skillID				= ai.SkillID;
					aiu.attackID			= ai.AttackID;
					tempAttackType.Add (aiu);
				}     
			}
			tempAttack.Clear();
		}

		return tempAttackType;
	}

	public void ClearData () {
		AttackInfo.ClearData ();
		alreadyUse.Clear ();
		attack.Clear ();
	}
	
	public void GetSkillCollection() {
		partyItem 		= new List<PartyItem>();
		UnitParty up 	= DeserializeData<UnitParty> ();
		for (int i 		= 0; i < up.items.Count; i++) {
			partyItem.Add(up.items[i]);
		}
		GetLeaderSkill ();
		DGTools.InsertSort<PartyItem,IComparer> (partyItem, this);
	}

	void GetLeaderSkill () {
		UnitParty up = DeserializeData<UnitParty> ();
		int id = up.items [0].unitUniqueId;
		AddLeadSkill(id);
		id =  up.items [4].unitUniqueId;
		AddLeadSkill (id);
	}

	void AddLeadSkill (int id) {
		if (id != -1) {
			UserUnitInfo firstLeader = GlobalData.tempUserUnitInfo [id];
//			Debug.LogError("AddLeadSkill : " + id + "  firstLeader.GetLeadSKill()  : " + firstLeader.GetLeadSKill());
			ProtobufDataBase pdb = GlobalData.tempNormalSkill[firstLeader.GetLeadSKill()];
			leaderSkill.Add(id,pdb);
		}
	}
	
	UnitParty GetunitParty(){
		return DeserializeData<UnitParty> ();
	} 
	
	public int Compare (object first, object second) {
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
			bloodNum += GlobalData.tempUserUnitInfo [unitUniqueID].GetBlood();
		}

		return bloodNum;
	}

	public Dictionary<int,int> GetPartyItem () {
		Dictionary<int,int> temp = new Dictionary<int, int> ();
		UnitParty up = DeserializeData<UnitParty> ();
		for (int i = 0; i < up.items.Count; i++) {
			PartyItem pi = up.items[i];
			temp.Add(pi.unitPos,pi.unitUniqueId);
		}
		//Debug.LogError("party count: " + temp.Count);
		return temp;
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

	public List<UserUnitInfo> GetUserUnit () {
		UnitParty uup = DeserializeData<UnitParty> ();
		List<UserUnitInfo> temp = new List<UserUnitInfo> ();
		foreach (var item in uup.items) {
			UserUnitInfo uui = GlobalData.tempUserUnitInfo[item.unitUniqueId];
			temp.Add(uui);
		}
		return temp;
	}
}

public class CalculateSkillUtility {
	public List<uint> haveCard = new List<uint>();
	public List<int> alreadyUseSkill = new List<int>();
}

public class AttackImageUtility {
	public int attackProperty = -1;
	public int userProperty = -1;
	public int attackID = -1;
	public UITexture attackUI = null;
	//------------test need data, delete it behind test done------------//
	//------------------------------------------------------------------//
	public int skillID = -1;
	public void PlayAttack () {
		if(attackUI != null) {
			attackUI.enabled = false;
			attackUI = null;
		}
	}
}

public class AttackInfo {
	private static int sequenceID = -1;
	public static void ClearData () {
		sequenceID = -1;
	}

	public AttackInfo (){
		sequenceID++;
		this.attackID = sequenceID;
	}

	private int attackID = -1;
	public int AttackID {
		get {return attackID;}
	}

	private int userUnitID = -1;
	public int UserUnitID {
		get { return userUnitID; }
		set { userUnitID = value; }
	}

	private int userPos = -1;
	public int UserPos {
		get {return userPos;}
		set {userPos = value;}
	}

	private int needCardNumber = -1;
	public int NeedCardNumber {
		get {return needCardNumber;}
		set {needCardNumber = value;}
	}
	
	private int skillID = -1;
	public int SkillID {
		get {return skillID;}
		set {skillID = value;}
	}
	
	private int attackType = -1;
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

	private int continuAttackMultip = 1;
	public int ContinuAttackMultip {
		get {return continuAttackMultip;}
		set {continuAttackMultip = value;}
	}

	private int enemyID = -1;

	public int EnemyID {
		get {return enemyID;}
		set {enemyID = value;}
	}

	private float injuryValue ;
	public float InjuryValue
	{
		get {return injuryValue;}
		set {injuryValue = value;}
	}

	private Object effect;
	public Object Effect {
		get{return effect;}
		set{effect = value;}
	}
	//------------test need data, delete it behind test done------------//
	//------------------------------------------------------------------//
	//public int originIndex = -1;
}

public class AISortByCardNumber : IComparer{
	public int Compare (object x, object y)
	{
		AttackInfo ai1 = x as AttackInfo;
		AttackInfo ai2 = y as AttackInfo;
		return ai1.NeedCardNumber.CompareTo(ai2.NeedCardNumber);
	}
}

public class AISortByUserpos : IComparer{
	public int Compare (object x, object y)
	{
		AttackInfo ai1 = x as AttackInfo;
		AttackInfo ai2 = x as AttackInfo;
		return ai1.UserPos.CompareTo(ai2.UserPos);
	}
}