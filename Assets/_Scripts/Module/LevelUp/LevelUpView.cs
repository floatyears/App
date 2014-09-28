using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class LevelUpView : ViewBase {
	private bool doNotClearData = false ;
	private bool fromLevelUpBack = false;

	private bool fromUnitDetail = false;
	private DataCenter dataCenter;
	private UIButton levelUpButton;
	private UIButton sortButton;
	
	private DragPanelDynamic dragPanel;
	
	private List<LevelUpUnitItem> unitItems = new List<LevelUpUnitItem> ();
	private List<UserUnit> myUnit = new List<UserUnit> ();
	
	private MyUnitItem prevSelectedItem;
	private MyUnitItem prevMaterialItem;
	
	private GameObject topObject;
	private GameObject bottomObject;

	/// <summary>
	/// index:0==base, 1~3==material, 4==friend.
	/// </summary>
	private LevelUpItem[] selectedItem = new LevelUpItem[6];
	private DragPanelConfigItem dragConfig;
	
	private const int baseItemIndex = 0;
	
	private const int friendItemIndex = 5;
	/// <summary>
	/// indx : 0==hplabel, 1==explabel, 2==atk label. 3==exp got label. 4==coin need label. 5==sortlabel;
	/// </summary>
	private UILabel[] infoLabel = new UILabel[6];


///////////////////////////////////////////////////////////////////////////////////////////////////////

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init (config, data);
		InitUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();

		ModuleManager.Instance.ShowModule (ModuleEnum.UnitSortModule,"from","level_up");
		ModuleManager.Instance.ShowModule (ModuleEnum.ItemCounterModule,"from","level_up");

		ClearFocus ();
		myUnit = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ();
		sortRule = SortUnitTool.GetSortRule (SortRuleByUI.LevelUp);
		SortUnitByCurRule();
		ShowData ();
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);

		topObject.transform.localPosition = new Vector3 (0, 1000, 0);
		bottomObject.transform.localPosition = new Vector3(-1000, 0, 0);

		iTween.MoveTo(topObject, iTween.Hash("y", 20, "time", 0.4f,"islocal", true));

		iTween.MoveTo (bottomObject, iTween.Hash ("x", 0, "time", 0.4f, "islocal", true, "oncomplete", "BottomRootMoveEnd", "oncompletetarget", gameObject));

		if( doNotClearData && viewData != null && viewData.ContainsKey("friendinfo")){
			SelectFriend(viewData["friendinfo"] as FriendInfo);
		}

