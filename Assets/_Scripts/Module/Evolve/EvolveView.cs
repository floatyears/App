using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class EvolveView : ViewBase {

	private const string hp = "HP";
	private const string type = "Type";
	private const string atk = "ATK";
	private const string race = "Race";
	private const string lv = "Lv";
	private const string coins = "Coins";
	private string rootPath = "Window";
	private Dictionary<string,UILabel> showInfoLabel = new Dictionary<string, UILabel>();
	/// <summary>
	/// 1: base. 2, 3, 4: material. 5: friend
	/// </summary>
	private Dictionary<GameObject,EvolveItem> evolveItem = new Dictionary<GameObject, EvolveItem> ();
	private Dictionary<int,EvolveItem> materialItem = new Dictionary<int, EvolveItem> ();
	private UIButton evolveButton;
	private EvolveItem baseItem;
	private EvolveItem friendItem;
	private FriendInfo friendInfo;
	private EvolveItem prevItem = null;
	private List<UserUnit> materialUnit = new List<UserUnit>();
	private int ClickIndex = 0;
	//	private FriendSelectLevelUpView friendWindow;
	private bool fromUnitDetail = false;
	
	private GameObject topObject;
	
	private GameObject unitItem;	
	private List<UserUnit> allData = new List<UserUnit>();
	private DragPanel unitItemDragPanel;
	
	private UserUnit selectBase = null;
	private UserUnit baseData = null;
	private SortRule _sortRule;
	
	private DragPanelConfigItem dragConfig;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config, data);
		InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitSortModule,"from","evolve");
		ModuleManager.Instance.ShowModule (ModuleEnum.ItemCounterModule,"from","evolve");

		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.UNITS);

		if (viewData != null && viewData.ContainsKey ("friendinfo")) {
			SelectFriend(viewData["friendinfo"] as FriendInfo);	
		} else {
			ClearAllItems();
		}

		MsgCenter.Instance.AddListener (CommandEnum.SortByRule, ReceiveSortInfo);

		ReadData ();
	}


	public override void HideUI () {
		base.HideUI ();
		ModuleManager.Instance.HideModule (ModuleEnum.UnitSortModule);
		ModuleManager.Instance.HideModule (ModuleEnum.ItemCounterModule);
//		MsgCenter.Instance.RemoveListener (CommandEnum.FriendBack, FriendBack);

		MsgCenter.Instance.RemoveListener (CommandEnum.SortByRule, ReceiveSortInfo);
		
		for (int i = 0; i < allData.Count; i++) {
			allData[i].isEnable = true;
		}

		if(viewData != null && viewData.ContainsKey("friendinfo")){
			viewData.Remove("friendinfo");
		}

	}

	void ReadData() {
		List<UserUnit> tuuList = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ();
		allData.Clear();
		if (tuuList != null) {
			allData.AddRange(tuuList);
			RefreshView();
		}
		RefreshCounter ();
	}

	private void ClearAllItems() {
		ClearMaterial();
		baseItem.Refresh(null);
		friendItem.Refresh(null);
	}

	public override void DestoryUI () {
		base.DestoryUI ();

		unitItemDragPanel.DestoryUI ();
	}
	//==========================================interface end ==========================
	////////////////////////////////////////////////////////////////////////////////////////////////
	


	//	private GameObject bottomObject;
	
	List<UserUnit> materialInfo = new List<UserUnit> ();

	public const string SetDragPanel = "setDragPanel";
	public const string RefreshData = "refreshData";
	public const string SelectBase = "selectBase";
	public const string SelectMaterial = "selectMaterial";

	void CheckCanEvolve () {
		bool haveBase = baseItem.userUnit != null; 
		bool haveFriend = friendItem.userUnit != null;
		bool haveMaterial = true;

		foreach (var item in materialItem.Values) {
			if(item.userUnit == null){
				continue;
			}
			if(!item.HaveUserUnit) {
				haveMaterial = false;
				break;
			}
		}

		if (haveBase && haveFriend && haveMaterial) {
			evolveButton.isEnabled = true;
		} else {
			evolveButton.isEnabled = false;
		}
	}

	void selectUnitMaterial(object data) {
		if (data == null) {
			return;	
		}
		List<UserUnit> hasMaterial = data as List<UserUnit>;
		if (hasMaterial == null) {
			UserUnit hasUnit = data as UserUnit;
//			Debug.LogError("selectUnitMaterial hasunit");
			materialItem[state].Refresh(hasUnit);
			List<UserUnit> materialList = new List<UserUnit>();
			for (int i = 2; i < 5; i++) {
				materialList.Add(materialItem[i].userUnit);
			}
			UnitMaterialList(materialList);
			return;
		}

		DisposeMaterial (hasMaterial);
	}

	void DisposeMaterial (List<UserUnit> itemInfo) {
		if (itemInfo == null || baseItem == null) {
			return;	
		}
		List<uint> evolveNeedUnit = new List<uint> (baseItem.userUnit.UnitInfo.evolveInfo.materialUnitId);

		for (int i = 0; i < evolveNeedUnit.Count ; i++) {
			UserUnit material = null;
			uint ID = evolveNeedUnit[i];
			bool isHave = true;

			for (int j = 0; j < itemInfo.Count; j++) {
				if(itemInfo[j] != null && itemInfo[j].UnitInfo.id == ID) {
					material = itemInfo[j];
					itemInfo.Remove(material);
					break;
				}
			}

			if(material == null) {
				bbproto.UserUnit uu = new bbproto.UserUnit();
				uu.unitId = ID;
				uu.userID = DataCenter.Instance.UserData.UserInfo.userId;
				material = uu;
				isHave = false;
			}
			materialItem[i + 2].Refresh(material, isHave);
		}
		CheckCanEvolve ();
	}

	void DisposeSelectData (UserUnit tuu) {
		if(tuu == null ) {
			return;
		}

		if (baseItem.userUnit != null && baseItem.userUnit.uniqueId == tuu.uniqueId) {
			return;	
		}
	
		if (state == 1 && tuu.UnitInfo.evolveInfo != null) {
			ClearMaterial();
			baseItem.Refresh(tuu);
			ShowEvolveInfo(tuu);
			UnitDisplayBaseData(tuu);
			CheckCanEvolve();
		}
	}

	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserData.UserInfo.unitMax);
		countArgs.Add ("posy",-725);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void ShowEvolveInfo (UserUnit tuu) {
		uint evolveUnitID = tuu.UnitInfo.evolveInfo.evolveUnitId;
		UnitInfo tui = DataCenter.Instance.UnitData.GetUnitInfo (evolveUnitID);

		showInfoLabel [hp].text = tuu.Hp + " -> [AA0000]" + tuu.CalculateHP (tui) + "[-]";
		showInfoLabel [atk].text = tuu.Attack + " -> [AA0000]" + tuu.CalculateATK (tui) + "[-]";
		showInfoLabel [lv].text = tuu.UnitInfo.maxLevel + " -> [AA0000]" + (tui == null ? "Unknown": tui.maxLevel.ToString()) + "[-]";
		showInfoLabel [type].text = tui.UnitTypeText;
		showInfoLabel [race].text = tui.UnitRace;//GetRaceText(tui.Race);
		showInfoLabel [coins].text = (tui.maxLevel * 500).ToString ();
	}

	private string GetRaceText(EUnitRace race){
		switch (race) {
		case EUnitRace.HUMAN:
			return TextCenter.GetText("RACE_Human");
		case EUnitRace.DRAGON:
			return TextCenter.GetText("RACE_Dragon");
		case EUnitRace.EVOLVEPARTS:
			return TextCenter.GetText("RACE_Evolveparts");
		case EUnitRace.BEAST:
			return TextCenter.GetText("RACE_Beast");
		case EUnitRace.LEGEND:
			return TextCenter.GetText("RACE_Legend");
		case EUnitRace.MONSTER:
			return TextCenter.GetText("RACE_Monster");
		case EUnitRace.MYTHIC:
			return TextCenter.GetText("RACE_Mythic");
		case EUnitRace.SCREAMCHEESE:
			return TextCenter.GetText("RACE_Screamcheese");
		case EUnitRace.UNDEAD:
			return TextCenter.GetText("RACE_Undead");
		default:
				break;
		}
		return "";
	}

	void ClearMaterial () {
		int index = 0;
		foreach (var item in evolveItem.Values) {
			if(index > 0 && index < 4)
				item.Refresh(null);
			index++;
		}
		materialUnit.Clear();
	}
 
	void LongPress (GameObject go) {
		ModuleManager.Instance.ShowModule(ModuleEnum.UnitDetailModule ,"unit",evolveItem [go].userUnit);
	}

	int state = 0;
	void ClickItem (GameObject go) {
		if ( baseItem.userUnit == null) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("base_Item_Null"));
			return;
		}

		ClickIndex = System.Int32.Parse (go.name);

		if (state != 1 && state == ClickIndex) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			return;
		}

		state = ClickIndex;

		if (state == 5) {
			ShieldEvolveButton (true);
		}

		CheckCanEvolve();

		HighLight (go);

		UnitDisplayState (state);

		if (state == 5) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//			EnterFriend();	
			ModuleManager.Instance.ShowModule(ModuleEnum.FriendSelectModule,"type","evolve","item",baseItem);
		} else {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
		}
	}

	void HighLight(GameObject go) {
		if (prevItem != null) {
			prevItem.highLight.enabled = false;	
		}

		if (go == null) {
			return;	
		}

		EvolveItem ei = evolveItem [go];
		ei.highLight.enabled = true;
		prevItem = ei;
	}

	void InitUI () {
		string path = rootPath + "/title/";
		
		topObject = FindChild (rootPath);
		
		for (int i = 1; i < 6; i++) {
			GameObject go = FindChild(path + i);
			UIEventListenerCustom ui = UIEventListenerCustom.Get(go);
			ui.LongPress = LongPress;
			ui.onClick = ClickItem;
			
			EvolveItem ei = new EvolveItem(i, go);
			evolveItem.Add(ei.itemObject, ei);
			if(i == 1 ) {
				baseItem = ei;
				ei.highLight.enabled = true;
				state = 1;
				prevItem = ei;
				continue;
			}
			else if (i == 5) {
				friendItem = ei;
				continue;
			}
			else{
				materialItem.Add(i,ei);
			}
			
			//			ei.haveLabel = go.transform.Find("HaveLabel").GetComponent<UILabel>();
			//			ei.maskSprite = go.transform.Find("Mask").GetComponent<UISprite>();
		}
		string path1 = rootPath + "/info_panel/";
		string suffixPath = "/Info";
		UILabel temp = transform.Find(path1 + hp + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (hp, temp);
		
		temp = transform.Find(path1 + atk + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (atk, temp);
		
		temp = transform.Find(path1 + lv + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (lv, temp);
		
		temp = transform.Find(path1 + type + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (type, temp);
		
		temp = transform.Find(path1 + race + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (race, temp);
		
		temp = transform.Find(path1 + coins + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (coins, temp);
		
		evolveButton = FindChild<UIButton> ("Evolve");
		FindChild ("Evolve/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_Evolve");
		ShieldEvolveButton (false);
		
		FindChild<UILabel> ("Window/info_panel/HP/label").text = TextCenter.GetText ("Text_HP");
		FindChild<UILabel> ("Window/info_panel/ATK/label").text = TextCenter.GetText ("Text_ATK");
		FindChild<UILabel> ("Window/info_panel/Lv/label").text = TextCenter.GetText ("Text_Level");
		FindChild<UILabel> ("Window/info_panel/Type/label").text = TextCenter.GetText ("Text_TYPE");
		FindChild<UILabel> ("Window/info_panel/Race/label").text = TextCenter.GetText ("Text_RACE");
		FindChild<UILabel> ("Window/info_panel/Coins/label").text = TextCenter.GetText ("Text_Coins");
		FindChild<UILabel> ("Window/title/1/Sprite_Base").text = TextCenter.GetText ("Text_BASE");
		FindChild<UILabel> ("Window/title/4/Text").text = TextCenter.GetText ("Text_Material");
		FindChild<UILabel> ("Window/title/5/Text").text = TextCenter.GetText ("Text_Friend");
		
		
		UIEventListenerCustom.Get (evolveButton.gameObject).onClick = Evolve;

		unitItemDragPanel = new DragPanel ("EvolveDragPanel","Prefabs/UI/UnitItem/MyUnitPrefab",typeof(EvolveDragItem), transform);
		_sortRule = SortUnitTool.GetSortRule (SortRuleByUI.Evolve);//SortRule.HP;

	}

	void SelectFriend(FriendInfo friendInfo) {
//		state = 1;
		foreach (var item in evolveItem) {
			ClickItem(item.Key);
			break;
		}

		if (friendInfo == null) {
			return;	
		}

		this.friendInfo = friendInfo;
		friendItem.Refresh (friendInfo.UserUnit);
		CheckCanEvolve ();
	}
	
	void Evolve(GameObject go) {
		UserUnit baseUserUnit = baseItem.userUnit;
		if (baseUserUnit.level < baseUserUnit.UnitInfo.maxLevel) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("notmaxleveltips"));
			return;
		}

		TipsManager.Instance.ShowMsgWindow( TextCenter.GetText("DownloadResourceTipTile"),TextCenter.GetText("DownloadResourceTipContent"),TextCenter.GetText("OK"),o=>{
			MsgCenter.Instance.AddListener(CommandEnum.ResourceDownloadComplete,o1 =>{
				List<ProtoBuf.IExtensible> evolveInfoList = new List<ProtoBuf.IExtensible> ();
				evolveInfoList.Add (baseItem.userUnit);
				evolveInfoList.Add (friendInfo);
				foreach (var item in materialItem.Values) {
					UserUnit tuu = item.userUnit;
					if(tuu != null) {
						evolveInfoList.Add(tuu);
					}
				}
				//				ExcuteCallback (evolveInfoList);
			});
			ModuleManager.Instance.ShowModule(ModuleEnum.ResourceDownloadModule);
		});
		return;


	}

	void ShieldEvolveButton (bool b) {
		evolveButton.isEnabled = b;
	}


	////////////////////////////////////////////////////////////////////////////
	/// 
	void UnitDisplayBaseData (object data) {
		UserUnit tuu = data as UserUnit;
		if (tuu == null) {
			return;
		}
		
		if (baseData != null) {
			baseData.isFocus = false;	
			baseData.isEnable = true;
//			unitItemDragPanel.RefreshItem(baseData);
		}
		
		baseData = allData.Find (a => a.MakeUserUnitKey () == tuu.MakeUserUnitKey ());
		
		baseData.isFocus = true;
		baseData.isEnable = false;
//		unitItemDragPanel.RefreshItem(baseData);
		
		materialInfo.Clear ();
		int count = tuu.UnitInfo.evolveInfo.materialUnitId.Count;
		int value = 3 - count;
		
		for (int i = 0; i < count; i++) {
			uint id = tuu.UnitInfo.evolveInfo.materialUnitId[i];
			UserUnit material = allData.Find(a=>a.UnitInfo.id == id);
			materialInfo.Add(material);
		}
		
		for (int i = 0; i < value; i++) {
			materialInfo.Add(null);
		}
		
		List<UserUnit> temp = new List<UserUnit> (materialInfo);
		selectUnitMaterial( temp);
	}

	void UnitDisplayState (object data) {
		state = (int)data;
		if (state != 5) {
			if (!gameObject.activeSelf) {
				gameObject.SetActive (true);
			}	
		} else {
			if(gameObject.activeSelf){
				gameObject.SetActive(false);
			}
		}
		
		switch (state) {
		case 1:
			ShowEvolve();
			break;
		case 2:
		case 3:
		case 4:
			ShowMaterial();
			break;
		case 5:
			break;
		default:
			break;
		}
	}

	void UnitMaterialList(object data) {
		materialInfo = data as List<UserUnit>;
		ShowMaterial();
	}

	private void ReceiveSortInfo(object msg){
		_sortRule = (SortRule)msg;
		SortUnitByCurRule();
		unitItemDragPanel.SetData<UserUnit> (allData,null,_sortRule);

	}
	
	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(_sortRule, allData);
		SortUnitTool.StoreSortRule (_sortRule, SortRuleByUI.Evolve);
		
	}

	void ShowEvolve () {
		for (int i = 0; i < allData.Count; i++) {
			if( allData[i].UnitInfo.evolveInfo != null ) {
				allData[i].isEnable = true;
			} else{
				allData[i].isEnable = false;
			}
		}
		
		
		baseData.isFocus = true;
		
//		unitItemDragPanel.RefreshItem (baseData);
		if (uiima != null) {
			uiima.isFocus = false;
			uiima.isEnable = true;
		}
		RefreshView ();
	}
	
	bool CheckMaterialInfoNull () {
		int index = state - 2;
		UserUnit uii = materialInfo [index];
		if (uii == null) {
			return 	true;
		} else {
			return false;
		}
	}
	
	UserUnit uiima;
	
	void ShowMaterial () {
		if ( CheckMaterialInfoNull () ) {
			if(uiima != null) {
				uiima.isFocus = false;
//				unitItemDragPanel.RefreshItem(uiima);
			}
			
			for (int i = 0; i < allData.Count; i++) {
				allData[i].isEnable = false;
			}
			RefreshView ();
			return;
		}
		
		int index = state - 2;
		UserUnit temp = materialInfo [index];
		if (temp == null) {
			return;
		}
		
		if (uiima != null) {
			uiima.isFocus = false;
		}
		
		uiima = allData.Find (a => a.MakeUserUnitKey () == temp.MakeUserUnitKey ());
		if (uiima != null) {
			uiima.isFocus = true;
		}
		
		uint id = temp.UnitInfo.id;
		
		if(baseData == null)
			baseData.isEnable = false;
		
		for (int i = 0; i < allData.Count; i++) {
			if(allData[i].UnitInfo.id != id) {
				allData[i].isEnable = false;
			} else{
				allData[i].isEnable = true;
			}
		}
		RefreshView ();
	}

	void RefreshView() {
		SortUnitByCurRule ();

		unitItemDragPanel.SetData<UserUnit>(allData,ClickDragItem as DataListener,_sortRule);

	}

	void ClickDragItem (object data) {
		EvolveDragItem evolveItem = data as EvolveDragItem;
		if (evolveItem == null) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			return;	
		}
		
		if (evolveItem.CanEvolve && state == 1) {
			selectBase = evolveItem.UserUnit;
			DisposeSelectData( selectBase);
			
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
			
			return;
		}
		
		bool normalItem = !evolveItem.IsParty && !evolveItem.IsFavorite;
		bool b = state != 1 && state != 5;
		
		if (normalItem && b) {
			if (CheckBaseNeedMaterial (evolveItem.UserUnit, state)) {
				selectUnitMaterial(evolveItem.UserUnit);
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
				return;
			}
		} 
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_invalid);
	}

	bool CheckBaseNeedMaterial (UserUnit tuu, int index) {
		int tempIndex = index - 2;
		List<uint> temp = baseData.UnitInfo.evolveInfo.materialUnitId; 
		if (tempIndex < temp.Count) {
			
			if (temp [tempIndex] == tuu.unitId) {
				return true;	
			} else {
				return false;	
			}
		}
		else {
			return false;	
		}
	}
//
//
//	public GameObject GetUnitItem(int i){
//		List<MyUnitItem> a = unitItemDragPanel.scrollItem;
//		if (i == -1) {
//			return a[a.Count - 1].gameObject;
//		} else {
//			return a[i].gameObject;
//		}
//	}
//	
//	public uint GetMaxLvUnitID(){
//		foreach (var item in unitItemDragPanel.scrollItem) {
//			if(item.UserUnit.level >= item.UserUnit.UnitInfo.maxLevel){
//				return item.UserUnit.unitId;
//			}
//		}
//		return 0;
//	}
//	public GameObject GetMaxLvUnitItem(){
//		foreach (var item in unitItemDragPanel.scrollItem) {
//			if(item.UserUnit.level >= item.UserUnit.UnitInfo.maxLevel){
//				return item.gameObject;
//			}
//		}
//		return null;
//	}
	
	public void SetItemVisible(uint unitId){
		foreach (var item in allData) {
			if(item.unitId == unitId)
			{
				allData.Remove(item);
				allData.Add(item);
				allData.Reverse();
				break;
			}
		}
		unitItemDragPanel.SetData<UserUnit> (allData);
	}
}

