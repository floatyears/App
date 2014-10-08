using UnityEngine;
using System.Collections;

public class EvolveDragItem : MyUnitItem {

	public bool CanEvolve = false;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}
	
	protected override void InitUI(){
		base.InitUI();

		IsFocus = false;
	}

	protected override void SetCommonState () {
		base.SetCommonState ();
		IsEnable = userUnit.isEnable;
//		Debug.LogError ("gameobject : " + gameObject + " isenable : " + IsEnable + " id : " + userUnit.UnitInfo.ID);
		IsFocus = userUnit.isFocus;
	}

	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
//		IsEnable = !IsParty;
	}
	
	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;

//		Debug.LogError ("UpdateFocus : " + gameObject + "tuserunit : " + userUnit.UnitInfo.ID + " lightSpr : " + lightSpr + " isfouce ; " + IsFocus);
	}
	
	protected override void RefreshState(){
		base.RefreshState();
		if(userUnit != null){
			IsParty = DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(userUnit.uniqueId);
			//IsEnable is FALSE as long as IsParty is TRUE
//			IsEnable = !IsParty;
		}
	}

	public override void SetData<T> (T data, params object[] args)
	{
		base.SetData (data, args);

		bool evolveInfoNull = userUnit.UnitInfo.evolveInfo != null;
		bool rareIsMax = userUnit.UnitInfo.maxStar > 0 && userUnit.UnitInfo.rare < userUnit.UnitInfo.maxStar;
		if(evolveInfoNull && rareIsMax) {
			CanEvolve = true;
		} else {
			IsEnable = false;
		}
//		if(!isParty && !isFavorite) {
//			normalDragItem.Add(edi);
//		}
	}

}
