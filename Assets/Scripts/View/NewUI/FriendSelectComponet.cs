using UnityEngine;
using System.Collections;

public class FriendSelectComponent : ConcreteComponent,IUICallback {

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
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void Callback (object data)
	{
		try {
			SceneEnum se = (SceneEnum)data;

			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}
}
