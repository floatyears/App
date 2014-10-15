using UnityEngine;
using System.Collections;

public class LevelUpItem : MyUnitItem {

	public DataListener callback;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}
	
	protected override void InitUI(){
		base.InitUI();
		partyLabel.enabled = true;
		partyLabel.text = "";
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}
	
	protected override void RefreshState(){
//		Debug.LogError ("RefreshState");
		base.RefreshState();

		if(userUnit != null){
			IsParty = DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(userUnit.uniqueId);
			//IsEnable is FALSE as long as IsParty is TRUE
//			Debug.LogError("IsParty : " + IsParty);
//			IsEnable = !IsParty;
//			Debug.LogError("IsEnable : " + IsEnable);
		} else {
			IsParty = false;
		}
	}

	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
		//		IsEnable = !IsParty;
	}
}
