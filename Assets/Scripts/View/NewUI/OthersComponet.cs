using UnityEngine;
using System.Collections;

public class OthersComponent : ConcreteComponent {

	private GameObject scrollerItem;
	private DragPanel othersScroller;

	public OthersComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();

		scrollerItem = Resources.Load("Prefabs/UI/Others/OthersScrollerItem") as GameObject;
		othersScroller = new DragPanel ("OthersScroller", scrollerItem);
		othersScroller.CreatUI ();
		othersScroller.AddItem (15);
		othersScroller.RootObject.SetItemWidth(150);
		othersScroller.RootObject.gameObject.transform.localPosition = -765*Vector3.up;
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		SetUIActive(true);
	}
	
	public override void HideUI () {
		base.HideUI ();
		SetUIActive(false);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void SetUIActive(bool b)
	{
		othersScroller.RootObject.gameObject.SetActive(b);
	}

}
