using UnityEngine;
using System.Collections.Generic;

public class RefreshPartyInfo : MonoBehaviour {
	private Dictionary<int, PageUnitItem> partyView;
	public int maxIndex = 4;

	private PageUnitItem friendItem;

	public void RefreshView (TUnitParty unitParty) {
		if (partyView == null) {
			InitComponent();
		}

		List<TUserUnit> partyMemberList = unitParty.GetUserUnit();

		for (int i = 0; i < partyMemberList.Count; i++){
			partyView[i].UserUnit = partyMemberList[i];
		}
	}

	public void InitComponent(bool initFriend = false) {
		partyView = new Dictionary<int, PageUnitItem> ();
		for (int i = 0; i < maxIndex; i++) {
//			Debug.LogError("i : " + i);
			PageUnitItem pui = PageUnitItem.Inject( transform.Find(i.ToString()).gameObject);
			partyView[i] = pui;
		}

		if (initFriend) {
			friendItem = PageUnitItem.Inject(transform.Find("Helper").gameObject);
		}
	}

	public void SetFriend(TUserUnit tuu) {
		friendItem.UserUnit = tuu;
	} 
}
