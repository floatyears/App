using UnityEngine;
using System.Collections;
using bbproto;

public enum CatalogState{
	UnKnown,
	Got,
	Meet
}

public class CatalogUnitItem : MyUnitItem {
	private UISprite avatarSprite;
	private UISprite erotemeSpr;
	private UISprite maskSprite;
	private UISprite translucentMaskSpr;
	private UILabel idLabel;
	public UIWidget widget;

	private UISprite type;
	private UISprite bg;

	protected override void InitUI(){

		avatarSprite = transform.FindChild("Sprite_Avatar").GetComponent<UISprite>();
		erotemeSpr = transform.FindChild("Sprite_Erotemer").GetComponent<UISprite>();
		maskSprite = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		maskSpr = maskSprite;
		//translucentMaskSpr = transform.FindChild("Sprite_Translucent").GetComponent<UISprite>();
		idLabel = transform.FindChild("Label_ID").GetComponent<UILabel>();
		type = transform.FindChild ("Sprite_Type").GetComponent<UISprite> ();
		bg = transform.FindChild ("Background").GetComponent<UISprite> ();
	}

	protected override void RefreshState(){
		if(userUnit == null){
			Debug.LogError(string.Format("gameObject named {0} , TUserUnit is NULL...", gameObject.name));
			avatarSprite.atlas = null;
			avatarSprite.spriteName = string.Empty;
			erotemeSpr.enabled = true;
			maskSprite.enabled = false;
			//translucentMaskSpr.enabled = false;
			type.spriteName = "avatar_border_none";
			bg.spriteName = "avatar_bg_none";
		}
		else{
			UIEventListenerCustom.Get(this.gameObject).LongPress = null;
			UIEventListenerCustom.Get(this.gameObject).onClick = null;
			if(DataCenter.Instance.UnitData.CatalogInfo.IsHaveUnit(userUnit.unitId)){
				ResourceManager.Instance.GetAvatarAtlas(userUnit.unitId, avatarSprite);
				erotemeSpr.enabled = false;
				maskSprite.enabled = false;
				UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
				UIEventListenerCustom.Get(this.gameObject).onClick = PressItem;
				type.spriteName = GetBorderSpriteName();
				bg.spriteName = GetAvatarBgSpriteName();
			}
			else if(DataCenter.Instance.UnitData.CatalogInfo.IsMeetNotHaveUnit(userUnit.unitId)){
				ResourceManager.Instance.GetAvatarAtlas(userUnit.unitId, avatarSprite);
				maskSprite.enabled = true;
				erotemeSpr.enabled = false;
				type.spriteName = "avatar_border_none";
				bg.spriteName = "avatar_bg_none";
			}
			else{
				avatarSprite.atlas = null;
				avatarSprite.spriteName = string.Empty;
				erotemeSpr.enabled = true;
				maskSprite.enabled = false;
				//translucentMaskSpr.enabled = false;
				type.spriteName = "avatar_border_none";
				bg.spriteName = "avatar_bg_none";
			}
			idLabel.text = "No. " + userUnit.unitId.ToString();
		}
	}

	protected override void UpdateFavoriteState(){

	}

	string GetBorderSpriteName () {
		switch (userUnit.UnitType) {
		case 1:
			return "avatar_border_fire";
		case 2:
			return "avatar_border_water";
		case 3:
			return "avatar_border_wind";
		case 4:
			return "avatar_border_light";
		case 5:
			return "avatar_border_dark";
		case 6:
			return "avatar_border_none";
		default:
			return "avatar_border_none";
			break;
		}
	}
	
	string GetAvatarBgSpriteName() {
		switch (userUnit.UnitType) {
		case 1:
			return "avatar_bg_fire";
		case 2:
			return "avatar_bg_water";
		case 3:
			return "avatar_bg_wind";
		case 4:
			return "avatar_bg_light";
		case 5:
			return "avatar_bg_dark";
		case 6:
			return "avatar_bg_none";
		default:
			return "avatar_bg_none";
			break;
		}
	}

}
