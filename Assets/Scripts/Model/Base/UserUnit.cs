using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class UserUnitParty {
	public static Dictionary<int,UnitParty> userUnitPartyInfo = new Dictionary<int, UnitParty> ();

}

public class AddBlood {


	public List<AttackInfo> CaculateAttack (List<uint> card,List<int> ignorSkillID) {
		return null;
	}
}

public class UserUnitInfo : ProtobufDataBase {
	public UserUnitInfo(UserUnit instance) : base (instance) { }
	private int currentBlood = -1;
	public int unitBaseInfo = -1;
	~UserUnitInfo() { }
	
	TempNormalSkill[] normalSkill = new TempNormalSkill[2];

	public int CalculateInjured(int attackType, int attackValue) {
		int beRetraintType = DGTools.BeRestraintType (attackType);
		int retraintType = DGTools.RestraintType (attackType);
		UserUnit uu = DeserializeData<UserUnit> ();
		int defense = DGTools.CaculateAddDefense (uu.addDefence);
		defense += GetUnitInfo ().power [uu.level].defense;
		float hurtValue = 0;
		if (beRetraintType == GetUnitInfo ().type) {
			hurtValue = attackValue * 2 - defense;
		} 
		else if (retraintType == GetUnitInfo ().type) {
			hurtValue = attackValue * 0.5f - defense;
		} 
		else {
			hurtValue = attackValue - defense;
		}
		if (hurtValue <= 0) {
			hurtValue = 1;
		}
		int hv = System.Convert.ToInt32 (hurtValue);
		currentBlood -= hv;
		return hv;
	}

	public List<AttackInfo> CaculateAttack (List<uint> card,List<int> ignorSkillID) {
		//Debug.LogError ("userunit id : " + GetUnitType () +  " card : " + card.Count + " ignorSkillID :" + ignorSkillID.Count);
		List<uint> copyCard 		= new List<uint> (card);
		List<AttackInfo> returnInfo = new List<AttackInfo> ();

		UserUnit uu 				= DeserializeData<UserUnit> ();
		UnitInfo ui					= GlobalData.tempUnitInfo [uu.id].DeserializeData<UnitInfo>();

		TempNormalSkill firstSkill	= GlobalData.tempNormalSkill [ui.skill1] as TempNormalSkill;
		TempNormalSkill secondSkill = GlobalData.tempNormalSkill [ui.skill2] as TempNormalSkill;

		if (normalSkill [0] == null) {
			AddSkill(firstSkill,secondSkill);
		}

		for (int i = 0; i < normalSkill.Length; i++) {
			TempNormalSkill tns = normalSkill[i];
			tns.DisposeUseSkillID(ignorSkillID);
			int count = tns.CalculateCard(copyCard);
			for (int j = 0; j < count; j++) {
				AttackInfo attack	= new AttackInfo();
				attack.AttackValue	= CaculateAttack(uu,ui,tns);
				attack.AttackType	= ui.type;
				attack.UserUnitID	= uu.uniqueId;
				tns.GetSkillInfo(attack);
				returnInfo.Add(attack);
			}
		}
		return returnInfo;
	}

	void AddSkill(TempNormalSkill firstSkill, TempNormalSkill secondSkill) {
		if (secondSkill.GetActiveBlocks() > firstSkill.GetActiveBlocks()) { // second skill first excute
			normalSkill[0] = secondSkill;
			normalSkill[1] = firstSkill;
		} 
		else {	
			normalSkill[0] = firstSkill;
			normalSkill[1] = secondSkill;
		}
	}

	protected int CaculateAttack (UserUnit uu, UnitInfo ui, TempNormalSkill tns) {
		int addAttack = uu.addAttack * 50;
		int attack = addAttack + ui.power [uu.level].attack;

		return tns.GetAttack(attack);
	}

	public int GetUnitType (){

		return GetUnitInfo().type;
	}

	UnitInfo GetUnitInfo() {
		UserUnit userUnit =  DeserializeData () as UserUnit;
		return GlobalData.tempUnitInfo [userUnit.id].DeserializeData<UnitInfo>();
	}

	public int GetBlood () {
		if (currentBlood == -1) {
			UserUnit uu = DeserializeData<UserUnit>();
			UnitInfo ui = GetUnitInfo() ;
			currentBlood += DGTools.CaculateAddBlood (uu.addHp);
			currentBlood += ui.power [uu.level].hp;
		}
//		Debug.LogError ("GetBlood : " + currentBlood);
		return currentBlood;
	}
}

public class PartyItemInfo : ProtobufDataBase {
	public PartyItemInfo (PartyItem instance) : base (instance) {
		UnitInfo UI = new UnitInfo ();
	}
}