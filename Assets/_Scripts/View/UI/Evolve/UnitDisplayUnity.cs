using UnityEngine;
using System.Collections.Generic;

public class UnitDisplayUnity : UIComponentUnity {
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		itemCounterEvolve.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.UnitDisplayState, UnitDisplayState);
		MsgCenter.Instance.AddListener (CommandEnum.UnitDisplayBaseData, UnitDisplayBaseData);
		MsgCenter.Instance.AddListener (CommandEnum.UnitMaterialList, UnitMaterialList);
		MsgCenter.Instance.AddListener (CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void HideUI () {
		base.HideUI ();
		itemCounterEvolve.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitDisplayState, UnitDisplayState);
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitDisplayBaseData, UnitDisplayBaseData);
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitMaterialList, UnitMaterialList);
		MsgCenter.Instance.RemoveListener (CommandEnum.SortByRule, ReceiveSortInfo);

		for (int i = 0; i < allData.Count; i++) {
			allData[i].isEnable = true;
		}
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		itemCounterEvolve.DestoryUI ();
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

	private ItemCounterEvolve itemCounterEvolve;

	private GameObject unitItem;

	private List<TUserUnit> allData = new List<TUserUnit>();
	private DragPanelDynamic unitItemDragPanel;
	private List<EvolveDragItem> evolveDragItem = new List<EvolveDragItem> ();
	private List<EvolveDragItem> normalDragItem = new List<EvolveDragItem> ();

	private TUserUnit selectBase = null;
	private TUserUnit baseData = null;
	
	private SortRule _sortRule;

	List<TUserUnit> materialInfo = new List<TUserUnit> ();
	Dictionary<string, object> TranferData = new Dictionary<string, object> ();
	int state = 1;

	void ClickItem (EvolveDragItem evolveItem) {
		if (evolveItem == null) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			return;	
		}

		if (evolveItem.CanEvolve && state == 1) {
			selectBase = evolveItem.UserUnit;
			TranferData.Clear();
			TranferData.Add(SelectBase, selectBase);
			ExcuteCallback(TranferData);

			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);

			return;
		}

		bool normalItem = !evolveItem.IsParty && !evolveItem.IsFavorite;
		bool b = state != 1 && state != 5;

		if (normalItem && b) {
			if (CheckBaseNeedMaterial (evolveItem.UserUnit, state)) {
				MsgCenter.Instance.Invoke (CommandEnum.selectUnitMaterial, evolveItem.UserUnit);
				AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
				return;
			}
		} 

		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_invalid);
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
		if ( CheckMaterialInfoNull () ) {
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
		RefreshView ();
	}
	
	void InitUI () {
		CreatPanel ();
		_sortRule = SortUnitTool.GetSortRule (SortRuleByUI.Evolve);//SortRule.HP;

		itemCounterEvolve = FindChild<ItemCounterEvolve> ("ItemCounterBar");
		itemCounterEvolve.Init ();
	}

	void CreatPanel () {
		GameObject go = Instantiate (EvolveDragItem.ItemPrefab) as GameObject;
		EvolveDragItem.Inject (go);
		unitItemDragPanel = new DragPanelDynamic (gameObject, go, 12, 3);

		DragPanelSetInfo dpsi = new DragPanelSetInfo ();
		dpsi.parentTrans = transform;
		dpsi.scrollerScale = Vector3.one;
		dpsi.position = -28 * Vector3.up;
		dpsi.clipRange = new Vector4 (0, -120, 640, 400);
		dpsi.gridArrange = UIGrid.Arrangement.Vertical;
		dpsi.maxPerLine = 3;
		dpsi.scrollBarPosition = new Vector3 (-320, -250, 0);
		dpsi.cellWidth = 100;
		dpsi.cellHeight = 100;
		dpsi.depth = 2;
	
		unitItemDragPanel.SetDragPanel (dpsi);
	}

	void SortButtoCallback(GameObject go) {
		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
	}

	private void ReceiveSortInfo(object msg){
		_sortRule = (SortRule)msg;
		SortUnitByCurRule();
		unitItemDragPanel.RefreshItem (allData);

		RefreshSortInfo ();
	}

	void RefreshSortInfo() {
		foreach (var item in unitItemDragPanel.scrollItem) {
			item.CurrentSortRule = _sortRule;
		}
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(_sortRule, allData);
		SortUnitTool.StoreSortRule (_sortRule, SortRuleByUI.Evolve);

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
		SortUnitByCurRule ();

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
			else {
				edi.IsEnable = false;
			}

			if(!edi.IsParty && !edi.IsFavorite) {
				normalDragItem.Add(edi);
			}
		}

		RefreshSortInfo ();
	}

	bool CheckCanEvolve(TUserUnit tuu) {
		return tuu.UnitInfo.evolveInfo != null ? true : false;
		
	}

	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		countArgs.Add ("posy",-295);
		itemCounterEvolve.UpdateView (countArgs);
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

	public GameObject GetMaxLvUnit(){
		foreach (var item in unitItemDragPanel.scrollItem) {
			if(item.UserUnit.Level >= item.UserUnit.UnitInfo.MaxLevel){
				return item.gameObject;
			}
		}
		return null;
	}
}
