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
		for (int i = 1; i <= 29; i++) {
			UnitInfo uiitem 	= new UnitInfo ();
			uiitem.id 			= (uint)i;
			uiitem.name			= "unit_" + i;
			uiitem.type 		= (EUnitType)(1+i%6);
			uiitem.skill1 		= (i - 1) * 2 % 10;
			uiitem.skill2 		= ((i - 1) * 2 + 1)%10;
			uiitem.powerType = new PowerType();
			uiitem.powerType.attackType = 2;
			uiitem.powerType.expType = 1;
			uiitem.powerType.hpType = 3;
			uiitem.cost = (i % 5);
			uiitem.race 		= (EUnitRace)(i%6);
			uiitem.rare 		= i%6;
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
//		for (uint i = 1; i < maxCount; i++) {
//			UserUnit uu 		= new UserUnit ();
//			uu.uniqueId 		= i;
//			uu.unitId 			= i;
//			uu.exp 				= 0;
//			uu.level 			= 1;
//			uu.addAttack 		= (int)i;
//			uu.addDefence		= 0;
//			uu.addHp 			= (int)i;
//			uu.limitbreakLv 	= 2;
//			uu.getTime 			= 0;
//			TUserUnit uui 	= new TUserUnit (uu);
////			if ( GlobalData.userInfo!=null )
////				GlobalData.userUnitList.Add (GlobalData.userInfo.UserId,uu.uniqueId, uui);
//		}
//		GlobalData.userUnitInfo [1].unitBaseInfo = 181;
//		GlobalData.userUnitInfo [2].unitBaseInfo = 85;
//		GlobalData.userUnitInfo [3].unitBaseInfo = 89;
//		GlobalData.userUnitInfo [4].unitBaseInfo = 80;
//		GlobalData.userUnitInfo [5].unitBaseInfo = 87;
	}

	void GenerateUserUnitParty () {
		UnitParty up = new UnitParty ();
		up.id = 0;
//		for (int i = 1; i <=  5; i++) {
//			PartyItem pi = new PartyItem();
//			pi.unitPos = i;
//			pi.unitUniqueId = (uint)i;
//			up.items.Add(pi);
//		}
		for (int i = 1; i < 4; i++) {
			PartyItem pi = new PartyItem();
			pi.unitPos = i;
			pi.unitUniqueId = (uint)i + 1;
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
