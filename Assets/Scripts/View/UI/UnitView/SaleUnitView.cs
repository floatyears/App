using UnityEngine;
using System.Collections;

public class SaleUnitView : MyUnitView {
	private UISprite pickedMarkSpr;

	protected override void ClickItem(GameObject item){

	}

	protected override void InitUI(){
		base.InitUI();
//		pickedMarkSpr = transform.FindChild()
	}

	protected override void InitState(){
		base.InitState();
		IsParty = DataCenter.Instance.PartyInfo.UnitIsInParty(userUnit.ID);
	}


}
