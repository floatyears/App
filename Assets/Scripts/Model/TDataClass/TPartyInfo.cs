using bbproto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPartyInfo : ProtobufDataBase {
	const int MAX_PARTY_GROUP_NUM = 4;
	private PartyInfo	instance;
	private List<TUnitParty> partyList;
	private bool isPartyItemModified = false;
	private bool isPartyGroupModified = false;
	private int originalPartyId = 0;
	//  dict: <{partyId}, <{UNIT_TYPE}, {ATK}> >
	private Dictionary<int, Dictionary<EUnitType, int> > attackValue = new Dictionary<int, Dictionary<EUnitType, int> > ();
	private Dictionary<int, int> totalHp = new  Dictionary<int, int> (); // <partyId, HP>

	public TPartyInfo(PartyInfo inst) : base (inst) { 
		instance = inst;

		AssignParty ();
	}

	private void AssignParty() {
		originalPartyId = instance.currentParty;

		this.partyList = new List<TUnitParty>();

		foreach (UnitParty party in instance.partyList) {
			Dictionary<EUnitType, int> atkVal = new Dictionary<EUnitType, int>();

//			foreach(PartyItem item in party.items) {
//				LogHelper.Log("--before sort ==> item{0}: {1}", item.unitPos, item.unitUniqueId);
//			}
			
			party.items.Sort( SortParty );
			
			TUnitParty tup = new TUnitParty(party);
			partyList.Add(tup);

			List<TUserUnit> userunit = tup.GetUserUnit();
			foreach(PartyItem item in party.items) {
				LogHelper.Log("++after sort party ==> item{0}: {1}", item.unitPos, item.unitUniqueId);
				if ( item.unitPos >= userunit.Count ) {
					LogHelper.Log("  Calculate party attack: INVALID item.unitPos{0} > count:{1}", item.unitPos, userunit.Count);
					continue;
				}
				EUnitType unitType = userunit[item.unitPos].UnitInfo.Type;
				if ( !atkVal.ContainsKey(unitType) )
					atkVal.Add(unitType, 0);
				if (!totalHp.ContainsKey(party.id) )
					totalHp.Add (party.id, 0);

				atkVal[ unitType ] += userunit[item.unitPos].Attack;
				totalHp[party.id] += userunit[item.unitPos].Hp;
			}

			attackValue.Add(party.id, atkVal );
		}

//		foreach(var item in attackValue) {
//			foreach(var it in item.Value) {
//
//				LogHelper.Log("atkVal: party{0} - <type:{1} atk={2}>", item.Key, it.Key, it.Value);
//			}
//		}

		foreach(var item in totalHp ) {
			LogHelper.Log("party{0}: totalHp={1}", item.Key, item.Value);
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
	public PartyInfo Object {
		get { return instance; } 
	}
	public	int	CurrentPartyId { 
		get { return instance.currentParty; } 
		set { instance.currentParty = value; }
	}

	public	TUnitParty	CurrentParty { 
		get { 
			if(this.partyList == null || CurrentPartyId > this.partyList.Count -1 ){
				LogHelper.Log("invalid partyList==null or CurrentPartyId:{0} is invalid.", CurrentPartyId);
				return null;
			}

			return this.partyList[CurrentPartyId]; } 
	}

	public	TUnitParty	NextParty { 
		get {
			if ( this.partyList == null )
				return null;

			CurrentPartyId += 1;

			if( CurrentPartyId >= MAX_PARTY_GROUP_NUM)
				CurrentPartyId = 0;
			if( CurrentPartyId > this.partyList.Count -1 )
				return null;

			isPartyGroupModified = (CurrentPartyId!=originalPartyId);
			instance.currentParty = CurrentPartyId;
			return this.partyList[CurrentPartyId]; 
		} 
	}

	public	TUnitParty	PrevParty { 
		get { 
			if ( this.partyList == null )
				return null;

			CurrentPartyId -= 1;
			if (CurrentPartyId<0)
				CurrentPartyId = MAX_PARTY_GROUP_NUM;

			if( CurrentPartyId > this.partyList.Count -1 ) {
				return null;
			}

			isPartyGroupModified = (CurrentPartyId!=originalPartyId);
			instance.currentParty = CurrentPartyId;
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
			MsgCenter.Instance.Invoke (CommandEnum.ReqChangeParty, this);
		}
	}

	public Dictionary<EUnitType, int> Attack {
		get {
			return attackValue[CurrentPartyId];
		}
	}

	public int TotalHp {
		get {
			return totalHp[CurrentPartyId];
		}
	}
}




