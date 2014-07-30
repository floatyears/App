using UnityEngine;
using bbproto;
using System.Collections.Generic;

public class FriendWindows : FriendHelperView {
	public System.Action<TFriendInfo> selectFriend;
	public bool isShow = false;

	public EvolveItem evolveItem;

	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		premiumBtn.gameObject.SetActive (true);
	}

	public override void ShowUI () {
		isShow = true;
		if (!gameObject.activeSelf) {
			gameObject.SetActive(true);
		}
		base.ShowUI ();
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);

		CheckFriend ();
	}

	public override void HideUI () {
		isShow = false;	
		if (gameObject.activeSelf) {
			gameObject.SetActive(false);
		}
	}

	public override void DestoryUI () {
		Destroy (gameObject);
	}
	
	protected override void ClickHelperItem (HelperUnitItem item) {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_success);
		Back (item.FriendInfo);
	}

	public void Back(TFriendInfo item) {
		if (selectFriend != null) {
			selectFriend(item);
		}
		HideUI ();
	}

	public GameObject GetHelperUnitItem(int i){
//		Debug.LogError ("generalDragPanel : " + generalDragPanel);
		return generalDragPanel.ScrollItem[i];
	}

	void CheckFriend() {
//		Debug.LogError ("CheckFriend : " + evolveItem);
		if (evolveItem == null) {
			return;	
		}

		DragPanel dragPanel = null;

		HelperRequire hr = evolveItem.userUnit.UnitInfo.evolveInfo.helperRequire;

		dragPanel = friendInfoTyp == FriendHelperView.FriendInfoType.General ? generalDragPanel : premiumDragPanel;

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++) {
			HelperUnitItem hui = dragPanel.ScrollItem[i].GetComponent<HelperUnitItem>();
			if(! CheckEvolve(hr, hui.UserUnit)) {
				hui.IsEnable = false;
			}
		}
	}

	bool CheckEvolve(HelperRequire hr, TUserUnit tuu) {
		bool level = tuu.Level >= hr.level;
		bool race = (hr.race == 0) || (tuu.UnitRace == (int)hr.race);
		bool type = (hr.type == 0) || (tuu.UnitType == (int)hr.type);
//		Debug.LogError ("level && race && type : " + (level && race && type));
		if (level && race && type) {
			return true;	
		} else {
			return false;	
		}
	}

}
