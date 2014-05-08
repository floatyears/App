using UnityEngine;
using System.Collections.Generic;

public class LevelUpOperateUnity : UIComponentUnity {
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitUI ();
		MsgCenter.Instance.AddListener (CommandEnum.LevelUpSucceed, ResetUIAfterLevelUp);
	}

	public override void ShowUI () {
		if (friendWindow != null && friendWindow.isShow) {
			friendWindow.gameObject.SetActive (true);
		} else {
			if (!gameObject.activeSelf) {
				gameObject.SetActive(true);
			}
		}
		base.ShowUI ();
		ClearFocus ();
		ShowData ();

		MsgCenter.Instance.AddListener (CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void HideUI () {
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail) {
			if (friendWindow != null && friendWindow.gameObject.activeSelf) {
				friendWindow.gameObject.SetActive (false);
			} 
		}else {
			if (friendWindow != null) {
				friendWindow.HideUI ();
			}	
		}

		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.LevelUpSucceed, ResetUIAfterLevelUp);
	}

	public override void ResetUIState () {
		ClearData ();

		CheckLevelUp ();

		sortRule = SortRule.Attack;
		ReceiveSortInfo (sortRule);
	}



	public override void CallbackView (object data) {
		base.CallbackView (data);
	}

	private SortRule _sortRule;
	public SortRule sortRule {
		get { return _sortRule; }
		set { 
			_sortRule = value;
			infoLabel[5].text = _sortRule.ToString();
		}
	}

	private DataCenter dataCenter;

	/// <summary>
	/// index:0==base, 1~3==material, 4==friend.
	/// </summary>
	private LevelUpItem[] selectedItem = new LevelUpItem[5];
	/// <summary>
	/// indx : 0==hplabel, 1==atkLabel, 2==exp need label. 3==exp got label. 4==coin need label. 5==sortlabel;
	/// </summary>
	private UILabel[] infoLabel = new UILabel[6];

	private UIImageButton levelUpButton;

	private UIButton sortButton;

	private DragPanel myUnitDragPanel;

	private List<PartyUnitItem> myUnitList = new List<PartyUnitItem> ();

	private List<TUserUnit> myUnit = new List<TUserUnit> ();

	private MyUnitItem prevSelectedItem;

	private MyUnitItem prevMaterialItem;

	private FriendWindows friendWindow;

	void ShowData () {

		if (myUnitDragPanel.DragPanelView == null) {
			InitDragPanel();	
		}
		myUnitList.Clear ();
		myUnit = dataCenter.UserUnitList.GetAllMyUnit ();
		int dataCount = myUnit.Count;
		List<GameObject> scroll = myUnitDragPanel.ScrollItem;
		int itemCount = scroll.Count - 1;	// scroll list index = 0  is reject item;
		if (dataCount > itemCount) {
			int addCount = dataCount - itemCount;
			myUnitDragPanel.AddItem (addCount, PartyUnitItem.ItemPrefab);
			for (int i = 0; i < dataCount; i++) {
				GameObject item = scroll [i + 1];
				PartyUnitItem pui = item.GetComponent<PartyUnitItem> ();
				if(pui == null) {
					pui = PartyUnitItem.Inject(item);
					pui.Init(myUnit [i]);
				}
				else{
					pui.UserUnit = myUnit[i];
				}
				pui.IsParty = dataCenter.PartyInfo.UnitIsInParty(myUnit[i].ID);
				pui.IsEnable = true;
				pui.callback = MyUnitClickCallback;
				myUnitList.Add(pui);
			}
		} else {
			for (int i = 0; i < dataCount; i++) {
				PartyUnitItem pui = scroll[i + 1].GetComponent<PartyUnitItem>();
				bool initEnable = pui.IsEnable;
				pui.UserUnit = myUnit[i];
				pui.IsParty = dataCenter.PartyInfo.UnitIsInParty(myUnit[i].ID);
				pui.IsEnable = initEnable;
				pui.callback = MyUnitClickCallback;
				myUnitList.Add(pui);
			}
			for (int i = scroll.Count - 1; i > dataCount ; i--) {
				myUnitDragPanel.RemoveItem(scroll[i]);
			}
		}


	}

	void InitUI() {
		dataCenter = DataCenter.Instance;
		for (int i = 1; i < 6; i++) {	//gameobject name is 1 ~ 6.
			LevelUpItem pui = FindChild<LevelUpItem>(i.ToString());
			selectedItem[i -1] = pui;
			pui.Init(null);
			pui.IsEnable = true;
			pui.IsFavorite = false;
			if(i == 1) {
				pui.callback = SelectedItemCallback;
				pui.PartyLabel.text = "Base";
				continue;
			}
			if(i == 5){
				pui.callback = SelectedFriendCallback;
				pui.PartyLabel.text = "Friend";
				continue;
			}

			pui.callback = SelectedItemCallback;
		}


		string path = "InfoPanel/Label_Value/";
		for (int i = 0; i < 5; i++) { //label name is 0 ~ 4
			infoLabel[i] = FindChild<UILabel>(path + i);
		}
		levelUpButton = FindChild<UIImageButton>("Button_LevelUp");
		UIEventListener.Get (levelUpButton.gameObject).onClick = LevelUpCallback;
		levelUpButton.isEnabled = false;
		path = "LevelUpBasePanel/SortButton";
		sortButton = FindChild<UIButton>(path);
		infoLabel[5] = FindChild<UILabel>(path + "/SortInfo");
		UIEventListener.Get (sortButton.gameObject).onClick = SortCallback;
		InitDragPanel ();
	}

	void InitDragPanel() {
		myUnitDragPanel = new DragPanel("PartyDragPanel", PartyUnitItem.ItemPrefab);
		myUnitDragPanel.CreatUI();
		Transform parent = FindChild<Transform>("LevelUpBasePanel");
		myUnitDragPanel.DragPanelView.SetScrollView(ConfigDragPanel.PartyListDragPanelArgs, parent);

		GameObject rejectItem = Resources.Load("Prefabs/UI/Friend/RejectItem") as GameObject;
		myUnitDragPanel.AddItem(1, rejectItem);
		GameObject rejectItemIns = myUnitDragPanel.ScrollItem[ 0 ];
		UIEventListener.Get(rejectItemIns).onClick = RejectCallback;
	}

	void ResetUIAfterLevelUp(object data) {
		ClearData ();
		uint blendID = (uint)data;
		TUserUnit tuu = dataCenter.UserUnitList.GetMyUnit (blendID);
		Debug.LogError ("tuu.ID : " + tuu.ID + " tuu.level : " + tuu.Level);
		selectedItem [0].UserUnit = tuu;
		UpdateBaseInfoView(selectedItem [0]);
	}

	void SelectedFriendCallback(LevelUpItem piv) {
		if (friendWindow == null) {
			friendWindow = DGTools.CreatFriendWindow();
			if(friendWindow == null) {
				return;
			}
		}
		gameObject.SetActive (false);
		friendWindow.selectFriend = SelectFriend;
		friendWindow.ShowUI ();
	}

	void SelectFriend(TFriendInfo friendInfo) {
		gameObject.SetActive (true);
		selectedItem [4].UserUnit = friendInfo.UserUnit;

		CheckLevelUp ();
	}
	
	/// <summary>
	/// Selecteds item's callback.
	/// </summary>
	void SelectedItemCallback(LevelUpItem piv) {
		if (prevMaterialItem == null) {
			DisposeNoPreMaterial (piv);
		} else {
			DisposeByPreMaterial(piv);
		}

		CheckLevelUp ();
	}

	void DisposeByPreMaterial(LevelUpItem lui) {
		if (CheckBaseItem (lui)) {
			return;	
		}
		EnabledItem (lui.UserUnit);
		lui.UserUnit = prevMaterialItem.UserUnit;
		lui.enabled = true;
		prevMaterialItem.IsEnable = false;

		ClearFocus ();
	}

	void DisposeNoPreMaterial(LevelUpItem piv) {
		if (CheckBaseItem (piv)) {
			ShieldParty (true,piv);		
		} else {
			ShieldParty(false,piv);
		}
		
		if (prevSelectedItem != null) {
			if(prevSelectedItem.Equals(piv)){
				ClearFocus();
			} else {
				prevSelectedItem.IsFocus = false;
				prevSelectedItem = piv;
				prevSelectedItem.IsFocus = true;

//				CheckLevelUp ();
			}
			return;
		}
		
		prevSelectedItem = piv;
		prevSelectedItem.IsFocus = true;	
	}

	/// <summary>
	/// drag panel item click.
	/// </summary>
	void MyUnitClickCallback(PartyUnitItem pui) {
		if (prevSelectedItem == null) {
			if (SetBaseItem (pui)) {
				return;	
			}
			
			int index = SetMaterialItem (pui);
			if (index > -1) {
				return;	
			}

			if (prevMaterialItem != null)
					prevMaterialItem.IsFocus = false;
			prevMaterialItem = pui;
			prevMaterialItem.IsFocus = true;

			CheckLevelUp ();
		} else {
			EnabledItem(prevSelectedItem.UserUnit);
			prevSelectedItem.IsFocus = false;
			prevSelectedItem.UserUnit = pui.UserUnit;
			pui.IsEnable = false;

			CheckLevelUp ();

			ClearFocus();
		}
	}

	void RejectCallback(GameObject go) {
		if (prevSelectedItem != null) {
			EnabledItem (prevSelectedItem.UserUnit);
			prevSelectedItem.UserUnit = null;
			prevSelectedItem.IsEnable = true;
		} else {
			for (int i = 3; i >= 0; i--) {
				LevelUpItem lui = selectedItem[i];
				if(lui.UserUnit == null) {
					continue;
				}
				EnabledItem(lui.UserUnit);
				lui.UserUnit = null;
				lui.IsEnable = true;
				if(i == 0) {
					ShieldParty(true,null);
				}
				break;
			}
		}

		CheckLevelUp ();

		ClearFocus ();
	}

	void ClearData() {
		if(friendWindow != null)
			friendWindow.HideUI ();
		foreach (var item in selectedItem) {
			item.UserUnit = null;
			item.IsEnable = true;
		}

		UpdateBaseInfoView (null);
	}
	
	void LevelUpCallback(GameObject go) {

		ExcuteCallback (levelUpInfo);
	}

	void SortCallback(GameObject go) {
		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
	}

	bool SetBaseItem(MyUnitItem pui) {
		if (selectedItem [0].UserUnit != null && !CheckBaseItem (prevSelectedItem)) { //index 0 is base item object.
			return false;
		}
		EnabledItem (selectedItem [0].UserUnit);
		selectedItem [0].UserUnit = pui.UserUnit;
		if (CheckIsParty (pui)) {
			selectedItem [0].IsEnable = true;
//			selectedItem [0].PartyLabel.text = "Base";
		}
		pui.IsEnable = false;
//		pui.PartyLabel.text = "Base";
		ClearFocus ();
		ShieldParty (false, null);

		CheckLevelUp ();

		return true;
	}

	void ClearFocus() {
		if (prevSelectedItem != null) {
			prevSelectedItem.IsFocus = false;
			prevSelectedItem = null;	
		}

		if (prevMaterialItem != null) {
			prevMaterialItem.IsFocus = false;
			prevMaterialItem = null;
		}
	}

	void ShieldParty(bool shield, MyUnitItem baseItem) {
		for (int i = 0; i < myUnitList.Count; i++) {
			PartyUnitItem pui = myUnitList [i];
			if(pui.IsParty || pui.IsFavorite) {
				if(baseItem != null && baseItem.UserUnit != null && pui.UserUnit.ID == baseItem.UserUnit.ID) {
					continue;
				}
				pui.IsEnable = shield;
			}
		}
	}

	int SetMaterialItem(MyUnitItem pui) {
		for (int i = 1; i < 4; i++) {	// 1~3 is material item object.
			if(selectedItem[i].UserUnit != null) {
				continue;
			}
			selectedItem[i].UserUnit = pui.UserUnit;
			pui.IsEnable = false;
			ClearFocus ();

			CheckLevelUp ();

			return i;
		}
		return -1;	// -1 == not add to materail list.
	}

	bool CheckBaseItem(MyUnitItem piv) {
		if (piv == null ) {
			return false;
		}

		if (piv.Equals (selectedItem [0])) {
			return true;
		}

		return false;
	}

	bool CheckIsParty(MyUnitItem piv) {
		if (piv == null ) {
			return false;
		}
		
		if (dataCenter.PartyInfo.UnitIsInParty(piv.UserUnit.ID)) {
			return true;
		}
		
		return false;
	}

	/// <summary>
	/// one item is material item and the other is selected item.
	/// </summary>
	bool SwitchMaterailToSelected(MyUnitItem selectedItem, MyUnitItem materialItem) {
		if (selectedItem == null || materialItem == null) {
			return false;	
		}

		if (!CheckBaseItem (selectedItem) || !CheckIsParty (materialItem)) {
			return false;
		}

		TUserUnit tuu = selectedItem.UserUnit;
		selectedItem.UserUnit = materialItem.UserUnit;
		materialItem.IsEnable = false;
		EnabledItem (tuu);
		return true;
	}
