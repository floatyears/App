using UnityEngine;
using System.Collections.Generic;

public class LevelUpDecoratorUnity : UIComponentUnity {

	private DragPanel materialScroller;
	private GameObject materialScrollerItem;

	private DragPanel friendScroller;
	private GameObject friendScrollerItem;

	private GameObject tabBase;
	private GameObject tabFriend;
	private GameObject tabMaterial;
	private GameObject hightLightBase;
	private GameObject hightLightFriend;
	private GameObject hightLightMaterial;

	private GameObject materialWindow;
	private GameObject friendWindow;
	private GameObject materialSortBar;
	private GameObject friendSortBar;
	private GameObject selectMaterialBtn1;
	private GameObject selectMaterialBtn2;

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
		tabBase = FindChild("Top_Window/Tabs_Window/Tab_Base");
		tabFriend = FindChild("Top_Window/Tabs_Window/Tab_Friend");
		tabMaterial = FindChild("Top_Window/Tabs_Window/Tab_Materials");

		hightLightBase = FindChild("Top_Window/Tabs_Window/Tab_Base/Hight_Light");
		hightLightFriend = FindChild("Top_Window/Tabs_Window/Tab_Friend/Hight_Light");
		hightLightMaterial = FindChild("Top_Window/Tabs_Window/Tab_Materials/Hight_Light");

		materialWindow = FindChild("Bottom_Window/Material_Window");
		friendWindow = FindChild("Bottom_Window/Friend_Window");

		selectMaterialBtn1 = FindChild("Bottom_Window/Material_Window/BtnSelect1");
		selectMaterialBtn2 = FindChild("Bottom_Window/Material_Window/BtnSelect2");

		materialSortBar = FindChild("Bottom_Window/Material_Window/SortButton");
		friendSortBar = FindChild("Bottom_Window/Friend_Window/SortButton");
		//-------------
		materialScrollerItem = Resources.Load("Prefabs/UI/Units/LevelUpScrollerItem") as GameObject;
		materialScroller = new DragPanel( "LevelUpScroller", materialScrollerItem );

		materialScroller.CreatUI();
		materialScroller.AddItem(45);
		materialScroller.RootObject.SetGridArgs( 120, 120, UIGrid.Arrangement.Vertical, 3);
		materialScroller.RootObject.SetScrollBar( -364,  -340,  0);
		materialScroller.RootObject.SetViewPosition( new Vector4(0,-120f,700,400) );

		materialScroller.RootObject.gameObject.transform.parent = gameObject.transform.FindChild("Bottom_Window/Material_Window");
		materialScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		materialScroller.RootObject.gameObject.transform.localPosition = -45*Vector3.up;
		//-------------

		friendScrollerItem = Resources.Load("Prefabs/UI/Units/LevelUpScrollerItem") as GameObject;
		friendScroller = new DragPanel( "SelectFriendScroller", materialScrollerItem );
		
		friendScroller.CreatUI();
		friendScroller.AddItem(15);
		friendScroller.RootObject.SetGridArgs( 120, 120, UIGrid.Arrangement.Horizontal, 0);
		friendScroller.RootObject.SetScrollBar( -364,  -120,  0);
		friendScroller.RootObject.SetViewPosition( new Vector4(0,0,700,200) );
		
		friendScroller.RootObject.gameObject.transform.parent = gameObject.transform.FindChild("Bottom_Window/Friend_Window");
		friendScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		friendScroller.RootObject.gameObject.transform.localPosition = -270*Vector3.up;

		//-------------
		UIEventListener.Get( materialSortBar ).onClick = SortMaterial;
		UIEventListener.Get( friendSortBar ).onClick = SortFriend;

		UIEventListener.Get(tabBase).onClick = ShowBaseWindow;
		UIEventListener.Get(tabFriend).onClick = ShowFriendWindow;
		UIEventListener.Get(tabMaterial).onClick = ShowMaterialWindow;

		materialWindow.SetActive( true );
		friendWindow.SetActive( false );

		hightLightBase.SetActive( true );
		hightLightFriend.SetActive( false );
		hightLightMaterial.SetActive( false );

		selectMaterialBtn1.SetActive(false);
		selectMaterialBtn2.SetActive(false);
	}

	private void ShowBaseWindow( GameObject go) 
	{
		materialWindow.SetActive( true );
		friendWindow.SetActive( false );
		hightLightBase.SetActive( true );
		hightLightFriend.SetActive( false );
		hightLightMaterial.SetActive( false );
		selectMaterialBtn1.SetActive(false);
		selectMaterialBtn2.SetActive(false);
	}
	private void ShowFriendWindow( GameObject go) 
	{
		materialWindow.SetActive( false );
		friendWindow.SetActive( true );

		hightLightBase.SetActive( false );
		hightLightFriend.SetActive( true );
		hightLightMaterial.SetActive( false );
		selectMaterialBtn1.SetActive(false);
		selectMaterialBtn2.SetActive(false);
	}

	private void ShowMaterialWindow( GameObject go) 
	{
		materialWindow.SetActive( true );
		friendWindow.SetActive( false );

		hightLightBase.SetActive( false );
		hightLightFriend.SetActive( false );
		hightLightMaterial.SetActive( true );

		selectMaterialBtn1.SetActive(true);
		selectMaterialBtn2.SetActive(true);
	}

	private void SortMaterial(GameObject go)
	{
		LogHelper.Log("Sort Material");
	}

	private void SortFriend(GameObject go)
	{
		LogHelper.Log("Sort Friend");
	}


}
