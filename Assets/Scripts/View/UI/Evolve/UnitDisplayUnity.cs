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
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitDisplayState, UnitDisplayState);
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitDisplayBaseData, UnitDisplayBaseData);
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitMaterialList, UnitMaterialList);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
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

	List<UnitItemInfo> materialInfo = new List<UnitItemInfo> ();
	Dictionary<string, object> TranferData = new Dictionary<string, object> ();
	int state = 1;

	void ClickItem (GameObject go) {
		UnitItemInfo uii = evolveItem.Find (a => a.scrollItem == go);
//		Debug.LogError ("unit : " + state);
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
			}
			else{
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
	}

	void CreatPanel () {
		if (unitItem == null) {
			unitItem = Resources.Load("Prefabs/UI/Friend/UnitItem") as GameObject;
		}

		unitItemDragPanel = new DragPanel ("UnitDisplay", unitItem);
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
		allItem.Clear ();
		for (int i = 0; i < allData.Count; i++) {
			TUserUnit tuu = allData[i];
			UnitItemInfo uii = allItem.Find(a=>a.userUnitItem.ID == tuu.ID);
			if(uii == default(UnitItemInfo)) {
				uii = new UnitItemInfo();
				uii.userUnitItem = tuu;
			}
			else{
				uii.userUnitItem = tuu;
			}
			allItem.Add(uii);
		}
		RefreshItem ();
	}

	void RefreshItem () {
		RefreshDragPanelView ();
		List<GameObject> scroll = unitItemDragPanel.ScrollItem;
		for (int i = 0; i < allItem.Count; i++) {
			UnitItemInfo uii = allItem[i];
			uii.scrollItem = scroll[i];
			RefreshView(uii);
		}
	}

	void RefreshView (UnitItemInfo uii) {
		Transform go = uii.scrollItem.transform;
		UITexture tex = go.Find ("Texture_Avatar").GetComponent<UITexture> ();
		Texture2D texture = uii.userUnitItem.UnitInfo.GetAsset (UnitAssetType.Avatar);
		tex.mainTexture = texture;
		uii.stateLabel = go.Find ("Label_Party").GetComponent<UILabel> ();
		uii.mask = go.Find ("Mask").GetComponent<UISprite> ();
		uii.star = go.Find ("StarMark").GetComponent<UISprite> ();
		uii.hightLight = go.Find ("HighLight").GetComponent<UISprite> ();
		UIEventListener.Get (go.gameObject).onClick = ClickItem;
		uii.IsFavorate (uii.userUnitItem.IsFavorite);
		bool b = DataCenter.Instance.PartyInfo.UnitIsInParty (uii.userUnitItem.ID);
		uii.IsPartyItem(b);
		if (b) {
			partyItem.Add(uii);		
		}

		if (uii.userUnitItem.IsFavorite == 0 && !b) {
			normalItem.Add(uii);	
		}

		bbproto.EvolveInfo ei = uii.userUnitItem.UnitInfo.evolveInfo;
		if (ei == null) {
			uii.SetMask (true);	
		} else {
			uii.SetMask(false);
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
		int value = scroll.Count - allItem.Count;
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
}
