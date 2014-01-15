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
			uiitem.skill1 = i * 2;
			uiitem.skill2 = i * 2 + 1;
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
		for (int i = 1; i < maxCount; i++) {
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
	private List<PartyItem> partyItem = new List<PartyItem> ();

	public UnitPartyInfo (object instance) : base (instance) { }

	~UnitPartyInfo () { }

	public int GetBlood () {
		UnitParty up = DeserializeData<UnitParty> ();
		int bloodNum = 0;
		for (int i = 0; i < up.items.Count; i++) {
			int unitUniqueID = up.items[i].unitUniqueId;
			UserUnit uu = GlobalData.tempUserUnitInfo[unitUniqueID].DeserializeData<UserUnit>();
			UnitInfo ui = GlobalData.tempUnitInfo[uu.id].DeserializeData<UnitInfo>() ;
			bloodNum += DGTools.CaculateAddBlood(uu.addHp);
			bloodNum += ui.power[uu.level].hp;
		}
		return bloodNum;
	}


	public void GetSkillCollection() {
		partyItem = new List<PartyItem>();
		UnitParty up = DeserializeData<UnitParty> ();

		for (int i = 0; i < up.items.Count; i++) {
			partyItem.Add(up.items[i]);
		}

		DGTools.InsertSort<PartyItem,IComparer> (partyItem, this);
	
	}

	public int Compare (object first, object second)
	{
		PartyItem firstUU = (PartyItem)first;
		PartyItem secondUU = (PartyItem)second;
		NormalSkill ns1 = GetSkill (firstUU);
		NormalSkill ns2 = GetSkill (secondUU);

		return ns1.activeBlocks.Count.CompareTo(ns2.activeBlocks.Count);
	}


	NormalSkill GetSkill (PartyItem pi) {
		UserUnit uu1 = GlobalData.tempUserUnitInfo[pi.unitUniqueId].DeserializeData() as UserUnit;
		UnitInfo ui1 = GlobalData.tempUnitInfo[uu1.id].DeserializeData<UnitInfo>();
		return GlobalData.tempNormalSkill [ui1.skill2].DeserializeData<NormalSkill> ();
	}
}
