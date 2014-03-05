using bbproto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPartyInfo : ProtobufDataBase {
	private PartyInfo	instance;
	private List<TUnitParty> partyList;
	private bool isModified = false;

	public TPartyInfo(PartyInfo inst) : base (inst) { 
		instance = inst;

		AssignParty ();
	}

	private void AssignParty() {
		this.partyList = new List<TUnitParty>();

		foreach (UnitParty party in instance.partyList) {

			foreach(PartyItem item in party.items) {
				LogHelper.Log("--before sort ==> item{0}: {1}", item.unitPos, item.unitUniqueId);
			}
			
			party.items.Sort( SortParty );
			
			foreach(PartyItem item in party.items) {
				LogHelper.Log("++after sort party ==> item{0}: {1}", item.unitPos, item.unitUniqueId);
				
			}

			TUnitParty pi = new TUnitParty(party);
			partyList.Add(pi);

		}
	}

	private static int SortParty(PartyItem item1, PartyItem item2) {
		if (item1.unitPos > item2.unitPos) {
			return 1;
		} else if (item1.unitPos < item2.unitPos) {
			return -1;
		}
		return 0;
	}
	

	//// property ////
	public	int	CurrentPartyId { 
		get { return instance.currentParty; } 
		set { instance.currentParty = value; }
	}

	public	TUnitParty	CurrentParty { 
		get { return this.partyList[CurrentPartyId]; } 
	}

	public	TUnitParty	NextParty { 
		get {
			CurrentPartyId += 1;
			return this.partyList[CurrentPartyId]; 
		} 
	}

	public	TUnitParty	PrevParty { 
		get { 
			CurrentPartyId -= 1;
			return this.partyList[CurrentPartyId]; 
		} 
	}

	public	bool ChangeParty(PartyItem item) { 

		if( CurrentPartyId >= instance.partyList.Count ){
			LogHelper.LogError("TPartyInfo.ChangeParty:: CurrentPartyId:{0} is invalid.", CurrentPartyId);
			return false;
		}

		if( item.unitPos >= instance.partyList[CurrentPartyId].items.Count ){
			LogHelper.LogError("TPartyInfo.ChangeParty:: item.unitPos:{0} is invalid.", item.unitPos);
			return false;
		}

		isModified = true;
		CurrentParty.SetPartyItem(item);
		instance.partyList[CurrentPartyId].items[item.unitPos] = item;

		return true;
	}
}




