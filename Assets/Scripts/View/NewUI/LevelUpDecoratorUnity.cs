using UnityEngine;
using System.Collections.Generic;

public class LevelUpDecoratorUnity : UIComponentUnity, IUICallback{

	private DragPanel baseScroller;
	private GameObject baseItem;
	private GameObject btnLevelUp;
	private DragPanel materialScroller;
	private GameObject materialItem;

	private DragPanel friendScroller;
	private GameObject friendItem;

	private GameObject baseTab;
	private GameObject friendTab;
	private GameObject materialTab;

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

	private List< GameObject > materialTabList = new List< GameObject >();
	private Dictionary< string, object > baseScrollerArgs = new Dictionary< string, object >();
	private Dictionary< string, object > materialScrollerArgs = new Dictionary< string, object >();
	private Dictionary< string, object > friendcrollerArgs = new Dictionary< string, object >();

	private Dictionary< GameObject, GameObject > focusDic = new Dictionary<GameObject, GameObject>();
	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);
		//AddUnitSprite();
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTween();
		isEmptyBase = true;
		isEmptyFriend = true;
		FocusOnPanel( baseTab );
	}
	
	public override void HideUI () {
		base.HideUI ();

		CleanTabs();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void InitUI() {
		InitTabs();
		InitPanels();
		focusDic.Add( baseTab, basePanel );
		focusDic.Add( materialTab, materialPanel );
		focusDic.Add ( friendTab, friendPanel );
	}

	private void FocusOnPanel(GameObject focus)
	{
		foreach( var tabKey in focusDic.Keys ) {
			if( tabKey == focus ) {
				tabKey.transform.FindChild("Hight_Light").gameObject.SetActive( true );
				tabKey.transform.FindChild( "Label_Title" ).GetComponent< UILabel >().color = Color.yellow;
				focusDic[ tabKey ].SetActive( true );
			}
			else {
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
		
		btnLevelUp = FindChild("Focus_Panels/Friend_Panel/Button_LevelUp");
		UIEventListener.Get( btnLevelUp ).onClick = LevelUp;

		UIEventListener.Get( baseSortBar ).onClick = SortBase;
		UIEventListener.Get( materialSortBar ).onClick = SortMaterial;
		UIEventListener.Get( friendSortBar ).onClick = SortFriend;
		CreateScrollerFriend();
		CreateScrollerBase();
		CreateScrollerMaterial();
	}
	
	private void CreateScrollerBase()
	{
		InitBaseScrollArgs();

		string itemResourcePath = "Prefabs/UI/Units/LevelUpScrollerItem";
		baseItem = Resources.Load(itemResourcePath) as GameObject;
		baseScroller = new DragPanel( "BaseScroller", baseItem );
		baseScroller.CreatUI();
		//Debug.LogError(TempConfig.unitAvatarSprite.Count);
		foreach (string avatar in TempConfig.unitAvatarSprite.Keys)
		{
			baseScroller.AddItem( 1,baseItem);
			UITexture tempUITex = baseItem.GetComponent< UITexture >();
			//Debug.Log( tempUITex.mainTexture.name);
			tempUITex.mainTexture = Resources.Load("Avatar/" + avatar) as Texture;
		}
		baseScroller.RootObject.SetScrollView( baseScrollerArgs );

		for(int i = 0; i < baseScroller.ScrollItem.Count; i++)
			UIEventListener.Get(baseScroller.ScrollItem[ i ].gameObject).onClick += PickBase;
	}

	private void CreateScrollerMaterial() {
		InitMaterialScrollArgs();

		string ItemPath = "Prefabs/UI/Units/LevelUpScrollerItem";
		materialItem = Resources.Load( ItemPath ) as GameObject;
		materialScroller = new DragPanel( "MaterialScroller", materialItem );
		materialScroller.CreatUI();
		foreach (string avatar in TempConfig.unitAvatarSprite.Keys) {
			materialScroller.AddItem( 1,materialItem);
			UITexture tempUITex = materialItem.GetComponent< UITexture >();
			tempUITex.mainTexture = Resources.Load("Avatar/" + avatar) as Texture;
		}
		materialScroller.RootObject.SetScrollView( materialScrollerArgs );
		for(int i = 0; i < baseScroller.ScrollItem.Count; i++)
			UIEventListener.Get(materialScroller.ScrollItem[ i ].gameObject).onClick = PickMaterial;
	}

	private void CreateScrollerFriend()
	{
		InitFriendScrollArgs();

		string ItemPath = "Prefabs/UI/Units/LevelUpScrollerItem";
		friendItem = Resources.Load( ItemPath ) as GameObject;
		friendScroller = new DragPanel( "FriendScroller", friendItem );
		friendScroller.CreatUI();
		foreach (string avatar in TempConfig.unitAvatarSprite.Keys ) {
			friendScroller.AddItem( 1,friendItem);
			UITexture tempUITex = friendItem.GetComponent< UITexture >();
			tempUITex.mainTexture = Resources.Load("Avatar/" + avatar) as Texture;
		}

		friendScroller.RootObject.SetScrollView( friendcrollerArgs );
		for(int i = 0; i < friendScroller.ScrollItem.Count; i++)
			UIEventListener.Get(friendScroller.ScrollItem[ i ].gameObject).onClick = PickFriend;
	}

	private void PickBase( GameObject go ) {
		if( isEmptyBase ) {
			baseCard = Instantiate(go) as GameObject;

			IUICallback call = origin as IUICallback;
			if(call != null ){
				call.Callback( baseCard );
			}
			isEmptyBase = false;
		}
		
	}
	
	private void PickFriend(GameObject go) {	
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
		if( materialCardList.Count < 4 ) {
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

	private void ShowTween() {
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if( list == null )
			return;
		foreach( var tweenPos in list) {		
			if( tweenPos == null )
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	public void Callback (object data)
	{
		GameObject go = data as GameObject;
		if(go != null)
		{
			go.transform.parent = baseTab.transform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
		}
	}

	private void LevelUp( GameObject go) {

		//Here Level Up Logic
		Debug.LogError("Level Up Logic Here!!!!");
		UIManager.Instance.ChangeScene( SceneEnum.UnitDetail );
	}


	private void InitBaseScrollArgs() {
		baseScrollerArgs.Add( "parentTrans", 			basePanel.transform					);
		baseScrollerArgs.Add( "scrollerScale", 		Vector3.one									);
		baseScrollerArgs.Add( "scrollerLocalPos" ,	-45*Vector3.up								);
		baseScrollerArgs.Add( "position", 				Vector3.zero 									);
		baseScrollerArgs.Add( "clipRange", 			new Vector4(-20, -120, 640, 400)		);
		baseScrollerArgs.Add( "gridArrange", 		UIGrid.Arrangement.Vertical 			);
		baseScrollerArgs.Add( "maxPerLine", 			3 												);
		baseScrollerArgs.Add( "scrollBarPosition", 	new Vector3(-320,-340,0)				);
		baseScrollerArgs.Add( "cellWidth", 			110 												);
		baseScrollerArgs.Add( "cellHeight",			110 												);
	}

	private void InitMaterialScrollArgs() {
		materialScrollerArgs.Add( "parentTrans", 		materialPanel.transform				);
		materialScrollerArgs.Add( "scrollerScale", 		Vector3.one								);
		materialScrollerArgs.Add( "scrollerLocalPos" ,	-45*Vector3.up							);
		materialScrollerArgs.Add( "position", 				Vector3.zero 							);
		materialScrollerArgs.Add( "clipRange", 			new Vector4(-20, -120, 640, 400 ));
		materialScrollerArgs.Add( "gridArrange", 		UIGrid.Arrangement.Vertical 		);
		materialScrollerArgs.Add( "maxPerLine", 			3 											);
		materialScrollerArgs.Add( "scrollBarPosition", 	new Vector3(-320,-340,0)		);
		materialScrollerArgs.Add( "cellWidth", 			110 											);
		materialScrollerArgs.Add( "cellHeight",			110 											);
	}

	private void InitFriendScrollArgs() {
		//Debug.LogError( friendPanel.transform.name);
		friendcrollerArgs.Add( "parentTrans", 		friendPanel.transform						);
		friendcrollerArgs.Add( "scrollerScale", 		Vector3.one									);
		friendcrollerArgs.Add( "scrollerLocalPos" ,	 -240*Vector3.up							);
		friendcrollerArgs.Add( "position", 				Vector3.zero 									);
		friendcrollerArgs.Add( "clipRange", 			new Vector4(0, 0, 640, 200 )				);
		friendcrollerArgs.Add( "gridArrange", 		UIGrid.Arrangement.Horizontal 		);
		friendcrollerArgs.Add( "maxPerLine", 		0 													);
		friendcrollerArgs.Add( "scrollBarPosition", 	new Vector3(-320,-92,0)				);
		friendcrollerArgs.Add( "cellWidth", 			110 												);
		friendcrollerArgs.Add( "cellHeight",			110 												);
	}

}
