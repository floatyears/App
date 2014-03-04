using bbproto;
using System.Collections;
using System.Collections.Generic;


public class TPartyInfo : ProtobufDataBase {
	private PartyInfo	instance;
	private List<TUnitParty> partyList;
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

	public	bool ChangeParty(TUnitParty party) { 
		this.partyList[party.ID] = party;

		return true;
	}
}




