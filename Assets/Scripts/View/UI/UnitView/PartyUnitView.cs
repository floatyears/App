using UnityEngine;
using System.Collections;

public class PartyUnitView : MyUnitView {
	public static PartyUnitView Inject(GameObject item){
		PartyUnitView view = item.AddComponent<PartyUnitView>();
		if (view == null) view = item.AddComponent<PartyUnitView>();
		return view;
	}
	public delegate void UnitItemCallback(PartyUnitView puv);
	public UnitItemCallback callback;

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
		IsFocus = false;

		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			IsEnable = !IsParty;
		}

		//Debug.Log("isParty is : " + isParty );

	}

	/// <summary>
	/// Updates the state of the party.
	/// 1. update the visible of party sign Label
	/// 2. update the visible of mask
	/// </summary>
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
			IsEnable = !IsParty;
		}
	}

}
