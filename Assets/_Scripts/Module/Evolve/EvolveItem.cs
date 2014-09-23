using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class EvolveItem {
	public GameObject itemObject;
//	public BoxCollider boxCollider;
	public UserUnit userUnit;
	public UISprite showTexture;
	public UILabel haveLabel;
	public UISprite maskSprite;
	public UISprite highLight;
	public UISprite borderSprite;
	public UISprite bgprite;
	public int index;
	public bool HaveUserUnit = true;
	private UIEventListenerCustom listener ;

	public EvolveItem (int index, GameObject target) {
		index = index;
		itemObject = target;
		Transform trans = target.transform;
		showTexture = trans.Find("Texture").GetComponent<UISprite>();
		highLight = trans.Find("Light").GetComponent<UISprite>();
		borderSprite = trans.Find("Sprite_Avatar_Border").GetComponent<UISprite>();
		bgprite = trans.Find("Sprite_Avatar_Bg").GetComponent<UISprite>(); 
//		boxCollider = target.GetComponent<BoxCollider>();
		highLight.enabled = false;
		listener = UIEventListenerCustom.Get (target);
		if (index == 1 || index == 5) {
			return;		
		}

		haveLabel = trans.Find ("HaveLabel").GetComponent<UILabel> ();
		maskSprite = trans.Find ("Mask").GetComponent<UISprite> ();
	}

	public void Refresh (UserUnit tuu, bool isHave = true) {
		userUnit = tuu;
		HaveUserUnit = isHave;
		ShowShield (!isHave);
		if (tuu == null) {
			showTexture.spriteName = "";
			borderSprite.enabled = false;
			bgprite.spriteName = "unit_empty_bg";
			listener.LongPress = null;
		} else {
			listener.LongPress = LongPress;
			borderSprite.enabled = true;
			ShowUnitType();
//			userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
//				showTexture.mainTexture = o as Texture2D;
//			});
			ResourceManager.Instance.GetAvatarAtlas(userUnit.UnitInfo.id, showTexture);
		}
	}

	void LongPress(GameObject target) {
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"unit",userUnit);
	}

	void ShowShield(bool show) {
		if(maskSprite != null && maskSprite.enabled != show) {
			maskSprite.enabled = show;
		}
		if(haveLabel != null && haveLabel.enabled != show) {
			haveLabel.enabled = show;
		}
//		if (boxCollider != null && boxCollider.enabled == show) {
//			boxCollider.enabled = !show;
//		}
	}

	private void ShowUnitType(){
		switch (userUnit.UnitInfo.type){
		case EUnitType.UFIRE :
			bgprite.spriteName = "avatar_bg_fire";
			borderSprite.spriteName = "avatar_border_fire";
			break;
		case EUnitType.UWATER :
			bgprite.spriteName = "avatar_bg_water";
			borderSprite.spriteName = "avatar_border_water";
			
			break;
		case EUnitType.UWIND :
			bgprite.spriteName = "avatar_bg_wind";
			borderSprite.spriteName = "avatar_border_wind";
			
			break;
		case EUnitType.ULIGHT :
			bgprite.spriteName = "avatar_bg_light";
			borderSprite.spriteName = "avatar_border_light";
			
			break;
		case EUnitType.UDARK :
			bgprite.spriteName = "avatar_bg_dark";
			borderSprite.spriteName = "avatar_border_dark";
			
			break;
		case EUnitType.UNONE :
			bgprite.spriteName = "avatar_bg_none";
			borderSprite.spriteName = "avatar_border_none";
			
			break;
		default:
			break;
		}
	}
}