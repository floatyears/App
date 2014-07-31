using UnityEngine;
using System.Collections;

public class LevelUpItem : MyUnitItem {
	public static LevelUpItem Inject(GameObject item){
		LevelUpItem view = item.GetComponent<LevelUpItem>();
		if (view == null) view = item.AddComponent<LevelUpItem>();
		return view;
	}
	public delegate void LevelUpItemCallback(LevelUpItem puv);
	public LevelUpItemCallback callback;
	
	protected override void ClickItem(GameObject item){
		Debug.LogError ("ClickItem :  " + callback);
		if(callback != null) {
			callback(this);
		}
	}
	
	protected override void InitUI(){
		base.InitUI();
		partyLabel.enabled = true;
		partyLabel.text = "";
	}
	
	protected override void InitState(){
		base.InitState();
		IsFocus = false;
		
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
//			IsEnable = !IsParty;
		}
	}
	
	protected override void UpdatePartyState(){
//		partyLabel.enabled = IsParty;
//		IsEnable = !IsParty;
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}
	
	protected override void RefreshState(){
		Debug.LogError ("RefreshState");
		base.RefreshState();
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			//IsEnable is FALSE as long as IsParty is TRUE
//			Debug.LogError("IsParty : " + IsParty);
			IsEnable = !IsParty;
			Debug.LogError("IsEnable : " + IsEnable);
		}
	}
}
