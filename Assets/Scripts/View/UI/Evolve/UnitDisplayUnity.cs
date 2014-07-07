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
		unitItemDragPanel.DestoryDragPanel ();
	}

	public override void ResetUIState () {
		state = 1;
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
	//========================================== interface end ==========================

	private GameObject unitItem;

	private List<TUserUnit> allData = new List<TUserUnit>();
	private DragPanelDynamic unitItemDragPanel;
	private List<EvolveDragItem> evolveDragItem = new List<EvolveDragItem> ();
	private List<EvolveDragItem> normalDragItem = new List<EvolveDragItem> ();

	private TUserUnit selectBase = null;
	private TUserUnit baseData = null;

//	private UIButton sortButton;
//	private UILabel sortLabel;
	private SortRule _sortRule;
	private SortRule sortRule {
		set { _sortRule = value; } //sortLabel.text = value.ToString(); }
		get { return _sortRule; }
	}

	List<TUserUnit> materialInfo = new List<TUserUnit> ();
	Dictionary<string, object> TranferData = new Dictionary<string, object> ();
	int state = 1;

	void ClickItem (EvolveDragItem evolveItem) {
		if (evolveItem == null) {
			return;	
		}
		if (evolveItem.CanEvolve && state == 1) {
			selectBase = evolveItem.UserUnit;
			TranferData.Clear();
			TranferData.Add(SelectBase, selectBase);
			ExcuteCallback(TranferData);
			return;
		}

		bool normalItem = !evolveItem.IsParty && !evolveItem.IsFavorite;
		bool b = state != 1 && state != 5;
		if (normalItem && b) {
			if (CheckBaseNeedMaterial (evolveItem.UserUnit, state)) {
				MsgCenter.Instance.Invoke (CommandEnum.selectUnitMaterial, evolveItem.UserUnit);
			}
		} 
	}

	void UnitDisplayBaseData (object data) {
		TUserUnit tuu = data as TUserUnit;
		if (tuu == null) {
			return;
		}
		if (baseData != null) {
			baseData.isFocus = false;	
			baseData.isEnable = true;
			unitItemDragPanel.RefreshItem(baseData);
//			Debug.LogError(baseData.isEnable);
		}
		baseData = allData.Find (a => a.MakeUserUnitKey () == tuu.MakeUserUnitKey ());

		baseData.isFocus = true;
		baseData.isEnable = false;
		unitItemDragPanel.RefreshItem(baseData);

		materialInfo.Clear ();
		int count = tuu.UnitInfo.evolveInfo.materialUnitId.Count;
		int value = 3 - count;

		for (int i = 0; i < count; i++) {
			uint id = tuu.UnitInfo.evolveInfo.materialUnitId[i];
			TUserUnit material = allData.Find(a=>a.UnitInfo.ID == id);
			materialInfo.Add(material);
		}

		for (int i = 0; i < value; i++) {
			materialInfo.Add(null);
		}

		List<TUserUnit> temp = new List<TUserUnit> (materialInfo);
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
		materialInfo = data as List<TUserUnit>;
//		materialInfo.Clear ();
//		materialInfo.AddRange (material);
		ShowMaterial();
	}

	void ShowEvolve () {
		for (int i = 0; i < allData.Count; i++) {
			if(CheckCanEvolve(allData[i])) {
				allData[i].isEnable = true;
			} else{
				allData[i].isEnable = false;
			}
		}


		baseData.isFocus = true;

		unitItemDragPanel.RefreshItem (baseData);
		if (uiima != null) {
			uiima.isFocus = false;
			uiima.isEnable = true;
		}
		RefreshView ();
//		unitItemDragPanel.RefreshItem (uiima);
	}

	bool CheckMaterialInfoNull () {
		int index = state - 2;
		TUserUnit uii = materialInfo [index];
		if (uii == null) {
			return 	true;
		} else {
			return false;
		}
	}

	TUserUnit uiima;
	void ShowMaterial () {
		if (CheckMaterialInfoNull ()) {
			if(uiima != null) {
				uiima.isFocus = false;
				unitItemDragPanel.RefreshItem(uiima);
			}

			for (int i = 0; i < allData.Count; i++) {
				allData[i].isEnable = false;
			}
			RefreshView ();
			return;
		}

		int index = state - 2;
		TUserUnit temp = materialInfo [index];
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

		uint id = temp.UnitInfo.ID;

		if(baseData == null)
			baseData.isEnable = false;

		for (int i = 0; i < allData.Count; i++) {
			if(allData[i].UnitInfo.ID != id) {
				allData[i].isEnable = false;
			} else{
				allData[i].isEnable = true;
			}
		}
//		for (int i = 0; i < allData.Count; i++) {
////			Debug.LogError("alldata : " + i + " alldata id : " + allData[i].UnitInfo.ID + " isenabel : " + allData[i].isEnable);
//		}
		RefreshView ();
	}
	
	void InitUI () {
		CreatPanel ();
//		sortButton = FindChild<UIButton> ("sort_bar");
//		UIEventListener.Get (sortButton.gameObject).onClick = SortButtoCallback;
//		sortLabel = FindChild<UILabel>("sort_bar/SortLabel");
		sortRule = SortRule.HP;
	}

	void CreatPanel () {
		GameObject go = Instantiate (EvolveDragItem.ItemPrefab) as GameObject;
		EvolveDragItem.Inject (go);
		unitItemDragPanel = new DragPanelDynamic (gameObject, go, 9, 3);

		DragPanelSetInfo dpsi = new DragPanelSetInfo ();
		dpsi.parentTrans = transform;
		dpsi.scrollerScale = Vector3.one;
		dpsi.position = -28 * Vector3.up;
		dpsi.clipRange = new Vector4 (0, -120, 640, 400);
		dpsi.gridArrange = UIGrid.Arrangement.Vertical;
		dpsi.maxPerLine = 3;
		dpsi.scrollBarPosition = new Vector3 (-320, -303, 0);
		dpsi.cellWidth = 100;
		dpsi.cellHeight = 100;
		dpsi.depth = 2;
	
		unitItemDragPanel.SetDragPanel (dpsi);
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
		unitItemDragPanel.RefreshItem (allData);
	}

	void DisposeCallback (KeyValuePair<string, object> info) {
		switch (info.Key) {
		case RefreshData:
			List<TUserUnit> tuuList = info.Value as List<TUserUnit>;
			allData.Clear();
			if (tuuList != null) {
				allData.AddRange(tuuList);
				RefreshView();
			}
			RefreshCounter ();
			break;
		default:
			break;
		}
	}

	void RefreshView() {
		List<MyUnitItem> myUnitItem = unitItemDragPanel.RefreshItem(allData);
		for (int i = evolveDragItem.Count - 1; i >= 0; i--) {
			evolveDragItem.RemoveAt(i);
		}
		foreach (var item in myUnitItem) {
			EvolveDragItem edi = item as EvolveDragItem;
			evolveDragItem.Add(edi);
			edi.callback = ClickItem;
			if(edi.UserUnit.UnitInfo.evolveInfo != null) {
				edi.CanEvolve = true;
			}
			if(!edi.IsParty && !edi.IsFavorite) {
				normalDragItem.Add(edi);
			}
		}
	}

	bool CheckCanEvolve(TUserUnit tuu) {
		return tuu.UnitInfo.evolveInfo != null ? true : false;
		
	}

	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	bool CheckBaseNeedMaterial (TUserUnit tuu, int index) {
		int tempIndex = index - 2;
		List<uint> temp = baseData.UnitInfo.evolveInfo.materialUnitId; 
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

	public GameObject GetUnitItem(int i){
		List<MyUnitItem> a = unitItemDragPanel.scrollItem;
		if (i == -1) {
			return a[a.Count - 1].gameObject;
		} else {
			return a[i].gameObject;
		}
	}
}