//
	void EnabledItem(TUserUnit tuu) {
		if (tuu == null) {
			return;	
		}
		for (int i = 0; i < myUnitList.Count; i++) {
			PartyUnitItem pui = myUnitList [i];
			if (pui.UserUnit.TUserUnitID == tuu.TUserUnitID) {
				if(pui.IsParty) {
					pui.PartyLabel.text = "Party";
				}
				else{
					pui.PartyLabel.text = "";
				}
				pui.IsEnable = true;
			}
		}
	}

	private void ReceiveSortInfo(object msg){
		sortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(_sortRule, myUnit);
		List<GameObject> scrollList = myUnitDragPanel.ScrollItem;
		for (int i = 1; i < scrollList.Count; i++){
			PartyUnitItem puv = scrollList[i].GetComponent<PartyUnitItem>();//myUnitList[i];
			puv.UserUnit = myUnit[ i - 1 ];
			puv.CurrentSortRule = sortRule;
		}
	}

	Queue<TUserUnit> levelUpInfo = new Queue<TUserUnit>() ;
	void CheckLevelUp() {
		levelUpInfo.Clear ();
		TUserUnit baseItem = selectedItem [0].UserUnit;
		if (baseItem == null) {
			levelUpButton.isEnabled = false;
			return;	
		}
		levelUpInfo.Enqueue (baseItem);

		TUserUnit friendInfo = selectedItem [4].UserUnit;
		if (friendInfo == null) {
			levelUpButton.isEnabled = false;
			return;	
		}
		levelUpInfo.Enqueue (friendInfo);

		for (int i = 1; i < 4; i++) {
			if(selectedItem[i].UserUnit != null) {
				levelUpInfo.Enqueue(selectedItem[i].UserUnit);
			}
		}

		if (levelUpInfo.Count == 2) {	// material is null; collection only have base and friend.
			levelUpButton.isEnabled = false;
			return;	
		}
	
		levelUpButton.isEnabled = true;
	}

	void UpdateBaseInfoView( MyUnitItem pui){
		MyUnitItem baseItem = selectedItem [0];
		if (pui == null) {
			foreach (var item in infoLabel) {
				item.text = "0";
			}
			baseItem.UserUnit = null;
			baseItem.IsEnable = true;
		} else {
			TUserUnit tuu = pui.UserUnit;
			TUnitInfo tu = tuu.UnitInfo;
			baseItem.UserUnit = tuu;
			int hp = DataCenter.Instance.GetUnitValue(tu.HPType,tuu.Level);
			infoLabel[0].text = hp.ToString();			
			int atk =  DataCenter.Instance.GetUnitValue(tu.AttackType, tuu.Level);
			infoLabel[1].text = atk.ToString();			
			infoLabel[2].text = tuu.NextExp.ToString();
			infoLabel[4].text = LevelUpTotalMoney().ToString();
			infoLabel[3].text = LevelUpCurExp().ToString();
		}
	}

	private const int CoinBase = 100;
	int LevelUpTotalMoney(){
		if (selectedItem[0].UserUnit == null){
			return 0;
		}
		int totalMoney = 0;
		for (int i = 1; i < 4; i++) {	//material index range
			if (selectedItem[i].UserUnit != null){
				totalMoney += CoinBase * selectedItem[i].UserUnit.Level;
			}
		}
		return totalMoney;
	}

	int LevelUpCurExp () {
		int devorExp = 0;
		for (int i = 1; i < 4; i++) {	//material index range
			if (selectedItem[i].UserUnit != null){
				devorExp += selectedItem[i].UserUnit .MultipleDevorExp(selectedItem[0].UserUnit);
			}
		}
		return devorExp;
	}
}
