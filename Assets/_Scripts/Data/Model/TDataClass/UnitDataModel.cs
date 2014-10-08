using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitDataModel : ProtobufDataBase {

	private StageInfo stageInfo;
	public StageInfo StageInfo {
		get { return stageInfo; }
		set { 
			stageInfo = value; 
//			stageInfo.InitStageId();
		}
	}
	
	public int evolvePartyID = 0;
	UnitParty tup;

	private UnitCatalogInfo catalogData;

	public void StoreData () {
		DataCenter.evolveInfo = this;
		UnitParty up = new UnitParty (0);
		
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
		
		evolvePartyID = DataCenter.Instance.UnitData.PartyInfo.AllParty.Count;
		
		up.id = evolvePartyID;
		
		tup = up;
		
		currentPartyID = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId;
		
		DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId = evolvePartyID;
		
		DataCenter.Instance.UnitData.PartyInfo.AllParty.Add (tup);
		
		BattleConfigData.Instance.party = tup;
		
		//		Debug.LogError (DataCenter.Instance.UnitData.PartyInfo.AllParty.Count + " id : " + tup.ID);
	}
	
	private int currentPartyID = 0;
	
	public  void ClearData () {
		DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId = currentPartyID;
		DataCenter.Instance.UnitData.PartyInfo.AllParty.Remove (tup);
		DataCenter.evolveInfo = null;
	}
	
	public List<UserUnit> evolveParty = new List<UserUnit>();


	private PartyInfo partyInfo;
	public PartyInfo PartyInfo { 
		get { return partyInfo; }
		set { 
			partyInfo = value;
			value.assignParty();
		}
	}


	private UnitCatalogInfo catalogInfo;
	public UnitCatalogInfo CatalogInfo { 
		get { return catalogInfo; }
		set { catalogInfo = value; }
	}


	private UserUnitList userUnitList;
	public UserUnitList UserUnitList {
		get { 
			if (userUnitList == null) {
				userUnitList = new UserUnitList();
			}
			return userUnitList; 
		}
		set { userUnitList = value; } 
	}

	private Dictionary<uint, UnitInfo> unitInfo = new Dictionary<uint, UnitInfo>();
	
	public UnitInfo GetUnitInfo(uint unitID) {
		if (unitInfo.ContainsKey(unitID)) {
			UnitInfo tui = unitInfo[unitID];
			return tui;
		}
		else {
			UnitInfo tui = DGTools.LoadUnitInfoProtobuf(unitID);
			if(tui == null) {
				Debug.LogError("uintid : " + unitID + " is invalid");
				return null;
			}
			unitInfo.Add(tui.id,tui);
			return tui;
		}
	}

	private Dictionary<int,PowerTable> unitValue = new Dictionary<int, PowerTable>();
	public Dictionary<int,PowerTable> UnitValue {
		get { 
			return unitValue; 
		}
		set { unitValue = value; } 
	}

	public int GetUnitValue(int type, int level) {
		if ( !UnitValue.ContainsKey(type)) {
			Debug.LogError("FATAL ERROR: GetUnitValue() :: type:"+type+" not exists in UnitValue.");
			return 0;
		}
		
		PowerTable pti = UnitValue[type];
		return pti.GetValue(level);
	}
}
