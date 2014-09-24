using UnityEngine;
using System.Collections;

public class SellUnitItem : MyUnitItem {
	public static SellUnitItem Inject(GameObject item){
		SellUnitItem view = item.GetComponent<SellUnitItem>();
		if (view == null) view = item.AddComponent<SellUnitItem>();
		return view;
	}

	public delegate void UnitItemCallback(SellUnitItem puv);
	public UnitItemCallback callback;

	private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/SaleUnitPrefab";
				itemPrefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject ;
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
		IsParty = DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(userUnit.uniqueId);
//		Debug.LogError("uid:"+userUnit.ID + " => "+DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(userUnit.ID));
		RefreshEnable();
	}

	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
		RefreshEnable();
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}

	private void RefreshEnable() {
		IsEnable = !(IsParty || IsFavorite);
	}

	protected override void RefreshState(){
		base.RefreshState();
		if(userUnit != null){
			IsParty = DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(userUnit.uniqueId);
			//IsEnable is FALSE as long as one state(IsParty or IsFavorite or other...) is TRUE
			RefreshEnable();
//			if (IsFavorite) Debug.LogWarning(">>>>> userUnit.ID:"+userUnit.ID+" isFavor!");
		}
	}

	protected override void PressItem(GameObject item){
		base.PressItem(item);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFavState);
	}

}
