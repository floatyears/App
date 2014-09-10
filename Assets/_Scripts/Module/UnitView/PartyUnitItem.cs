using UnityEngine;
using System.Collections;

public class PartyUnitItem : MyUnitItem {
	public static PartyUnitItem Inject(GameObject item){
		PartyUnitItem view = item.GetComponent<PartyUnitItem>();
		if (view == null) view = item.AddComponent<PartyUnitItem>();
		return view;
	}
	public delegate void UnitItemCallback(PartyUnitItem puv);
	public UnitItemCallback callback;

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void InitUI(){
		base.InitUI();

		gameObject.transform.FindChild ("Label_Party").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Party");
	}
	
	protected override void InitState(){
		base.InitState();
		IsFocus = false;

		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			IsEnable = !IsParty;
		}
	}

	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
		IsEnable = !IsParty;
	}

	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}

	protected override void RefreshState(){
		base.RefreshState();
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			//IsEnable is FALSE as long as IsParty is TRUE
			IsEnable = !IsParty;
		}
	}

}



public class LevelUpUnitItem : MyUnitItem {

	public static LevelUpUnitItem Inject(GameObject target = null){
		LevelUpUnitItem view;
		if (target == null) {
			target = GameObject.Instantiate (MyUnitItem.ItemPrefab) as GameObject;	
			view = target.AddComponent<LevelUpUnitItem>();
		} else {
			view = target.GetComponent<LevelUpUnitItem>();	
			if (view == null)
				view = target.AddComponent<LevelUpUnitItem>();
		}

		return view;
	}

	public delegate void UnitItemCallback(LevelUpUnitItem puv);
	public UnitItemCallback callback;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}
	
	protected override void InitUI(){
		base.InitUI();

		gameObject.transform.FindChild ("Label_Party").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Party");
	}
	
	protected override void InitState(){
		base.InitState();
		IsFocus = false;
		
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			IsEnable = !IsParty;
		}
	}
	
	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
//		Debug.LogError ("partyLabel.enabled : " + IsParty);
//		IsEnable = !IsParty;
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}
	
	protected override void RefreshState(){
		base.RefreshState();
		if(userUnit != null){
//			Debug.LogError("---   1 ------ RefreshState userUnit.ID : " + userUnit.ID + "isparty : " + isParty);
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
//			Debug.LogError("---   2 ------ RefreshState userUnit.ID : " + userUnit.ID + "isparty : " + isParty);
		}
	}

	protected override void PressItem(GameObject item){
		base.PressItem(item);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFavState);
	}
}
