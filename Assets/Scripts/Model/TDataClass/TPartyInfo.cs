using bbproto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPartyInfo : ProtobufDataBase {
    const int MAX_PARTY_GROUP_NUM = 5;
    private PartyInfo	instance;
    private List<TUnitParty> partyList;
    private bool isPartyItemModified = false;
    private bool isPartyGroupModified = false;
	public bool IsPartyGroupModified{
		get{
			return isPartyGroupModified;
		}
		set{
			isPartyGroupModified = value;
//			Debug.LogError("IsPartyGroupModified : " + isPartyGroupModified);
//			if(isPartyGroupModified)
				MsgCenter.Instance.Invoke(CommandEnum.ModifiedParty, null);
		}
	}

    private int originalPartyId = 0;

    public TPartyInfo(PartyInfo inst) : base (inst) { 
        instance = inst;

//		for (int i = 0; i < inst.partyList.Count; i++) {
//			for (int j = 0; j < inst.partyList[i].items.Count; j++) {
////				Debug.LogError(i + " j : " + j + " nst.partyList[i].item : " + inst.partyList[i].items[j].unitUniqueId);
//			}
//		}

        assignParty();
    }

    public bool UnitIsInCurrentParty(uint uniqueId) {
        return CurrentParty.HasUnit(uniqueId);
    }

	public bool UnitIsInCurrentParty(TUserUnit tuu) {
		if (tuu.userID != DataCenter.Instance.UserInfo.UserId) {
			return false;	
		}
		return CurrentParty.HasUnit(tuu.ID);
	}

	public bool UnitIsInParty(TUserUnit tuu) {
		if (tuu.userID != DataCenter.Instance.UserInfo.UserId) {
			return false;	
		}
		return UnitIsInParty(tuu.ID);
	}

    public bool UnitIsInParty(uint uniqueId) {
        foreach (var party in partyList) {
            if (party.HasUnit(uniqueId)) {
                return true;
            }
        }
        return false;
    }

    private void assignParty() {
        originalPartyId = instance.currentParty;

        this.partyList = new List<TUnitParty>();

        foreach (UnitParty party in instance.partyList) {
            Dictionary<EUnitType, int> atkVal = new Dictionary<EUnitType, int>();

//			foreach(PartyItem item in party.items) {
//				LogHelper.Log("--before sort ==> item{0}: {1}", item.unitPos, item.unitUniqueId);
//			}
			
            party.items.Sort(SortParty);
			
            TUnitParty tup = new TUnitParty(party);
            partyList.Add(tup);
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
	

    //// property ////
    public PartyInfo Object {
        get { return instance; } 
    }
    public	int	 CurrentPartyId { 
        get { return instance.currentParty; } 
        set { instance.currentParty = value; }
    }

    public	TUnitParty	CurrentParty { 
        get { 
            if (this.partyList == null || CurrentPartyId > this.partyList.Count - 1) {
                //LogHelper.Log("invalid partyList==null or CurrentPartyId:{0} is invalid.", CurrentPartyId);
                return null;
            }

            //LogHelper.LogError("CurrentParty[{0}].UserUnit.Count: {1}", CurrentPartyId,this.partyList[CurrentPartyId].GetUserUnit().Count);
//			for(int pos=0; pos<4; pos++){
//				if (pos < this.partyList[CurrentPartyId].GetUserUnit().Count )
//					LogHelper.LogError("CurrentParty[{0}].UserUnit[{1}].UniqueId: {2}", CurrentPartyId, pos, this.partyList[CurrentPartyId].GetUserUnit()[ pos ].ID);
//			}
		
            return this.partyList[CurrentPartyId];
        } 
    }

    public List<TUnitParty> AllParty {
        get {
            return this.partyList;
        }
    }

    public TUnitParty NextParty { 
        get {
            if (this.partyList == null)
                return null;

            CurrentPartyId += 1;

            if (CurrentPartyId > MAX_PARTY_GROUP_NUM - 1)
                CurrentPartyId = 0;
            if (CurrentPartyId > this.partyList.Count - 1)
                return null;

			IsPartyGroupModified = (CurrentPartyId != originalPartyId);
            instance.currentParty = CurrentPartyId;
            return this.partyList[CurrentPartyId]; 
        } 
    }

    public TUnitParty PrevParty { 
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
            instance.currentParty = CurrentPartyId;
            return this.partyList[CurrentPartyId]; 
        } 
    }

	public TUnitParty TargetParty(int targetPartyId){
		if(this.partyList == null)
			return null;
		if((targetPartyId > MAX_PARTY_GROUP_NUM - 1) || targetPartyId < 0){
			Debug.LogError("Target Party ID is Invaild!!!");
			return null;
		}

		CurrentPartyId = targetPartyId;

		isPartyGroupModified = (CurrentPartyId != originalPartyId);
		instance.currentParty = CurrentPartyId;
		return this.partyList[CurrentPartyId]; 
	}


	public bool IsCostOverflow(int pos, uint newUniqueId) {
		if( newUniqueId != 0 ) { // check cost max
			int newCost = DataCenter.Instance.UserUnitList.GetMyUnit( newUniqueId ).UnitInfo.Cost;
			int oldCost = 0;
			if( CurrentParty.UserUnit[pos] != null ) {
				oldCost = CurrentParty.UserUnit[pos].UnitInfo.Cost;
			}
			
			if ( (CurrentParty.TotalCost - oldCost + newCost) > DataCenter.Instance.UserInfo.CostMax ) {
				ViewManager.Instance.ShowTipsLabel(TextCenter.GetText("CostLimitText"));
				Debug.LogError("TPartyInfo.ChangeParty:: costTotal="+(CurrentParty.TotalCost - oldCost + newCost)+" > "+DataCenter.Instance.UserInfo.CostMax);
				return true;
			}
		}

		return false;
	}

	public	bool ChangeParty(int pos, uint newUniqueId) { 
        if (CurrentPartyId >= instance.partyList.Count) {
			Debug.LogError("TPartyInfo.ChangeParty:: CurrentPartyId:"+CurrentPartyId+" is invalid.");
            return false;
        }

        if (pos >= instance.partyList[CurrentPartyId].items.Count) {
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
        instance.partyList[CurrentPartyId].items[pos] = item;

		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);
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
            ChangeParty cp = new ChangeParty();
            cp.OnRequest(this, onRspChangeParty);
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

			}else { //change party is success
				originalPartyId = instance.currentParty;
				isPartyItemModified = false;
				isPartyGroupModified = false;
			}
//			bool success = (rspChangeParty.header.code == 0 );
        }

    }
}




