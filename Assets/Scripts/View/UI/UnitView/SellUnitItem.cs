using UnityEngine;
using System.Collections;

public class SellUnitItem : MyUnitItem {

	public static SellUnitItem Inject(GameObject item){
		SellUnitItem view = item.AddComponent<SellUnitItem>();
//		if (view == null) view = item.AddComponent<SaleUnitView>();
		return view;
	}

	public delegate void UnitItemCallback(SellUnitItem puv);
	public UnitItemCallback callback;

	private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				itemPrefab = Resources.Load("Prefabs/UI/UnitItem/SaleUnitPrefab") as GameObject ;
			}
			return itemPrefab;
		}
	}
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void InitUI(){
		base.InitUI();
	}

	protected override void InitState(){
		base.InitState();
		IsParty = DataCenter.Instance.PartyInfo.UnitIsInParty(userUnit.ID);
	}

	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
//		Debug.LogError("IsParty : " + IsParty + " gameobjecy: " + gameObject);
//		Debug.LogError("partyLabel.enabled : " + partyLabel.enabled + " gameobjecy: " + gameObject);
		IsEnable = !IsParty;
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}
	
	protected override void RefreshState(){
		base.RefreshState();
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			IsEnable = !IsParty;
		}
	}

}
