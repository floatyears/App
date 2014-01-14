using UnityEngine;
using System.Collections;

public class FriendSelectComponent : ConcreteComponent {

	private DragPanel friendsScroller;
	private GameObject friendItem;

	public FriendSelectComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();

		friendItem = Resources.Load("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
		friendsScroller = new DragPanel ("FriendSelectScroller", friendItem);
		friendsScroller.CreatUI();
		friendsScroller.AddItem (13);
		friendsScroller.RootObject.SetItemWidth(140);
		friendsScroller.RootObject.gameObject.transform.localPosition = -680*Vector3.up;

		for(int i = 0; i < friendsScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(friendsScroller.ScrollItem[ i ].gameObject).onClick = PickFriend;
		}
	}


	void PickFriend(GameObject btn)
	{
		if(viewComponent is IUICallback) {
			IUICallback call = viewComponent as IUICallback;
			call.Callback(true);
		}
	}


	public override void ShowUI () {
		base.ShowUI ();
		friendsScroller.RootObject.gameObject.SetActive(true);
	}
	
	public override void HideUI () {
		base.HideUI ();
		friendsScroller.RootObject.gameObject.SetActive(false);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

}
