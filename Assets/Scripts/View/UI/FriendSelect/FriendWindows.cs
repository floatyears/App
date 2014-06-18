using UnityEngine;
using System.Collections.Generic;

public class FriendWindows : FriendHelperView {
	public System.Action<TFriendInfo> selectFriend;
	public bool isShow = false;
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
//		transform.localPosition -= transform.parent.localPosition;
	}

	public override void ShowUI () {
		isShow = true;
		if (!gameObject.activeSelf) {
			gameObject.SetActive(true);
		}
		base.ShowUI ();

//		transform.localPosition += new Vector3 (0f, -485f, 0f);
//		generalDragPanel.DragPanelView.scrollView.transform.localPosition += new Vector3 (0f, -50f, 0f);
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}

	public override void HideUI () {
		isShow = false;	
		if (gameObject.activeSelf) {
			gameObject.SetActive(false);
		}
//		base.HideUI ();
	}

	public override void DestoryUI () {
		Destroy (gameObject);
	}
	
	protected override void ClickHelperItem (HelperUnitItem item) {
		if (selectFriend != null) {
			selectFriend(item.FriendInfo);
		}
		HideUI ();
	}

	public GameObject GetHelperUnitItem(int i){
		return generalDragPanel.ScrollItem[i];
	}

}
