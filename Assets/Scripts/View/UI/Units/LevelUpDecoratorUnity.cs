using UnityEngine;
using System.Collections.Generic;

public class LevelUpDecoratorUnity : UIComponentUnity, IUICallback {
	private DragPanel baseScroller;
	private GameObject baseItem;
	private GameObject btnLevelUp;
	private DragPanel materialScroller;
	private GameObject materialItem;
                 
	private DragPanel friendScroller;
	private GameObject friendItem;

	private GameObject baseTab;
	private UITexture baseTexture;
	private GameObject friendTab;
	private UITexture friendTexture;
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
	private List<UITexture> materialTexture = new List<UITexture>();
	private Dictionary< string, object > baseScrollerArgs = new Dictionary< string, object >();
	private Dictionary< string, object > materialScrollerArgs = new Dictionary< string, object >();
	private Dictionary< string, object > friendcrollerArgs = new Dictionary< string, object >();
	private Dictionary< GameObject, GameObject > focusDic = new Dictionary<GameObject, GameObject>();

	private List<UserUnitInfo> userUnitInfoList = new List<UserUnitInfo>();
	private Dictionary<GameObject, UserUnitInfo> baseItemInfo = new Dictionary<GameObject, UserUnitInfo>();
	private Dictionary<GameObject, UnitBaseInfo> materialItemInfo = new Dictionary<GameObject, UnitBaseInfo>();
	public List<UnitBaseInfo> selectMaterial = new List<UnitBaseInfo>();

	public override void Init(UIInsConfig config, IUIOrigin origin) {
		base.Init(config, origin);
		InitUI();
	}

	void function(object data) {

	}

	public override void ShowUI() {
		base.ShowUI();
		ShowTween();
		isEmptyBase = true;
		isEmptyFriend = true;
		FocusOnPanel(baseTab);
	}
	
	public override void HideUI(){
		base.HideUI();
		CleanTabs();
	}
	
	public override void DestoryUI() {
		base.DestoryUI();
	}

	void InitUI() {
		GetUnitInfoList();
		InitTabs();
		InitPanels();
		focusDic.Add(baseTab, basePanel);
		focusDic.Add(materialTab, materialPanel);
		focusDic.Add(friendTab, friendPanel);
	}

	void GetUnitInfoList() {
		UnitPartyInfo upi = ModelManager.Instance.GetData(ModelEnum.UnitPartyInfo, new ErrorMsg()) as UnitPartyInfo;
		userUnitInfoList = upi.GetUserUnit();
	}

	private void FocusOnPanel(GameObject focus) {
		foreach (var tabKey in focusDic.Keys) {
			if (tabKey == focus) {
				tabKey.transform.FindChild("high_light").gameObject.SetActive(true);
				tabKey.transform.FindChild("label_title").GetComponent< UILabel >().color = Color.yellow;
				focusDic [tabKey].SetActive(true);
			} else{
				tabKey.transform.FindChild("high_light").gameObject.SetActive(false);
				tabKey.transform.FindChild("label_title").GetComponent< UILabel >().color = Color.white;
				focusDic [tabKey].SetActive(false);
			}
		}
	}


	private void InitTabs() {
		baseTab = FindChild("Focus_Tabs/Base_Tab");
		baseTexture = baseTab.GetComponentInChildren<UITexture>();
		friendTab = FindChild("Focus_Tabs/Friend_Tab");
		friendTexture = friendTab.GetComponentInChildren<UITexture>();
		materialTab = FindChild("Focus_Tabs/Material_Tab");

		for (int i = 1; i < 5; i++) {
			GameObject go = materialTab.transform.FindChild("Material" + i).gameObject;
			materialTabList.Add(go);
			materialTexture.Add(go.GetComponentInChildren<UITexture>());
		}
		UIEventListener.Get(baseTab).onClick = FocusOnPanel;
		UIEventListener.Get(friendTab).onClick = FocusOnPanel;
		UIEventListener.Get(materialTab).onClick = FocusOnPanel;
	}

