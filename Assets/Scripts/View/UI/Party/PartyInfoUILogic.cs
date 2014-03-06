using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoUILogic : ConcreteComponent {

	public PartyInfoUILogic(string uiName):base(uiName) {}

	void GetPartyInfoData(){
		Dictionary<string, object> dataDic = new Dictionary<string, object>();
//		dataDic.Add("hp", GlobalData.partyInfo.CurrentParty.PartyItems[0].);
	}

}
