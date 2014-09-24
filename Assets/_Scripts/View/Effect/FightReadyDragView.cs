using UnityEngine;
using System.Collections;
using bbproto;

public class FightReadyDragView : DragSliderBase {

	protected override void InitTrans () {
		moveParent = transform.Find("MoveParent");
		cacheRightParent = transform.Find("RightParent");
		cacheLeftParent = transform.Find ("LeftParent");
	}

	public override void RefreshData () {
		UnitParty current = DataCenter.Instance.UnitData.PartyInfo.CurrentParty;
		UnitParty prev = DataCenter.Instance.UnitData.PartyInfo.GetPrePartyData;
		UnitParty next = DataCenter.Instance.UnitData.PartyInfo.GetNextPartyData;
		
		FightReadyPage rpi = moveParent.GetComponent<FightReadyPage> ();
		rpi.RefreshParty (current);
		
		cacheLeftParent.GetComponent<FightReadyPage> ().RefreshParty (prev);
		cacheRightParent.GetComponent<FightReadyPage> ().RefreshParty (next);
		
		dragChangeViewData.RefreshView (rpi.partyViewList);
	}

	public override void RefreshData (UnitParty tup) {
		FightReadyPage rpi = moveParent.GetComponent<FightReadyPage> ();
		rpi.RefreshParty (tup);
	}
}
