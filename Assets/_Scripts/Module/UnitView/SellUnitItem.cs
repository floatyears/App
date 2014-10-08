using UnityEngine;
using System.Collections;

public class SellUnitItem : MyUnitItem {

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
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
		}
	}

	public override void ItemCallback (params object[] args)
	{
		switch(args[0].ToString()){
		case "mark_item":
			if(args[1] as SellUnitItem == this){
				transform.FindChild("Sprite_Clycle").GetComponent<UISprite>().enabled = true;
				transform.FindChild("Sprite_Mask").GetComponent<UISprite>().enabled = true;
				transform.FindChild("Label_TopRight").GetComponent<UILabel>().text = ((int)args[2] + 1).ToString();
			}

			break;
		case "cancel_mark":
			if(args[1] as SellUnitItem == this){
				transform.FindChild("Sprite_Clycle").GetComponent<UISprite>().enabled = false;
				transform.FindChild("Sprite_Mask").GetComponent<UISprite>().enabled = false;
				transform.FindChild("Label_TopRight").GetComponent<UILabel>().text = string.Empty;
			}
			break;
		case "cancel_all":
			if(!isParty){
				transform.FindChild("Sprite_Clycle").GetComponent<UISprite>().enabled = false;
				transform.FindChild("Sprite_Mask").GetComponent<UISprite>().enabled = false;
			}

			transform.FindChild("Label_TopRight").GetComponent<UILabel>().text = string.Empty;
			break;
		}
	}


}
