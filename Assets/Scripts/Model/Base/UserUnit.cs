using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class UserUnitParty {
	public static Dictionary<int,UnitParty> userUnitPartyInfo = new Dictionary<int, UnitParty> ();

}

public class UserUnitInfo : ProtobufDataBase {
	public UserUnitInfo(UserUnit instance) : base (instance) {
		EAttackType eat = EAttackType.ATK_ALL;
		EValueType evt = EValueType.FIXED;
		EBoostType ebt = EBoostType.BOOST_ATTACK;
		EBoostTarget ebta = EBoostTarget.UNIT_RACE;
		EPeriod ep = EPeriod.EP_EVERY_ROUND;

	}
	
	~UserUnitInfo() {

	}
}

public class PartyItemInfo : ProtobufDataBase {
	public PartyItemInfo (PartyItem instance) : base (instance) {
		UnitInfo UI = new UnitInfo ();
	}
}