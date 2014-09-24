using bbproto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bbproto{
public partial class PartyInfo : ProtoBuf.IExtensible {
	
    const int MAX_PARTY_GROUP_NUM = 5;

    private bool isPartyItemModified = false;
    private bool isPartyGroupModified = false;
	public bool IsPartyGroupModified{
		get {
			return isPartyGroupModified;
		}
		set {
			isPartyGroupModified = value;
			MsgCenter.Instance.Invoke(CommandEnum.ModifiedParty, null);
		}
	}

    private int originalPartyId = 0;

    public bool UnitIsInCurrentParty(uint uniqueId) {
        return CurrentParty.HasUnit(uniqueId);
    }

	public bool UnitIsInCurrentParty(UserUnit tuu) {
		if (tuu.userID != DataCenter.Instance.UserData.UserInfo.userId) {
			return false;
		}

		return CurrentParty.HasUnit(tuu.uniqueId);
	}

	public bool UnitIsInParty(UserUnit tuu) {
		if( tuu==null ) {
			Debug.LogError("UnitIsInParty(tuu) >>> ERROR: tuu is NULL.");
			return false;
		}
		if (tuu.userID != DataCenter.Instance.UserData.UserInfo.userId) {
			return false;
		}
		return UnitIsInParty(tuu.uniqueId);
	}

    public bool UnitIsInParty(uint uniqueId) {
        foreach (var party in partyList) {
            if (party.HasUnit(uniqueId)) {
                return true;
            }
        }
        return false;
    }

    public void assignParty() {
		originalPartyId = currentParty;
		currParty = partyList [CurrentPartyId];
//        this.partyList = new List<UnitParty>();

        foreach (UnitParty party in partyList) {
            Dictionary<EUnitType, int> atkVal = new Dictionary<EUnitType, int>();
			
            party.items.Sort(SortParty);
			party.Init();

//			foreach (var item in party.items) {
//				Debug.LogError(party.id + " item unitPos : " + item.unitPos + " item unitUniqueId : " + item.unitUniqueId);
//			}

//			partyList.Add(party);
        }
    }

    private static int SortParty(PartyItem item1, PartyItem item2) {
        if (item1.unitPos > item2.unitPos) {
            return 1;
        }
        else if (item1.unitPos < item2.unitPos) {
            return -1;
        }
        return 0;
    }
	
    public	int	 CurrentPartyId { 
        get { return currentParty; } 
        set { currentParty = value; 
				currParty = partyList[CurrentPartyId];
			}
    }

	private UnitParty currParty;

    public	UnitParty CurrentParty { 
        get { 
			return currParty;
        } 
    }

    public List<UnitParty> AllParty {
        get {
            return this.partyList;
        }
    }

    public UnitParty NextParty { 
        get {
            if (this.partyList == null)
                return null;

            CurrentPartyId += 1;

            if (CurrentPartyId > MAX_PARTY_GROUP_NUM - 1)
                CurrentPartyId = 0;
            if (CurrentPartyId > this.partyList.Count - 1)
                return null;

			IsPartyGroupModified = (CurrentPartyId != originalPartyId);
            currentParty = CurrentPartyId;
            return this.partyList[CurrentPartyId]; 
        } 
    }

	public UnitParty GetNextPartyData {
		get {
			if (this.partyList == null)
				return null;
			
			int nextPartyID = CurrentPartyId + 1;
			
			if (nextPartyID > MAX_PARTY_GROUP_NUM - 1)
				nextPartyID = 0;
			if (nextPartyID > this.partyList.Count - 1)
				return null;

			return this.partyList[nextPartyID]; 
		}
	}

	public UnitParty GetPrePartyData {
		get {
			if (this.partyList == null)
				return null;
			
			int prePartyID = CurrentPartyId - 1;

			if (prePartyID < 0)
				prePartyID = MAX_PARTY_GROUP_NUM - 1;
			
			if (prePartyID > this.partyList.Count - 1) {
				return null;
			}

			return this.partyList[prePartyID]; 
		}
	}

    public UnitParty PrevParty { 
		get { 
            if (this.partyList == null)
                return null;

            CurrentPartyId -= 1;
            if (CurrentPartyId < 0)
                CurrentPartyId = MAX_PARTY_GROUP_NUM - 1;

            if (CurrentPartyId > this.partyList.Count - 1) {
                return null;
            }

			IsPartyGroupModified = (CurrentPartyId != originalPartyId);
            currentParty = CurrentPartyId;
            return this.partyList[CurrentPartyId]; 
        } 
    }

	public UnitParty TargetParty(int targetPartyId){
		if(this.partyList == null)
			return null;
		if((targetPartyId > MAX_PARTY_GROUP_NUM - 1) || targetPartyId < 0){
			Debug.LogError("Target Party ID is Invaild!!!");
			return null;
		}

		CurrentPartyId = targetPartyId;

		isPartyGroupModified = (CurrentPartyId != originalPartyId);
		currentParty = CurrentPartyId;
		return this.partyList[CurrentPartyId]; 
	}


	public bool IsCostOverflow(int pos, uint newUniqueId) {
		if( newUniqueId != 0 ) { // check cost max
			int newCost = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit( newUniqueId ).UnitInfo.cost;
			int oldCost = 0;
			if( CurrentParty.UserUnit[pos] != null ) {
					oldCost = CurrentParty.UserUnit[pos].UnitInfo.cost;
			}
			
			if ( (CurrentParty.TotalCost - oldCost + newCost) > DataCenter.Instance.UserData.UserInfo.costMax ) {
				TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("CostLimitText"));
				Debug.LogError("TPartyInfo.ChangeParty:: costTotal="+(CurrentParty.TotalCost - oldCost + newCost)+" > "+DataCenter.Instance.UserData.UserInfo.costMax);
				return true;
			}
		}

		return false;
	}

	public	bool ChangeParty(int pos, uint newUniqueId) { 
        if (CurrentPartyId >= partyList.Count) {
			Debug.LogError("TPartyInfo.ChangeParty:: CurrentPartyId:"+CurrentPartyId+" is invalid.");
            return false;
        }

        if (pos >= partyList[CurrentPartyId].items.Count) {
			Debug.LogError("TPartyInfo.ChangeParty:: item.unitPos:"+pos+" is invalid.");
            return false;
        }

		if( IsCostOverflow(pos, newUniqueId) ) 
			return false;

        isPartyItemModified = true;
		CurrentParty.SetPartyItem(pos, newUniqueId);

        //update instance
        PartyItem item = new PartyItem();
        item.unitPos = pos;
		item.unitUniqueId = newUniqueId;
        partyList[CurrentPartyId].items[pos] = item;

		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.UnitData.PartyInfo.CurrentParty);
		MsgCenter.Instance.Invoke(CommandEnum.ModifiedParty, null);
        return true;
    }

