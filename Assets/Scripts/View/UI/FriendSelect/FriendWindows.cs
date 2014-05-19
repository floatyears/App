using UnityEngine;
using System.Collections;

public class FriendWindows : FriendHelperView {
	public System.Action<TFriendInfo> selectFriend;
	public bool isShow = false;
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
	}

	public override void ShowUI () {
		isShow = true;
		if (!gameObject.activeSelf) {
			gameObject.SetActive(true);
		}
		base.ShowUI ();

		NoviceGuideStepEntityManager.Instance ().NextState ();
	}

	public override void HideUI () {
//		Debug.LogError ("FriendWindows HideUI");
		isShow = false;	
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	protected override void ClickHelperItem (HelperUnitItem item) {
		if (selectFriend != null) {
			selectFriend(item.FriendInfo);
		}
		HideUI ();
	}

	public GameObject GetHelperUnitItem(int i){
		return dragPanel.ScrollItem[i];
	}

}
