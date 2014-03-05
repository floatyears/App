using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageUILogic : ConcreteComponent {


	public PartyPageUILogic(string uiName):base(uiName) {}

	public override void Callback(object data){
		base.Callback(data);

		string call = data as string;
		if(call == "PageForward"){
			//CallBackNextPageData();
		}
		if(call == "PageBack"){

		}

	}

	void GetCurPartyPageData(){

	}

	void CallBackNextPageData(){
//		List<TUserUnit> curPartyUnitDic = GlobalData.partyInfo.CurrentParty.PartyItems;
//		return curPartyUnitDic
//		GlobalData.partyInfo.CurrentParty.PartyItems[0]
	}

	void CallBackPrePageData(){

	}



}
