using UnityEngine;
using System.Collections;

public class SaleUnitView : MyUnitView {
//	private UISprite pickedMarkSpr;
	public delegate void UnitItemCallback(SaleUnitView puv);
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
		IsParty = DataCenter.Instance.PartyInfo.UnitIsInParty(userUnit.ID);
	}

}