	private void InitPanels(){
		basePanel = FindChild("Focus_Panels/Base_Panel");
		materialPanel = FindChild("Focus_Panels/Material_Panel");
		friendPanel = FindChild("Focus_Panels/Friend_Panel");

		baseSortBar = FindChild("Focus_Panels/Base_Panel/SortButton");
		materialSortBar = FindChild("Focus_Panels/Material_Panel/SortButton");
		friendSortBar = FindChild("Focus_Panels/Friend_Panel/SortButton");
		
		//btnLevelUp = FindChild("Focus_Panels/Friend_Panel/Button_LevelUp");
		//UIEventListener.Get(btnLevelUp).onClick = LevelUp;

		UIEventListener.Get(baseSortBar).onClick = SortBase;
		UIEventListener.Get(materialSortBar).onClick = SortMaterial;
		UIEventListener.Get(friendSortBar).onClick = SortFriend;
		CreateScrollerFriend();
		CreateScrollerBase();
		CreateScrollerMaterial();
	}
	
	private void CreateScrollerBase(){
		InitBaseScrollArgs();
		baseItem = Resources.Load("Prefabs/UI/Friend/UnitItem") as GameObject;
		//baseItem = GlobalData.ItemObject;
		baseScroller = new DragPanel("BaseScroller", baseItem);
		baseScroller.CreatUI();
		baseScroller.AddItem(userUnitInfoList.Count);
		for (int i = 0; i < baseScroller.ScrollItem.Count; i++){
			GameObject item = baseScroller.ScrollItem [i];
			UITexture tex = item.GetComponentInChildren<UITexture>();
			UnitBaseInfo ubi = GlobalData.tempUnitBaseInfo [userUnitInfoList [i].unitBaseInfo];
			tex.mainTexture = Resources.Load(ubi.GetHeadPath) as Texture2D;
			baseItemInfo.Add(item, userUnitInfoList [i]);
			UIEventListenerCustom ulc = UIEventListenerCustom.Get(item);
			ulc.onClick = PickBase;
			ulc.LongPress = LongPressPickBase;
		}

		baseScroller.RootObject.SetScrollView(baseScrollerArgs);
	}

