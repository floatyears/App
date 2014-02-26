using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TUnitParty : ProtobufDataBase, IComparer, ILeaderSkill {
	private List<PartyItem> partyItem = new List<PartyItem> ();		
	private Dictionary<uint,ProtobufDataBase> leaderSkill = new Dictionary<uint,ProtobufDataBase> ();
	public Dictionary<uint,ProtobufDataBase> LeadSkill {
		get { return leaderSkill; }
	}
	private Dictionary<int,TUserUnit> userUnit ;
	public Dictionary<int,TUserUnit> UserUnit {
		get {
			if(userUnit == null) {
				userUnit = new Dictionary<int,TUserUnit>();
				for (int i = 0; i < partyItem.Count; i++) {
					TUserUnit uui = GlobalData.userUnitInfo[partyItem[i].unitUniqueId];
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

	private UnitParty instance;
	public TUnitParty (object instance) : base (instance) { 
		this.instance = instance as UnitParty;
		MsgCenter.Instance.AddListener (CommandEnum.ActiveReduceHurt, ReduceHurt);
	}
	
	public void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveReduceHurt, ReduceHurt);
	}
	
	AttackInfo reduceHurt = null;
	
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
			TUserUnit unitInfo = GlobalData.userUnitInfo [partyItem [i].unitUniqueId];
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
			tempUnitInfo = GlobalData.userUnitInfo [partyItem [i].unitUniqueId];
			tempAttack.AddRange(tempUnitInfo.CaculateAttack (skillUtility.haveCard, skillUtility.alreadyUseSkill));
			if (tempAttack.Count > 0) {
				for (int j = 0; j < tempAttack.Count; j++) {
					AttackInfo ai 			= tempAttack [j];
					ai.UserPos 				= partyItem[i].unitPos;
					areaItemAttackInfo.Add (ai);
					skillUtility.alreadyUseSkill.Add (ai.SkillID);
					AttackImageUtility aiu 	= new AttackImageUtility();
					aiu.attackProperty		= ai.AttackType;
					aiu.userProperty 		= GlobalData.userUnitInfo[ai.UserUnitID].GetUnitType();
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
		uint id = instance.items [0].unitUniqueId;
		AddLeadSkill(id);
		id = instance.items [4].unitUniqueId;
		AddLeadSkill (id);
	}
	
	void AddLeadSkill (uint id) {
		if (id != -1) {
			TUserUnit firstLeader = GlobalData.userUnitInfo [id];
			ProtobufDataBase pdb = GlobalData.normalSkill[firstLeader.GetLeadSKill()];
			if(leaderSkill.ContainsKey(id)) {
				leaderSkill[id] = pdb;
			}
			else{
				leaderSkill.Add(id,pdb);
			}
		}
	}
	
	UnitParty GetunitParty(){
		return instance;
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
			bloodNum += GlobalData.userUnitInfo [unitUniqueID].GetInitBlood();
		}
		return bloodNum;
	}
	
	public int GetBlood () {
//		UnitParty up = DeserializeData<UnitParty> ();
		int bloodNum = 0;
		for (int i = 0; i < instance.items.Count; i++) {
			uint unitUniqueID = instance.items [i].unitUniqueId;
			bloodNum += GlobalData.userUnitInfo [unitUniqueID].GetBlood();
		}
		return bloodNum;
	}
	
	public Dictionary<int,uint> GetPartyItem () {
		Dictionary<int,uint> temp = new Dictionary<int, uint> ();
//		UnitParty up = DeserializeData<UnitParty> ();
		for (int i = 0; i < instance.items.Count; i++) {
			PartyItem pi = instance.items[i];
			temp.Add(pi.unitPos,pi.unitUniqueId);
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
		TUserUnit tuu = GlobalData.userUnitInfo [pi.unitUniqueId];
		UserUnit uu1 = tuu.GetObject;
		UnitInfo ui1 = tuu.GetUnitInfo();			 //GlobalData.unitInfo[uu1.unitId].DeserializeData<UnitInfo>();
		return GlobalData.normalSkill [ui1.skill2].DeserializeData<NormalSkill> ();
	}
	
	public List<TUserUnit> GetUserUnit () {
//		UnitParty uup = DeserializeData<UnitParty> ();

		List<TUserUnit> temp = new List<TUserUnit> ();
		foreach (var item in instance.items) {
			TUserUnit uui = GlobalData.userUnitInfo[item.unitUniqueId];
			temp.Add(uui);
		}
		return temp;
	}
	
	public Dictionary<int, TUserUnit> GetPosUnitInfo () {
//		UnitParty uup = DeserializeData<UnitParty> ();
		Dictionary<int,TUserUnit> temp = new Dictionary<int,TUserUnit> ();
		foreach (var item in instance.items) {
			TUserUnit uui = GlobalData.userUnitInfo[item.unitUniqueId];
			temp.Add(item.unitPos,uui);
		}
		//		Debug.LogError (temp.Count + " GetPosUnitInfo " + uup.items.Count);
		return temp;
	}
}