using UnityEngine;
using System.Collections.Generic;

public class UnitDisplayUnity : UIComponentUnity {
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void Callback (object data) {
		Dictionary<string,object> dic = data as Dictionary<string,object>;
		foreach (var item in dic) {
			DisposeCallback(item);
		}

	}


	public const string SetDragPanel = "setDragPanel";
	public const string RefreshData = "refreshData";
	//==========================================interface end ==========================

	private GameObject unitItem;
	private DragPanel unitItemDragPanel;
	private List<TUserUnit> allData = new List<TUserUnit>();
	private List<UnitItemInfo> allItem = new List<UnitItemInfo>();
	private List<UnitItemInfo> partyItem = new List<UnitItemInfo>();

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
		UIEventListener.Get (go.gameObject).onClick = ClickItem;
		uii.IsFavorate (uii.userUnitItem.isFavorate);
		bool b = DataCenter.Instance.PartyInfo.UnitIsInParty (uii.userUnitItem.ID);
		uii.IsPartyItem(b);
	}

	void ClickItem (GameObject go) {

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
