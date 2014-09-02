using UnityEngine;
using System.Collections;

public class PartyDragView : DragSliderBase {

	protected override void InitTrans () {
		moveParent = transform.Find ("DragParent");
		cacheLeftParent = transform.Find("LeftCache");
		cacheRightParent = transform.Find("RightCache");
	}

	public override void RefreshData () {
		TUnitParty current = DataCenter.Instance.PartyInfo.CurrentParty;
		TUnitParty prev = DataCenter.Instance.PartyInfo.GetPrePartyData;
		TUnitParty next = DataCenter.Instance.PartyInfo.GetNextPartyData;
		
		RefreshPartyInfo rpi = moveParent.GetComponent<RefreshPartyInfo> ();
		rpi.RefreshView (current);
		
		cacheLeftParent.GetComponent<RefreshPartyInfo> ().RefreshView (prev);
		cacheRightParent.GetComponent<RefreshPartyInfo> ().RefreshView (next);
		
		dragChangeViewData.RefreshView (rpi.partyView);
	}
}
