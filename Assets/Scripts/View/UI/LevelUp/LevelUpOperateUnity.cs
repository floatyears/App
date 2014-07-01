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
			if(fromUnitDetail) {
				fromUnitDetail = false;
			}
		}

		base.ShowUI ();
		ClearFocus ();
		ShowData ();
		MsgCenter.Instance.AddListener (CommandEnum.SortByRule, ReceiveSortInfo);
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.SortByRule, ReceiveSortInfo);
		if (UIManager.Instance.nextScene == SceneEnum.UnitDetail) {
			fromUnitDetail = true;
			if (friendWindow != null && friendWindow.gameObject.activeSelf) {
				friendWindow.gameObject.SetActive (false);
			} 
		} else {
			if (friendWindow != null) {
				friendWindow.HideUI();
			}
		}
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		if(friendWindow != null)
			friendWindow.DestoryUI ();
		sortRule = SortRule.None;
		myUnitDragPanel.DestoryDragPanel ();
		MsgCenter.Instance.RemoveListener (CommandEnum.LevelUpSucceed, ResetUIAfterLevelUp);
	}
	bool clear = true;
	public override void ResetUIState () {
		clear = true;
		ClearData ();
		CheckLevelUp ();
	}

	public override void CallbackView (object data) {
		base.CallbackView (data);
	}

	private static SortRule _sortRule = SortRule.None;
	public static SortRule sortRule {
		get { return _sortRule; }
		set {
			_sortRule = value;
		}
	}

	private bool fromUnitDetail = false;
	
	private DataCenter dataCenter;

	/// <summary>
	/// index:0==base, 1~3==material, 4==friend.
	/// </summary>
	private LevelUpItem[] selectedItem = new LevelUpItem[6];

	private const int baseItemIndex = 0;

	private const int friendItemIndex = 5;
	/// <summary>
	/// indx : 0==hplabel, 1==explabel, 2==atk label. 3==exp got label. 4==coin need label. 5==sortlabel;
	/// </summary>
	private UILabel[] infoLabel = new UILabel[6];

	private string hp = "0";
	public string Hp{ 
		set { 
			hp = value;
			infoLabel [0].text = value;}//hp.ToString();}
	}

	private string atk = "0";
	public string Atk{ 
		set { 
			atk = value;
			infoLabel [1].text = value;}//atk.ToString();}
	}

	private string expNeed = "0";
	public string ExpNeed{ 
		set { 
			expNeed = value;
			infoLabel [3].text = value;}//expNeed.ToString();}
	}

	private int expGot = 0;
	public int ExpGot{ 
		set {
			expGot = value; 
			infoLabel[2].text = expGot.ToString();

			TUserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
			if(baseInfo == null)
				return;
			TUnitInfo tu = baseInfo.UnitInfo;
			int toLevel = tu.GetLevelByExp (expGot + baseInfo.Exp);
//			Debug.LogError("toLevel: " + toLevel+ " expGot:"+expGot +" baseExp:"+ baseInfo.Exp+" totalExp:"+(expGot + baseInfo.Exp));
			if (expGot == 0) {
				Hp = baseInfo.Hp + "";// + "->" + tu.GetHpByLevel(toLevel);
				Atk =  baseInfo.Attack + "";// + "->" + tu.GetAtkByLevel(toLevel);
				ExpNeed = baseInfo.Level + "";// + "->" + toLevel;
			}else{
				Hp = baseInfo.Hp + " -> " + tu.GetHpByLevel(toLevel);
				Atk =  baseInfo.Attack + " -> " + tu.GetAtkByLevel(toLevel);
				ExpNeed = baseInfo.Level + " -> " + toLevel;
			}

		}
	}

	private int coinNeed = 0;
	public int CoinNeed { 
		set {coinNeed = value; infoLabel[4].text = coinNeed.ToString();}
	}

	private UIButton levelUpButton;

	private UIButton sortButton;

	private DragPanelDynamic myUnitDragPanel;

	private List<LevelUpUnitItem> myUnitList = new List<LevelUpUnitItem> ();

	private List<TUserUnit> myUnit = new List<TUserUnit> ();

	private MyUnitItem prevSelectedItem;

	private MyUnitItem prevMaterialItem;

	private FriendWindows friendWindow;

	void ShowData () {
//		Debug.LogError ("show data : " + clear);
		if (!clear) {
			return;	
		}

		clear = false;

		if (myUnitDragPanel == null) {
			InitDragPanel();	
		}

		myUnit = dataCenter.UserUnitList.GetAllMyUnit ();

		if (_sortRule != SortRule.None) {
			SortUnitTool.SortByTargetRule(_sortRule, myUnit);
		}

		myUnitDragPanel.RefreshItem (myUnit);

		foreach (var item in myUnitDragPanel.scrollItem) {
			LevelUpUnitItem pui = item as LevelUpUnitItem;
			pui.callback = MyUnitClickCallback;
			pui.IsParty = dataCenter.PartyInfo.UnitIsInParty(pui.UserUnit);
			myUnitList.Add(pui);
		}

		RefreshCounter ();
	}

	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void InitUI() {
		dataCenter = DataCenter.Instance;
		for (int i = 1; i < 7; i++) {	//gameobject name is 1 ~ 6.
			LevelUpItem pui = FindChild<LevelUpItem>("Top/" + i.ToString());
			selectedItem[i -1] = pui;
			pui.Init(null);
			pui.IsEnable = true;
			pui.IsFavorite = false;
			if(i == 1) {	//base item ui.
				pui.callback = SelectedItemCallback;
				pui.PartyLabel.text = TextCenter.GetText("Text_BASE");
				continue;
			}
			if(i == 6){		//friend item ui.
				pui.callback = SelectedFriendCallback;
				pui.PartyLabel.text = TextCenter.GetText("Text_Friend");
				continue;
			}

			pui.callback = SelectedItemCallback;
		}
		string path = "Top/InfoPanel/Label_Value/";
		string[] texts = new string[] {
						TextCenter.GetText ("Text_HP"),
						TextCenter.GetText ("Text_ATK"),
						TextCenter.GetText ("Text_EXP"),
						TextCenter.GetText ("Text_Level"),
						TextCenter.GetText ("Text_Coins")
				};
		for (int i = 0; i < 5; i++) { //label name is 0 ~ 4
			infoLabel[i] = FindChild<UILabel>(path + i);
			FindChild<UILabel>("Top/InfoPanel/_Label" + i).text = texts[i];
		}
		levelUpButton = FindChild<UIButton>("Button_LevelUp");
		FindChild ("Button_LevelUp/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_Level_Up");
//		FindChild ("Top/1/Label_Party").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_BASE");
//		FindChild ("Top/6/Label_Party").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Friend");

		UIEventListener.Get (levelUpButton.gameObject).onClick = LevelUpCallback;
		levelUpButton.isEnabled = false;
		path = "Middle/LevelUpBasePanel/SortButton";
		sortButton = FindChild<UIButton>(path);
		UIEventListener.Get (sortButton.gameObject).onClick = SortCallback;
		InitDragPanel ();
	}

	void InitDragPanel() {
		GameObject go = Instantiate (LevelUpUnitItem.ItemPrefab) as GameObject;
		LevelUpUnitItem.Inject (go);
		GameObject parent = FindChild<Transform>("Middle/LevelUpBasePanel").gameObject;
		myUnitDragPanel = new DragPanelDynamic (parent, go, 9, 3);

		DragPanelSetInfo dpsi = new DragPanelSetInfo ();
		dpsi.parentTrans = parent.transform;
		dpsi.clipRange = new Vector4 (0, -100, 640, 315);
		dpsi.gridArrange = UIGrid.Arrangement.Vertical;
		dpsi.scrollBarPosition = new Vector3 (-320, -250, 0);
		dpsi.maxPerLine = 3;
		dpsi.depth = 2;	
		myUnitDragPanel.SetDragPanel (dpsi);
		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/RejectItem", o =>{
			GameObject rejectItem = o as GameObject;
			GameObject rejectItemIns = myUnitDragPanel.AddRejectItem (rejectItem);
			rejectItemIns.transform.FindChild("Label_Text").GetComponent<UILabel>().text = TextCenter.GetText ("Text_Reject");
			UIEventListener.Get(rejectItemIns).onClick = RejectCallback;
		});
	}

	void ResetUIAfterLevelUp(object data) {
		ClearData ();
		uint blendID = (uint)data;
		TUserUnit tuu = dataCenter.UserUnitList.GetMyUnit (blendID);
		selectedItem [baseItemIndex].UserUnit = tuu;
		clear = true;
		ShowData ();

		myUnitDragPanel.RefreshItem (tuu);
		ShieldParty (false, null);
	}

	void SelectedFriendCallback(LevelUpItem piv) {
		if (friendWindow == null) {
			friendWindow = DGTools.CreatFriendWindow();
			if(friendWindow == null) {
				return;
			}
		}
		Debug.LogError ("friendWindow : " + friendWindow);
		gameObject.SetActive (false);
		friendWindow.selectFriend = SelectFriend;
		friendWindow.ShowUI ();
	}

	void SelectFriend(TFriendInfo friendInfo) {
		gameObject.SetActive (true);
		selectedItem [friendItemIndex].UserUnit = friendInfo.UserUnit;
		selectedItem [friendItemIndex].IsEnable = true;
		RefreshFriend ();
		CheckLevelUp ();
	}
	
	/// <summary>
	/// Selecteds material item's callback.
	/// </summary>
	void SelectedItemCallback(LevelUpItem piv) {
		if (prevMaterialItem == null) {
			DisposeNoPreMaterial (piv);
		} else {
			DisposeByPreMaterial(piv);
		}
		RefreshMaterial ();
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
			}
			return;
		}
		
		prevSelectedItem = piv;
		prevSelectedItem.IsFocus = true;	
	}

	/// <summary>
	/// drag panel item click.
	/// </summary>
	void MyUnitClickCallback(LevelUpUnitItem pui) {
		if (pui.IsFavorite) {
			return;		
		}
		if (prevSelectedItem == null) {
			if (SetBaseItemPreSelectItemNull (pui)) {
				return;	
			}
			
			int index = SetMaterialItem (pui);

			if (index > -1) {
				RefreshMaterial();
				return;	
			}

			if (prevMaterialItem != null) {
				prevMaterialItem.IsFocus = false;
			}

			prevMaterialItem = pui;
			prevMaterialItem.IsFocus = true;

			CheckLevelUp ();
		} else {
			if (SetBaseItem (pui)) {
				return;	
			}

			EnabledItem(prevSelectedItem.UserUnit);
			prevSelectedItem.IsFocus = false;
			prevSelectedItem.UserUnit = pui.UserUnit;
			pui.IsEnable = false;
			RefreshMaterial();
			CheckLevelUp ();
			ClearFocus();
		}
	}

	void RejectCallback(GameObject go) {
		if (prevSelectedItem != null) {
			bool isBase = prevSelectedItem.Equals(selectedItem[baseItemIndex]);
			EnabledItem (prevSelectedItem.UserUnit);
			prevSelectedItem.UserUnit = null;
			prevSelectedItem.IsEnable = true;
			if(isBase) {
				UpdateBaseInfoView ();
			}
			else{
				RefreshMaterial();
			}
		} else {
			for (int i = 4; i >= 0; i--) {
				LevelUpItem lui = selectedItem[i];
				if(lui.UserUnit == null) {
					continue;
				}
				EnabledItem(lui.UserUnit);
				lui.UserUnit = null;
				lui.IsEnable = true;
				if(i == baseItemIndex) {
					ShieldParty(true,null);
					UpdateBaseInfoView ();
				}
				else{
					RefreshMaterial();
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
		ClearInfoPanelData ();
	}

	void ClearInfoPanelData() {
		Hp = "0";
		Atk = "0";
		ExpNeed = "0";
		ExpGot = 0;
		CoinNeed = 0;
	}
	
	void LevelUpCallback(GameObject go) {
		if (dataCenter.AccountInfo.Money < coinNeed) {
			ViewManager.Instance.ShowTipsLabel("not enough money");
			return;
		}

		ExcuteCallback (levelUpInfo);
	}

	void SortCallback(GameObject go) {
		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
	}

	bool SetBaseItemPreSelectItemNull(MyUnitItem pui) {
		if (selectedItem [baseItemIndex].UserUnit != null ) { //index 0 is base item object.
			if(selectedItem [baseItemIndex].UserUnit.MakeUserUnitKey() == pui.UserUnit.MakeUserUnitKey()) {
				ShieldParty (false, null);
				return true;
			} else{
				return false;
			}
		}
		
		EnabledItem (selectedItem [baseItemIndex].UserUnit);
		selectedItem [baseItemIndex].UserUnit = pui.UserUnit;
		UpdateBaseInfoView ();
		if (CheckIsParty (pui)) {
			selectedItem [baseItemIndex].IsEnable = true;
		}
		pui.IsEnable = false;
		ClearFocus ();

		ShieldParty (false, null);
		
		CheckLevelUp ();
		
		return true;
	}

	bool SetBaseItem(MyUnitItem pui) {
		if (selectedItem [baseItemIndex].UserUnit != null && !CheckBaseItem (prevSelectedItem)) { //index 0 is base item object.
			return false;
		}

		EnabledItem (selectedItem [baseItemIndex].UserUnit);
		selectedItem [baseItemIndex].UserUnit = pui.UserUnit;
		UpdateBaseInfoView ();
		if (CheckIsParty (pui)) {
			selectedItem [baseItemIndex].IsEnable = true;
		}
		pui.IsEnable = false;
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
			LevelUpUnitItem pui = myUnitList [i];
			if(pui.IsParty || pui.IsFavorite) {
				if(baseItem != null && baseItem.UserUnit != null && pui.UserUnit.ID == baseItem.UserUnit.ID) {
					continue;
				}
				pui.IsEnable = shield;
			}
		}
	}

	int SetMaterialItem(MyUnitItem pui) {
		for (int i = 1; i < 5; i++) {	// 1~3 is material item object.
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

		if (piv.UserUnit.MakeUserUnitKey() == selectedItem [baseItemIndex].UserUnit.MakeUserUnitKey() ) {
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
			LevelUpUnitItem pui = myUnitList [i];
			if (pui.UserUnit.TUserUnitID == tuu.TUserUnitID) {
				if(pui.IsParty) {
					pui.PartyLabel.text = TextCenter.GetText("Text_Party");
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
		if (_sortRule == SortRule.None) {
			return;	
		}
		SortUnitTool.SortByTargetRule(_sortRule, myUnit);
		myUnitDragPanel.RefreshItem (myUnit);
//		List<GameObject> scrollList = myUnitDragPanel.ScrollItem;
//		for (int i = 1; i < scrollList.Count; i++){
//			PartyUnitItem puv = scrollList[i].GetComponent<PartyUnitItem>();//myUnitList[i];
//			TUserUnit tuu = myUnit[ i - 1 ];
//			puv.UserUnit = tuu;
//			puv.CurrentSortRule = sortRule;
//		}
	}

	List<TUserUnit> levelUpInfo = new List<TUserUnit>() ;
	void CheckLevelUp() {
		levelUpInfo.Clear ();

		TUserUnit baseItem = selectedItem [baseItemIndex].UserUnit;
		if (baseItem == null) {
			levelUpButton.isEnabled = false;
			return;	
		}
		levelUpInfo.Add (baseItem);

		TUserUnit friendInfo = selectedItem [friendItemIndex].UserUnit;
		if (friendInfo == null) {
			levelUpButton.isEnabled = false;
			return;	
		}
		levelUpInfo.Add (friendInfo);

		for (int i = 1; i < 5; i++) {
			if(selectedItem[i].UserUnit != null) {
				levelUpInfo.Add(selectedItem[i].UserUnit);
			}
		}

		if (levelUpInfo.Count == 2) {	// material is null; collection only have base and friend.
			levelUpButton.isEnabled = false;
			return;	
		}
	
		levelUpButton.isEnabled = true;


	}

	void UpdateBaseInfoView(){
		TUserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
		if (baseInfo == null) {
			ClearInfoPanelData();
			return;	
		}

		//int exp = GetLevelExp ();


		RefreshMaterial ();

		if (baseInfo == null) {
			ClearInfoPanelData ();
			//			return;	
		}
//		TUnitInfo tu = baseInfo.UnitInfo;
//		int toLevel = tu.GetLevelByExp (expGot + baseInfo.Exp);
//		if (expGot == 0) {
//			Hp = baseInfo.Hp + "";// + "->" + tu.GetHpByLevel(toLevel);
//			Atk =  baseInfo.Attack + "";// + "->" + tu.GetAtkByLevel(toLevel);
//			ExpNeed = baseInfo.Level + "";// + "->" + toLevel;
//		}else{
//			Hp = baseInfo.Hp + "->" + tu.GetHpByLevel(toLevel);
//			Atk =  baseInfo.Attack + "->" + tu.GetAtkByLevel(toLevel);
//			ExpNeed = baseInfo.Level + "->" + toLevel;
//		}


	}

	void RefreshMaterial() {
		//TUserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
		if (CheckBaseIsNull()) {
			return;
		}

		ExpGot = LevelUpCurExp();
		CoinNeed = LevelUpTotalMoney();

		RefreshFriend ();
	}

	void RefreshFriend() {
		if (CheckBaseIsNull()) {
			return;	
		}

		TUserUnit friend = selectedItem [friendItemIndex].UserUnit;
		if (friend == null) {
			return ;
		}

		TUserUnit baseInfo = selectedItem[baseItemIndex].UserUnit;
		ExpGot = System.Convert.ToInt32(LevelUpCurExp() * DGTools.AllMultiple (friend, baseInfo) );
	}

	bool CheckBaseIsNull() {
		TUserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
		if (baseInfo == null) {
			return true;	
		}
		return false;
	}
	
	private const int CoinBase = 100;
	int LevelUpTotalMoney(){
		if (selectedItem[baseItemIndex].UserUnit == null){
			return 0;
		}
		int totalMoney = 0;
		for (int i = 1; i < 5; i++) {	//material index range
			if (selectedItem[i].UserUnit != null){
				totalMoney ++;
			}
		}

		return selectedItem [baseItemIndex].UserUnit.Level * CoinBase * totalMoney;
	}

	int LevelUpCurExp () {
		int devorExp = 0;
		for (int i = 1; i < 5; i++) {	//material index range
			if (selectedItem[i].UserUnit != null){
				devorExp += selectedItem[i].UserUnit.MultipleMaterialExp(selectedItem[baseItemIndex].UserUnit);
			}
		}
//		Debug.LogError (devorExp);
		return devorExp;
	}

	public LevelUpUnitItem GetPartyUnitItem(int i){
		if (i == -1) {
			return myUnitList[myUnitList.Count-1];
		} else {
			return myUnitList [i];
		}
	}
}