//	public void ChangeParty(Dictionary<int, PageUnitView> viewData) { 
//		TUnitParty tup = CurrentParty;
//		int count = viewData.Count;
//		for (int i = tup.Object.items.Count; i < count; i++) {
//			PartyItem pi = new PartyItem();
//			tup.Object.items.Add(pi);
//		}
//
//		foreach (var item in viewData) {
//			//Debug.LogError("ChangeParty : " + item.Key);
//
//			PartyItem pi = tup.Object.items[item.Key];
//			pi.unitPos = item.Key;
//			if(item.Value.UserUnit == null) {
//				pi.unitUniqueId = 0;
//			}
//			else {
//				pi.unitUniqueId = item.Value.UserUnit.ID;
//			}
//		}
//	}

    public bool IsModified {
        get { return this.isPartyItemModified || isPartyGroupModified; }
    }

    public void ExitParty() {
//		Debug.LogError("ExitParty");
        if (IsModified) {
			UnitController.Instance.ChangeParty(this,onRspChangeParty);
        }
		//Debug.Log("ExitParty(), IsModified is : " + IsModified);
    }

    public void onRspChangeParty(object data) {
        //nothing to do
        if (data != null) {
            bbproto.RspChangeParty rsp = data as bbproto.RspChangeParty;

            LogHelper.Log("rspChangeParty code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			if (rsp.header.code != ErrorCode.SUCCESS){
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);

			} else { //change party is success
				if ( this.isPartyItemModified ){ 
					Umeng.GA.Event("PartyChangeMember");
				}
				if ( this.isPartyGroupModified ) {
					Umeng.GA.Event("PartyChangeGroup");
				}

				originalPartyId = currentParty;
				isPartyItemModified = false;
				isPartyGroupModified = false;
			}
//			bool success = (rspChangeParty.header.code == 0 );
        }

    }
}

}


