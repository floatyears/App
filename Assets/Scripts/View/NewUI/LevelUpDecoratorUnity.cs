using UnityEngine;
using System.Collections.Generic;

public class LevelUpDecoratorUnity : UIComponentUnity {

	private DragPanel scroller;
	private GameObject scrollerItem;

	private GameObject tab_base;
	private GameObject tab_friend;
	private GameObject tab_material;
	private GameObject hightLight_base;
	private GameObject hightLight_friend;
	private GameObject hightLight_material;

	private GameObject baseWindow;
	private GameObject friendWindow;

	private GameObject sortBar;

	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);

		InitUI();
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

	void InitUI()
	{
		tab_base = FindChild("Top/tab_base");
		tab_friend = FindChild("Top/tab_friend");
		tab_material = FindChild("Top/tab_materials");

		hightLight_base = FindChild("Top/tab_base/Hight_light");
		hightLight_friend = FindChild("Top/tab_friend/Hight_light");
		hightLight_material = FindChild("Top/tab_materials/Hight_light");

		baseWindow = FindChild("Bottom/BaseWindow");
		friendWindow = FindChild("Bottom/FriendWindow");

		sortBar = FindChild("Bottom/BaseWindow/SortBar");

		scrollerItem = Resources.Load("Prefabs/UI/Units/LevelUpScrollerItem") as GameObject;
		scroller = new DragPanel( "LevelUpScroller", scrollerItem );

		scroller.CreatUI();
		scroller.AddItem(45);
		scroller.RootObject.SetGridArgs( 120, 120, UIGrid.Arrangement.Vertical, 3);
		scroller.RootObject.SetScrollBar( -364,  -340,  0);
		scroller.RootObject.SetViewPosition( new Vector4(0,-120f,700,400) );

		scroller.RootObject.gameObject.transform.parent = gameObject.transform.FindChild("Bottom/BaseWindow");
		scroller.RootObject.gameObject.transform.localScale = Vector3.one;
		scroller.RootObject.gameObject.transform.localPosition = -45*Vector3.up;

		UIEventListener.Get(sortBar).onClick = SortCard;

		UIEventListener.Get(tab_base).onClick = ShowBaseWindow;
		UIEventListener.Get(tab_friend).onClick = ShowFriendWindow;
		UIEventListener.Get(tab_material).onClick = ShowMaterialWindow;

		baseWindow.SetActive( true );
		friendWindow.SetActive( false );

		hightLight_base.SetActive( true );
		hightLight_friend.SetActive( false );
		hightLight_material.SetActive( false );
	}

	private void ShowBaseWindow( GameObject go) 
	{
		baseWindow.SetActive( true );
		friendWindow.SetActive( false );
		hightLight_base.SetActive( true );
		hightLight_friend.SetActive( false );
		hightLight_material.SetActive( false );
	}
	private void ShowFriendWindow( GameObject go) 
	{
		baseWindow.SetActive( false );
		friendWindow.SetActive( true );

		hightLight_base.SetActive( false );
		hightLight_friend.SetActive( true );
		hightLight_material.SetActive( false );
	}

	private void ShowMaterialWindow( GameObject go) 
	{
		baseWindow.SetActive( true );
		friendWindow.SetActive( false );

		hightLight_base.SetActive( false );
		hightLight_friend.SetActive( false );
		hightLight_material.SetActive( true );
	}

	private void SortCard(GameObject go)
	{
		LogHelper.Log("Sort Card");
	}


}
