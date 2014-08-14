using UnityEngine;
using System.Collections.Generic;

public class RefreshPartyInfo : MonoBehaviour {
	public List<PageUnitItem> partyView = null;
	public int maxIndex = 4;

	private PageUnitItem friendItem;

	public void RefreshView (TUnitParty unitParty) {
		if (partyView == null || partyView.Count == 0) {
			InitComponent();
		}
		List<TUserUnit> partyMemberList = unitParty.GetUserUnit();
		for (int i = 0; i < partyMemberList.Count; i++){
			partyView[i].UserUnit = partyMemberList[i];
		}
	}

	public void InitComponent(bool initFriend = false) {
		partyView = new List<PageUnitItem> ();

		for (int i = 0; i < maxIndex; i++) {
			PageUnitItem pui = PageUnitItem.Inject( transform.Find(i.ToString()).gameObject);
			partyView.Add(pui);
		}

		if (initFriend) {
			friendItem = PageUnitItem.Inject(transform.Find("Helper").gameObject);
		}
	}

	public void SetFriend(TUserUnit tuu) {
		friendItem.UserUnit = tuu;
	} 
}
