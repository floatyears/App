//using UnityEngine;
//using System.Collections.Generic;
//using bbproto;
//
//public class LevelUpView : ViewBase {
//	private bool doNotClearData = false ;
//	private bool fromLevelUpBack = false;
//
//	private bool fromUnitDetail = false;
////	private DataCenter dataCenter;
//	private UIButton levelUpButton;
//	private UIButton sortButton;
//	private UIButton autoSelect;
//	
//	private DragPanel dragPanel;
//	
////	private List<LevelUpUnitItem> unitItems = new List<LevelUpUnitItem> ();
////	private List<UserUnit> myUnit = new List<UserUnit> ();
//
//	private MyUnitItem prevSelectedItem;
//	private MyUnitItem prevMaterialItem;
//	
//	private GameObject topObject;
//	private GameObject bottomObject;
//
//	/// <summary>
//	/// index:0==base, 1~3==material, 4==friend.
//	/// </summary>
//
//	
//	private const int baseItemIndex = 0;
//	
//	private const int friendItemIndex = 5;
//
//	List<KeyValuePair<int,UserUnit>> sortDic;
//	/// <summary>
//	/// indx : 0==hplabel, 1==explabel, 2==atk label. 3==exp got label. 4==coin need label. 5==sortlabel;
//	/// </summary>
//	private UILabel[] infoLabel = new UILabel[6];
//
//
//	public override void Init (UIConfigItem config, Dictionary<string, object> data = null) {
//		base.Init (config, data);
//		InitUI ();
//
//		sortDic = new List<KeyValuePair<int, UserUnit>> ();
//	}
//
//	public override void ShowUI () {
//		base.ShowUI ();
//
//		ModuleManager.Instance.ShowModule (ModuleEnum.UnitSortModule,"from","level_up");
//		ModuleManager.Instance.ShowModule (ModuleEnum.ItemCounterModule,"from","level_up");
//
//		ClearFocus ();
////		myUnit = ;
//		_sortRule = SortUnitTool.GetSortRule (SortRuleByUI.LevelUp);
//		SortUnitByCurRule();
//		ShowData ();
//		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
//
//
//		topObject.transform.localPosition = new Vector3 (0, 1000, 0);
//		bottomObject.transform.localPosition = new Vector3(-1000, 0, 0);
//
//		iTween.MoveTo(topObject, iTween.Hash("y", 20, "time", 0.4f,"islocal", true));
//
//		iTween.MoveTo (bottomObject, iTween.Hash ("x", 0, "time", 0.4f, "islocal", true, "oncomplete", "BottomRootMoveEnd", "oncompletetarget", gameObject));
//
//		if( doNotClearData && viewData != null && viewData.ContainsKey("friendinfo")){
//			SelectFriend(viewData["friendinfo"] as FriendInfo);
//		}
//
//		doNotClearData = false;
//		fromLevelUpBack = false;
//	}
//
//	public override void HideUI () {
//
//		ModuleManager.Instance.HideModule (ModuleEnum.UnitSortModule);
//		ModuleManager.Instance.HideModule (ModuleEnum.ItemCounterModule);
//
//		if(selectedItem[baseItemIndex].UserUnit != null) {
//			selectedItem[baseItemIndex].UserUnit.isEnable = true;
//			selectedItem[baseItemIndex].UserUnit.isFocus = false;
//		}
//
//		if(viewData != null && viewData.ContainsKey("friendinfo")){
//			viewData.Remove("friendinfo");
//		}
//
//		//clear selected
//		if( !doNotClearData ) {
//			ClearData(true);
//		}
//
//		base.HideUI ();
//		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
//	}
//
//	public override void DestoryUI () {
//		base.DestoryUI ();
//		_sortRule = SortRule.None;
//		dragPanel.DestoryUI ();
//	}
//
//	void BottomRootMoveEnd(){
//		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.LEVEL_UP);
//	}
//
//	private SortRule _sortRule = SortRule.None;
//	
//	private string hp = "0";
//	public string Hp{ 
//		set { 
//			hp = value;
//			infoLabel [0].text = value;}//hp.ToString();}
//	}
//
//	private string atk = "0";
//	public string Atk{ 
//		set { 
//			atk = value;
//			infoLabel [1].text = value;}//atk.ToString();}
//	}
//
//	private string expNeed = "0";	// level label
//	public string ExpNeed{ 
//		set { 
//			expNeed = value;
//			infoLabel [3].text = value;}//expNeed.ToString();}
//	}
//
//
//	
//	void ShowData () {
//		dragPanel.SetData<UserUnit> (DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit (),MyUnitClickCallback as DataListener);
//
//		RefreshSortInfo ();
//		RefreshCounter ();
//	}
//
//
//
//	private void RefreshCounter(){
//		Dictionary<string, object> countArgs = new Dictionary<string, object>();
//		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
//		countArgs.Add("current", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
//		countArgs.Add("max", DataCenter.Instance.UserData.UserInfo.unitMax);
//		countArgs.Add("posy",-745);
//		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
//	}
//
//	void InitUI() {
////		dataCenter = DataCenter.Instance;
////		iTween.MoveTo(topRoot, iTween.Hash("y", 150, "time", 0.4f,"islocal", true));
////		iTween.MoveTo (bottomRoot, iTween.Hash ("x", 0, "time", 0.4f, "islocal", true, "oncomplete", "BottomRootMoveEnd", "oncompletetarget", gameObject)); 
//
//		topObject = FindChild("Top");
//		bottomObject = FindChild ("Middle");
//
//
//	}
//
//	void InitDragPanel() {
//
//		dragPanel = new DragPanel ("LevelUpDragPanel", "Prefabs/UI/UnitItem/MyUnitPrefab",typeof(LevelUpUnitItem), transform);//LevelUpUnitItem.Inject().gameObject, 12, 3);
//
//
//		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/RejectItem", o =>{
//			GameObject rejectItem = Instantiate( o) as GameObject;
//			dragPanel.AddItemToGrid (rejectItem,0);
//			rejectItem.transform.FindChild("Label_Text").GetComponent<UILabel>().text = TextCenter.GetText ("Text_Reject");
//			UIEventListenerCustom.Get(rejectItem).onClick = RejectCallback;
//		});
//	}
//
//	public void ResetUIAfterLevelUp(uint blendID) {
//		ClearData (false);
//		UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit (blendID);
//		selectedItem [baseItemIndex].SetData<UserUnit>(tuu);
//		DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ().Find (a => a.MakeUserUnitKey () == tuu.MakeUserUnitKey ()).isEnable = false;
//		ShowData ();
////		dragPanel.SetData<UserUnit> (tuu);
//		ShieldPartyAndFavorite (false, null);
//		CheckLevelUp ();
//	}
//
//	void SelectedFriendCallback(object data) {
//		LevelUpItem piv = data as LevelUpItem;
//		doNotClearData = true;
//			
//		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
//
//		ModuleManager.Instance.ShowModule (ModuleEnum.FriendSelectModule,"type","levelup");
//	}
//
//
//
//	
//	/// <summary>
//	/// Selecteds material item's callback.
//	/// </summary>
//	void SelectedItemCallback(object data) {
//
//	}
//
//	void SelectBaseItemCallback(object data){
//		LevelUpItem piv = data as LevelUpItem;
//		if (prevSelectedItem == null) {
//			if (prevMaterialItem == null) {
//				DisposeNoPreMaterial (piv);
//			} else {
//				DisposeByPreMaterial (piv);
//			}
//		}
//
//		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
//
//		RefreshMaterial ();
//		CheckLevelUp ();
//	}
//
//
//
//
//
//	/// <summary>
//	/// drag panel item click.
//	/// </summary>
//	void MyUnitClickCallback(object data) {
//		LevelUpUnitItem pui = data as LevelUpUnitItem;
////		Debug.LogError()
//		if (prevSelectedItem == null) {
//			if (SetBaseItemPreSelectItemNull (pui)) {
//				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
//				return;
//			}
//			
//			int index = SetMaterialItem (pui);
//
//			if (index > -1) {
//				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
//				RefreshMaterial();
//				return;	
//			}
//
//			if (prevMaterialItem != null) {
//				prevMaterialItem.IsFocus = false;
//			}
//
//			prevMaterialItem = pui;
//			prevMaterialItem.IsFocus = true;
//
//			CheckLevelUp ();
//		} else {
//			if (SetBaseItem (pui)) {
////				Debug.LogError("SetBaseItem");
//				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
//				return;	
//			}
////			Debug.LogError("else");
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
//
//			EnabledItem(prevSelectedItem.UserUnit);
//			prevSelectedItem.IsFocus = false;
//			prevSelectedItem.SetData<UserUnit>(pui.UserUnit);
//			pui.IsEnable = false;
//			RefreshMaterial();
//			CheckLevelUp ();
//			ClearFocus();
//		}
//	}
//
//	void RejectCallback(GameObject go) {
//
//	}
//
//	void ClearData(bool remainBase) {
////		if(friendWindow != null)
////			friendWindow.HideUI ();
//
//		for (int i=0; i<selectedItem.Length; i++) {
//			if(remainBase && i==0) {
//				continue;
//			}
//
//			selectedItem[i].SetData<UserUnit>(null);
//			selectedItem[i].IsEnable = true;
//		}
//		ClearInfoPanelData ();
//	}
//
//	void ClearInfoPanelData() {
//		Hp = "0";
//		Atk = "0";
//		ExpNeed = "0";
//		ExpGot = 0;
//		CoinNeed = 0;
//	}
//
//	//LevelUp Button ClickCallback
//
////	void SortCallback(GameObject go) {
////		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
////	}
//
//	bool SetBaseItemPreSelectItemNull(MyUnitItem pui) {
//		if (selectedItem [baseItemIndex].UserUnit != null ) { //index 0 is base item object.
//			if(selectedItem [baseItemIndex].UserUnit.MakeUserUnitKey() == pui.UserUnit.MakeUserUnitKey()) {
//				ShieldPartyAndFavorite (false, null);
//				return true;
//			} else{
//				return false;
//			}
//		}
//		
//		EnabledItem (selectedItem [baseItemIndex].UserUnit);
//		selectedItem [baseItemIndex].SetData<UserUnit>(pui.UserUnit);
//		UpdateBaseInfoView ();
//		if (CheckIsParty (pui)) {
//			selectedItem [baseItemIndex].IsEnable = true;
//		}
//		pui.IsEnable = false;
//		ClearFocus ();
//
//		ShieldPartyAndFavorite (false, null);
//		
//		CheckLevelUp ();
//		
//		return true;
//	}
//
//	bool SetBaseItem(MyUnitItem pui) {
//		if (selectedItem [baseItemIndex].UserUnit != null && !CheckBaseItem (prevSelectedItem)) { //index 0 is base item object.
//			return false;
//		}
//
//		EnabledItem (selectedItem [baseItemIndex].UserUnit);
//		selectedItem [baseItemIndex].SetData<UserUnit>(pui.UserUnit);
//		UpdateBaseInfoView ();
//		if (CheckIsParty (pui)) {
//			selectedItem [baseItemIndex].IsEnable = true;
//		}
//		pui.IsEnable = false;
//		ClearFocus ();
//		ShieldPartyAndFavorite (false, selectedItem [baseItemIndex]);
//
//		CheckLevelUp ();
//
//		return true;
//	}
//
//	void ClearFocus() {
//		if (prevSelectedItem != null) {
//			prevSelectedItem.IsFocus = false;
//			prevSelectedItem = null;	
//		}
//
//		if (prevMaterialItem != null) {
//			prevMaterialItem.IsFocus = false;
//			prevMaterialItem = null;
//		}
//	}
//	
//	void ShieldPartyAndFavorite(bool shield, MyUnitItem baseItem) {
//		dragPanel.ItemCallback ("shield_item", baseItem != null ? baseItem.UserUnit : null,shield);
//	}
//
//	int SetMaterialItem(MyUnitItem pui) {
//		for (int i = 1; i < 5; i++) {	// 1~4 is material item object.
//			if(selectedItem[i].UserUnit != null) {
//				continue;
//			}
//			selectedItem[i].SetData<UserUnit>(pui.UserUnit);
//			pui.IsEnable = false;
//			ClearFocus ();
//
//			CheckLevelUp ();
//
//			return i;
//		}
//
//		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_invalid);
//
//		return -1;	// -1 == not add to materail list.
//	}
//
//	bool CheckBaseItem(MyUnitItem piv) {
//		if (piv == null ) {
//			return false;
//		}
//		if (piv.UserUnit == null) {
//			return false;	
//		}
//		if (piv.UserUnit.MakeUserUnitKey() == selectedItem [baseItemIndex].UserUnit.MakeUserUnitKey() ) {
//			return true;
//		}
//
//		return false;
//	}
//
//	bool CheckIsParty(MyUnitItem piv) {
//		if (piv == null ) {
//			return false;
//		}
//		
//		if (DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(piv.UserUnit.uniqueId) > 0) {
//			return true;
//		}
//		
//		return false;
//	}
//
//	/// <summary>
//	/// one item is material item and the other is selected item.
//	/// </summary>
//	bool SwitchMaterailToSelected(MyUnitItem selectedItem, MyUnitItem materialItem) {
//		if (selectedItem == null || materialItem == null) {
//			return false;	
//		}
//
//		if (!CheckBaseItem (selectedItem) || !CheckIsParty (materialItem)) {
//			return false;
//		}
//
//		UserUnit tuu = selectedItem.UserUnit;
//		selectedItem.SetData<UserUnit>(materialItem.UserUnit);
//		materialItem.IsEnable = false;
//		EnabledItem (tuu);
//		return true;
//	}
////
//	void EnabledItem(UserUnit tuu) {
//		if (tuu == null) {
//			return;	
//		}
////		dragPanel.SetData<UserUnit> (DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit(),);
//		dragPanel.ItemCallback ("enable_item", tuu);
//	}
////
////	void FriendBack(object data) {
//////		if (friendWindow == null) {
//////			return;	
//////		}
//////
//////		friendWindow.Back (null);
////	}
//
//	private void ReceiveSortInfo(object msg){
//		_sortRule = (SortRule)msg;
//		SortUnitByCurRule();
////		dragPanel.RefreshItem (myUnit);
////		dragPanel.RefreshSortInfo (sortRule);
////		RefreshSortInfo ();
//	}
//
//	void RefreshSortInfo() {
////		foreach (var item in myUnitDragPanel.scrollItem) {
////			item.CurrentSortRule= sortRule;
////		}
//	}
//
//	private void SortUnitByCurRule(){
//		if (_sortRule == SortRule.None) {
//			return;	
//		}
//		SortUnitTool.SortByTargetRule(_sortRule, DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ());
//		SortUnitTool.StoreSortRule (_sortRule, SortRuleByUI.LevelUp);
//
//	}
//
//	bool CheckBaseIsNull() {
//		UserUnit baseInfo = selectedItem [baseItemIndex].UserUnit;
//		if (baseInfo == null) {
//			return true;	
//		}
//		return false;
//	}
//	
//
//
//
//
////	public LevelUpUnitItem GetPartyUnitItem(uint id){
////
////		foreach (var item in unitItems) {
////			if(item.UserUnit.unitId == id){
////				return item;
////			}
////		}
////		return null;
////	}
////
////	public void SetItemVisible(uint unitId){
////		foreach (var item in myUnit) {
////			if(item.unitId == unitId)
////			{
////				myUnit.Remove(item);
////				myUnit.Add(item);
////				myUnit.Reverse();
////				break;
////			}
////		}
////
//////		dragPanel.RefreshItem (myUnit);
////
////	}
//
//	public void RefreshUnitItem(UserUnit unit) {
//		List<UserUnit> myUnit = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ();
//		for (int i=0; i<myUnit.Count; i++) {
//			if (myUnit[i]!=null && myUnit[i].uniqueId == unit.uniqueId ) {
//				myUnit[i] = unit;
//				break;
//			}
//		}
//
////		dragPanel.RefreshItem(myUnit);
//		ShieldPartyAndFavorite (false, null);
//	}
//}
