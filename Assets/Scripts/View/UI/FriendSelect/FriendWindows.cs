using UnityEngine;
using System.Collections.Generic;

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

		Dictionary<string, object> dic = new Dictionary<string, object> ();
		DragPanelSetInfo dpsi = new DragPanelSetInfo ();
//		dpsi.position = new Vector3 (0f, -485f, 0f);
//		dragPanel.DragPanelView.SetDragPanel (dpsi);
		transform.localPosition += new Vector3 (0f, -485f, 0f);
		NoviceGuideStepEntityManager.Instance ().StartStep ();
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
