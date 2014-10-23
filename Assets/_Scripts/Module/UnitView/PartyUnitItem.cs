using UnityEngine;
using System.Collections;
using bbproto;

public class PartyUnitItem : MyUnitItem {

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void InitUI(){
		base.InitUI();
		IsFocus = false;
		gameObject.transform.FindChild ("Label_Party").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Party");
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
			IsParty = DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(userUnit.uniqueId);
			//IsEnable is FALSE as long as IsParty is TRUE
			IsEnable = !IsParty;
		}
	}

	public override void SetData<T> (T data, params object[] args)
	{
		base.SetData (data, args);
		if (userUnit.unitId == 86) {
			gameObject.tag = "unitId_86";	
		}
	}

	public override void ItemCallback(params object[] args){
		switch (args [0].ToString ()) {
		case "out_party":
			if(userUnit.Equals(args[1] as UserUnit)) {
				IsParty = false;
			}
			break;
		case "reject_item":
			if(userUnit.Equals(args[1] as UserUnit)){
				IsParty = false;
				IsEnable = true;
			}
			break;
		}
	}
	
}



public class LevelUpUnitItem : MyUnitItem {

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}
	
	protected override void InitUI(){
		base.InitUI();

		IsFocus = false;
		gameObject.transform.FindChild ("Label_Party").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Party");
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
			IsParty = DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(userUnit.uniqueId);
		}
	}

	public override void ItemCallback (params object[] args)
	{
		switch (args [0].ToString ()) {
		case "enable_item":
			if (userUnit.TUserUnitID == (args[1]	as UserUnit).TUserUnitID) {
				if(isParty) {
					PartyLabel.text = TextCenter.GetText("Text_Party");
				}
				else{
					PartyLabel.text = "";
				}
				IsEnable = true;
			}
			break;
		case "shield_item":
			if(isParty || IsFavorite) {
				
				if(args[1] != null && userUnit.uniqueId == (args[1] as UserUnit).uniqueId) {
					return;
				}
				IsEnable = (bool)args[2];
			}
			break;
		case "auto_select":
			if(userUnit == (UserUnit)args[1])
			{
				callback(this);
			}
			break;
		}
	}


	void Update(){
		if(gameObject.layer == GameLayer.NoviceGuide){
			Debug.Log("novice guide layer");
		}
	}
}
