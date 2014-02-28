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
			uiitem.id 			= (uint)i;
			uiitem.name			= "unit_" + i;
			uiitem.type 		= (EUnitType)i;
			uiitem.skill1 		= (i - 1) * 2;
			uiitem.skill2 		= (i - 1) * 2 + 1;
			uiitem.powerType = new PowerType();
			uiitem.powerType.attackType = 2;
			uiitem.powerType.expType = 1;
			uiitem.powerType.hpType = 3;

			uiitem.rare 		= i;
			uiitem.maxLevel 	= 10;
			if(i == 1){
				uiitem.leaderSkill = 21;
				uiitem.activeSkill = 32;
			}
			if(i == 2) {
				uiitem.activeSkill = 38;
			}
			if(i == 5) {
				uiitem.leaderSkill = 22;
			}

			uiitem.passiveSkill = 49;
			TUnitInfo tui = new TUnitInfo(uiitem);
			GlobalData.unitInfo.Add(uiitem.id, tui);
		}

//		GlobalData.unitInfo [1].unitBaseInfoID = 181;
//		GlobalData.unitInfo [2].unitBaseInfoID = 85;
//		GlobalData.unitInfo [3].unitBaseInfoID = 89;
//		GlobalData.unitInfo [4].unitBaseInfoID = 80;
//		GlobalData.unitInfo [5].unitBaseInfoID = 87;
	}

	//Lynn Add
	void AddUnitInfoConfig(){
//		UnitInfo unitInfo;
//		TempUnitInfo tui;
//
//		unitInfo = new UnitInfo();
//		unitInfo.id = 1;
//		unitInfo.name = "aaa";
//		unitInfo.type = EUnitType.UDARK;
//		unitInfo.cost = 6;
//		tui = new TempUnitInfo(unitInf o);
//		GlobalData.tempUnitInfo.Add(unitInfo.id, tui);

	}



	void GenerateUserUnit () {
		for (uint i = 1; i < maxCount; i++) {
			UserUnit uu 		= new UserUnit ();
			uu.uniqueId 		= i;
			uu.unitId 			= i;
			uu.exp 				= 0;
			uu.level 			= 1;
			uu.addAttack 		= (int)i;
			uu.addDefence		= 0;
			uu.addHp 			= (int)i;
			uu.limitbreakLv 	= 2;
			uu.getTime 			= 0;
			TUserUnit uui 	= new TUserUnit (uu);
			GlobalData.userUnitInfo.Add (i, uui);
		}
//		GlobalData.userUnitInfo [1].unitBaseInfo = 181;
//		GlobalData.userUnitInfo [2].unitBaseInfo = 85;
//		GlobalData.userUnitInfo [3].unitBaseInfo = 89;
//		GlobalData.userUnitInfo [4].unitBaseInfo = 80;
//		GlobalData.userUnitInfo [5].unitBaseInfo = 87;
	}

	void GenerateUserUnitParty () {
		UnitParty up = new UnitParty ();
		up.id = 0;
		for (int i = 1; i <  6; i++) {
			PartyItem pi = new PartyItem();
			pi.unitPos = i;
			pi.unitUniqueId = (uint)i;
			up.items.Add(pi);
		}
		TUnitParty upi = new TUnitParty (up);

		ModelManager.Instance.AddData (ModelEnum.UnitPartyInfo, upi);
	}
}

public class ConfigUserUnit {
	
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

//public class AttackInfo {
//	private static int sequenceID = -1;
//	public static void ClearData () {
//		sequenceID = -1;
//	}
//
//	public AttackInfo (){
//		sequenceID++;
//		this.attackID = sequenceID;
//	}
//
//	private int attackID = -1;
//	public int AttackID {
//		get {return attackID;}
//	}
//
//	private uint userUnitID = 0;
//	public uint UserUnitID {
//		get { return userUnitID; }
//		set { userUnitID = value; }
//	}
//
//	private int userPos = -1;
//	public int UserPos {
//		get {return userPos;}
//		set {userPos = value;}
//	}
//
//	private int needCardNumber = -1;
//	public int NeedCardNumber {
//		get {return needCardNumber;}
//		set {needCardNumber = value;}
//	}
//	
//	private int skillID = -1;
//	public int SkillID {
//		get {return skillID;}
//		set {skillID = value;}
//	}
//	
//	private int attackType = -1;
//	public int AttackType {
//		get { return attackType; }
//		set {attackType = value; }
//	}
//	
//	private int attackRange ;
//	/// <summary>
//	/// 0 = single attack
//	/// 1 = all attack
//	/// 2 = recover hp
//	/// </summary>
//	/// <value>The attack range.</value>
//	public int AttackRange {
//		get { return attackRange; }
//		set { attackRange = value; }
//	}
//	
//	private float attackValue ;
//	public float AttackValue {
//		get {return attackValue; }
//		set {attackValue = value;}
//	}
//
//	private int continuAttackMultip = 1;
//	public int ContinuAttackMultip {
//		get {return continuAttackMultip;}
//		set {continuAttackMultip = value;}
//	}
//
//	private uint enemyID = 0;
//
//	public uint EnemyID {
//		get {return enemyID;}
//		set {enemyID = value;}
//	}
//
//	private int injuryValue ;
//	public int InjuryValue
//	{
//		get {return injuryValue;}
//		set {injuryValue = value;}
//	}
//
//	private Object effect;
//	public Object Effect {
//		get {return effect;}
//		set {effect = value;}
//	}
//	private bool ignoreDefense = false;
//	public bool IgnoreDefense {
//		get {return ignoreDefense;}
//		set {ignoreDefense = value;}
//	}
//
//	private int attackRound = 1; 
//	public int AttackRound {
//		get { return attackRound; }
//		set { attackRound = value; }
//	}
//	//------------test need data, delete it behind test done------------//
//	//------------------------------------------------------------------//
//	//public int originIndex = -1;
//}
//
//public class AISortByCardNumber : IComparer{
//	public int Compare (object x, object y)
//	{
//		AttackInfo ai1 = x as AttackInfo;
//		AttackInfo ai2 = y as AttackInfo;
//		return ai1.NeedCardNumber.CompareTo(ai2.NeedCardNumber);
//	}
//}
//
//public class AISortByUserpos : IComparer{
//	public int Compare (object x, object y)
//	{
//		AttackInfo ai1 = x as AttackInfo;
//		AttackInfo ai2 = x as AttackInfo;
//		return ai1.UserPos.CompareTo(ai2.UserPos);
//	}
//}