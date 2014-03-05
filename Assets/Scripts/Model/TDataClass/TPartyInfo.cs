using bbproto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPartyInfo : ProtobufDataBase {
	private PartyInfo	instance;
	private List<TUnitParty> partyList;
	private bool isPartyItemModified = false;
	private bool isPartyGroupModified = false;
	private int originalPartyId = 0;

	public TPartyInfo(PartyInfo inst) : base (inst) { 
		instance = inst;

		AssignParty ();
	}

	private void AssignParty() {
		originalPartyId = instance.currentParty;

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
		get { 
			if( CurrentPartyId >= this.partyList.Count -1 )
				return null;

			return this.partyList[CurrentPartyId]; } 
	}

	public	TUnitParty	NextParty { 
		get {
			CurrentPartyId += 1;

			if( CurrentPartyId>4)
				CurrentPartyId = 0;
			if( CurrentPartyId >= this.partyList.Count -1 )
				return null;

			isPartyGroupModified = (CurrentPartyId!=originalPartyId);
			return this.partyList[CurrentPartyId]; 
		} 
	}

	public	TUnitParty	PrevParty { 
		get { 
			CurrentPartyId -= 1;
			if (CurrentPartyId<0)
				CurrentPartyId=4;


			if( CurrentPartyId >= this.partyList.Count -1 )
				return null;

			isPartyGroupModified = (CurrentPartyId!=originalPartyId);

			return this.partyList[CurrentPartyId]; 
		} 
	}

	public	bool ChangeParty(int pos, uint unitUniqueId) { 

		if( CurrentPartyId >= instance.partyList.Count ){
			LogHelper.LogError("TPartyInfo.ChangeParty:: CurrentPartyId:{0} is invalid.", CurrentPartyId);
			return false;
		}

		if( pos >= instance.partyList[CurrentPartyId].items.Count ){
			LogHelper.LogError("TPartyInfo.ChangeParty:: item.unitPos:{0} is invalid.", pos);
			return false;
		}

		isPartyItemModified = true;
		CurrentParty.SetPartyItem(pos, unitUniqueId);

		//updte 
		PartyItem item = new PartyItem();
		item.unitPos = pos;
		item.unitUniqueId = unitUniqueId;
		instance.partyList[CurrentPartyId].items[pos] = item;

		return true;
	}

	public bool IsModified {
		get { return this.isPartyItemModified||isPartyGroupModified; }
	}

	public void ExitParty() {
		if ( IsModified ) {
			MsgCenter.Instance.Invoke (CommandEnum.ReqModifyParty, this);
		}
	}
}




