using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TUnitParty : ProtobufDataBase, IComparer, ILeaderSkill {
	private List<PartyItem> partyItem = new List<PartyItem> ();		
	private Dictionary<uint,ProtobufDataBase> leaderSkill = new Dictionary<uint,ProtobufDataBase> ();


	private UnitParty instance;
	public TUnitParty (object instance) : base (instance) { 
		this.instance = instance as UnitParty;
		MsgCenter.Instance.AddListener (CommandEnum.ActiveReduceHurt, ReduceHurt);

		reAssignData();
	}
	
	public void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveReduceHurt, ReduceHurt);
	}


	public Dictionary<uint,ProtobufDataBase> LeadSkill {
		get { return leaderSkill; }
	}
	private Dictionary<int,TUserUnit> userUnit ;

	//!!! UserUnit Only Avaiable after called GetSkillCollection()
	public Dictionary<int,TUserUnit> UserUnit {
		get {
			if(userUnit == null) {
//				Debug.LogError("TUnitParty :: userunit is null. new it..");
				userUnit = new Dictionary<int,TUserUnit>();
				for (int i = 0; i < partyItem.Count; i++) {
					TUserUnit uui = GlobalData.userUnitList.GetMyUnit(partyItem[i].unitUniqueId);
					userUnit.Add(partyItem[i].unitPos,uui);
//					Debug.LogError("TUnitParty :: userunit.add "+i);
				}
			}else{
//				Debug.LogError("TUnitParty :: userunit is not null" + userUnit.Count);
			}
			return userUnit;
		}
	}

	AttackInfo reduceHurt = null;

	private int totalHp = 0;
	private int totalCost = 0;
	private Dictionary<EUnitType, int>  typeAttackValue = new Dictionary<EUnitType, int> ();

	private void initTypeAtk(Dictionary<EUnitType, int> atkVal, EUnitType type) {
		if ( !atkVal.ContainsKey( type ) )
			atkVal.Add(type, 0);
		else
			atkVal[type] = 0;
	}
	//calculate each Type Attack, party's totalHp, totalCost
	private void reAssignData() {
		List<TUserUnit> uu = GetUserUnit();
		if (uu == null){
			Debug.LogError("TUnitParty.AssignData :: GetUserUnit() return null.");
			return;
		}
//		LogHelper.LogError("uu.count:{0}", uu.Count);

		totalHp = totalCost = 0;
		initTypeAtk(typeAttackValue, EUnitType.UWIND);
		initTypeAtk(typeAttackValue, EUnitType.UFIRE);
		initTypeAtk(typeAttackValue, EUnitType.UWATER);
		initTypeAtk(typeAttackValue, EUnitType.ULIGHT);
		initTypeAtk(typeAttackValue, EUnitType.UDARK);
		initTypeAtk(typeAttackValue, EUnitType.UNONE);

		foreach(PartyItem item in instance.items) {
			if ( item.unitPos >= uu.Count ) {
				LogHelper.LogError("  Calculate party attack: INVALID item.unitPos{0} > count:{1}", item.unitPos, uu.Count);
				continue;
			}
			if ( uu[item.unitPos]==null ) //it's empty party item
				continue;

//			Debug.LogError("uu[item.unitPos].UnitInfo="+uu[item.unitPos].UnitInfo);

			EUnitType unitType = uu[item.unitPos].UnitInfo.Type;

			typeAttackValue[ unitType ] += uu[item.unitPos].Attack;
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
	private Dictionary<int, CalculateSkillUtility> alreadyUse = new Dictionary<int, CalculateSkillUtility> ();	 
	private Dictionary<int, List<AttackInfo>> attack = new Dictionary<int, List<AttackInfo>> ();
	public Dictionary<int, List<AttackInfo>> Attack {
		get {return attack;}
	}

	void ReduceHurt(object data) {
		reduceHurt = data as AttackInfo;
		if (reduceHurt != null) {
			if(reduceHurt.AttackRound == 0) {
				reduceHurt = null;
			}
		}
	}
	
	public int CaculateInjured (int attackType, float attackValue) {
		float Proportion = 1f / (float)partyItem.Count;
		float attackV = attackValue * Proportion;
		float hurtValue = 0;
		for (int i = 0; i < partyItem.Count; i++) {
			TUserUnit unitInfo = GlobalData.userUnitList.GetMyUnit(partyItem [i].unitUniqueId);
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
			crh = new CalculateRecoverHP ();		
		}
		CalculateSkillUtility skillUtility = CheckSkillUtility (areaItemID, cardID);
		List<AttackInfo> areaItemAttackInfo = CheckAttackInfo (areaItemID);
		areaItemAttackInfo.Clear ();
		TUserUnit tempUnitInfo;
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
			tempUnitInfo = GlobalData.userUnitList.GetMyUnit(partyItem [i].unitUniqueId);
			tempAttack.AddRange(tempUnitInfo.CaculateAttack (skillUtility.haveCard, skillUtility.alreadyUseSkill));
			if (tempAttack.Count > 0) {
				for (int j = 0; j < tempAttack.Count; j++) {
					AttackInfo ai 			= tempAttack [j];
					ai.UserPos 				= partyItem[i].unitPos;
					areaItemAttackInfo.Add (ai);
					skillUtility.alreadyUseSkill.Add (ai.SkillID);
					AttackImageUtility aiu 	= new AttackImageUtility();
					aiu.attackProperty		= ai.AttackType;
					aiu.userProperty 		= GlobalData.userUnitList.GetMyUnit(ai.UserUnitID).UnitType;
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
//		UnitParty up 	= DeserializeData<UnitParty> ();
		for (int i 		= 0; i < instance.items.Count; i++) {
			partyItem.Add(instance.items[i]);
		}
		GetLeaderSkill ();
		DGTools.InsertSort<PartyItem,IComparer> (partyItem, this);
	}
	
	void GetLeaderSkill () {
//		UnitParty up = DeserializeData<UnitParty> ();
		if (instance.items.Count > 0) {
		uint id = instance.items [0].unitUniqueId;
		AddLeadSkill(id);


		}
		else if(instance.items.Count > 4){
			uint id = instance.items [4].unitUniqueId;
		AddLeadSkill (id);
	}
	
	}
	
	void AddLeadSkill (uint id) {
		if (id != -1) {
			TUserUnit firstLeader = GlobalData.userUnitList.GetMyUnit(id);
			ProtobufDataBase pdb = GlobalData.skill[firstLeader.LeadSKill];
			if(leaderSkill.ContainsKey(id)) {
				leaderSkill[id] = pdb;
			}
			else{
				leaderSkill.Add(id,pdb);
			}
		}
	}
	
	public UnitParty Object{
		get { return instance; }
	} 

	public int ID { //party id
		get { return instance.id; }
	}

	public void SetPartyItem(int pos, uint unitUniqueId) {

		if( pos < instance.items.Count) {
			PartyItem item = new PartyItem();
			item.unitPos = pos;
			item.unitUniqueId = unitUniqueId;

			//update instance and userUnit
			instance.items[item.unitPos] = item;
			if( userUnit!=null && userUnit.ContainsKey(item.unitPos) )
				userUnit[item.unitPos] = GlobalData.userUnitList.GetMyUnit(item.unitUniqueId);
			//LogHelper.LogError(" SetPartyItem:: => pos:{0} uniqueId:{1}",item.unitPos, item.unitUniqueId);
			this.reAssignData();
		}
		else {
			//LogHelper.LogError ("SetPartyItem :: item.unitPos:{0} is invalid.", pos);
		}
	}

	public int Compare (object first, object second) {
		PartyItem firstUU 	= (PartyItem)first;
		PartyItem secondUU 	= (PartyItem)second;
		NormalSkill ns1 	= GetSecondSkill (firstUU);
		NormalSkill ns2 	= GetSecondSkill (secondUU);
		return ns1.activeBlocks.Count.CompareTo(ns2.activeBlocks.Count);
	}
	
	public int GetInitBlood () {
//		UnitParty up = DeserializeData<UnitParty> ();
		int bloodNum = 0;
		for (int i = 0; i < instance.items.Count; i++) {
			uint unitUniqueID = instance.items [i].unitUniqueId;
			bloodNum += GlobalData.userUnitList.GetMyUnit(unitUniqueID).InitBlood;
		}
		return bloodNum;
	}
	
	public int GetBlood () {
//		UnitParty up = DeserializeData<UnitParty> ();
		int bloodNum = 0;
		for (int i = 0; i < instance.items.Count; i++) {
			uint unitUniqueID = instance.items [i].unitUniqueId;
			bloodNum += GlobalData.userUnitList.GetMyUnit(unitUniqueID).Blood;
		}
		return bloodNum;
	}
	
	public Dictionary<int,uint> GetPartyItem () {
		Dictionary<int,uint> temp = new Dictionary<int, uint> ();
//		UnitParty up = DeserializeData<UnitParty> ();
		for (int i = 0; i < instance.items.Count; i++) {
			PartyItem pi = instance.items[i];
			temp.Add(pi.unitPos,pi.unitUniqueId);
//			Debug.LogError("pi.unitPos : " + pi.unitPos + " pi.unitUniqueId : " + pi.unitUniqueId);
		}
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
		TUserUnit tuu = GlobalData.userUnitList.GetMyUnit(pi.unitUniqueId);
		UserUnit uu1 = tuu.Object;
		UnitInfo ui1 = tuu.UnitInfo.Object;			 //GlobalData.unitInfo[uu1.unitId].DeserializeData<UnitInfo>();
		TNormalSkill ns = GlobalData.skill [ui1.skill2] as TNormalSkill;
		return ns.Object;
	}
	
	public List<TUserUnit> GetUserUnit () {
//		UnitParty uup = DeserializeData<UnitParty> ();

		List<TUserUnit> temp = new List<TUserUnit> ();
		foreach (var item in instance.items) {
			TUserUnit uui = GlobalData.userUnitList.GetMyUnit(item.unitUniqueId);
			temp.Add(uui);
		}
		return temp;
	}

	public SkillBase GetLeaderSkillInfo() {
		TUserUnit uui = GlobalData.userUnitList.GetMyUnit(instance.items[0].unitUniqueId);
		if ( uui == null )
			return null;

		SkillBaseInfo sbi = GlobalData.skill[ uui.LeadSKill ];
		if (sbi==null)
			return null;
		return sbi.GetSkillInfo();
	}

	public Dictionary<int, TUserUnit> GetPosUnitInfo () {
//		UnitParty uup = DeserializeData<UnitParty> ();
		Dictionary<int,TUserUnit> temp = new Dictionary<int,TUserUnit> ();
		foreach (var item in instance.items) {
//			Debug.LogError("item.unitUniqueId : " + item.unitUniqueId);
			TUserUnit uui = GlobalData.userUnitList.GetMyUnit(item.unitUniqueId);
//			Debug.LogError("GetPosUnitInfo : " + uui);
			temp.Add(item.unitPos,uui);
		}
		//		Debug.LogError (temp.Count + " GetPosUnitInfo " + uup.items.Count);
		return temp;
	}
}