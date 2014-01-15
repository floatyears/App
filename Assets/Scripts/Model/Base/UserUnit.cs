using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class UserUnitParty {
	public static Dictionary<int,UnitParty> userUnitPartyInfo = new Dictionary<int, UnitParty> ();

}

public class UserUnitInfo : ProtobufDataBase {
	public UserUnitInfo(UserUnit instance) : base (instance) { }
	
	~UserUnitInfo() { }
	
	TempNormalSkill[] normalSkill = new TempNormalSkill[2];

	public List<AttackInfo> CaculateAttack (List<uint> card) {
		List<uint> copyCard = new List<uint> (card);
		List<AttackInfo> returnInfo = new List<AttackInfo> ();

		UserUnit uu = DeserializeData () as UserUnit;
		UnitInfo ui = GlobalData.tempUnitInfo [uu.id].DeserializeData() as UnitInfo;

		TempNormalSkill firstSkill = GlobalData.tempNormalSkill [ui.skill1] as TempNormalSkill;
		TempNormalSkill secondSkill = GlobalData.tempNormalSkill [ui.skill2] as TempNormalSkill;

		if (normalSkill [0] == null) {
			AddSkill(firstSkill,secondSkill);
		}

		for (int i = 0; i < normalSkill.Length; i++) {
			if(normalSkill[i].CalculateCard(copyCard)) {
				AttackInfo attack = new AttackInfo();
				attack.AttackValue = CaculateAttack(uu,ui);
				attack.AttackType = ui.type;
				attack.UserUnitID = uu.uniqueId;
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

	protected int CaculateAttack (UserUnit uu, UnitInfo ui) {
		int addAttack = uu.addAttack * 50;
		int attack = addAttack + ui.power [uu.level].attack;

		return attack;
	}

	public int CaculateAttack () {
		UserUnit userUnit =  DeserializeData () as UserUnit;
		UnitInfo unitInfo = GlobalData.tempUnitInfo [userUnit.id].DeserializeData() as UnitInfo;

		return CaculateAttack(userUnit,unitInfo);
	}


}

public class PartyItemInfo : ProtobufDataBase {
	public PartyItemInfo (PartyItem instance) : base (instance) {
		UnitInfo UI = new UnitInfo ();
	}
}