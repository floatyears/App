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

	private List<TUserUnit> allData = new List<TUserUnit>();
	private DragPanelDynamic unitItemDragPanel;
	private List<EvolveDragItem> evolveDragItem = new List<EvolveDragItem> ();
	private List<EvolveDragItem> normalDragItem = new List<EvolveDragItem> ();

	private TUserUnit selectBase  = null;
	private MyUnitItem baseData = null;

	private UIButton sortButton;
	private UILabel sortLabel;
	private SortRule _sortRule;
	private SortRule sortRule {
		set { _sortRule = value; sortLabel.text = value.ToString(); }
		get { return _sortRule; }
	}

	List<MyUnitItem> materialInfo = new List<MyUnitItem> ();
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

		if (!normalItem && b) {
			if(CheckBaseNeedMaterial (evolveItem.UserUnit,state)) {
				TranferData.Clear();
				TranferData.Add(SelectMaterial, evolveItem.UserUnit);
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
			baseData.IsFocus = false;	
		}
		baseData = evolveDragItem.Find (a => a.UserUnit.MakeUserUnitKey () == tuu.MakeUserUnitKey ());
		baseData.IsFocus = true;
		materialInfo.Clear ();
		int count = tuu.UnitInfo.evolveInfo.materialUnitId.Count;
		int value = 3 - count;
		for (int i = 0; i < count; i++) {
			uint id = tuu.UnitInfo.evolveInfo.materialUnitId[i];
			MyUnitItem uii = normalDragItem.Find(a=>a.UserUnit.UnitInfo.ID == id);
//			Debug.LogError("material : " + id + " uii : " + uii + " normaldragitem : " + normalDragItem.Count + " baseData :" + baseData.UserUnit.UnitInfo.ID);
			if(uii != default(MyUnitItem)) {
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
				temp.Add(materialInfo[i].UserUnit);
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
			else {
				MyUnitItem uii = normalDragItem.Find(a=>a.UserUnit.UnitInfo.ID == material[i].ID);
				materialInfo.Add(uii);
			}
		}
		ShowMaterial();
	}

	void ShowEvolve () {
		for (int i = 0; i < evolveDragItem.Count; i++) {
			MyUnitItem uii = evolveDragItem[i];
			EvolveDragItem edi = uii as EvolveDragItem;
			if(edi.CanEvolve) {
				edi.IsEnable = true;
			} else{
				edi.IsEnable = false;
			}
		}

		baseData.IsFocus = true;

		if (uiima != null) {
			uiima.IsFocus = false;
		}
	}

	bool CheckMaterialInfoNull () {
		int index = state - 2;
		MyUnitItem uii = materialInfo [index];
		if (uii == null) {
			return 	true;
		} else {
			return false;
		}
	}

	MyUnitItem uiima;
	void ShowMaterial () {
		if (CheckMaterialInfoNull ()) {
			if(uiima != null) {
				uiima.IsFocus = false;
			}

			for (int i = 0; i < evolveDragItem.Count; i++) {
				evolveDragItem[i].IsEnable = false;
			}
			return;
		}

		int index = state - 2;
		MyUnitItem temp = materialInfo [index];
		if (temp == null) {
			return;
		}

		if (uiima != null) {
			uiima.IsFocus = false;
		}
		uiima = temp;
		List<EvolveDragItem> enableMaterial = new List<EvolveDragItem> ();
		uint id = uiima.UserUnit.UnitInfo.ID;
		enableMaterial = normalDragItem.FindAll(a=>a.UserUnit.UnitInfo.ID == id);
		if (enableMaterial.Count == 1) {
			enableMaterial.Clear();
			return;
		}
		for (int i = 0; i < evolveDragItem.Count; i++) {
			EvolveDragItem uii = evolveDragItem[i];
			if(enableMaterial.Contains(uii)) {
				uii.IsEnable = false;
			}
			else{
				uii.IsEnable = true;
			}
		}
		uiima.IsEnable = true;
		baseData.IsEnable = false;
	}
	
	void InitUI () {
		CreatPanel ();
		sortButton = FindChild<UIButton> ("sort_bar");
		UIEventListener.Get (sortButton.gameObject).onClick = SortButtoCallback;
		sortLabel = FindChild<UILabel>("sort_bar/SortLabel");
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
//		evolveDragItem.Clear ();
//		foreach (var item in myUnitItem) {
//			EvolveDragItem edi = item as EvolveDragItem;
//			evolveDragItem.Add(edi);
//		}
	}

	void DisposeCallback (KeyValuePair<string, object> info) {
		switch (info.Key) {
		case RefreshData:
			List<TUserUnit> tuuList = info.Value as List<TUserUnit>;
			if (tuuList != null) {
				allData = tuuList;
				List<MyUnitItem> myUnitItem = unitItemDragPanel.RefreshItem(allData);
				evolveDragItem.Clear();
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
			break;
		default:
			break;
		}
	}

	bool CheckBaseNeedMaterial (TUserUnit tuu, int index) {
		int tempIndex = index - 2;
		List<uint> temp = baseData.UserUnit.UnitInfo.evolveInfo.materialUnitId;
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