//		if( fromLevelUpBack ) {
//			ClearData(true);
//		}

		doNotClearData = false;
		fromLevelUpBack = false;
	}

	public override void HideUI () {

		ModuleManager.Instance.HideModule (ModuleEnum.UnitSortModule);
		ModuleManager.Instance.HideModule (ModuleEnum.ItemCounterModule);

		if(selectedItem[baseItemIndex].UserUnit != null) {
			selectedItem[baseItemIndex].UserUnit.isEnable = true;
			selectedItem[baseItemIndex].UserUnit.isFocus = false;
		}

		if(viewData != null && viewData.ContainsKey("friendinfo")){
			viewData.Remove("friendinfo");
		}

		//clear selected
		if( !doNotClearData ) {
			ClearData(true);
		}

		base.HideUI ();
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		sortRule = SortRule.None;
		dragPanel.DestoryDragPanel ();
	}

	public override void CallbackView(params object[] args) {
		base.CallbackView (args);
	}

	private static SortRule _sortRule = SortRule.None;
	public static SortRule sortRule {
		get { return _sortRule; }
		set {
			_sortRule = value;
		}
	}
	
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

	private string expNeed = "0";	// level label
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

			UserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
			if(baseInfo == null)
				return;
			UnitInfo tu = baseInfo.UnitInfo;
			int toLevel = tu.GetLevelByExp (expGot + baseInfo.exp);
			if (expGot == 0) {
				Hp = baseInfo.Hp + "";// + "->" + tu.GetHpByLevel(toLevel);
				Atk =  baseInfo.Attack + "";// + "->" + tu.GetAtkByLevel(toLevel);
				ExpNeed = baseInfo.level + "";// + "->" + toLevel;
			}else{
				Hp = baseInfo.Hp + " -> " + "[AA0000]" + tu.GetHpByLevel(toLevel) + "[-]";
				Atk =  baseInfo.Attack + " -> [AA0000]" + tu.GetAtkByLevel(toLevel) + "[-]";
				ExpNeed = baseInfo.level + " -> [AA0000]" + toLevel + "[-]";
			}

		}
	}

	private int coinNeed = 0;
	public int CoinNeed { 
		set {coinNeed = value; infoLabel[4].text = coinNeed.ToString();}
	}

	void ShowData () {

		if (dragPanel == null) {
			InitDragPanel();
		}

		dragPanel.RefreshItem (myUnit);
		unitItems.Clear ();
//		Debug.LogError (" ShowData : " + myUnitDragPanel.scrollItem.Count + " item : " + myUnitDragPanel.scrollItem [0]);
		foreach (var item in dragPanel.scrollItem) {
			LevelUpUnitItem pui = item as LevelUpUnitItem;
			pui.callback = MyUnitClickCallback;
			pui.IsParty = dataCenter.UnitData.PartyInfo.UnitIsInParty(pui.UserUnit);

			unitItems.Add(pui);
		}

		RefreshSortInfo ();
		RefreshCounter ();
	}



	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserData.UserInfo.unitMax);
		countArgs.Add("posy",-745);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void InitUI() {
		dataCenter = DataCenter.Instance;
//		iTween.MoveTo(topRoot, iTween.Hash("y", 150, "time", 0.4f,"islocal", true));
//		iTween.MoveTo (bottomRoot, iTween.Hash ("x", 0, "time", 0.4f, "islocal", true, "oncomplete", "BottomRootMoveEnd", "oncompletetarget", gameObject)); 

		topObject = FindChild("Top");
		bottomObject = FindChild ("Middle");

		for (int i = 1; i <= 6; i++) {	//gameobject name is 1 ~ 6.
			LevelUpItem pui = FindChild<LevelUpItem>("Top/" + i.ToString());
			selectedItem[i -1] = pui;
			pui.Init(null);
			pui.IsEnable = true;
			pui.IsFavorite = false;
			if(i == 1) {	//base item ui.
				pui.callback = SelectBaseItemCallback;
				pui.PartyLabel.text = TextCenter.GetText("Text_BASE");
				continue;
			}
			if(i == 5){		//friend item ui.
//				pui.callback = SelectedFriendCallback;
				pui.PartyLabel.text = TextCenter.GetText("Text_Material");
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

		UIEventListenerCustom.Get (levelUpButton.gameObject).onClick = LevelUpCallback;
		levelUpButton.isEnabled = false;
		InitDragPanel ();
	}

	void InitDragPanel() {
//		GameObject go = Instantiate (LevelUpUnitItem.ItemPrefab) as GameObject;
//		LevelUpUnitItem.Inject (go);
		GameObject parent = FindChild<Transform>("Middle/LevelUpBasePanel").gameObject;

		dragConfig = DataCenter.Instance.GetConfigDragPanelItem ("LevelUpDragPanel");
		dragPanel = new DragPanelDynamic (parent, LevelUpUnitItem.Inject().gameObject, 12, 3);
		dragPanel.SetScrollView(dragConfig, transform);


		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/RejectItem", o =>{
			GameObject rejectItem = o as GameObject;
			GameObject rejectItemIns = dragPanel.AddRejectItem (rejectItem);
			rejectItemIns.transform.FindChild("Label_Text").GetComponent<UILabel>().text = TextCenter.GetText ("Text_Reject");
			UIEventListenerCustom.Get(rejectItemIns).onClick = RejectCallback;
		});
	}

	public void ResetUIAfterLevelUp(uint blendID) {
		ClearData (false);
		UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit (blendID);
		selectedItem [baseItemIndex].UserUnit = tuu;
//		clear = true;
		myUnit = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ();
		myUnit.Find (a => a.MakeUserUnitKey () == tuu.MakeUserUnitKey ()).isEnable = false;
		ShowData ();
		dragPanel.RefreshItem (tuu);
		ShieldPartyAndFavorite (false, null);
		CheckLevelUp ();
	}

	void SelectedFriendCallback(LevelUpItem piv) {
		doNotClearData = true;

		gameObject.SetActive (false);
//		friendWindow.evolveItem = null;
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
//		friendWindow.selectFriend = SelectFriend;
//		friendWindow.ShowUI ();
		ModuleManager.Instance.ShowModule (ModuleEnum.FriendSelectModule,"type","levelup");
	}

	FriendInfo levelUpUerFriend;
	void SelectFriend(FriendInfo friendInfo) {
		gameObject.SetActive (true);

		if (friendInfo == null) {
			return;	
		}
		levelUpUerFriend = friendInfo;
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

	void SelectBaseItemCallback(LevelUpItem piv){
		if (prevSelectedItem == null) {
			if (prevMaterialItem == null) {
				DisposeNoPreMaterial (piv);
			} else {
				DisposeByPreMaterial (piv);
			}
		}

		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);

		RefreshMaterial ();
		CheckLevelUp ();
	}

	void DisposeByPreMaterial(LevelUpItem lui) {
		if (CheckBaseItem (prevMaterialItem)) {
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
			ShieldPartyAndFavorite (true,piv);		
		} else {
			ShieldPartyAndFavorite(false,piv);
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
//		Debug.LogError()
		if (prevSelectedItem == null) {
			if (SetBaseItemPreSelectItemNull (pui)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
				return;
			}
			
			int index = SetMaterialItem (pui);

			if (index > -1) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
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
//				Debug.LogError("SetBaseItem");
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
				return;	
			}
//			Debug.LogError("else");
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);

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
			if(prevSelectedItem.UserUnit == null) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			} else {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
			}

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
			bool success = false;
			for (int i = 4; i >= 0; i--) {
				LevelUpItem lui = selectedItem[i];
				if(lui.UserUnit == null) {
					continue;
				}
				success = true;
				EnabledItem(lui.UserUnit);
				lui.UserUnit = null;
				lui.IsEnable = true;
				if(i == baseItemIndex) {
					ShieldPartyAndFavorite(true,null);
					UpdateBaseInfoView ();
				}
				else{
					RefreshMaterial();
				}
				break;
			}

			if(success) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
			} else {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			}
		}

		CheckLevelUp ();
		ClearFocus ();
	}

	void ClearData(bool remainBase) {
//		if(friendWindow != null)
//			friendWindow.HideUI ();

		for (int i=0; i<selectedItem.Length; i++) {
			if(remainBase && i==0) {
				continue;
			}

			selectedItem[i].UserUnit = null;
			selectedItem[i].IsEnable = true;
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

	//LevelUp Button ClickCallback
	void LevelUpCallback(GameObject go) {
		if (dataCenter. UserData.AccountInfo.money < coinNeed) {
			TipsManager.Instance.ShowTipsLabel("not enough money");
			return;
		}
		DataCenter.Instance.FriendData.useFriend = levelUpUerFriend;
//		ExcuteCallback (levelUpInfo);
		ModuleManager.SendMessage (ModuleEnum.LevelUpModule, "DoLevelUp", levelUpInfo);

		fromLevelUpBack = true;
	}

//	void SortCallback(GameObject go) {
//		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
//	}

	bool SetBaseItemPreSelectItemNull(MyUnitItem pui) {
		if (selectedItem [baseItemIndex].UserUnit != null ) { //index 0 is base item object.
			if(selectedItem [baseItemIndex].UserUnit.MakeUserUnitKey() == pui.UserUnit.MakeUserUnitKey()) {
				ShieldPartyAndFavorite (false, null);
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

		ShieldPartyAndFavorite (false, null);
		
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
		ShieldPartyAndFavorite (false, selectedItem [baseItemIndex]);

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
	
	void ShieldPartyAndFavorite(bool shield, MyUnitItem baseItem) {
		for (int i = 0; i < unitItems.Count; i++) {
//			if(levelUpItems[i].gameObject == null) {
//				levelUpItems.RemoveAt(i);
//				continue;
//			}

			LevelUpUnitItem pui = unitItems [i];
			pui.IsEnable = true;
			if(pui.IsParty || pui.IsFavorite) {

				if(baseItem != null && baseItem.UserUnit != null && pui.UserUnit.uniqueId == baseItem.UserUnit.uniqueId) {
					continue;
				}
				pui.IsEnable = shield;
			}
		}

//		for (int i = 0; i < myUnitList.Count; i++) {
//			if(myUnitList[i].gameObject == null) {
//				continue;
//			}
//
//			LevelUpUnitItem pui = myUnitList [i];
//			if(pui.IsParty || pui.IsFavorite) {
//
//				if(baseItem != null && baseItem.UserUnit != null && pui.UserUnit.ID == baseItem.UserUnit.ID) {
//					continue;
//				}
//				pui.IsEnable = shield;
//			}
//		}
	}

	int SetMaterialItem(MyUnitItem pui) {
		for (int i = 1; i < 5; i++) {	// 1~4 is material item object.
			if(selectedItem[i].UserUnit != null) {
				continue;
			}
			selectedItem[i].UserUnit = pui.UserUnit;
			pui.IsEnable = false;
			ClearFocus ();

			CheckLevelUp ();

			return i;
		}

		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_invalid);

		return -1;	// -1 == not add to materail list.
	}

	bool CheckBaseItem(MyUnitItem piv) {
		if (piv == null ) {
			return false;
		}
		if (piv.UserUnit == null) {
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
		
		if (DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(piv.UserUnit.uniqueId)) {
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

		UserUnit tuu = selectedItem.UserUnit;
		selectedItem.UserUnit = materialItem.UserUnit;
		materialItem.IsEnable = false;
		EnabledItem (tuu);
		return true;
	}
//
	void EnabledItem(UserUnit tuu) {
		if (tuu == null) {
			return;	
		}
		for (int i = 0; i < unitItems.Count; i++) {
			LevelUpUnitItem pui = unitItems [i];
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
//
//	void FriendBack(object data) {
////		if (friendWindow == null) {
////			return;	
////		}
////
////		friendWindow.Back (null);
//	}

	private void ReceiveSortInfo(object msg){
		sortRule = (SortRule)msg;
		SortUnitByCurRule();
		dragPanel.RefreshItem (myUnit);
		dragPanel.RefreshSortInfo (sortRule);
//		RefreshSortInfo ();
	}

	void RefreshSortInfo() {
//		foreach (var item in myUnitDragPanel.scrollItem) {
//			item.CurrentSortRule= sortRule;
//		}
	}

	private void SortUnitByCurRule(){
		if (_sortRule == SortRule.None) {
			return;	
		}
		SortUnitTool.SortByTargetRule(_sortRule, myUnit);
		SortUnitTool.StoreSortRule (_sortRule, SortRuleByUI.LevelUp);

	}

	List<UserUnit> levelUpInfo = new List<UserUnit>() ;
	void CheckLevelUp() {
		levelUpInfo.Clear ();

		UserUnit baseItem = selectedItem [baseItemIndex].UserUnit;
		if (baseItem == null) {
			levelUpButton.isEnabled = false;
			return;	
		}
		levelUpInfo.Add (baseItem);

		UserUnit friendInfo = selectedItem [friendItemIndex].UserUnit;
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
		UserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
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

		UserUnit friend = selectedItem [friendItemIndex].UserUnit;
		if (friend == null) {
			return ;
		}

		UserUnit baseInfo = selectedItem[baseItemIndex].UserUnit;
		ExpGot = System.Convert.ToInt32(LevelUpCurExp() * DGTools.AllMultiple (friend, baseInfo) );
	}

	bool CheckBaseIsNull() {
		UserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
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

		return selectedItem [baseItemIndex].UserUnit.level * CoinBase * totalMoney;
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

	public LevelUpUnitItem GetPartyUnitItem(uint id){

		foreach (var item in unitItems) {
			if(item.UserUnit.unitId == id){
				return item;
			}
		}
		return null;
	}

	public void SetItemVisible(uint unitId){
		foreach (var item in myUnit) {
			if(item.unitId == unitId)
			{
				myUnit.Remove(item);
				myUnit.Add(item);
				myUnit.Reverse();
				break;
			}
		}

		dragPanel.RefreshItem (myUnit);

	}

	public void RefreshUnitItem(UserUnit unit) {
		for (int i=0; i<myUnit.Count; i++) {
			if (myUnit[i]!=null && myUnit[i].uniqueId == unit.uniqueId ) {
				myUnit[i] = unit;
				break;
			}
		}

		dragPanel.RefreshItem(myUnit);
		ShieldPartyAndFavorite (false, null);
	}
}
