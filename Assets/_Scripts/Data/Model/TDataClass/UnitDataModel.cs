using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitDataModel : ProtobufDataBase {

	private StageInfo stageInfo;
	public StageInfo StageInfo {
		get { return stageInfo; }
		set { stageInfo = value; }
	}
	
	public int evolvePartyID = 0;
	UnitParty tup;

	private UnitCatalogDataModel catalogData;

	public void StoreData () {
		DataCenter.evolveInfo = this;
		UnitParty up = new UnitParty ();
		
		for (int i = 0; i < evolveParty.Count; i++) {
			PartyItem pi = new PartyItem();
			pi.unitPos = i;
			
			if(evolveParty[i] == null) {
				//				Debug.LogError(i + " evolveParty.count : " + evolveParty.Count);
				pi.unitUniqueId = 0;
			}
			else{
				pi.unitUniqueId = evolveParty[i].uniqueId;
			}
			up.items.Add(pi);
		}
		
		for (int i = evolveParty.Count; i < 4; i++) {
			PartyItem pi = new PartyItem();
			pi.unitPos = i;
			pi.unitUniqueId = 0; //data is null;
			up.items.Add(pi);
		}
		
		evolvePartyID = DataCenter.Instance.PartyInfo.AllParty.Count;
		
		up.id = evolvePartyID;
		
		tup = up;
		
		currentPartyID = DataCenter.Instance.PartyInfo.CurrentPartyId;
		
		DataCenter.Instance.PartyInfo.CurrentPartyId = evolvePartyID;
		
		DataCenter.Instance.PartyInfo.AllParty.Add (tup);
		
		BattleConfigData.Instance.party = tup;
		
		//		Debug.LogError (DataCenter.Instance.PartyInfo.AllParty.Count + " id : " + tup.ID);
	}
	
	private int currentPartyID = 0;
	
	public  void ClearData () {
		DataCenter.Instance.PartyInfo.CurrentPartyId = currentPartyID;
		DataCenter.Instance.PartyInfo.AllParty.Remove (tup);
		DataCenter.evolveInfo = null;
	}
	
	public List<UserUnit> evolveParty = new List<UserUnit>();
}
