using bbproto;
using System.Collections;
using System.Collections.Generic;


public class TPartyInfo : ProtobufDataBase {
	private PartyInfo	instance;
	public TPartyInfo(PartyInfo inst) : base (inst) { 
		instance = inst;

		AssignParty ();
	}

	private void AssignParty() {
		this.partys = new Dictionary<int, List<uint> > ();

		foreach (UnitParty party in instance.partyList) {
			List<uint> unitItems = new List<uint>();
			
			foreach(PartyItem item in party.items) {
				LogHelper.Log("--before sort ==> item{0}: {1}", item.unitPos, item.unitUniqueId);
			}
			
			party.items.Sort( SortParty );
			
			foreach(PartyItem item in party.items) {
				LogHelper.Log("++insert party ==> item{0}: {1}", item.unitPos, item.unitUniqueId);
				
				unitItems.Add(item.unitUniqueId);
			}
			
			partys.Add(party.id, unitItems);
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

	//<partyId, Party>
	private Dictionary<int, List<uint> > partys;

	
	//// property ////
	public	int	CurrentParty { 
		get { return instance.currentParty; } 
		set { instance.currentParty = value; }
	}

	public	Dictionary<int, List<uint> >	Party { 
		get { return this.partys; } 
	}
	
	public	bool ChangeParty(int partyId, List<uint> party) { 

		for (int i=0; i < partys[partyId].Count; i++){
			partys[partyId][i] = party[i];

			//TODO: update instance.partyList =>[partyId]
		}

		return true;
	}
}




