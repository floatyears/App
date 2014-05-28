using UnityEngine;
using System.Collections.Generic;

public class UnitDisplayUnity : UIComponentUnity {
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.UnitDisplayState, UnitDisplayState);
		MsgCenter.Instance.AddListener (CommandEnum.UnitDisplayBaseData, UnitDisplayBaseData);
		MsgCenter.Instance.AddListener (CommandEnum.UnitMaterialList, UnitMaterialList);
		MsgCenter.Instance.AddListener (CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitDisplayState, UnitDisplayState);
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitDisplayBaseData, UnitDisplayBaseData);
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitMaterialList, UnitMaterialList);
		MsgCenter.Instance.RemoveListener (CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void ResetUIState () {
		state = 1;
//		selectBase = null;
//		baseData.userUnitItem = null;

//		sortRule = SortRule.Attack;
//		ReceiveSortInfo (sortRule);
	}

	public override void CallbackView (object data) {
		Dictionary<string,object> dic = data as Dictionary<string,object>;
		foreach (var item in dic) {
			DisposeCallback(item);
		}
	}
	
	public const string SetDragPanel = "setDragPanel";
	public const string RefreshData = "refreshData";
	public const string SelectBase = "selectBase";
	public const string SelectMaterial = "selectMaterial";
	//==========================================interface end ==========================

	private GameObject unitItem;
	private DragPanel unitItemDragPanel;
	private List<TUserUnit> allData = new List<TUserUnit>();
	private List<UnitItemInfo> allItem = new List<UnitItemInfo>();
	private List<UnitItemInfo> partyItem = new List<UnitItemInfo>();
	private List<UnitItemInfo> normalItem = new List<UnitItemInfo> ();
	private List<UnitItemInfo> evolveItem = new List<UnitItemInfo> ();
	private TUserUnit selectBase  = null;
	private UnitItemInfo baseData = null;

	private UIButton sortButton;
	private UILabel sortLabel;
	private SortRule _sortRule;
	private SortRule sortRule {
		set { _sortRule = value; sortLabel.text = value.ToString(); }
		get { return _sortRule; }
	}

	List<UnitItemInfo> materialInfo = new List<UnitItemInfo> ();
	Dictionary<string, object> TranferData = new Dictionary<string, object> ();
	int state = 1;

	void ClickItem (GameObject go) {
//		Debug.LogError ("go : " + go);
		UnitItemInfo uii = evolveItem.Find (a => a.scrollItem == go);
		if (uii != default(UnitItemInfo) && state == 1) {
			selectBase = uii.userUnitItem;
			TranferData.Clear();
			TranferData.Add(SelectBase, selectBase);
			ExcuteCallback(TranferData);
			return;
		}
		uii = normalItem.Find (a => a.scrollItem == go);
		bool b = state != 1 && state != 5;
		if (uii != default(UnitItemInfo) && b) {
			if(CheckBaseNeedMaterial (uii.userUnitItem,state)) {
				TranferData.Clear();
				TranferData.Add(SelectMaterial, uii.userUnitItem);
				ExcuteCallback(TranferData);
			}
		}
	}

	void UnitDisplayBaseData (object data) {
		TUserUnit tuu = data as TUserUnit;
		if (tuu == null) {
			return;
		}
		if (baseData != null) {
			baseData.hightLight.enabled = false;	
		}
		baseData = evolveItem.Find (a => a.userUnitItem.ID == tuu.ID);
		baseData.hightLight.enabled = true;
		materialInfo.Clear ();
		int count = tuu.UnitInfo.evolveInfo.materialUnitId.Count;
		int value = 3 - count;
		for (int i = 0; i < count; i++) {
			uint id = tuu.UnitInfo.evolveInfo.materialUnitId[i];
			UnitItemInfo uii = normalItem.Find(a=>a.userUnitItem.UnitInfo.ID == id);
			if(uii != default(UnitItemInfo)) {
				materialInfo.Add(uii);
			} else{
				materialInfo.Add(null);
			}
		}
		for (int i = 0; i < value; i++) {
			materialInfo.Add(null);
		}
		List<TUserUnit> temp = new List<TUserUnit> ();
		for (int i = 0; i < materialInfo.Count; i++) {
			if(materialInfo[i] == null) {
				temp.Add(null);
			}else{
				temp.Add(materialInfo[i].userUnitItem);
			}
		}
		MsgCenter.Instance.Invoke (CommandEnum.selectUnitMaterial, temp);
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
		List<TUserUnit> material = data as List<TUserUnit>;
		materialInfo.Clear ();
		for (int i = 0; i < material.Count; i++) {
			if(material[i] == null) {
				materialInfo.Add(null);
			}
			else{
				UnitItemInfo uii = normalItem.Find(a=>a.userUnitItem.ID == material[i].ID) ;
				materialInfo.Add(uii);

			}
		}
		ShowMaterial();
	}

	void ShowEvolve () {
		for (int i = 0; i < allItem.Count; i++) {
			UnitItemInfo uii = allItem[i];
			if(evolveItem.Contains(uii)) {
				uii.SetMask(false);
			}
			else{
				uii.SetMask(true);
			}
		}

		baseData.hightLight.enabled = true;
		if (uiima != null) {
			uiima.hightLight.enabled = false;
		}
	}

	bool CheckMaterialInfoNull () {
		int index = state - 2;
		UnitItemInfo uii = materialInfo [index];
		if (uii == null) {
			return 	true;
		} else {
			return false;
		}
	}

	UnitItemInfo uiima;
	void ShowMaterial () {
		if (CheckMaterialInfoNull ()) {
			if(uiima != null) {
				uiima.hightLight.enabled = false;
			}
			for (int i = 0; i < allItem.Count; i++) {
				allItem[i].SetMask(true);
			}
			return;
		}

		int index = state - 2;
		UnitItemInfo temp = materialInfo [index];
		if (temp == null) {
			return;
		}

		if (uiima != null) {
			uiima.hightLight.enabled = false;
		}
		uiima = temp;
		List<UnitItemInfo> tempMaterial = new List<UnitItemInfo> ();
		uint id = uiima.userUnitItem.UnitID;
		tempMaterial = normalItem.FindAll(a=>a.userUnitItem.UnitID == id);
		if (tempMaterial.Count == 1) {
			tempMaterial.Clear();
		}
		for (int i = 0; i < allItem.Count; i++) {
			UnitItemInfo uii = allItem[i];
			if(tempMaterial.Contains(uii)) {
				uii.SetMask(false);
			}
			else{
				uii.SetMask(true);
			}
		}
		uiima.hightLight.enabled = true;
		baseData.hightLight.enabled = false;
	}
	
	void InitUI () {
		CreatPanel ();
		sortButton = FindChild<UIButton> ("sort_bar");
		UIEventListener.Get (sortButton.gameObject).onClick = SortButtoCallback;
		sortLabel = FindChild<UILabel>("sort_bar/SortLabel");
		sortRule = SortRule.HP;

	}

	void CreatPanel () {
		if (unitItem == null) {
			unitItem = Resources.Load("Prefabs/UI/Friend/UnitItem") as GameObject;
		}

		unitItemDragPanel = new DragPanel ("UnitDisplay", unitItem);

	
	}

	void SortButtoCallback(GameObject go) {
		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
	}

	private void ReceiveSortInfo(object msg){
		sortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(sortRule, allData);
		List<GameObject> scrollList = unitItemDragPanel.ScrollItem;
		for (int i = 1; i < scrollList.Count; i++){
			UnitItemInfo uii = scrollList[i].GetComponent<UnitItemInfo>();
			uii.userUnitItem = allData[i];
			RefreshView(uii);
		}
	}

	void DisposeCallback (KeyValuePair<string, object> info) {
		switch (info.Key) {
		case SetDragPanel:
			DragPanelSetInfo dpsi = info.Value as DragPanelSetInfo;
			if(dpsi != null && unitItemDragPanel != null) {
				unitItemDragPanel.CreatUI();
				unitItemDragPanel.DragPanelView.SetDragPanel(dpsi);
			}
			break;
		case RefreshData:
			List<TUserUnit> tuuList = info.Value as List<TUserUnit>;
			if (tuuList != null) {
				allData = tuuList;
				ShowUnitItem ();
			}
			break;
		default:
			break;
		}
	}

	void ShowUnitItem () {
		RefreshDragPanelView ();
		List<GameObject> scroll = unitItemDragPanel.ScrollItem;
		allItem.Clear ();
		for (int i = 0; i < allData.Count; i++) {
			GameObject scrollItem = scroll[i];
			TUserUnit tuu = allData[i];
			UnitItemInfo uii =  allItem.Find(a=>a.userUnitItem.ID == tuu.ID);

			if(uii == default(UnitItemInfo)) {
				uii = scrollItem.AddComponent<UnitItemInfo>();
				uii.scrollItem = scrollItem;
				uii.userUnitItem = tuu;
			}
			else{
				uii.scrollItem = scrollItem;
				uii.userUnitItem = tuu;
			}
			allItem.Add(uii);
			RefreshView(uii);
		}
//		RefreshItem ();
	}

	void RefreshView (UnitItemInfo uii) {
		uii.callback = ClickItem;
		bool b = uii.isPartyItem;
		if (b && !partyItem.Contains(uii)) {
			partyItem.Add(uii);		
		}

		if (uii.userUnitItem.IsFavorite == 0 && !b && !normalItem.Contains(uii))  {
			normalItem.Add(uii);
		}

		bbproto.EvolveInfo ei = uii.userUnitItem.UnitInfo.evolveInfo;
		if (ei != null && !evolveItem.Contains(uii)) {
			evolveItem.Add(uii);
		}
	}

	bool CheckBaseNeedMaterial (TUserUnit tuu, int index) {
		int tempIndex = index - 2;
		List<uint> temp = baseData.userUnitItem.UnitInfo.evolveInfo.materialUnitId;
		if (tempIndex < temp.Count) {
			uint materialNeed = temp [tempIndex];
			if (materialNeed == tuu.UnitID) {
				return true;	
			} else {
				return false;	
			}
		} 
		else {
			return false;	
		}
	}

	void RefreshDragPanelView () {
		List<GameObject> scroll = unitItemDragPanel.ScrollItem;
		int value = scroll.Count - allData.Count;
		if (value > 0) {
			int endValue = scroll.Count - value - 1;
			for (int i = scroll.Count - 1; i > endValue; i++) {
				unitItemDragPanel.RemoveItem (scroll [i]);
			}		
		}
		else if(value < 0) {
			int endValue = Mathf.Abs(value);
			unitItemDragPanel.AddItem(endValue);
		}
		unitItemDragPanel.Refresh ();
	}

	public GameObject GetUnitItem(int i){

		List<GameObject> a = unitItemDragPanel.ScrollItem;
		if (i == -1) {
		return a[a.Count - 1];
		} else {
			return a[i];
		}
		
	}
}