	void LongPressPickBase(GameObject go)
	{
		UserUnitInfo uui = baseItemInfo [go];
		MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, uui);
	}

	private UserUnitInfo selectUnit;
        private void PickBase(GameObject go) {
		selectUnit = baseItemInfo [go];
		baseTexture.mainTexture = go.GetComponentInChildren<UITexture>().mainTexture;
	}

	private void CreateScrollerMaterial() {
		InitMaterialScrollArgs();
//		materialItem = GlobalData.ItemObject;
		materialItem = Resources.Load("Prefabs/UI/Friend/UnitItem") as GameObject;
		materialScroller = new DragPanel("MaterialScroller", materialItem);
		materialScroller.CreatUI();
		materialScroller.AddItem(GlobalData.HaveCard.Count);
		for (int i = 0; i < materialScroller.ScrollItem.Count; i++) {
			GameObject target = materialScroller.ScrollItem [i];
			UITexture tex = target.GetComponentInChildren<UITexture>();
			UnitBaseInfo ubi = GlobalData.tempUnitBaseInfo [GlobalData.HaveCard [i]];
			tex.mainTexture = Resources.Load(ubi.GetHeadPath) as Texture2D;
			UIEventListenerCustom ulc = UIEventListenerCustom.Get(target);
			ulc.onClick = PickMaterial;
			ulc.LongPress = LongPressPickMaterial;
			materialItemInfo.Add(target, ubi);
		}
		materialScroller.RootObject.SetScrollView(materialScrollerArgs);
	}

	void LongPressPickMaterial(GameObject go)
	{
		UnitBaseInfo ubi = materialItemInfo [go];
		MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, ubi);
	}

	private void PickMaterial(GameObject go) {
		if (selectMaterial.Count < 4){
			Texture tex = go.GetComponentInChildren<UITexture>().mainTexture;
			materialTabList [selectMaterial.Count].GetComponentInChildren<UITexture>().mainTexture = tex;
			UnitBaseInfo ubi = materialItemInfo [go];
			selectMaterial.Add(ubi);
		}
	}

	private void CreateScrollerFriend(){
		InitFriendScrollArgs();
		friendItem = GlobalData.ItemObject;
		friendScroller = new DragPanel("FriendScroller", friendItem);
		friendScroller.CreatUI();
		friendScroller.AddItem(1);
		GameObject target = friendScroller.ScrollItem [0];
		UIEventListenerCustom ulc = UIEventListenerCustom.Get(target);
		ulc.onClick = PickFriend;
		ulc.LongPress = LongPressPickFriend;
		friendScroller.RootObject.SetScrollView(friendcrollerArgs);
	}

	void LongPressPickFriend(GameObject go){
		MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, GlobalData.FriendBaseInfo);
	}
	
	private void PickFriend(GameObject go){	
		Texture tex = go.GetComponentInChildren<UITexture>().mainTexture;
		friendScroller.ScrollItem [0].GetComponentInChildren<UITexture>().mainTexture = tex;
	}
	
	private void SortBase(GameObject go){
		LogHelper.Log("Sort Base");
	}

	private void SortMaterial(GameObject go){
		LogHelper.Log("Sort Material");
	}
	
	private void SortFriend(GameObject go){
		LogHelper.Log("Sort Friend");
	}

	private void CleanTabs(){
		GameObject.Destroy(baseCard);
		GameObject.Destroy(friendCard);

		foreach (GameObject go in materialCardList){
			GameObject.Destroy(go);
		}

		materialCardList.Clear();
	}

	private void ShowTween(){
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	public void Callback(object data){}

	private void LevelUp(GameObject go){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
	}


	private void InitBaseScrollArgs()
	{
		baseScrollerArgs.Add("parentTrans", basePanel.transform);
		baseScrollerArgs.Add("scrollerScale", Vector3.one);
		baseScrollerArgs.Add("scrollerLocalPos", -45 * Vector3.up);
		baseScrollerArgs.Add("position", Vector3.zero);
		baseScrollerArgs.Add("clipRange", new Vector4(0, -120, 640, 400));
		baseScrollerArgs.Add("gridArrange", UIGrid.Arrangement.Vertical);
		baseScrollerArgs.Add("maxPerLine", 3);
		baseScrollerArgs.Add("scrollBarPosition", new Vector3(-320, -340, 0));
		baseScrollerArgs.Add("cellWidth", 115);
		baseScrollerArgs.Add("cellHeight", 115);
	}

	private void InitMaterialScrollArgs()
	{
		materialScrollerArgs.Add("parentTrans", materialPanel.transform);
		materialScrollerArgs.Add("scrollerScale", Vector3.one);
		materialScrollerArgs.Add("scrollerLocalPos", -45 * Vector3.up);
		materialScrollerArgs.Add("position", Vector3.zero);
		materialScrollerArgs.Add("clipRange", new Vector4(0, -120, 640, 400));
		materialScrollerArgs.Add("gridArrange", UIGrid.Arrangement.Vertical);
		materialScrollerArgs.Add("maxPerLine", 3);
		materialScrollerArgs.Add("scrollBarPosition", new Vector3(-320, -340, 0));
		materialScrollerArgs.Add("cellWidth", 110);
		materialScrollerArgs.Add("cellHeight", 110);
	}

	private void InitFriendScrollArgs()
	{
		friendcrollerArgs.Add("parentTrans", friendPanel.transform);
		friendcrollerArgs.Add("scrollerScale", Vector3.one);
		friendcrollerArgs.Add("scrollerLocalPos", -255 * Vector3.up);
		friendcrollerArgs.Add("position", Vector3.zero);
		friendcrollerArgs.Add("clipRange", new Vector4(0, 0, 640, 200));
		friendcrollerArgs.Add("gridArrange", UIGrid.Arrangement.Horizontal);
		friendcrollerArgs.Add("maxPerLine", 0);
		friendcrollerArgs.Add("scrollBarPosition", new Vector3(-320, -130, 0));
		friendcrollerArgs.Add("cellWidth", 115);
		friendcrollerArgs.Add("cellHeight", 130);
	}
}
