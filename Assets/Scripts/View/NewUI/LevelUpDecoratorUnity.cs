using UnityEngine;
using System.Collections.Generic;

public class LevelUpDecoratorUnity : UIComponentUnity {

	private DragPanel materialScroller;
	private GameObject materialScrollerItem;

	private DragPanel friendScroller;
	private GameObject friendScrollerItem;

	private GameObject baseTab;
	private GameObject friendTab;
	private GameObject materialTab;

	private List< GameObject > materialTabList = new List< GameObject >();

	private GameObject basePanel;
	private GameObject materialPanel;
	private GameObject friendPanel;
	
	private GameObject baseSortBar;
	private GameObject materialSortBar;
	private GameObject friendSortBar;

	private GameObject selectMaterialBtn1;
	private GameObject selectMaterialBtn2;

	private bool isEmptyBase;
	private bool isEmptyFriend;

	private GameObject baseCard = null;
	private GameObject friendCard = null;
	private List< GameObject > materialCardList = new List<GameObject>();


	private Dictionary< GameObject, GameObject > focusDic = new Dictionary<GameObject, GameObject>();

	private string currentFocus;

	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);

		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTweenPostion( 0.2f );
		isEmptyBase = true;
		isEmptyFriend = true;
		FocusOnPanel( baseTab );
	}
	
	public override void HideUI () {
		base.HideUI ();
		ShowTweenPostion();
		CleanTabs();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void InitUI()
	{
		InitTabs();
		InitPanels();

		focusDic.Add( baseTab, basePanel );
		focusDic.Add( materialTab, materialPanel );
		focusDic.Add ( friendTab, friendPanel );

	}

	private void FocusOnPanel(GameObject focus)
	{
		foreach( var tabKey in focusDic.Keys )
		{
			if( tabKey == focus )
			{
				tabKey.transform.FindChild("Hight_Light").gameObject.SetActive( true );
				tabKey.transform.FindChild( "Label_Title" ).GetComponent< UILabel >().color = Color.yellow;
				focusDic[ tabKey ].SetActive( true );
			}
			else
			{
				tabKey.transform.FindChild("Hight_Light").gameObject.SetActive( false );
				tabKey.transform.FindChild( "Label_Title" ).GetComponent< UILabel >().color = Color.white;
				focusDic[ tabKey ].SetActive( false );
			}
		}
	}


	private void InitTabs()
	{
		baseTab = FindChild("Focus_Tabs/Base_Tab");
		friendTab = FindChild("Focus_Tabs/Friend_Tab");
		materialTab = FindChild("Focus_Tabs/Material_Tab");

		materialTabList.Add( materialTab.transform.FindChild("Material4").gameObject);
		materialTabList.Add( materialTab.transform.FindChild("Material3").gameObject);
		materialTabList.Add( materialTab.transform.FindChild("Material2").gameObject);
		materialTabList.Add( materialTab.transform.FindChild("Material1").gameObject);

		UIEventListener.Get( baseTab ).onClick = FocusOnPanel;
		UIEventListener.Get( friendTab ).onClick = FocusOnPanel;
		UIEventListener.Get( materialTab ).onClick = FocusOnPanel;
	}

	private void InitPanels()
	{
		basePanel = FindChild("Focus_Panels/Base_Panel");
		materialPanel = FindChild("Focus_Panels/Material_Panel");
		friendPanel = FindChild("Focus_Panels/Friend_Panel");

		baseSortBar = FindChild("Focus_Panels/Base_Panel/SortButton");
		materialSortBar = FindChild("Focus_Panels/Material_Panel/SortButton");
		friendSortBar = FindChild("Focus_Panels/Friend_Panel/SortButton");

		UIEventListener.Get( baseSortBar ).onClick = SortBase;
		UIEventListener.Get( materialSortBar ).onClick = SortMaterial;
		UIEventListener.Get( friendSortBar ).onClick = SortFriend;

		CreateScrollerBase();
		CreateScrollerFriend();
		CreateScrollerMaterial();
	}
	
	private void CreateScrollerBase()
	{
		string ItemPath = "Prefabs/UI/Units/LevelUpScrollerItem";
		materialScrollerItem = Resources.Load( ItemPath ) as GameObject;
		materialScroller = new DragPanel( "LevelUpScroller", materialScrollerItem );
		
		materialScroller.CreatUI();
		materialScroller.AddItem(45);
		materialScroller.RootObject.SetGridArgs( 120, 120, UIGrid.Arrangement.Vertical, 3);
		materialScroller.RootObject.SetScrollBar( -364,  -340,  0);
		materialScroller.RootObject.SetViewPosition( new Vector4(0,-120f,700,400) ); 
		
		materialScroller.RootObject.gameObject.transform.parent = basePanel.transform;
		materialScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		materialScroller.RootObject.gameObject.transform.localPosition = -45*Vector3.up;
		
		for(int i = 0; i < materialScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(materialScroller.ScrollItem[ i ].gameObject).onClick += PickBase;
		}
	}

	private void CreateScrollerMaterial()
	{
		string ItemPath = "Prefabs/UI/Units/LevelUpScrollerItem";
		materialScrollerItem = Resources.Load( ItemPath ) as GameObject;
		materialScroller = new DragPanel( "LevelUpScroller", materialScrollerItem );
		
		materialScroller.CreatUI();
		materialScroller.AddItem(45);
		materialScroller.RootObject.SetGridArgs( 120, 120, UIGrid.Arrangement.Vertical, 3);
		materialScroller.RootObject.SetScrollBar( -364,  -340,  0);
		materialScroller.RootObject.SetViewPosition( new Vector4(0,-120f,700,400) ); 
		
		materialScroller.RootObject.gameObject.transform.parent = materialPanel.transform;
		materialScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		materialScroller.RootObject.gameObject.transform.localPosition = -45*Vector3.up;

		for(int i = 0; i < materialScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(materialScroller.ScrollItem[ i ].gameObject).onClick += PickMaterial;
		}
	}

	private void CreateScrollerFriend()
	{
		string ItemPath = "Prefabs/UI/Units/LevelUpScrollerItem";
		friendScrollerItem = Resources.Load( ItemPath ) as GameObject;
		friendScroller = new DragPanel( "SelectFriendScroller", materialScrollerItem );
		friendScroller.CreatUI();
		friendScroller.AddItem(15);
		friendScroller.RootObject.SetGridArgs( 120, 120, UIGrid.Arrangement.Horizontal, 0);
		friendScroller.RootObject.SetScrollBar( -364,  -120,  0);
		friendScroller.RootObject.SetViewPosition( new Vector4(0,0,700,200) );
		
		friendScroller.RootObject.gameObject.transform.parent = friendPanel.transform;
		friendScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		friendScroller.RootObject.gameObject.transform.localPosition = -270*Vector3.up;

		for(int i = 0; i < friendScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(friendScroller.ScrollItem[ i ].gameObject).onClick += PickFriend;
		}
	}

	private void PickBase( GameObject go )
	{
		LogHelper.Log("Pick The Material Which Level up needed! ");
		if( isEmptyBase )
		{
			baseCard = Instantiate(go) as GameObject;
			baseCard.transform.parent = baseTab.transform;
			baseCard.transform.localPosition = Vector3.zero;
			baseCard.transform.localScale = Vector3.one;
			
			isEmptyBase = false;
		}
		
	}
	
	private void PickFriend(GameObject go)
	{
		LogHelper.Log("Pick The Friend Which Levelp up needed!");
		
		if( isEmptyFriend )
		{
			friendCard = Instantiate(go) as GameObject;
			friendCard.transform.parent = friendTab.transform;
			friendCard.transform.localPosition = Vector3.zero;
			friendCard.transform.localScale = Vector3.one;
			
			isEmptyFriend = false;
		}
	}

	private void PickMaterial(GameObject go)
	{
		LogHelper.Log("Pick The Friend Which Levelp up needed!");
		
		if( materialCardList.Count < 4 )
		{
			GameObject temp = Instantiate(go) as GameObject;
			temp.transform.parent = materialTabList[ materialCardList.Count ].transform;
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localScale = 0.8f*Vector3.one;
			materialCardList.Add( temp );

		}
	}

	private void SortBase(GameObject go)
	{
		LogHelper.Log("Sort Base");
	}

	private void SortMaterial(GameObject go)
	{
		LogHelper.Log("Sort Material");
	}
	
	private void SortFriend(GameObject go)
	{
		LogHelper.Log("Sort Friend");
	}

	private void CleanTabs()
	{
		GameObject.Destroy( baseCard );
		GameObject.Destroy( friendCard );

		foreach( GameObject go in materialCardList )
		{
			GameObject.Destroy( go );
		}

		materialCardList.Clear();
	}

	private void ShowTweenPostion( float mDelay = 0f, UITweener.Method mMethod = UITweener.Method.Linear ) 
	{
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		
		if( list == null )
			return;
		
		foreach( var tweenPos in list)
		{		
			if( tweenPos == null )
				continue;
			
			Vector3 temp;
			temp = tweenPos.to;
			tweenPos.to = tweenPos.from;
			tweenPos.from = temp;
			
			tweenPos.delay = mDelay;
			tweenPos.method = mMethod;
			
			tweenPos.Reset();
			tweenPos.PlayForward();
			
		}
	}
}
