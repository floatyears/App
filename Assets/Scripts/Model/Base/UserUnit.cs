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
	
	~UserUnitInfo() { }
	
	TempNormalSkill[] normalSkill = new TempNormalSkill[2];

	public List<AttackInfo> CaculateAttack (List<uint> card,List<int> ignorSkillID) {
		Debug.LogError ("userunit id : " + GetUnitType () +  " card : " + card.Count + " ignorSkillID :" + ignorSkillID.Count);
		List<uint> copyCard 		= new List<uint> (card);
		List<AttackInfo> returnInfo = new List<AttackInfo> ();

		UserUnit uu 				= DeserializeData () as UserUnit;
		UnitInfo ui					= GlobalData.tempUnitInfo [uu.id].DeserializeData() as UnitInfo;

		TempNormalSkill firstSkill	= GlobalData.tempNormalSkill [ui.skill1] as TempNormalSkill;
		TempNormalSkill secondSkill = GlobalData.tempNormalSkill [ui.skill2] as TempNormalSkill;

		if (normalSkill [0] == null) {
			AddSkill(firstSkill,secondSkill);
		}

		for (int i = 0; i < normalSkill.Length; i++) {
			TempNormalSkill tns = normalSkill[i];
			tns.DisposeUseSkillID(ignorSkillID);
			int count = tns.CalculateCard(copyCard);
			Debug.Log("count --- : " +count);
			for (int j = 0; j < count; j++) {
				AttackInfo attack	= new AttackInfo();
				attack.AttackValue	= CaculateAttack(uu,ui);
				attack.AttackType	= ui.type;
				attack.UserUnitID	= uu.uniqueId;
				attack.SkillID		= tns.GetID();
				attack.AttackRange	= tns.GetAttackRange();
				returnInfo.Add(attack);
			}
		}
		Debug.LogError ("userunit id : " + GetUnitType () + " returnInfo.Count : " + returnInfo.Count);
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

	protected int CaculateAttack (UserUnit uu, UnitInfo ui) {
		int addAttack = uu.addAttack * 50;
		int attack = addAttack + ui.power [uu.level].attack;

		return attack;
	}

	public int CaculateAttack () {
		UserUnit userUnit =  DeserializeData () as UserUnit;
		UnitInfo unitInfo = GlobalData.tempUnitInfo [userUnit.id].DeserializeData<UnitInfo>();

		return CaculateAttack(userUnit,unitInfo);
	}

	public int GetUnitType (){

		return GetUnitInfo().type;
	}

	UnitInfo GetUnitInfo() {
		UserUnit userUnit =  DeserializeData () as UserUnit;
		return GlobalData.tempUnitInfo [userUnit.id].DeserializeData<UnitInfo>();
	}
}

public class PartyItemInfo : ProtobufDataBase {
	public PartyItemInfo (PartyItem instance) : base (instance) {
		UnitInfo UI = new UnitInfo ();
	}
}